namespace MAGES.Experimental.EmbodimentJARIA
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Networking;

    /// <summary>
    /// Class to handle agent behavior.
    /// </summary>
    [MarkExperimental]
    public class Agent : MonoBehaviour
    {
        private Dictionary<string, Type> behaviorMapping = new Dictionary<string, Type>
        {
            { "question_conversation", typeof(QuestionGenerationBehavior) },
            { "mages_jaria", typeof(MAGESJariaBehavior) },
        };

        private IBehavior behavior;
        private EmbodimentJARIAModule embodimentModule;
        private NetworkingModule networkingModule;
        private SpeechInput speechInput;
        private AgentConfiguration agentconfig;
        private AudioClip currentSynthesizedAudioClip;
        private UnityEvent onButtonsChange;
        private bool endedSynthesizingSync = false;
        private bool hasSynthesizedClip = false;
        private bool currentlyInteracting = false;
        private string userText = string.Empty;

        /// <summary>
        /// Gets or sets the agent configuration.
        /// </summary>
        public AgentConfiguration AgentConfiguration
        {
            get { return agentconfig; }
            set { agentconfig = value; }
        }

        /// <summary>
        /// Gets event called when buttons references are updated.
        /// </summary>
        public UnityEvent OnButtonsChange
        {
            get { return onButtonsChange; }
        }

        /// <summary>
        /// Set to end SynthesizingSync.
        /// </summary>
        /// <param name="state"> The state as to end Synthesizing or not.</param>
        public void EndedSynthesizingSync(bool state)
        {
            endedSynthesizingSync = state;
        }

        /// <summary>
        /// Initialize the logic of this agent.
        /// </summary>
        public void Start()
        {
            embodimentModule = Hub.Instance.Get<EmbodimentJARIAModule>();
            networkingModule = Hub.Instance.Get<NetworkingModule>();
            speechInput = embodimentModule.UserSubsComponent.GetComponent<SpeechInput>();
            behavior = (IBehavior)Activator.CreateInstance(behaviorMapping[agentconfig.AgentBehavior]);

            agentconfig.StartButton.onClick.AddListener(async () => { await Interact(); });
            agentconfig.CancelButton.onClick.AddListener(Cancel);

            networkingModule.Integration.NetworkButtonAction(agentconfig.AgentInteraction, NetworkInteract, 0);
            networkingModule.Integration.NetworkButtonAction(agentconfig.AgentInteraction, StopAgent, 1);
            agentconfig.StartButton.onClick.AddListener(() => networkingModule.Integration.InvokeButtonRemoteCalls(agentconfig.AgentInteraction, 0));
            agentconfig.CancelButton.onClick.AddListener(() => networkingModule.Integration.InvokeButtonRemoteCalls(agentconfig.AgentInteraction, 1));

            agentconfig.SpeechHandler.Recognizing += (s, e) =>
            {
                speechInput.WaitingForInput = false;
                userText = agentconfig.SpeechHandler.TextDuringRecognition;
            };

            agentconfig.SpeechHandler.UserInputRecorded += async (s, e) =>
            {
                userText = agentconfig.SpeechHandler.RecognizedUserInput;
                agentconfig.SpeechHandler.StopSpeechRecognitionAsync();

                if (userText != string.Empty)
                {
                    await behavior.ExecuteAsync(agentconfig.SpeechHandler.RecognizedUserInput, agentconfig.AgentID, agentconfig);
                }
            };

            onButtonsChange = new UnityEvent();
            onButtonsChange.AddListener(ChangeInteractionButtons);

            StartCoroutine(PlaySynthesizedChunks());
        }

        /// <summary>
        /// Continuous update of subs.
        /// </summary>
        public void Update()
        {
            if (embodimentModule.UserSubsText.text != userText && !speechInput.WaitingForInput && currentlyInteracting)
            {
                embodimentModule.UserSubsText.text = userText;
            }
        }

        private void ChangeInteractionButtons()
        {
            agentconfig.StartButton.onClick.AddListener(async () => { await Interact(); });
            agentconfig.CancelButton.onClick.AddListener(() => { Cancel(); });

            if (networkingModule.IsInitialized)
            {
                networkingModule.Integration.NetworkButtonAction(agentconfig.AgentInteraction, NetworkInteract, 0);
                networkingModule.Integration.NetworkButtonAction(agentconfig.AgentInteraction, StopAgent, 1);
                agentconfig.StartButton.onClick.AddListener(() => networkingModule.Integration.InvokeButtonRemoteCalls(agentconfig.AgentInteraction, 0));
                agentconfig.CancelButton.onClick.AddListener(() => networkingModule.Integration.InvokeButtonRemoteCalls(agentconfig.AgentInteraction, 1));
            }
        }

        private async Task Interact()
        {
            currentlyInteracting = true;
            agentconfig.OnInteract.Invoke();
            agentconfig.TextHandler.IsCanceled = false;
            if (behavior.ExecuteOnStartup)
            {
                await behavior.ExecuteAsync(string.Empty, agentconfig.AgentID, agentconfig);
            }

            agentconfig.SpeechHandler.StartSpeechRecognitionAsync();
            agentconfig.CancelButton.gameObject.SetActive(true);
            agentconfig.StartButton.gameObject.SetActive(false);
            agentconfig.AgentOutput.GetComponent<AgentOutput>().StartAgentThinking();
            embodimentModule.UserSubsComponent.SetActive(true);
            speechInput.AgentConfig = agentconfig;
            speechInput.ResetUserInputText();
        }

        private void NetworkInteract()
        {
            currentlyInteracting = true;
            agentconfig.OnInteract.Invoke();
            agentconfig.TextHandler.IsCanceled = false;
            agentconfig.AgentOutput.GetComponent<AgentOutput>().StartAgentThinking();
            agentconfig.StartButton.GetComponentInChildren<TMP_Text>().text = "Another user is interacting";
            agentconfig.StartButton.interactable = false;
        }

        private void Cancel()
        {
            StopAgent();

            agentconfig.CancelButton.gameObject.SetActive(false);
            embodimentModule.UserSubsComponent.SetActive(false);
            agentconfig.StartButton.gameObject.SetActive(true);
            agentconfig.SpeechHandler.StopSpeechRecognitionAsync();
        }

        private void StopAgent()
        {
            currentlyInteracting = false;
            agentconfig.OnCancel.Invoke();
            agentconfig.TextHandler.IsCanceled = true;
            agentconfig.TextHandler.StopVoice();
            agentconfig.TextHandler.ClearQueue();
            agentconfig.CharacterAudioSource.Stop();
            agentconfig.StartButton.interactable = true;
            agentconfig.StartButton.GetComponentInChildren<TMP_Text>().text = "Interact";
        }

        private IEnumerator PlaySynthesizedChunks()
        {
            while (true)
            {
                if (agentconfig.TextHandler.AudioClips.Count > 0)
                {
                    yield return CreateAudioClip();

                    if (currentSynthesizedAudioClip != null)
                    {
                        OnFirstClip();
                        hasSynthesizedClip = true;
                        GetComponent<AudioSource>().clip = currentSynthesizedAudioClip;
                        GetComponent<AudioSource>().Play();
                        string agentSubtitles = GetNextSubs();

                        while (GetComponent<AudioSource>().isPlaying)
                        {
                            agentconfig.AgentOutputText.text = agentSubtitles;
                            yield return null;
                        }
                    }
                }
                else if (!agentconfig.TextHandler.IsSynthesizing && hasSynthesizedClip)
                {
                    ResetAgent();
                }

                yield return null;
            }
        }

        private void OnFirstClip()
        {
            if (hasSynthesizedClip)
            {
                return;
            }
            else
            {
                hasSynthesizedClip = true;
                agentconfig.AgentOutput.GetComponent<AgentOutput>().StopAgentThinking();
                agentconfig.OnStartTalking.Invoke();
            }
        }

        private void ResetAgent()
        {
            hasSynthesizedClip = false;
            embodimentModule.UserSubsText.text = speechInput.DefaultText;
            agentconfig.AgentOutput.GetComponent<AgentOutput>().ResetAgentThinking();
            agentconfig.OnEndTalking.Invoke();

            if (currentlyInteracting)
            {
                speechInput.ResetUserInputText();
            }

            if (agentconfig.StartButton.interactable)
            {
                agentconfig.SpeechHandler.StartSpeechRecognitionAsync();
            }
        }

        private IEnumerator CreateAudioClip()
        {
            string wavFilePath = "Response.wav";
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, wavFilePath);
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Response.wav", agentconfig.TextHandler.AudioClips.Dequeue());
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                currentSynthesizedAudioClip = audioClip;
            }
        }

        private string GetNextSubs()
        {
            return agentconfig.TextHandler.Subtitles.Dequeue().Trim();
        }
    }
}