namespace MAGES.Experimental.EmbodimentJARIA.Editor
{
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using MAGES.Editor;
    using MAGES.Utilities.Collections;
    using Newtonsoft.Json;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    /// <summary>
    /// Inspector for the <see cref="AgentConfiguration"/> component.
    /// </summary>
    [MarkExperimental]
    [CustomEditor(typeof(AgentConfiguration))]
    public class AgentInspector : Editor
    {
        [SerializeField]
        private VisualTreeAsset uxml;
        private VisualElement root;
        private VisualElement tempFoldoutElement;
        private Foldout configFoldout;
        private VisualTreeAsset foldoutElementTemplate;
        private ObjectField characterInputPrefab;
        private TextField characterInputScene;
        private ObjectField interactionButtonsInput;
        private ObjectField agentOutputInput;
        private DropdownField embodimentCharacter;
        private TextField agentIdField;
        private TextField textHandlerKey;
        private TextField speechHandlerKey;
        private TextField textspeechHandlersRegion;
        private TextField voiceID;
        private TextField customEndpointID;
        private TextField languageID;

        /// <summary>
        /// Creates the VisualElement for the Inspector of <see cref="AgentConfiguration"/> component.
        /// </summary>
        /// <returns>The inspector's VisualElement.</returns>
        public override VisualElement CreateInspectorGUI()
        {
            // Load custom editor
            root = new VisualElement();
            foldoutElementTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.oramavr.mages.core/JARIA/EmbodimentJARIA/Editor/Agents/FoldoutElement.uxml");

            if (uxml == null)
            {
                uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.oramavr.mages.core/JARIA/EmbodimentJARIA/Editor/Agents/AgentInspector.uxml");
            }

            if (uxml == null)
            {
                InspectorElement.FillDefaultInspector(root, serializedObject, this);
                return root;
            }

            uxml.CloneTree(root);

            // Agent ID
            agentIdField = root.Q<TextField>("AgentID");
            agentIdField.value = (target as AgentConfiguration).AgentID;
            agentIdField.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).AgentID = evt.newValue;
                EditorUtility.SetDirty(target);
            });

            // Agent Embodiment Character
            embodimentCharacter = root.Q<DropdownField>("EmbodimentCharacter");
            characterInputScene = root.Q<TextField>("CharacterInputScene");
            characterInputPrefab = root.Q<ObjectField>("CharacterInputPrefab");
            if (!(target as AgentConfiguration).SceneCharacter)
            {
                embodimentCharacter.index = 1;
                characterInputScene.visible = false;
                characterInputPrefab.visible = true;
                characterInputPrefab.value = (target as AgentConfiguration).EmbodimentCharacterPrefab;
                characterInputPrefab.objectType = typeof(GameObject);
            }

            embodimentCharacter.RegisterValueChangedCallback((evt) =>
            {
                if (evt.newValue == "Scene")
                {
                    (target as AgentConfiguration).SceneCharacter = true;
                    embodimentCharacter.index = 0;
                    characterInputScene.value = (target as AgentConfiguration).EmbodimentCharacterScene;
                    characterInputScene.visible = true;
                    characterInputPrefab.visible = false;
                }

                if (evt.newValue == "Prefab")
                {
                    (target as AgentConfiguration).SceneCharacter = false;
                    embodimentCharacter.index = 1;
                    characterInputScene.visible = false;
                    characterInputPrefab.visible = true;
                    characterInputPrefab.value = (target as AgentConfiguration).EmbodimentCharacterPrefab;
                    characterInputPrefab.objectType = typeof(GameObject);
                }

                EditorUtility.SetDirty(target);
            });

            characterInputScene.value = (target as AgentConfiguration).EmbodimentCharacterScene;
            characterInputScene.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).EmbodimentCharacterScene = evt.newValue;
                EditorUtility.SetDirty(target);
            });

            characterInputPrefab.value = (target as AgentConfiguration).EmbodimentCharacterPrefab;
            characterInputPrefab.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).EmbodimentCharacterPrefab = (GameObject)evt.newValue;
                EditorUtility.SetDirty(target);
            });

            // Agent Components
            interactionButtonsInput = root.Q<ObjectField>("InteractionComponent");
            interactionButtonsInput.value = (target as AgentConfiguration).AgentInteractionComponent;
            interactionButtonsInput.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).AgentInteractionComponent = (GameObject)evt.newValue;
                EditorUtility.SetDirty(target);
            });

            agentOutputInput = root.Q<ObjectField>("AgentOutputComponent");
            agentOutputInput.value = (target as AgentConfiguration).AgentOutputComponent;
            agentOutputInput.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).AgentOutputComponent = (GameObject)evt.newValue;
                EditorUtility.SetDirty(target);
            });

            // STT & TTS Keys and Region
            speechHandlerKey = root.Q<TextField>("SpeechHandlerKey");
            speechHandlerKey.value = (target as AgentConfiguration).SpeechHandlerKey;
            speechHandlerKey.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).SpeechHandlerKey = evt.newValue;
                EditorUtility.SetDirty(target);
            });

            textHandlerKey = root.Q<TextField>("TextHandlerKey");
            textHandlerKey.value = (target as AgentConfiguration).TextHandlerKey;
            textHandlerKey.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).TextHandlerKey = evt.newValue;
                EditorUtility.SetDirty(target);
            });

            textspeechHandlersRegion = root.Q<TextField>("TextSpeechHandlersRegion");
            textspeechHandlersRegion.value = (target as AgentConfiguration).TextSpeechHandlerRegion;
            textspeechHandlersRegion.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).TextSpeechHandlerRegion = evt.newValue;
                EditorUtility.SetDirty(target);
            });

            // Voice ID
            voiceID = root.Q<TextField>("voiceID");
            voiceID.value = (target as AgentConfiguration).VoiceID;
            voiceID.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).VoiceID = evt.newValue;
                EditorUtility.SetDirty(target);
            });

            // Custom Endpoint ID
            customEndpointID = root.Q<TextField>("endpointID");
            customEndpointID.value = (target as AgentConfiguration).CustomEndpointID;
            customEndpointID.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).CustomEndpointID = evt.newValue;
                EditorUtility.SetDirty(target);
            });

            // Custom Endpoint ID
            languageID = root.Q<TextField>("languageCode");
            languageID.value = (target as AgentConfiguration).LanguageID;
            languageID.RegisterValueChangedCallback((evt) =>
            {
                (target as AgentConfiguration).LanguageID = evt.newValue;
                EditorUtility.SetDirty(target);
            });

            // Agent Details
            Button getAgentButton = root.Q<Button>("GetAgentButton");
            SerializableDictionary<string, string> agentDetailsRef = (target as AgentConfiguration).AgentDetails;
            configFoldout = root.Q<Foldout>("Config");

            foreach (string key in agentDetailsRef.Keys)
            {
                tempFoldoutElement = foldoutElementTemplate.Instantiate();
                tempFoldoutElement.Q<TextField>().label = key;
                tempFoldoutElement.Q<TextField>().value = agentDetailsRef[key] != null ? agentDetailsRef[key].ToString() : "Not found";
                tempFoldoutElement.Q<TextField>().SetEnabled(false);
                configFoldout.Add(tempFoldoutElement);
            }

            getAgentButton.clicked += async () =>
            {
                string config = await EmbodiedAIRequest.GetAgentFromID(agentIdField.value, DeveloperAuthentication.Instance);
                if (string.IsNullOrEmpty(config))
                {
                    configFoldout.Clear();
                    Debug.Log("Didn't find an agent with this ID or empty agent details");
                    return;
                }

                SerializableDictionary<string, object> dictRef = JsonConvert.DeserializeObject<SerializableDictionary<string, object>>(config);
                SerializableDictionary<string, string> dictRefString = new SerializableDictionary<string, string>();
                configFoldout.Clear();
                foreach (string key in dictRef.Keys.Where(key => !key.Equals("userTable")))
                {
                    VisualElement tempFoldoutElement = foldoutElementTemplate.Instantiate();
                    tempFoldoutElement.Q<TextField>().label = KeyReformat(key);
                    tempFoldoutElement.Q<TextField>().value = dictRef[key] != null ? dictRef[key].ToString() : "Not found";
                    tempFoldoutElement.Q<TextField>().SetEnabled(false);
                    configFoldout.Add(tempFoldoutElement);
                    dictRefString.Add(key, dictRef[key].ToString());
                }

                (target as AgentConfiguration).AgentDetails = dictRefString;
                EditorUtility.SetDirty(target);
            };

            return root;
        }

        // Key name reformat
        private string KeyReformat(string input)
        {
            StringBuilder res = new StringBuilder();
            bool capital = true;

            if (input.Equals("id"))
            {
                return "ID";
            }

            input = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
            input.Replace("_", " ");

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    capital = true;
                    res.Append(c);
                }
                else if (capital)
                {
                    capital = false;
                    res.Append(char.ToUpper(c));
                }
                else
                {
                    res.Append(char.ToLower(c));
                }
            }

            return res.ToString();
        }
    }
}
