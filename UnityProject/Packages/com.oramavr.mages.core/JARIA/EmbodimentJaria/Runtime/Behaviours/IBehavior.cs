namespace MAGES.Experimental.EmbodimentJARIA
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a behavior.
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        /// Gets a value indicating whether if the behavior should execute on startup.
        /// </summary>
        public bool ExecuteOnStartup { get; }

        /// <summary>
        /// Task to execute the behavior.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <param name="agentID">The agent's ID.</param>
        /// <param name="agentConfig">The agent's configuration.</param>
        /// <returns>Task.</returns>
        public Task ExecuteAsync(string input, string agentID, AgentConfiguration agentConfig);
    }
}