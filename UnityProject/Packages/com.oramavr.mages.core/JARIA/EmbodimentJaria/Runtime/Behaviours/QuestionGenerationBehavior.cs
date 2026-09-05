namespace MAGES.Experimental.EmbodimentJARIA
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MAGES.JARIA;
    using UnityEngine.Scripting;

    /// <summary>
    /// Class to handle question generation behavior.
    /// </summary>
    [Preserve]
    [MarkExperimental]
    public class QuestionGenerationBehavior : IBehavior
    {
        private int numberOfConversations = 2;
        private string activeQuestion = string.Empty;
        private bool referToContext = true;
        private int conversationsCounter = 0;
        private string activeAnswer = string.Empty;
        private UserAuthenticationModule uam = Hub.Instance.Get<UserAuthenticationModule>();

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionGenerationBehavior"/> class.
        /// Default constructor.
        /// </summary>
        public QuestionGenerationBehavior()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionGenerationBehavior"/> class.
        /// Constructor with number of conversations and refer to context.
        /// </summary>
        /// <param name="numberOfConversations">Number of conversations.</param>
        /// <param name="referToContext">Whether to refer to context.</param>
        public QuestionGenerationBehavior(int numberOfConversations, bool referToContext)
        {
            this.numberOfConversations = numberOfConversations;
            this.referToContext = referToContext;
        }

        /// <summary>
        /// Gets a value indicating whether the behavior should execute on startup.
        /// </summary>
        public bool ExecuteOnStartup => true;

        /// <inheritdoc cref="IBehavior"/>
        public async Task ExecuteAsync(string input, string agentID, AgentConfiguration agentConfig)
        {
            await Task.Run(async () =>
            {
                HttpResponseMessage response;
                if (activeQuestion == string.Empty)
                {
                    Dictionary<object, object> parameters = new Dictionary<object, object>
                    {
                        { "find_docs", referToContext },
                        { "agent_id", agentID },
                    };

                    response = await JariaRequest.PrepareRequest(input, uam, parameters);
                    Dictionary<object, object> questionAnswer = await JariaRequest.HandleJsonResponseAsync(response);
                    activeQuestion = questionAnswer["question"].ToString();
                    activeAnswer = questionAnswer["answer"].ToString();
                    string text2Speak = string.Copy(activeQuestion);
                    await agentConfig.TextHandler.HandleTextAsync(text2Speak);
                }
                else
                {
                    bool provideAnswer = conversationsCounter >= numberOfConversations;
                    Dictionary<object, object> requestData = new Dictionary<object, object>
                    {
                        { "question", activeQuestion },
                        { "provide_answer", provideAnswer },
                        { "agent_id",  agentID },
                    };
                    if (activeAnswer != string.Empty)
                    {
                        requestData.Add("correct_answer", activeAnswer);
                    }

                    response = await JariaRequest.PrepareRequest(input, uam, requestData);
                    await EmbodiedAIRequest.HandleStreamingResponseAsync(response, agentConfig);
                    conversationsCounter++;
                }
            });
        }
    }
}
