namespace MAGES.Experimental.EmbodimentJARIA
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MAGES.JARIA;
    using UnityEngine.Scripting;

    /// <summary>
    /// Class to handle JARIA behavior.
    /// </summary>
    [Preserve]
    [MarkExperimental]
    public class MAGESJariaBehavior : IBehavior
    {
        private UserAuthenticationModule uam = Hub.Instance.Get<UserAuthenticationModule>();

        /// <summary>
        /// Gets a value indicating whether the behavior should execute on startup.
        /// </summary>
        public bool ExecuteOnStartup => false;

        /// <inheritdoc cref="IBehavior"/>/>
        public async Task ExecuteAsync(string input, string agentID, AgentConfiguration agentConfig)
        {
            await Task.Run(async () =>
            {
                Dictionary<object, object> parameters = new Dictionary<object, object>
                    {
                        { "agentName", agentID },
                    };
                HttpResponseMessage response = await JariaRequest.PrepareRequest(input, uam, parameters);

                await EmbodiedAIRequest.HandleStreamingResponseAsync(response, agentConfig);

                agentConfig.TextHandler.IsSynthesizing = false;
            });
        }
    }
}
