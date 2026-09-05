namespace MAGES.Drivers.XR
{
    using UnityEngine;

    /// <summary>
    /// Offset for controllers for a specific loader.
    /// </summary>
    [System.Serializable]
    public struct XRLoaderOffset
    {
        /// <summary>
        /// The loader this offset is for.
        /// </summary>
        public string Loader;

        /// <summary>
        /// The camera height offset.
        /// </summary>
        public Pose HeadOffset;

        /// <summary>
        /// The left controller's offset.
        /// </summary>
        public Pose LeftControllerOffset;

        /// <summary>
        /// The right controller's offset.
        /// </summary>
        public Pose RightControllerOffset;

        private static readonly XRLoaderOffset IdentityOffset = new XRLoaderOffset { Loader = string.Empty, HeadOffset = Pose.identity, LeftControllerOffset = Pose.identity, RightControllerOffset = Pose.identity };

        /// <summary>
        /// Gets the zero position, and identity rotation offset.
        /// </summary>
        public static XRLoaderOffset Identity => IdentityOffset;

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{Loader}] - Head: {HeadOffset}, Left: {LeftControllerOffset}, Right: {RightControllerOffset}";
        }
    }
}
