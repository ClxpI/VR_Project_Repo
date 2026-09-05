namespace MAGES.Experimental.EmbodimentJARIA
{
    using System.Collections.Generic;
    using System.Linq;
    using MAGES;
    using MAGES.Utilities;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// The root module for the JARIA.
    /// </summary>
    [MarkExperimental]
    [CreateAssetMenu(fileName = "JARIA", menuName = "MAGES/JARIA/JARIAModule")]
    public class EmbodimentJARIAModule : HubModule
    {
        [SerializeField]
        private GameObject subsUI;

        [SerializeField]
        private List<AgentConfiguration> agents = new List<AgentConfiguration>();

        private GameObject subsComponent;

        private TextMeshProUGUI userSubsText;

        /// <summary>
        /// Gets the Subs component.
        /// </summary>
        public GameObject UserSubsComponent
        {
            get { return subsComponent; }
        }

        /// <summary>
        /// Gets the reference to user subs Text.
        /// </summary>
        public TextMeshProUGUI UserSubsText
        {
            get { return userSubsText; }
        }

        /// <inheritdoc/>
        public override void Startup()
        {
            InitializeUserSubs();
        }

        /// <inheritdoc/>
        public override void Shutdown()
        {
        }

        /// <summary>
        /// Call to initialize an agent by its index.
        /// </summary>
        /// <param name="index"> The agent's index inside MAGESJaria module.</param>
        /// <returns>Returns the agent configuration.</returns>
        public AgentConfiguration InitializeAgent(int index)
        {
            var ag = agents[index];
            ag.InitializeAgent();
            return ag;
        }

        /// <summary>
        /// Call to initialize an agent by its ID.
        /// </summary>
        /// <param name="agentID"> The agent's ID.</param>
        /// /// <returns>Returns the agent configuration.</returns>
        public AgentConfiguration InitializeAgent(string agentID)
        {
            var agent = agents.SingleOrDefault(ag => ag.AgentID == agentID);

            if (agent == null)
            {
                Debug.Log("Agent with ID " + agentID + "could not be found.");
                return null;
            }

            agent.InitializeAgent();
            return agent;
        }

        /// <summary>
        /// Call to get an agent by its ID.
        /// </summary>
        /// <param name="agentID"> The agent's ID.</param>
        /// /// <returns>Returns the agent configuration.</returns>
        public AgentConfiguration GetAgentFromID(string agentID)
        {
            var agent = agents.SingleOrDefault(ag => ag.AgentID == agentID);

            if (agent == null)
            {
                Debug.Log("Agent with ID " + agentID + "could not be found.");
                return null;
            }

            return agent;
        }

        /// <summary>
        /// Call to get an agent by its idex.
        /// </summary>
        /// <param name="agentIndex"> The agent's index.</param>
        /// /// <returns>Returns the agent configuration.</returns>
        public AgentConfiguration GetAgentFromIndex(int agentIndex)
        {
            return agents[agentIndex];
        }

        /// <summary>
        /// Clears the agent's history request.
        /// </summary>
        /// <param name="authAPI">Authentication manager reference.</param>
        public async void ClearHistory(IAuthenticationAPI authAPI)
        {
            await EmbodiedAIRequest.ClearAgentHistoty(authAPI);
        }

        private void InitializeUserSubs()
        {
            // Initialize User subs and camera
            subsComponent = GameObjectSpawner.Instance.SpawnObject(subsUI);
            subsComponent.GetComponent<LookAtObject>().TargetObject(Camera.main.transform);
            userSubsText = subsComponent.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "SpeechInput").GetComponent<TextMeshProUGUI>();
        }
    }
}
