using UnityEngine;

public class LiftButtonPanel : MonoBehaviour
{
    [Header("Lift Parts to Move (Detal_1 → Detal_7)")]
    public Transform[] liftParts;

    [Header("Movement Settings")]
    public float moveStep = 0.05f;

    // ✅ Public method Unity will recognize in OnClick dropdown
    public void MoveLiftUp()
    {
        MoveLift(Vector3.up * moveStep);
    }

    // ✅ Public method Unity will recognize in OnClick dropdown
    public void MoveLiftDown()
    {
        MoveLift(Vector3.down * moveStep);
    }

    // Shared movement logic
    private void MoveLift(Vector3 direction)
    {
        if (liftParts == null || liftParts.Length == 0)
        {
            Debug.LogWarning("Lift parts not assigned!");
            return;
        }

        foreach (Transform part in liftParts)
        {
            if (part != null)
                part.position += direction;
        }
    }
}
