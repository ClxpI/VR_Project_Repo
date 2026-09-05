namespace MAGES.Experimental.EmbodimentJARIA.MicrosoftCongitiveServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.CognitiveServices.Speech;
    using UnityEngine.Scripting;

    /// <summary>
    /// Class that handles text to speech using Azure Cognitive Services.
    /// </summary>
    [Preserve]
    public class AzureTextHandler : ITextToSpeechHandler
    {
        private SpeechSynthesizer synthesizer;
        private SpeechConfig config;
        private Queue<byte[]> clipQueue;
        private Queue<string> subtitleQueue;
        private string receivedText;
        private bool isSynthesizing = false;
        private bool isCanceled = false;

        /// <summary>
        /// Gets or Sets the agent logic script.
        /// </summary>
        public Agent TheAgent { get; set; }

        /// <summary>
        /// Gets the text handler config.
        /// </summary>
        public SpeechConfig TextHandlerConfig
        {
            get { return config; }
        }

        /// <inheritdoc cref="ITextToSpeechHandler"/>
        public Queue<string> Subtitles { get => subtitleQueue; set => subtitleQueue = value; }

        /// <inheritdoc cref="ITextToSpeechHandler"/>
        public Queue<byte[]> AudioClips { get => clipQueue; set => clipQueue = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the synthesizer is synthesizing.
        /// </summary>
        public bool IsSynthesizing
        {
            get => isSynthesizing;
            set
            {
                isSynthesizing = value;
                if (!value)
                {
                    TheAgent.EndedSynthesizingSync(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to cancel or not.
        /// </summary>
        public bool IsCanceled { get => isCanceled; set => isCanceled = value; }

        /// <inheritdoc cref="ITextToSpeechHandler"/>
        public async Task HandleTextAsync(string text)
        {
            if (subtitleQueue.Contains(text))
            {
                return;
            }

            receivedText = text;
            subtitleQueue.Enqueue(receivedText);
            await synthesizer.SpeakTextAsync(text);
        }

        /// <inheritdoc cref="ITextToSpeechHandler"/>
        public void Startup()
        {
            clipQueue = new Queue<byte[]>();
            subtitleQueue = new Queue<string>();
            Hub.Instance.Get<NetworkingModule>().Integration.AddMessageRemoteAction(TheAgent.AgentConfiguration.AgentOutput, async (message) => await HandleTextAsync(message));

            try
            {
                config = SpeechConfig.FromSubscription(TheAgent.AgentConfiguration.SpeechHandlerKey, TheAgent.AgentConfiguration.TextSpeechHandlerRegion);
            }
            catch
            {
                UnityEngine.Debug.LogError("Text Handler failed to start due to empty/incorrect key or region.");
            }

            if (TheAgent.AgentConfiguration.VoiceID != null)
            {
                config.SpeechSynthesisVoiceName = TheAgent.AgentConfiguration.VoiceID;
            }

            synthesizer = new SpeechSynthesizer(config, null);
            synthesizer.SynthesisCanceled += (s, e) =>
            {
                isSynthesizing = false;
            };

            synthesizer.SynthesisCompleted += (s, e) =>
            {
                clipQueue.Enqueue(e.Result.AudioData);
            };
        }

        /// <summary>
        /// Clears the queue.
        /// </summary>
        public void ClearQueue()
        {
            clipQueue.Clear();
            subtitleQueue.Clear();
        }

        /// <summary>
        /// Stops the voice.
        /// </summary>
        public async void StopVoice()
        {
            await synthesizer.StopSpeakingAsync();
            synthesizer.Dispose();
            Startup();
        }

        /// <summary>
        /// Sets the message to be synced.
        /// </summary>
        /// <param name="message"> The message to be synced.</param>
        public void SyncMessage(string message)
        {
            if (!Hub.Instance.Get<NetworkingModule>().IsInitialized)
            {
                return;
            }

            Hub.Instance.Get<NetworkingModule>().Integration.SyncMessage(TheAgent.AgentConfiguration.AgentOutput, message);
        }
    }
}