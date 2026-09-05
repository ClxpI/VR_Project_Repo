namespace MAGES.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Tests the hard prefab references.
    /// </summary>
    public class TestHardAssetReferences
    {
        /// <summary>
        /// Tests that the hard prefab references are static fields.
        /// </summary>
        [Test]
        public void TestHardAssetReferencesAreStaticFields()
        {
            var fields = HardAssetReferenceAttribute.Query(null);

            foreach (var f in fields)
            {
                Assert.True(f.IsStatic, $"The hard asset reference '{f.DeclaringType.Name}.{f.Name}' is not a static field.");
            }
        }

        /// <summary>
        /// Tests that the runtime hard prefab references are valid.
        /// </summary>
        [Test]
        public void TestHardAssetReferencesRuntimeValid()
        {
            var fields = HardAssetReferenceAttribute.Query(HardAssetReferenceAttribute.Type.Resource);

            foreach (var f in fields)
            {
                if (!f.IsStatic)
                {
                    continue;
                }

                var value = f.GetValue(null) as string;
                Assert.NotNull(value, $"The hard asset reference '{f.DeclaringType.Name}.{f.Name}' is null.");
                Assert.NotNull(Resources.Load(value), $"The hard asset reference '{f.DeclaringType.Name}.{f.Name}' is not valid.");
            }
        }

        /// <summary>
        /// Tests that the edit-time hard prefab references are valid.
        /// </summary>
        [Test]
        public void TestHardAssetReferencesEditorValid()
        {
            var fields = HardAssetReferenceAttribute.Query(HardAssetReferenceAttribute.Type.Asset);

            foreach (var f in fields)
            {
                if (!f.IsStatic)
                {
                    continue;
                }

                var value = f.GetValue(null) as string;
                Assert.NotNull(value, $"The hard asset reference '{f.DeclaringType.Name}.{f.Name}' is null.");
                Assert.IsTrue(AssetDatabase.LoadAllAssetsAtPath(value).Any(), $"The hard asset reference '{f.DeclaringType.Name}.{f.Name}' is not valid.");
            }
        }
    }
}
