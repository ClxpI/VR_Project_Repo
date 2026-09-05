namespace MAGES.Experimental.EmbodimentJARIA
{
    using System;
    using System.Linq;
    using MAGES.Utilities;
    using MAGES.Utilities.Collections;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// The Agent configuration.
    /// </summary>
    [MarkExperimental]
    [CreateAssetMenu(fileName = "Agent", menuName = "MAGES/JARIA/Agent")]
    public class AgentConfiguration : ScriptableObject
    {
        [SerializeField]
        private string agentID;

        [SerializeField]
        private GameObject embodimentCharacterPrefab;

        [SerializeField]
        private string embodimentCharacterScene;

        [SerializeField]
        private bool sceneCharacter;

        [SerializeField]
        private GameObject interactionComponent;

        [SerializeField]
        private GameObject agentOutputComponet;

        [SerializeField]
        private string textspeechHandlerRegion;

        [SerializeField]
        private string textHandlerKey;

        [SerializeField]
        private string speechHandlerKey;

        [SerializeField]
        private string voiceID;

        [SerializeField]
        private string customEndpoint;

        [SerializeField]
        private string languadeID;

        private SerializableDictionary<string, string> agentDetails = new SerializableDictionary<string, string>();

        private string agentBehavior;

        private GameObject embodimentCharacter;

        private Animator characterAnimator;

        private AudioSource characterAudioSource;

        private GameObject interactionUI;

        private Button startButton;

        private Button cancelButton;

        private GameObject agentOutput;

        private TextMeshProUGUI agentOutputText;

        private ITextToSpeechHandler textHandler;

        private ISpeechToTextHandler speechHandler;

        private UnityEvent onInteract = new();

        private UnityEvent onCancel = new();

        private UnityEvent onStartTalking = new();

        private UnityEvent onEndTalking = new();

        private Agent spawnedAgent;

        /// <summary>
        /// Gets or Sets the AgentID.
        /// </summary>
        public string AgentID
        {
            get { return agentID; }
            set { agentID = value; }
        }

        /// <summary>
        /// Gets or Sets the Agent Details.
        /// </summary>
        public SerializableDictionary<string, string> AgentDetails
        {
            get { return agentDetails; }
            set { agentDetails = value; }
        }

        /// <summary>
        /// Gets the Agent Behavior.
        /// </summary>
        public string AgentBehavior
        {
            get { return agentBehavior; }
        }

        /// <summary>
        /// Gets or sets the embodiment character as a prefab.
        /// </summary>
        public GameObject EmbodimentCharacterPrefab
        {
            get { return embodimentCharacterPrefab; }
            set { embodimentCharacterPrefab = value; }
        }

        /// <summary>
        /// Gets or sets the embodiment character in scene.
        /// </summary>
        public string EmbodimentCharacterScene
        {
            get { return embodimentCharacterScene; }
            set { embodimentCharacterScene = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the character is from scene or a prefab.
        /// </summary>
        public bool SceneCharacter
        {
            get { return sceneCharacter; }
            set { sceneCharacter = value; }
        }

        /// <summary>
        /// Gets the spawned embodiment character.
        /// </summary>
        public GameObject EmbodimentCharacter
        {
            get { return embodimentCharacter; }
        }

        /// <summary>
        /// Gets or sets the interaction component.
        /// </summary>
        public GameObject AgentInteractionComponent
        {
            get { return interactionComponent; }
            set { interactionComponent = value; }
        }

        /// <summary>
        /// Gets or sets the interaction component reference.
        /// </summary>
        public GameObject AgentInteraction
        {
            get { return interactionUI; }
            set { interactionUI = value; }
        }

        /// <summary>
        /// Gets the startButton.
        /// </summary>
        public Animator CharacterAnimator
        {
            get { return characterAnimator; }
        }

        /// <summary>
        /// Gets the audiosource.
        /// </summary>
        public AudioSource CharacterAudioSource
        {
            get { return characterAudioSource; }
        }

        /// <summary>
        /// Gets the startButton.
        /// </summary>
        public Button StartButton
        {
            get { return startButton; }
        }

        /// <summary>
        /// Gets the startButton.
        /// </summary>
        public Button CancelButton
        {
            get { return cancelButton; }
        }

        /// <summary>
        /// Gets or sets the agent output component.
        /// </summary>
        public GameObject AgentOutputComponent
        {
            get { return agentOutputComponet; }
            set { agentOutputComponet = value; }
        }

        /// <summary>
        /// Gets the agent output component.
        /// </summary>
        public GameObject AgentOutput
        {
            get { return agentOutput; }
        }

        /// <summary>
        /// Gets the agent output text.
        /// </summary>
        public TextMeshProUGUI AgentOutputText
        {
            get { return agentOutputText; }
        }

        /// <summary>
        /// Gets the text handler ref.
        /// </summary>
        public ITextToSpeechHandler TextHandler
        {
            get { return textHandler; }
        }

        /// <summary>
        /// Gets the speech handler ref.
        /// </summary>
        public ISpeechToTextHandler SpeechHandler
        {
            get { return speechHandler; }
        }

        /// <summary>
        /// Gets or Sets the region for SpeechHandler.
        /// </summary>
        public string TextSpeechHandlerRegion
        {
            get { return textspeechHandlerRegion; }
            set { textspeechHandlerRegion = value; }
        }

        /// <summary>
        /// Gets or Sets the key for TextHandler.
        /// </summary>
        public string TextHandlerKey
        {
            get { return textHandlerKey; }
            set { textHandlerKey = value; }
        }

        /// <summary>
        /// Gets or Sets the key for SpeechHandler.
        /// </summary>
        public string SpeechHandlerKey
        {
            get { return speechHandlerKey; }
            set { speechHandlerKey = value; }
        }

        /// <summary>
        /// Gets or Sets the key for custom endpoint URL or key.
        /// </summary>
        public string CustomEndpointID
        {
            get { return customEndpoint; }
            set { customEndpoint = value; }
        }

        /// <summary>
         /// Gets or Sets the language ID.
         /// </summary>
        public string LanguageID
        {
            get { return languadeID; }
            set { languadeID = value; }
        }

        /// <summary>
        /// Gets or Sets the voice ID for text handler configuration.
        /// </summary>
        public string VoiceID
        {
            get { return voiceID; }
            set { voiceID = value; }
        }

        /// <summary>
        /// Gets or sets event for interact.
        /// </summary>
        public UnityEvent OnInteract
        {
            get { return onInteract; }
            set { onInteract = value; }
        }

        /// <summary>
        /// Gets or sets event for cancel.
        /// </summary>
        public UnityEvent OnCancel
        {
            get { return onCancel; }
            set { onCancel = value; }
        }

        /// <summary>
        /// Gets or sets event called when agent starts talking.
        /// </summary>
        public UnityEvent OnStartTalking
        {
            get { return onStartTalking; }
            set { onStartTalking = value; }
        }

        /// <summary>
        /// Gets or sets event called when agent stops talking.
        /// </summary>
        public UnityEvent OnEndTalking
        {
            get { return onEndTalking; }
            set { onEndTalking = value; }
        }

        /// <summary>
        /// Call to initialize the agent.
        /// </summary>
        public void InitializeAgent()
        {
            agentBehavior = "mages_jaria";

            if (sceneCharacter)
            {
                embodimentCharacter = GameObject.Find(embodimentCharacterScene);
            }
            else
            {
                embodimentCharacter = GameObjectSpawner.Instance.SpawnObject(embodimentCharacterPrefab);
            }

            interactionUI = GameObjectSpawner.Instance.SpawnObject(interactionComponent, embodimentCharacter.transform);
            characterAnimator = embodimentCharacter.GetComponent<Animator>();
            characterAudioSource = embodimentCharacter.GetOrAddComponent<AudioSource>();

            startButton = interactionUI.GetComponent<InteractionButtonsRef>().GetStartButton;
            cancelButton = interactionUI.GetComponent<InteractionButtonsRef>().GetCancelButton;

            agentOutput = GameObjectSpawner.Instance.SpawnObject(agentOutputComponet);
            agentOutputText = agentOutput.GetComponent<AgentOutput>().OutputText;

            var textToSpeechType = ReflectionUtilities.GetAllTypesImplementingInterface<ITextToSpeechHandler>(AppDomain.CurrentDomain).FirstOrDefault();
            var speechToTextType = ReflectionUtilities.GetAllTypesImplementingInterface<ISpeechToTextHandler>(AppDomain.CurrentDomain).FirstOrDefault();

            if (textToSpeechType == null || speechToTextType == null)
            {
                startButton.onClick.AddListener(() => { Debug.Log("Azure Speech SDK is needed to interact with Embodied AI agents. You can download it from MAGES Panel."); });
                return;
            }

            textHandler = (ITextToSpeechHandler)Activator.CreateInstance(textToSpeechType);
            speechHandler = (ISpeechToTextHandler)Activator.CreateInstance(speechToTextType);

            StartAgent();
        }

        /// <summary>
        /// Required to be called when changing buttons.
        /// </summary>
        /// <param name="newStartButton">The new start button.</param>
        /// <param name="newCancelButton">The new cancel button.</param>
        public void ChangeButtons(Button newStartButton, Button newCancelButton)
        {
            startButton = newStartButton;
            cancelButton = newCancelButton;
            spawnedAgent.OnButtonsChange.Invoke();
        }

        private void StartAgent()
        {
            if (embodimentCharacter.GetComponent<Agent>())
            {
                Destroy(embodimentCharacter.GetComponent<Agent>());
            }

            spawnedAgent = embodimentCharacter.AddComponent<Agent>();
            spawnedAgent.AgentConfiguration = this;

            try
            {
                textHandler.TheAgent = embodimentCharacter.GetComponent<Agent>();
                textHandler.Startup();
            }
            catch (NullReferenceException)
            {
                Debug.LogError("Text Handler implementation missing.");
            }

            try
            {
                speechHandler.TheAgent = embodimentCharacter.GetComponent<Agent>();
                speechHandler.Startup();
            }
            catch (NullReferenceException)
            {
                Debug.LogError("Speech Handler implementation missing.");
            }
        }
    }
}