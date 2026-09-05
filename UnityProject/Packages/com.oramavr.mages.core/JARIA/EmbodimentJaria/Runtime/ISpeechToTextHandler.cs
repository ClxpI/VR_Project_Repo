namespace MAGES.Experimental.EmbodimentJARIA
{
    using System;

    /// <summary>
    /// Interface for handling speech recognition.
    /// </summary>
    public interface ISpeechToTextHandler
    {
        /// <summary>
        /// Event that is triggered when user input is recorded.
        /// </summary>
        event EventHandler UserInputRecorded;

        /// <summary>
        /// Event that is triggered when the recognizer is recognizing.
        /// </summary>
        event EventHandler Recognizing;

        /// <summary>
        /// Gets or sets the Agent script.
        /// </summary>
        public Agent TheAgent { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user input has been recognized.
        /// </summary>
        public bool SpeechRecognized { get; }

        /// <summary>
        /// Gets or sets the text during recognition.
        /// </summary>
        public string TextDuringRecognition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the handler is waiting for input.
        /// </summary>
        bool WaitingForInput { get; set; }

        /// <summary>
        /// Gets or sets the recognized user input.
        /// </summary>
        string RecognizedUserInput { get; set; }

        /// <summary>
        /// Starts the speech recognition.
        /// </summary>
        void StartSpeechRecognitionAsync();

        /// <summary>
        /// Resets the user's recognized input.
        /// </summary>
        void ResetRecognizedInput();

        /// <summary>
        /// Stops the speech recognition.
        /// </summary>
        void StopSpeechRecognitionAsync();

        /// <summary>
        /// Starts up the speech handler.
        /// </summary>
        void Startup();
    }
}