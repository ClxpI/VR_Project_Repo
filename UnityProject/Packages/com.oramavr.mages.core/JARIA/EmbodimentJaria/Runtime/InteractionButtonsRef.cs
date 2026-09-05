namespace MAGES.Experimental.EmbodimentJARIA
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Script to get start and cancel buttons from interaction UI.
    /// </summary>
    [MarkExperimental]
    public class InteractionButtonsRef : MonoBehaviour
    {
        [SerializeField]
        private Button startButton;

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private TMP_Text infoText;

        /// <summary>
        /// Gets the start button reference.
        /// </summary>
        public Button GetStartButton
        {
            get { return startButton; }
        }

        /// <summary>
        /// Gets the cancel button reference.
        /// </summary>
        public Button GetCancelButton
        {
            get { return cancelButton; }
        }

        private void Start()
        {
            var uam = Hub.Instance.Get<UserAuthenticationModule>();

            if (uam != null)
            {
                if (uam.HasActiveLicense)
                {
                    infoText.gameObject.SetActive(false);
                    startButton.interactable = true;
                    cancelButton.interactable = true;
                }
                else
                {
                    infoText.gameObject.SetActive(true);
                    startButton.interactable = false;
                    cancelButton.interactable = false;
                }
            }
        }
    }
}