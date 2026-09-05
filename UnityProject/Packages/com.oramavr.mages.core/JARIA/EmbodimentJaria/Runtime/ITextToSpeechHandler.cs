namespace MAGES.Experimental.EmbodimentJARIA
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for handling text to speech.
    /// </summary>
    public interface ITextToSpeechHandler
    {
        /// <summary>
        /// Gets or sets the Agent script.
        /// </summary>
        public Agent TheAgent { get; set; }

        /// <summary>
        /// Gets or sets the subtitles queue for the subtitles of the audio clips.
        /// </summary>
        public Queue<string> Subtitles { get; set; }

        /// <summary>
        /// Gets or sets the audio clips queue.
        /// </summary>
        public Queue<byte[]> AudioClips { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Text Handler is synthesizing.
        /// </summary>
        public bool IsSynthesizing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Text Handler is canceled.
        /// </summary>
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Handles the text to be spoken.
        /// </summary>
        /// <param name="text">Text to be spoken.</param>
        /// <returns>Task.</returns>
        Task HandleTextAsync(string text);

        /// <summary>
        /// Starts up the text handler.
        /// </summary>
        void Startup();

        /// <summary>
        /// Called to stop the voice.
        /// </summary>
        void StopVoice();

        /// <summary>
        /// Called to clear text handlers queue.
        /// </summary>
        void ClearQueue();

        /// <summary>
        /// Called to pass the message that is to be synced.
        /// </summary>
        /// /// <param name="message">The message to be synced.</param>
        public void SyncMessage(string message);
    }
}