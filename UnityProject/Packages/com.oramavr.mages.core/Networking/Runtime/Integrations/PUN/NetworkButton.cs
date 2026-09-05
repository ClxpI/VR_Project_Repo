#if PHOTON_UNITY_NETWORKING
namespace MAGES.Networking
{
    using System;
    using System.Collections.Generic;
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// Network Controller for interaction buttons.
    /// </summary>
    public class NetworkButton : MonoBehaviour
    {
        // private bool remotePressed;
        [SerializeField]
        private List<ButtonInfo> networkButtons = new List<ButtonInfo>();

        /// <summary>
        /// Sets for a specific button the apropriate network action on click.
        /// </summary>
        /// <param name="index">Button's index in list.</param>
        /// <param name="unityAction">Button's action.</param>
        public void AddNetworkAction(int index, UnityAction unityAction)
        {
            try
            {
                networkButtons[index].RemoteCalls.Add(unityAction);
            }
            catch (NullReferenceException)
            {
                throw new Exception("Could not find a button with this index in list");
            }
        }

        /// <summary>
        /// Called to invoke button's actions.
        /// </summary>
        /// <param name="index">Button's index in list.</param>
        /// <returns>The client number who called the invoke.</returns>
        public int InvokeButtonRemoteCalls(int index)
        {
            if (!Hub.Instance.Get<NetworkingModule>().IsInitialized)
            {
                return -1;
            }

            GetComponent<PhotonView>().RPC("ButtonPress", RpcTarget.Others, index);
            return PhotonNetwork.LocalPlayer.ActorNumber;
        }

        [PunRPC]
        private void ButtonPress(int index)
        {
            foreach (UnityAction a in networkButtons[index].RemoteCalls)
            {
                a.Invoke();
            }
        }

        /// <summary>
        /// Class to house button name and reference.
        /// </summary>
        [System.Serializable]
        public class ButtonInfo
        {
            [SerializeField]
            private string buttonName;
            [SerializeField]
            private Button buttonRefernce;
            private List<UnityAction> remoteCalls = new ();

            /// <summary>
            /// Gets the button name.
            /// </summary>
            public string ButtonName
            {
                get { return buttonName; }
            }

            /// <summary>
            /// Gets the button refernce.
            /// </summary>
            public Button ButtonRefernce
            {
                get { return buttonRefernce; }
            }

            /// <summary>
            /// Gets the list actions of this button.
            /// </summary>
            public List<UnityAction> RemoteCalls
            {
                get { return remoteCalls; }
            }
        }
    }
}
#endif