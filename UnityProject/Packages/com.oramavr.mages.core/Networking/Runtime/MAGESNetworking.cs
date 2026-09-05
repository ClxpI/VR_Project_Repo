namespace MAGES.Networking
{
    using System.Collections;
    using System.Collections.Generic;
    using MAGES;
    using MAGES.Interaction;
    using MAGES.Interaction.Interactables;
    using MAGES.Utilities;
    using MAGES.Utilities.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// The MAGES networking module.
    /// </summary>
    [CreateAssetMenu(fileName = "MAGESNetworking", menuName = "MAGES/Networking/MAGESNetworking")]
    public class MAGESNetworking : NetworkingModule
    {
        [SerializeField]
        private GameObject messageHandlerPrefab;
        [SerializeField]
        private GameObject networkingInfoUI;

        /// <summary>
        /// Map of gameobject references to prefab paths.
        /// </summary>
        private SerializableDictionary<GameObject, string> pathReferences;

        /// <summary>
        /// Map of gameobject references to prefab paths.
        /// </summary>
        private SerializableDictionary<string, GameObject> pathToGameObject;

        /// <summary>
        /// Map of prefab paths to local prefab IDs.
        /// </summary>
        private SerializableDictionary<string, int> pathToPrefabID;

        private List<int> remoteActivatedObjects;

        /// <summary>
        /// Gets the network prefab responsible for synchronizing the game state and exchanging messages between users.
        /// </summary>
        public GameObject MessageHandlerPrefab => messageHandlerPrefab;

        /// <summary>
        /// Gets the map of gameobject references to prefab paths.
        /// </summary>
        public SerializableDictionary<GameObject, string> PathReferences => pathReferences;

        /// <summary>
        /// Gets the map of gameobject references to prefab paths.
        /// </summary>
        public SerializableDictionary<string, GameObject> PathToGameObject => pathToGameObject;

        /// <summary>
        /// Gets the map of prefab paths to local prefab IDs.
        /// </summary>
        public SerializableDictionary<string, int> PathToPrefabID => pathToPrefabID;

        /// <inheritdoc cref="NetworkingModule.EstablishConnectionToMainServer"/>
        public override bool EstablishConnectionToMainServer(string args)
        {
            pathReferences = GameObjectPathReferences.Instance.GetGameObjectsPaths();
            pathToGameObject = GameObjectPathReferences.Instance.GetPathToGameObjects();
            pathToPrefabID = GameObjectPathReferences.Instance.GetPathToPrefabID();

            if (Integration != null && Integration.EstablishConnectionToMainServer(args))
            {
                IsInitialized = true;
                return true;
            }

            return false;
        }

        /// <inheritdoc cref="NetworkingModule.DisconnectFromMainServer"/>
        public override void DisconnectFromMainServer()
        {
            IsInitialized = false;
            if (Integration == null)
            {
                return;
            }

            Integration.Disconnect();
        }

        /// <inheritdoc cref="NetworkingModule.CreateRoom"/>
        public override bool CreateRoom(string roomName)
        {
            if (Integration == null)
            {
                return false;
            }

            IsHost = true;
            Hub.Instance.StartCoroutine(SpawnSessionUI(roomName));
            return Integration.CreateRoom(roomName);
        }

        /// <inheritdoc cref="NetworkingModule.JoinRoom"/>
        public override bool JoinRoom(string roomName = null)
        {
            return Integration != null && Integration.JoinRoom(roomName);
        }

        /// <inheritdoc cref="NetworkingModule.ExitCurrentRoom"/>
        public override bool ExitCurrentRoom()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="NetworkingModule.GetAvailableRooms"/>
        public override List<string> GetAvailableRooms()
        {
            return Integration?.GetAvailableRooms();
        }

        /// <inheritdoc cref="NetworkingModule.GetAvailableRooms"/>
        public override string GetCurrentConnectedRoom()
        {
            return Integration?.GetCurrentConnectedRoom();
        }

        /// <inheritdoc cref="NetworkingModule.OnDisconnected"/>
        public override void OnDisconnected(string cause)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="NetworkingModule.OnConnected"/>
        public override void OnConnected(string info)
        {
            var interactionModule = Hub.Instance.Get<InteractionSystemModule>();
#if PHOTON_UNITY_NETWORKING
            OnConnectedToRoom.Invoke();
            var integration = (PUNIntegration)Hub.Instance.Get<NetworkingModule>().Integration;
            var leftHandPrefab = interactionModule.LeftHandPrefab;
            var leftHandInstance = interactionModule.LeftHand;
            if (leftHandInstance != null)
            {
                integration.InitNetworkHand(leftHandPrefab, leftHandInstance, true);
            }

            var rightHandPrefab = interactionModule.RightHandPrefab;
            var rightHandInstance = interactionModule.RightHand;
            if (rightHandInstance != null)
            {
                integration.InitNetworkHand(rightHandPrefab, rightHandInstance, true);
            }

            var avatarPrefab = interactionModule.AvatarPrefab;
            var avatarInstance = interactionModule.Avatar;
            integration.InitNetworkAvatar(avatarPrefab, avatarInstance, true);
#endif
        }

        /// <inheritdoc cref="NetworkingModule.OwnershipRequest"/>
        public override bool OwnershipRequest(MAGESObject obj)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc cref="NetworkingModule.LinkNetworkObject"/>
        public override bool LinkNetworkObject(GameObject remotePrefab, GameObject localPrefab)
        {
            return Integration?.LinkNetworkObject(remotePrefab, localPrefab);
        }

        /// <inheritdoc cref="NetworkingModule.NetworkSpawn"/>
        public override GameObject NetworkSpawn(GameObject prefab, Transform prefabParent = null, bool isUnique = true)
        {
            var spawnedObject = Integration?.SpawnObject(prefab, prefabParent, isUnique);
            return spawnedObject;
        }

        /// <inheritdoc cref="NetworkingModule.RemoteDestroyComponent"/>
        public override void RemoteDestroyComponent(GameObject gameObject, string componentType)
        {
            Integration?.DestroyComponent(gameObject, componentType);
        }

        /// <inheritdoc cref="NetworkingModule.RequestChangeState"/>
        public override void RequestChangeState(byte changeState, string actionID)
        {
            Integration?.RequestChangeState(changeState, actionID);
        }

        /// <inheritdoc cref="NetworkingModule.GetConnectedUsersToRoom"/>
        public override int GetConnectedUsersToRoom(string roomName)
        {
            if (Integration == null)
            {
                return -1;
            }

            return Integration.GetConnectedUsersToRoom(roomName);
        }

        /// <inheritdoc cref="NetworkingModule.GetConnectedUsersToCurrentRoom"/>
        public override int GetConnectedUsersToCurrentRoom()
        {
            if (Integration == null)
            {
                return -1;
            }

            return Integration.GetConnectedUsersToCurrentRoom();
        }

        /// <inheritdoc cref="NetworkingModule.AddSyncTransform"/>
        public override void AddSyncTransform(GameObject objectToBeSynchronized)
        {
            Integration?.AddSyncTransform(objectToBeSynchronized);
        }

        /// <inheritdoc cref="NetworkingModule.HasAuthority"/>
        public override bool HasAuthority(GameObject networkObject)
        {
            if (Integration == null || !IsInitialized)
            {
                return false;
            }

            return Integration.HasAuthority(networkObject);
        }

        /// <inheritdoc cref="NetworkingModule.RegisterActivatedObject"/>
        public override void RegisterActivatedObject(GameObject grabbableObject, bool isActivated)
        {
            if (Integration == null)
            {
                return;
            }

            var id = Integration.GetNetworkID(grabbableObject);
            if (id > 0)
            {
                remoteActivatedObjects ??= new List<int>();
                if (isActivated)
                {
                    if (!remoteActivatedObjects.Contains(id))
                    {
                        remoteActivatedObjects.Add(id);
                    }
                }
                else
                {
                    remoteActivatedObjects.Remove(id);
                }
            }
        }

        /// <inheritdoc cref="NetworkingModule.IsRemoteActivated"/>
        public override bool IsRemoteActivated(GameObject grabbableObject)
        {
            if (Integration == null || remoteActivatedObjects == null)
            {
                Debug.LogError("IsRemoteActivated was called but network integration is null.");
                return false;
            }

            var id = Integration.GetNetworkID(grabbableObject);
            if (id < 0)
            {
                Debug.LogError("IsRemoteActivated was called for an non-network object.");
                return false;
            }

            return remoteActivatedObjects.Contains(id);
        }

        /// <inheritdoc cref="NetworkingModule.Startup"/>
        public override void Startup()
        {
#if PHOTON_UNITY_NETWORKING
            Integration = Hub.Instance.gameObject.GetOrAddComponent<PUNIntegration>();
#endif
            Integration?.OnStartup();

            InteractionManager[] interactionManagerArray = FindObjectsOfType<InteractionManager>();
            foreach (InteractionManager interactionManager in interactionManagerArray)
            {
                interactionManager.RegisterInteractableEnter.AddListener((registerEnterInteractionEventArgs) =>
                {
                    registerEnterInteractionEventArgs.Interactable.SelectEntered.AddListener(RequestAuthority);
                });

                interactionManager.DeregisterInteractableEnter.AddListener((registerEnterInteractionEventArgs) =>
                {
                    registerEnterInteractionEventArgs.Interactable.SelectEntered.RemoveListener(RequestAuthority);
                });
            }

            remoteActivatedObjects = new List<int>();
        }

        /// <inheritdoc cref="NetworkingModule.GetPrefabIDFromNetwork"/>
        public override int GetPrefabIDFromNetwork(GameObject prefab, out bool isUnique)
        {
            if (Integration == null)
            {
                isUnique = false;
                return -1;
            }

            return Integration.GetPrefabIDFromNetwork(prefab, out isUnique);
        }

        /// <inheritdoc cref="NetworkingModule.Shutdown"/>
        public override void Shutdown()
        {
            InteractionManager[] interactionManagerArray = FindObjectsOfType<InteractionManager>();
            foreach (InteractionManager interactionManager in interactionManagerArray)
            {
                interactionManager.RegisterInteractableEnter.RemoveAllListeners();
                interactionManager.DeregisterInteractableEnter.RemoveAllListeners();
            }

            Integration?.Disconnect();
        }

        /// <inheritdoc/>
        public override int GetLocalPlayerID()
        {
#if PHOTON_UNITY_NETWORKING
            return Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber;
#else
            return -1;
#endif
        }

        /// <inheritdoc/>
        public override IEnumerable<PlayerDescription> GetConnectedPlayers()
        {
#if PHOTON_UNITY_NETWORKING
            if (!Photon.Pun.PhotonNetwork.InRoom)
            {
                return null;
            }

            var players = Photon.Pun.PhotonNetwork.PlayerList;

            var result = new PlayerDescription[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                result[i] = new PlayerDescription()
                {
                    Identifier = players[i].ActorNumber,
                    Name = players[i].NickName,
                };
            }

            return result;
#else
            return null;
#endif
        }

        /// <inheritdoc/>
        public override void SyncMessage(GameObject netObject, string message)
        {
            if (Integration == null)
            {
                return;
            }

            Integration.SyncMessage(netObject, message);
        }

        /// <inheritdoc/>
        public override void AddMessageRemoteAction(GameObject netObject, UnityAction<string> action)
        {
            if (Integration == null)
            {
                return;
            }

            Integration.AddMessageRemoteAction(netObject, action);
        }

        /// <inheritdoc/>
        public override void NetworkButtonAction(GameObject networkObject, UnityAction action, int index)
        {
            if (Integration == null)
            {
                return;
            }

            Integration.NetworkButtonAction(networkObject, action, index);
        }

        /// <inheritdoc/>
        public override void InvokeButtonRemoteCalls(GameObject networkObject, int index)
        {
            if (Integration == null)
            {
                return;
            }

            Integration.InvokeButtonRemoteCalls(networkObject, index);
        }

        /// <summary>
        /// Raise an event that says that a player has connected, used in integration.
        /// </summary>
        /// <param name="actorNumber">The player's actor number.</param>
        /// <param name="playerName">The player's nickname.</param>
        internal void SendPlayerConnected(int actorNumber, string playerName)
        {
            OnUserConnected.Invoke(actorNumber, playerName);
        }

        /// <summary>
        /// Raise an event that says that a player has disconnected, used in integration.
        /// </summary>
        /// <param name="actorNumber">The player's actor number.</param>
        /// <param name="playerName">The player's nickname.</param>
        internal void SendPlayerDisconnected(int actorNumber, string playerName)
        {
            OnUserDisconnected.Invoke(actorNumber, playerName);
        }

        private void RequestAuthority(SelectEnterInteractionEventArgs interactionEventArgs)
        {
            if (Integration == null)
            {
                return;
            }

            var interactable = (Grabbable)interactionEventArgs.Interactable;
            var networkObject = interactable.gameObject;
            if (RequestAuthorityValidation(networkObject))
            {
                Integration.RequestAuthority(networkObject);
            }
        }

        private bool RequestAuthorityValidation(GameObject grabbable)
        {
            if (Integration == null)
            {
                return false;
            }

            var rb = grabbable.GetComponent<Rigidbody>();
            return Integration.GetNetworkID(grabbable) > 0 && rb != null && IsInitialized;
        }

        private IEnumerator SpawnSessionUI(string roomName)
        {
            while (!IsInitialized || Integration.GetCurrentConnectedRoom() != roomName)
            {
                yield return null;
            }

            GameObjectSpawner.Instance.SpawnObject(networkingInfoUI, isUnique: false);
        }
    }
}
