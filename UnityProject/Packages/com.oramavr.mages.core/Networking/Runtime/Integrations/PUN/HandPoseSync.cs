#if PHOTON_UNITY_NETWORKING

namespace MAGES.Networking
{
    using MAGES.Interaction;
    using MAGES.Interaction.Interactors;
    using MAGES.Networking;
    using Photon.Pun;
    using UnityEngine;

    /// <summary>
    /// Synchronization of hand pose transforms over network.
    /// </summary>
    public class HandPoseSync : SyncTransformPhoton
    {
        private BaseInteractor interactor;

        /// <summary>
        /// Initialises The transform sync of hands.
        /// </summary>
        /// <param name="interactorObject">The interactor that will be synced across network.</param>
        public void Initialize(GameObject interactorObject)
        {
            if (interactorObject != null)
            {
                interactor = interactorObject.GetComponent<BaseInteractor>();
            }

            InitTransforms(interactorObject);
        }

        /// <inheritdoc cref="IPunObservable.OnPhotonSerializeView"/>
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                interactor.AttachInteractorToInteractable();
                SendTransforms(stream);
                interactor.DetachInteractorToInteractable();
            }
            else
            {
                ApplyTransforms(stream);
            }
        }

        private void Start()
        {
            if (!PhotonViewRef.IsMine)
            {
                Initialize(null);
            }
        }

        private void InitTransforms(GameObject interactorObject)
        {
            if (interactorObject == null)
            {
                Initialise();
                return;
            }

            HandPoser handPoser = interactorObject.GetComponentInChildren<HandPoser>();
            if (handPoser != null)
            {
                foreach (FingerPoser fingerPoser in handPoser.FingerPosers)
                {
                    foreach (Transform bone in fingerPoser.Bones)
                    {
                        Transforms.Add(bone);
                    }
                }
            }

            Initialise();
        }
    }
}

#endif