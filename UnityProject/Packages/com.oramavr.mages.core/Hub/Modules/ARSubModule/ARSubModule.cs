namespace MAGES.AR
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.XR.ARFoundation;

    /// <summary>
    /// A SubModule responsible for handling the switch to AR and back.
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class ARSubModule : SubModule
    {
        private HashSet<GameObject> objectsToRemove;
        private GameObject arsessionObject;
        private CameraClearFlags previousCameraFlags;
        private Color previousCameraColor;
        private bool isActive = false;

        /// <summary>
        /// Gets or sets the Event raised when AR is enabled.
        /// </summary>
        public UnityEvent AREnabled { get; set; } = new UnityEvent();

        /// <summary>
        /// Gets or sets the Event raised when AR is disabled.
        /// </summary>
        public UnityEvent ARDisabled { get; set; } = new UnityEvent();

        /// <summary>
        /// Gets a value indicating whether AR is enabled.
        /// </summary>
        public bool IsEnabled => isActive;

        /// <summary>
        /// Register an object to be disabled when AR is enabled.
        /// </summary>
        /// <param name="go">The gameobject to be disabled.</param>
        public void RegisterObjectToRemove(GameObject go)
        {
            objectsToRemove.Add(go);
        }

        /// <summary>
        /// Deregister an object to be disabled when AR is enabled.
        /// </summary>
        /// <param name="go">The gameobject to be disabled.</param>
        public void DeregisterObjectToRemove(GameObject go)
        {
            objectsToRemove.Remove(go);
        }

        /// <summary>
        /// Enables/Disables AR.
        /// </summary>
        /// <param name="enable">If true, AR is enabled, else it is disabled.</param>
        public void SetEnabled(bool enable)
        {
            Camera mainCamera = Hub.Instance.Rig.HeadCamera;
            arsessionObject.SetActive(enable);
            SetObjectsActive(!enable);
            if (enable)
            {
                previousCameraFlags = mainCamera.clearFlags;
                previousCameraColor = mainCamera.backgroundColor;
                mainCamera.clearFlags = CameraClearFlags.SolidColor;
                mainCamera.backgroundColor = new Color(0, 0, 0, 0);
                AREnabled?.Invoke();
                isActive = true;
            }
            else
            {
                mainCamera.clearFlags = previousCameraFlags;
                mainCamera.backgroundColor = previousCameraColor;
                ARDisabled?.Invoke();
                isActive = false;
            }
        }

        /// <summary>
        /// Setup the AR Session GameObject.
        /// </summary>
        /// <param name="go">The AR Session GameObject.</param>
        public void SetARSessionObject(GameObject go)
        {
            arsessionObject = go;
        }

        /// <inheritdoc/>
        public override void Shutdown()
        {
            objectsToRemove.Clear();
        }

        /// <inheritdoc/>
        public override void Startup()
        {
            arsessionObject = new GameObject("ARSession");
            arsessionObject.AddComponent<ARSession>();

            Camera mainCamera = Hub.Instance.Rig.HeadCamera;
            previousCameraFlags = mainCamera.clearFlags;
            previousCameraColor = mainCamera.backgroundColor;

            ARCameraManager arcameraManager = mainCamera.gameObject.AddComponent<ARCameraManager>();
            arcameraManager.requestedBackgroundRenderingMode = CameraBackgroundRenderingMode.AfterOpaques;
            mainCamera.gameObject.AddComponent<ARCameraBackground>();

            objectsToRemove = new HashSet<GameObject>();

            arsessionObject.SetActive(false);
        }

        /// <summary>
        /// Enables/Disables the gameobjects whether AR is active or not.
        /// </summary>
        /// <param name="enable">If true the gameobjects will be enabled, else they will be disabled.</param>
        private void SetObjectsActive(bool enable)
        {
            foreach (GameObject go in objectsToRemove)
            {
                go.SetActive(enable);
            }
        }
    }
}