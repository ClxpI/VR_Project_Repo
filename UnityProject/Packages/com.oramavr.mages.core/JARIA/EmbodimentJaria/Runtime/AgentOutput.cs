namespace MAGES.Experimental.EmbodimentJARIA
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Class used to handle JARIA's output.
    /// </summary>
    [MarkExperimental]
    public class AgentOutput : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI outputText;
        [SerializeField]
        private Image backgroundImage;
        private string defaultWaitingText = "Waiting";
        private string defaultThinkingText = "Thinking";
        private int state = 0;
        private List<GameObject> otherOutputLayers = new List<GameObject>();
        private Vector2 padding = new Vector2(10, 10);
        private RectTransform textRectTransform;
        private RectTransform backgroundRectTransform;

        /// <summary>
        /// Gets or sets to return agent output text.
        /// </summary>
        public TMPro.TextMeshProUGUI OutputText
        {
            get { return outputText; }
            set { outputText = value; }
        }

        /// <summary>
        /// Gets or sets to return agent output text.
        /// </summary>
        public Image BackgroundImage
        {
            get { return backgroundImage; }
            set { backgroundImage = value; }
        }

        /// <summary>
        /// Gets the list of other UI layers under the same canva where the agent output is.
        /// </summary>
        public List<GameObject> GetOtherOutputLayers
        {
            get { return otherOutputLayers; }
        }

        /// <summary>
        /// Add to the list a second UI layer under the canva where the agent output is.
        /// </summary>
        /// <param name="layer"> UI layer that is with the agent output UI.</param>
        public void AddOtherOutputLayers(GameObject layer)
        {
            otherOutputLayers.Add(layer);
        }

        /// <summary>
        /// Starts the agent's thinking output.
        /// </summary>
        public void StartAgentThinking()
        {
            state = 0;
            StartCoroutine(DefaultUIAnimation());
        }

        /// <summary>
        /// Stops the agent's thinking output.
        /// </summary>
        public void StopAgentThinking()
        {
            state = 2;
        }

        /// <summary>
        /// Resets the agent's thinking output.
        /// </summary>
        public void ResetAgentThinking()
        {
            state = 0;
        }

        private void Start()
        {
            if (backgroundImage != null)
            {
                backgroundRectTransform = backgroundImage.GetComponent<RectTransform>();
            }

            if (outputText != null)
            {
                textRectTransform = outputText.GetComponent<RectTransform>();
            }

            outputText.text = defaultWaitingText;
        }

        private void Update()
        {
            if (outputText != null && backgroundRectTransform != null && textRectTransform != null)
            {
                // Get the size of the text content
                Vector2 textSize = outputText.GetRenderedValues(false);

                // Calculate the new size for the background image with padding
                Vector2 newSize = textSize + padding;

                // Apply the new size to the background RectTransform
                backgroundRectTransform.sizeDelta = newSize;

                // Align the bottom-left corner of the background to the bottom-left corner of the text
                // backgroundRectTransform.anchoredPosition = new Vector2(textRectTransform.anchoredPosition.x, textRectTransform.anchoredPosition.y - textSize.y + padding.y);/*textRectTransform.anchoredPosition;*/
            }
        }

        private IEnumerator DefaultUIAnimation()
        {
            while (true)
            {
                uint dotCounter = 0;
                while (outputText.text.StartsWith(defaultWaitingText) || outputText.text.StartsWith(defaultThinkingText))
                {
                    if (dotCounter == 4 || dotCounter == 0)
                    {
                        dotCounter = 0;
                        outputText.text = outputText.text.StartsWith(defaultWaitingText) ? defaultWaitingText : defaultThinkingText;
                        dotCounter++;
                    }
                    else
                    {
                        dotCounter++;
                        outputText.text += ".";
                    }

                    yield return new WaitForSeconds(0.5f);
                }

                if (state == 0)
                {
                    outputText.text = defaultWaitingText;
                }
                else if (state == 1)
                {
                    outputText.text = defaultThinkingText;
                }

                yield return null;
            }
        }
    }
}