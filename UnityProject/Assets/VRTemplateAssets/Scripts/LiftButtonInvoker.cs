using UnityEngine;

public class LiftButtonInvoker : MonoBehaviour
{
    public LiftButtonPanel liftPanel;
    public bool moveUp = true;

    public void Trigger()
    {
        if (liftPanel == null) return;

        if (moveUp)
            liftPanel.MoveLiftUp();
        else
            liftPanel.MoveLiftDown();
    }
}
