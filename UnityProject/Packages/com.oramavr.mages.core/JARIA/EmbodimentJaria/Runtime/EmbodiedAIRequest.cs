namespace MAGES.Experimental.EmbodimentJARIA
{
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using MAGES.Utilities;
    using UnityEngine;

    /// <summary>
    /// Handles requests of the embodiment AI module.
    /// </summary>
    [MarkExperimental]
    public class EmbodiedAIRequest : MonoBehaviour
    {
        /// <summary>
        /// Handles the streaming response from JARIA.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="agentConfig">The agent configuration.</param>
        /// <returns>Task.</returns>
        public static async Task HandleStreamingResponseAsync(HttpResponseMessage response, AgentConfiguration agentConfig)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                byte[] buffer = new byte[1];
                MemoryStream partialBuffer = new MemoryStream();
                int bytesRead;
                string totalResponse = string.Empty;
                var handler = agentConfig.TextHandler;
                handler.IsSynthesizing = true;

                while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    if (handler.IsCanceled)
                    {
                        handler.IsCanceled = false;
                        break;
                    }

                    /*
                    * OpenAI sends a character as a UTF8 sequence. But not all characters are 1-byte.
                    * In this streaming request we are reading on each loop one byte and then check if it is a complete UTF8 character.
                    * If it is not complete we continue so we read another byte and try again.
                    * UTF8 has been designed in such way so that all characters are exlusive based on the numbers of bytes.
                    */
                    partialBuffer.Write(buffer, 0, bytesRead);
                    var partialArray = partialBuffer.ToArray();
                    if (!IsCompleteUTF8Sequence(partialArray))
                    {
                        continue;
                    }

                    string result = Encoding.UTF8.GetString(partialArray);
                    partialBuffer.SetLength(0);  // Clear the buffer if decoding succeeded

                    totalResponse += result;

                    if (result == "." || result == "?" || result == "!" || result == ";")
                    {
                        int lastIndexOfDot = totalResponse.LastIndexOf(".");
                        if (lastIndexOfDot > 0)
                        {
                            char previousChar = totalResponse[lastIndexOfDot - 1];

                            if (char.IsDigit(previousChar) && result == ".")
                            {
                                continue;
                            }
                        }

                        if (handler != null)
                        {
                            totalResponse = totalResponse.Replace("#", string.Empty);
                            string text2Speak = string.Copy(totalResponse);
                            handler.HandleTextAsync(text2Speak).RunTaskAndExecuteActionInMainThreadWhenFinished(() => handler.SyncMessage(text2Speak));
                            totalResponse = string.Empty;
                        }

                        totalResponse = string.Empty;
                    }
                }

                handler.IsSynthesizing = false;
            }
        }

        /// <summary>
        /// Gets the agent info from ID.
        /// </summary>
        /// <param name="agentID">The agent's ID.</param>
        /// <param name="authAPI">Authentication manager reference.</param>
        /// <returns>Returns the agent info.</returns>
        public static async Task<string> GetAgentFromID(string agentID, IAuthenticationAPI authAPI)
        {
            var user = await authAPI.GetUser();
            var organization = user.Item2.Value.Tenant.Name;
            Task<HttpResponseMessage> response = authAPI.SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, $"https://jariaapi.oramavr.com/api/controller/agents/libraryNames/{organization}/{agentID}"), HttpCompletionOption.ResponseHeadersRead);
            HttpResponseMessage agentResponse = await response;
            string agentConfifg = agentResponse.Content.ReadAsStringAsync().Result;
            return agentConfifg;
        }

        /// <summary>
        /// Clears the agent's history.
        /// </summary>
        /// <param name="authAPI">Authentication manager reference.</param>
        /// <returns>Task.</returns>
        public static async Task<string> ClearAgentHistoty(IAuthenticationAPI authAPI)
        {
            var user = await authAPI.GetUser();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://jariaapi.oramavr.com/api/controller/clearhistory");
            requestMessage.Content = new StringContent($"\"{user.Item2.Value.Id}\"", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await authAPI.SendRequestAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            string res = response.Content.ReadAsStringAsync().Result;
            return res;
        }

        private static bool IsCompleteUTF8Sequence(byte[] byteArray)
        {
            int length = byteArray.Length;
            if (length == 0)
            {
                return false;
            }

            // Determine the expected number of bytes for the character based on the first byte
            int expectedLength;
            byte firstByte = byteArray[0];

            // Number of bytes detection
            if ((firstByte & 0b10000000) == 0)
            {
                expectedLength = 1;
            }
            else if ((firstByte & 0b11100000) == 0b11000000)
            {
                expectedLength = 2;
            }
            else if ((firstByte & 0b11110000) == 0b11100000)
            {
                expectedLength = 3;
            }
            else if ((firstByte & 0b11111000) == 0b11110000)
            {
                expectedLength = 4;
            }
            else
            {
                return false; // Invalid leading byte
            }

            // Check if we have the expected number of bytes in the sequence
            if (length < expectedLength)
            {
                return false;
            }

            // Verify continuation bytes (they should all match 10xxxxxx pattern)
            for (int i = 1; i < expectedLength; i++)
            {
                // Not a valid continuation byte
                if ((byteArray[i] & 0b11000000) != 0b10000000)
                {
                    return false;
                }
            }

            return true;  // Valid UTF-8 character sequence
        }
    }
}