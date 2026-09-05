using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRFireExtinguisher : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public ParticleSystem foamSpray;
    public float extinguishRange = 3f;
    public LayerMask fireLayer;

    private bool isHeld = false;

    void OnEnable()
    {
        grabInteractable.activated.AddListener(OnTriggerPressed);
        grabInteractable.deactivated.AddListener(OnTriggerReleased);
        grabInteractable.selectEntered.AddListener(_ => isHeld = true);
        grabInteractable.selectExited.AddListener(_ => {
            isHeld = false;
            foamSpray.Stop();
        });
    }

    void OnDisable()
    {
        grabInteractable.activated.RemoveAllListeners();
        grabInteractable.deactivated.RemoveAllListeners();
    }

    void OnTriggerPressed(ActivateEventArgs args)
    {
        if (!foamSpray.isPlaying)
            foamSpray.Play();
    }

    void OnTriggerReleased(DeactivateEventArgs args)
    {
        if (foamSpray.isPlaying)
            foamSpray.Stop();
    }

    void Update()
    {
        if (isHeld && foamSpray.isPlaying)
        {
            Ray ray = new Ray(foamSpray.transform.position, foamSpray.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, extinguishRange, fireLayer))
            {
                ParticleSystem fire = hit.collider.GetComponent<ParticleSystem>();
                if (fire != null && fire.isPlaying)
                {
                    fire.Stop();
                }
            }
        }
    }
}
