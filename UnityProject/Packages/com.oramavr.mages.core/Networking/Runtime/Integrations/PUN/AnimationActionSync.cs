#if PHOTON_UNITY_NETWORKING

using MAGES.SceneGraph;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// Additional Script that is attached onto the AnimationBehavior to sync the animation between the users.
/// </summary>
public class AnimationActionSync : MonoBehaviour
{
    private AnimationBehavior animationBehavior;
    private PhotonView photonView;
    private float prevNormalizedDistance;

    /// <summary>
    /// Syncs the question scramble between the users.
    /// </summary>
    /// <param name="newNormalizedDiastance">The time that will play the animation.</param>
    [PunRPC]
    public void UpdateAnimation(float newNormalizedDiastance)
    {
        animationBehavior.NormalizedDistance = newNormalizedDiastance;
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        animationBehavior = GetComponent<AnimationBehavior>();
    }

    private void Update()
    {
        // Adds a threshold to the animation sync.
        if (Mathf.Abs(animationBehavior.NormalizedDistance - prevNormalizedDistance) > 0.005f)
        {
            photonView.RPC("UpdateAnimation", RpcTarget.Others, animationBehavior.NormalizedDistance);
            prevNormalizedDistance = animationBehavior.NormalizedDistance;
        }
    }
}

#endif