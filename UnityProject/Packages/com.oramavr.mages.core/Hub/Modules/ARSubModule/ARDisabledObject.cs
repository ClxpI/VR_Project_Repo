namespace MAGES.AR
{
    using UnityEngine;

    /// <summary>
    /// A class representin an object to be disabled when AR is activated.
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class ARDisabledObject : MonoBehaviour
    {
        private ARSubModule arsubModule;

        private void Start()
        {
            arsubModule = Hub.GetSubModule<ARSubModule>();
            if (arsubModule != null)
            {
                arsubModule.RegisterObjectToRemove(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (arsubModule != null)
            {
                arsubModule.DeregisterObjectToRemove(gameObject);
            }
        }
    }
}