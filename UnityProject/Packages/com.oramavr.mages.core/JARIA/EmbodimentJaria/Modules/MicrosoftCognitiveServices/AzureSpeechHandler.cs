namespace MAGES.Experimental.EmbodimentJARIA.MicrosoftCongitiveServices
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.CognitiveServices.Speech;
    using UnityEngine.Scripting;

    /// <summary>
    /// Class that handles speech recognition using Azure Cognitive Services.
    /// </summary>
    [Preserve]
    public class AzureSpeechHandler : ISpeechToTextHandler
    {
        private SpeechRecognizer recognizer;
        private SpeechConfig config;
        private bool waitingForInput = true;
        private bool speechRecognized = false;
        private string recognizedUserInput;
        private string textDuringRecognition;

        /// <summary>
        /// Event that is triggered when the recognizer is recognizing.
        /// </summary>
        public event EventHandler Recognizing;

        /// <inheritdoc cref="ISpeechToTextHandler"/>
        public event EventHandler UserInputRecorded;

        /// <summary>
        /// Gets or Sets the agent logic script.
        /// </summary>
        public Agent TheAgent { get; set; }

        /// <summary>
        /// Gets the speech handler config.
        /// </summary>
        public SpeechConfig SpeechHandlerConfig
        {
            get { return config; }
        }

        /// <inheritdoc cref="ISpeechToTextHandler"/>
        public bool SpeechRecognized
        {
            get => speechRecognized;
        }

        /// <inheritdoc cref="ISpeechToTextHandler"/>
        public bool WaitingForInput { get => waitingForInput; set => waitingForInput = value; }

        /// <summary>
        /// Gets or sets the text during recognition.
        /// </summary>
        public string TextDuringRecognition
        {
            get => textDuringRecognition; set
            {
                textDuringRecognition = value;
                TextRecognized();
            }
        }

        /// <inheritdoc cref="ISpeechToTextHandler"/>
        public string RecognizedUserInput
        {
            get => recognizedUserInput; set
            {
                recognizedUserInput = value;
                if (string.IsNullOrEmpty(recognizedUserInput))
                {
                    return;
                }

                UserInputRecognized();
            }
        }

        /// <inheritdoc cref="ISpeechToTextHandler"/>
        public void ResetRecognizedInput()
        {
            speechRecognized = false;
            recognizedUserInput = string.Empty;
        }

        /// <inheritdoc cref="ISpeechToTextHandler"/>
        public async void StartSpeechRecognitionAsync()
        {
            ResetRecognizedInput();
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISpeechToTextHandler"/>
        public async void StopSpeechRecognitionAsync()
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISpeechToTextHandler"/>
        public void Startup()
        {
            try
            {
                config = SpeechConfig.FromSubscription(TheAgent.AgentConfiguration.SpeechHandlerKey, TheAgent.AgentConfiguration.TextSpeechHandlerRegion);
            }
            catch
            {
                UnityEngine.Debug.LogError("Speech Handler failed to start due to empty/incorrect key or region.");
            }

            if (!string.IsNullOrEmpty(TheAgent.AgentConfiguration.CustomEndpointID))
            {
                config.EndpointId = TheAgent.AgentConfiguration.CustomEndpointID;
            }

            if (!string.IsNullOrEmpty(TheAgent.AgentConfiguration.LanguageID))
            {
                config.SpeechRecognitionLanguage = TheAgent.AgentConfiguration.LanguageID;
            }

            recognizer = new SpeechRecognizer(config);
            {
                recognizer.SpeechStartDetected += (s, e) =>
                {
                    waitingForInput = false;
                };

                recognizer.SpeechEndDetected += (s, e) =>
                {
                    waitingForInput = true;
                };

                recognizer.Recognizing += (s, e) =>
                {
                    TextDuringRecognition = e.Result.Text;
                };

                recognizer.Recognized += async (s, e) =>
                {
                    await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        RecognizedUserInput = e.Result.Text;
                    }
                    else if (e.Result.Reason == ResultReason.NoMatch)
                    {
                        UnityEngine.Debug.LogError("Azure-Speech-Handler: Speech could not be recognized.");
                    }
                    else if (e.Result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = CancellationDetails.FromResult(e.Result);
                    }
                };
            }
        }

        /// <summary>
        /// Invokes the UserInputRecorded event.
        /// </summary>
        protected virtual void UserInputRecognized()
        {
            speechRecognized = true;
            UserInputRecorded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the TextRecognized event.
        /// </summary>
        protected virtual void TextRecognized()
        {
            Recognizing?.Invoke(this, EventArgs.Empty);
        }
    }
}
