namespace MAGES.Experimental.EmbodimentJARIA
{
    using System.Collections;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// Class used to handle the speech input from the user.
    /// </summary>
    [MarkExperimental]
    public class SpeechInput : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI userInput;

        [SerializeField]
        private string defaultText;

        [SerializeField]
        private LookAtObject lookAtObject;
        private AgentConfiguration agentConfig;
        private bool waitingForInput = true;
        private string lastInput = string.Empty;

        private uint dotCounter = 0;

        /// <summary>
        /// Gets or sets the default text to be displayed when waiting for input.
        /// </summary>
        public string DefaultText
        {
            get => defaultText;
            set => defaultText = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this property is used to check if we are currently waiting for input.
        /// </summary>
        public bool WaitingForInput
        {
            get => waitingForInput;
            set => waitingForInput = value;
        }

        /// <summary>
        /// Sets a value indicating the currently interacting agent configuration.
        /// </summary>
        public AgentConfiguration AgentConfig
        {
            set => agentConfig = value;
        }

        /// <summary>
        /// Gets or sets this property is used to check if we are currently waiting for input.
        /// </summary>
        public TextMeshProUGUI UserInput
        {
            get => userInput;
            set => userInput = value;
        }

        /// <summary>
        /// Resets the user input text UI to the default values.
        /// </summary>
        public void ResetUserInputText()
        {
            waitingForInput = true;
            userInput.text = defaultText;
            StartCoroutine(DefaultUIAnimation());
        }

        private IEnumerator DefaultUIAnimation()
        {
            while (waitingForInput)
            {
                if (dotCounter == 4 || dotCounter == 0)
                {
                    dotCounter = 0;
                    userInput.text = defaultText;
                    dotCounter++;
                }
                else
                {
                    dotCounter++;
                    userInput.text += ".";
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void Start()
        {
            StartCoroutine(CheckInputToSendAsItIs());
        }

        private void LateUpdate()
        {
            var target = lookAtObject.Target;
            Vector3 targetPosition = target.position + target.forward;
            targetPosition.y = target.position.y - 0.5f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 3.6f);
        }

        private void OnEnable()
        {
            lookAtObject = GetComponent<LookAtObject>();
        }

        private IEnumerator CheckInputToSendAsItIs()
        {
            float timer = 3f;

            while (true)
            {
                string currentInput = userInput.text;

                if (currentInput != lastInput || userInput.text.StartsWith(defaultText) || agentConfig.SpeechHandler.SpeechRecognized)
                {
                    timer = 3f;
                    lastInput = currentInput;
                }

                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    if (!agentConfig.SpeechHandler.SpeechRecognized)
                    {
                        agentConfig.SpeechHandler.RecognizedUserInput = currentInput;
                    }

                    timer = 3f;
                }

                yield return null;
            }
        }
    }
}