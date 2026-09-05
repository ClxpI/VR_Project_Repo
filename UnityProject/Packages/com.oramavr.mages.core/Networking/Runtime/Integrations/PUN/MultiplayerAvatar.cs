namespace MAGES.Networking
{
#if PHOTON_UNITY_NETWORKING
    using System;
    using MAGES.Interaction;
    using MAGES.Utilities;
    using Photon.Pun;
#endif
    using UnityEngine;

    /// <summary>
    /// Synchronizes the avatar of a player in multiplayer.
    /// </summary>
    public class MultiplayerAvatar : MonoBehaviour
    {
#if PHOTON_UNITY_NETWORKING
        private PhotonView photonView;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            if (photonView == null)
            {
                Debug.LogError("MultiplayerAvatar requires a photon view script.");
                return;
            }

            byte gender = 0;
            if (photonView.IsMine)
            {
                var avatar = new AvatarData();
                avatar.Load();
                gender = Convert.ToByte(avatar.BodyType);
                var localPlayerCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;
                localPlayerCustomProperties["Gender"] = gender;
                PhotonNetwork.LocalPlayer.SetCustomProperties(localPlayerCustomProperties);
                ApplyGender(gender);
            }
            else
            {
                var remoteGender = (byte)photonView.Owner.CustomProperties["Gender"];
                ApplyGender(remoteGender);
                gameObject.SetLayerRecursively(0);
            }
        }

        private void ApplyGender(byte remoteGender)
        {
            var avatarOffset = transform.GetChild(0);
            for (var i = 0; i < avatarOffset.childCount; i++)
            {
                if (i != remoteGender)
                {
                    Destroy(avatarOffset.GetChild(i).gameObject);
                }
            }
        }
#endif
    }
}
