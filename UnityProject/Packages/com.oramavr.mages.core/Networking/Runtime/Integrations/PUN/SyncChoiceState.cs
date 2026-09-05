namespace MAGES.Networking
{
#if PHOTON_UNITY_NETWORKING
    using System.Collections;
    using System.Collections.Generic;
    using MAGES.SceneGraph;
    using Photon.Pun;
    using UnityEngine;

    /// <summary>
    /// Syncs the state of the <see cref="MAGES.SceneGraph.ChoiceActionData">.
    /// </summary>
    public class SyncChoiceState : MonoBehaviourPunCallbacks
    {
        private string actionId;
        private BaseActionData action;

        /// <summary>
        /// Called when the choice is changed via some other client.
        /// </summary>
        /// <param name="choice">The new choice.</param>
        [PunRPC]
        public void SyncChoice(int choice)
        {
            var choiceAction = action as ChoiceActionData;
            choiceAction.CurrentChoice = choice;
        }

        /// <summary>
        /// Initializes the sync state class.
        /// </summary>
        /// <param name="action">The action that this state affects.</param>
        /// <param name="actionObject">The object that will affect the path of the action.</param>
        public void Init(BaseActionData action, GameObject actionObject)
        {
            this.action = action;
            actionObject.GetComponentInChildren<ChoiceBehavior>().OnChoiceChanged.AddListener((choice) =>
            {
                SendChoice(choice);
            });
        }

        private void SendChoice(int choice)
        {
            photonView.RPC("SyncChoice", RpcTarget.Others, choice);
        }
    }

#endif
}