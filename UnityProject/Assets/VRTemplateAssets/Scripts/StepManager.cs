using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to load a new scene

namespace Unity.VRTemplate
{
    /// <summary>
    /// Controls the steps in the in coaching card.
    /// </summary>
    public class StepManager : MonoBehaviour
    {
        [Serializable]
        class Step
        {
            [SerializeField]
            public GameObject stepObject;

            [SerializeField]
            public string buttonText;
        }

        [SerializeField]
        public TextMeshProUGUI m_StepButtonTextField;

        [SerializeField]
        List<Step> m_StepList = new List<Step>();

        int m_CurrentStepIndex = 0;

        public void Next()
        {
            // Deactivate current step
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);

            // Check if this is the last step
            if (m_CurrentStepIndex == m_StepList.Count - 1)
            {
                GoToSecondScenario();
                return;
            }

            // Move to the next step
            m_CurrentStepIndex++;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
            m_StepButtonTextField.text = m_StepList[m_CurrentStepIndex].buttonText;
        }

        void GoToSecondScenario()
        {
            // Load the scene called "QuizScene"
            SceneManager.LoadScene("QuizScene");
        }
    }
}
