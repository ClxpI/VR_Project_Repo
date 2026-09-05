#if PHOTON_UNITY_NETWORKING
namespace MAGES.Networking
{
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Class that sync a message across clients.
    /// </summary>
    public class SyncMessage : MonoBehaviour
    {
        private UnityEvent<string> messageReceivedEvent = new UnityEvent<string>();

        /// <summary>
        /// Attach a unity action which syncs a message across users.
        /// </summary>
        /// <param name="action">An action to subscribe on message recieved event, invoke.</param>
        public void AddMessageRemoteAction(UnityAction<string> action)
        {
            messageReceivedEvent.AddListener(action);
        }

        /// <summary>
        /// Called invoke and sync the message.
        /// </summary>
        /// <param name="message">The message to be synced.</param>
        public void InvokeMessageReceivedRemoteCalls(string message)
        {
            if (!Hub.Instance.Get<NetworkingModule>().IsInitialized)
            {
                return;
            }

            GetComponent<PhotonView>().RPC("SendMessagePRC", RpcTarget.Others, message);
        }

        [PunRPC]
        private void SendMessagePRC(string message)
        {
            messageReceivedEvent.Invoke(message);
        }
    }
}
#endif