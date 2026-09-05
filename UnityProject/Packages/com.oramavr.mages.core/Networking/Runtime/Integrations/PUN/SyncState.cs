#if PHOTON_UNITY_NETWORKING
namespace MAGES.Networking
{
    using Photon.Pun;
    using System.Diagnostics;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    /// <summary>
    /// Synchronizes the state of the MAGES scenegraph.
    /// </summary>
    public class SyncState : MonoBehaviourPunCallbacks
    {
        private Hashtable customRoomProperties;
        private bool isUndoActive = false;

        private void Awake()
        {
            customRoomProperties = new Hashtable();
            Hub.Instance.Get<SceneGraphModule>().ForEachAction(AddActionToHashTable);
            Hub.Instance.Get<SceneGraphModule>().ActionInitialized(UpdateCustomPropertiesOnInit);
            Hub.Instance.Get<SceneGraphModule>().ActionPerformed(UpdateCustomPropertiesOnPerform);
            Hub.Instance.Get<SceneGraphModule>().ActionUndone(UpdateCustomPropertiesOnUndo);

            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
        }

        private void UpdateCustomPropertiesOnInit(BaseActionData data)
        {
            if (!data.IsReplicated)
            {
                return;
            }

            if (isUndoActive)
            {
                isUndoActive = false;
                return;
            }

            Hashtable changes = new Hashtable { { data.ID, 1 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(changes);
        }

        private void UpdateCustomPropertiesOnPerform(BaseActionData data, bool test)
        {
            if (!data.IsReplicated)
            {
                return;
            }

            Hashtable changes = new Hashtable { { data.ID, 2 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(changes);
        }

        private void UpdateCustomPropertiesOnUndo(BaseActionData data)
        {
            if (!data.IsReplicated)
            {
                return;
            }

            if (isUndoActive)
            {
                return;
            }

            isUndoActive = true;
            Hashtable changes = new Hashtable { { data.ID, 0 } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(changes);
        }

        private void AddActionToHashTable(BaseActionData actionData)
        {
            if (customRoomProperties.ContainsKey(actionData.ID))
            {
                return;
            }

            customRoomProperties.Add(actionData.ID, (int)actionData.State);
        }
    }
}
#endif