namespace MAGES.Tests
{
    using System.Collections.Generic;
    using System.Reflection;
    using MAGES.Analytics;
    using MAGES.DataContainer;
    using MAGES.SceneGraph;
    using NUnit.Framework;
    using UnityEngine;

    /// <summary>
    /// Class for testing the analytics errors.
    /// </summary>
    public class TestErrors
    {
        /// <summary>
        /// Test for the Collision error.
        /// </summary>
        [Test]
        public void TestCollisionError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Inject action records to data container.
            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            // Inject collision records to data container.
            dataContainer.StoreData(
                $"mages.analytics.collisions.enter.Scalpel.Cauterizer",
                new CollisionRecord { Name = "Scalpel", OtherName = "Cauterizer", ActionNames = new List<string>() { "Action1" }, Timestamp = 2.0f });
            dataContainer.StoreData(
                    $"mages.analytics.collisions.enter.Scalpel.Leg",
                    new CollisionRecord { Name = "Scalpel", OtherName = "Leg", ActionNames = new List<string>() { "Action1" }, Timestamp = 4.0f });
            dataContainer.StoreData(
                    $"mages.analytics.collisions.exit.Cauterizer.Leg",
                    new CollisionRecord { Name = "Cauterizer", OtherName = "Leg", ActionNames = new List<string>() { "Action2" }, Timestamp = 10.0f });

            // Test case where the collision error should trigger.
            {
                // Create error specification of type Collision between Scalpel and Cauterizer.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel.Cauterizer" },
                    BaseData = new AnalyticsTypes.ErrorCollider("CollisionErrorScalpCaut", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = "Cauterizer",
                    ProcessorArguments = new List<object> { "Action1" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action1(1).CollisionErrorScalpCaut(1).Penalty(%)");
                string name = (string)dataContainer.GetData("mages.analytics.errors.Action1(1).CollisionErrorScalpCaut(1).Action");
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50, penalty); // NOTE: The penalty is 50 since thats the default value and we do not change it.
                Assert.AreEqual("Action1", name);
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(25, final_score);
            }

            // Test case where the collision error should not trigger.
            {
                // Create error specification of type Collision between Scalpel and Cauterizer.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action2", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel.Cauterizer" },
                    BaseData = new AnalyticsTypes.ErrorCollider("CollisionErrorScalpCaut", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = "Cauterizer",
                    ProcessorArguments = new List<object> { "Action2" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(50, final_score);
            }

            // Test case where the collision error should not trigger. (No collision entries exist)
            {
                // Create error specification of type Collision between Scalpel and Cauterizer.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action2", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel2.Cauterizer2" },
                    BaseData = new AnalyticsTypes.ErrorCollider("CollisionErrorScalpCaut2", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = "Cauterizer2",
                    ProcessorArguments = new List<object> { "Action2" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(50, final_score);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Time Limit error.
        /// </summary>
        [Test]
        public void TestTimeLimitError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Inject action records to data container.
            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            // Test case where the time limit error triggers with upper bound
            {
                // Create error specification of type Collision between Scalpel and Cauterizer.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    BaseData = new AnalyticsTypes.TimeLimit("TimeLimitError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = 10f,
                    ProcessorArguments = new() { "Action1", "Upper bound" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action1(1).TimeLimitError(1).Penalty(%)");
                string name = (string)dataContainer.GetData("mages.analytics.errors.Action1(1).TimeLimitError(1).Action");
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50, penalty); // NOTE: The penalty is 50 since thats the default value and we do not change it.
                Assert.AreEqual("Action1", name);
                Assert.AreEqual(50f, init_score);
                Assert.AreEqual(25f, final_score);
            }

            // Test case where the time limit error dows not trigger with upper bound
            {
                // Create error specification of type Collision between Scalpel and Cauterizer.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    BaseData = new AnalyticsTypes.TimeLimit("TimeLimitError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = 15f,
                    ProcessorArguments = new() { "Action1", "Upper bound" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50f, init_score);
                Assert.AreEqual(25f, final_score); // NOTE: The final score is 25 since the time limit error triggered in the test case above.
            }

            // Test case where the time limit error triggers with lower bound
            {
                // Create error specification of type Collision between Scalpel and Cauterizer.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    BaseData = new AnalyticsTypes.TimeLimit("TimeLimitError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = 15f,
                    ProcessorArguments = new() { "Action1", "Lower bound" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action1(1).TimeLimitError(1).Penalty(%)");
                string name = (string)dataContainer.GetData("mages.analytics.errors.Action1(1).TimeLimitError(1).Action");
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50, penalty); // NOTE: The penalty is 50 since thats the default value and we do not change it.
                Assert.AreEqual("Action1", name);
                Assert.AreEqual(50f, init_score);
                Assert.AreEqual(25f, final_score);
            }

            // Test case where the time limit error does not trigger with lower bound
            {
                // Create error specification of type Collision between Scalpel and Cauterizer.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    BaseData = new AnalyticsTypes.TimeLimit("TimeLimitError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = 8f,
                    ProcessorArguments = new() { "Action1", "Lower bound" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50f, init_score);
                Assert.AreEqual(25f, final_score); // NOTE: The final score is 25 since the time limit error triggered in the test case above.
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Rotation Limit error.
        /// </summary>
        [Test]
        public void TestRotationLimitError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Create rotation restriction error.
            AnalyticsTypes.RotationRestriction rotationRestriction = new AnalyticsTypes.RotationRestriction("RotationError", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
            {
                TargetPrefab = new GameObject("Target"),
                MaxRotation = new Vector3(0, 90, 0),
                Name = "RotationError",
                Penalty = 20,
                Info = "Exceeded max rotation values.",
                TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
            };

            // Test case where the rotation limit error triggers.
            {
                // Set the rotation of the target prefab to a value that will trigger the error.
                rotationRestriction.TargetPrefab.transform.rotation = Quaternion.Euler(0, 100, 0);

                // Store the transform data of the target prefab.
                TransformRecord transformData = new TransformRecord
                {
                    Name = "Target(Clone)",
                    Position = rotationRestriction.TargetPrefab.transform.position,
                    Rotation = rotationRestriction.TargetPrefab.transform.rotation,
                    ActionNames = new List<string> { "Action1" },
                    Timestamp = 0.0f,
                    DistanceCovered = 0.0f,
                    DistanceToCompare = 0.0f,
                };

                dataContainer.StoreData($"mages.analytics.transforms." + $"{rotationRestriction.TargetPrefab.gameObject.name}(Clone)", transformData);

                var entry = new MAGESAnalyticsEntry("Action1", 100);

                // Create error specification of type RotationRestriction.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new() { $"{rotationRestriction.TargetPrefab.gameObject.name}(Clone)" },
                    BaseData = rotationRestriction,
                    ProcessorTarget = rotationRestriction.MaxRotation,
                    ProcessorArguments = new() { "Action1" },
                    Processor = DefaultProcessors.ProcessorRotationLimit,
                    Iterator = DefaultIterators.EventIterator,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action1(1).RotationError(1).Penalty(%)");
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(penalty, 20);
                Assert.AreEqual(init_score, 100);
                Assert.AreEqual(final_score, 80);
            }

            // Test case where the rotation limit error does not trigger.
            {
                // Set the rotation of the target prefab to a value that will trigger the error.
                rotationRestriction.TargetPrefab.transform.rotation = Quaternion.Euler(0, 80, 0);

                // Store the transform data of the target prefab.
                TransformRecord transformData = new TransformRecord
                {
                    Name = "Target(Clone)",
                    Position = rotationRestriction.TargetPrefab.transform.position,
                    Rotation = rotationRestriction.TargetPrefab.transform.rotation,
                    ActionNames = new List<string> { "Action1" },
                    Timestamp = 0.0f,
                    DistanceCovered = 0.0f,
                    DistanceToCompare = 0.0f,
                };

                dataContainer.StoreData($"mages.analytics.transforms." + $"{rotationRestriction.TargetPrefab.gameObject.name}(Clone)", transformData);

                var entry = new MAGESAnalyticsEntry("Action1", 100);

                // Create error specification of type RotationRestriction.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new() { $"{rotationRestriction.TargetPrefab.gameObject.name}(Clone)" },
                    BaseData = rotationRestriction,
                    ProcessorTarget = rotationRestriction.MaxRotation,
                    ProcessorArguments = new() { "Action1" },
                    Processor = DefaultProcessors.ProcessorRotationLimit,
                    Iterator = DefaultIterators.EventIterator,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(init_score, 100);
                Assert.AreEqual(final_score, 80); // NOTE: The final score is 80 since the error triggered in the test case above.
            }

            // Test case where the rotation limit error does not trigger. (No entry exists in data container)
            {
                // Create rotation restriction error.
                AnalyticsTypes.RotationRestriction rotationRestriction2 = new AnalyticsTypes.RotationRestriction("RotationError", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
                {
                    TargetPrefab = new GameObject("NonExistentGameObject"),
                    MaxRotation = new Vector3(0, 90, 0),
                    Name = "RotationError",
                    Penalty = 20,
                    Info = "Exceeded max rotation values.",
                    TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
                };

                var entry = new MAGESAnalyticsEntry("Action1", 100);

                // Create error specification of type RotationRestriction.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new() { $"NonExistentGameObject(Clone)" },
                    BaseData = rotationRestriction2,
                    ProcessorTarget = rotationRestriction2.MaxRotation,
                    ProcessorArguments = new() { "Action1" },
                    Processor = DefaultProcessors.ProcessorRotationLimit,
                    Iterator = DefaultIterators.EventIterator,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(init_score, 100);
                Assert.AreEqual(final_score, 80); // NOTE: The final score is 80 since the error triggered in the test case above.
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Range Of Motion Limit error.
        /// </summary>
        [Test]
        public void TestRangeOfMotionLimitError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Create range of motion error.
            AnalyticsTypes.RangeOfMotionLimit rangeOfMotionLimit = new AnalyticsTypes.RangeOfMotionLimit("ROML", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
            {
                TargetPrefab = new GameObject("Target2"),
                MaxDistance = 5.0f,
                Name = "ROML",
                Penalty = 20,
                Info = "Moved object more than the allowed distance.",
                TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
            };

            // Test case where the range of motion limit error triggers.
            {
                // Set the position of the target prefab to a value that will trigger the error.
                TransformRecord transformData = new TransformRecord
                {
                    Name = "Target2(Clone)",
                    Position = rangeOfMotionLimit.TargetPrefab.transform.position,
                    Rotation = rangeOfMotionLimit.TargetPrefab.transform.rotation,
                    ActionNames = new List<string> { "Action2" },
                    Timestamp = 0.0f,
                    DistanceCovered = 10.0f,
                    DistanceToCompare = 0.0f,
                };

                dataContainer.StoreData($"mages.analytics.transforms." + $"{rangeOfMotionLimit.TargetPrefab.gameObject.name}(Clone)", transformData);

                var entry = new MAGESAnalyticsEntry("Action2", 100);

                // Create error specification of type RangeOfMotionLimit.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new() { $"{rangeOfMotionLimit.TargetPrefab.gameObject.name}(Clone)" },
                    BaseData = rangeOfMotionLimit,
                    ProcessorTarget = rangeOfMotionLimit.MaxDistance,
                    ProcessorArguments = new() { "Action2" },
                    Processor = DefaultProcessors.ProcessorRangeOfMotion,
                    Iterator = DefaultIterators.EventIterator,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action2(1).ROML(1).Penalty(%)");
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(penalty, 20);
                Assert.AreEqual(init_score, 100);
                Assert.AreEqual(final_score, 80);
            }

            // Test case where the range of motion limit error does not trigger.
            {
                // Set the position of the target prefab to a value that will trigger the error.
                TransformRecord transformData = new TransformRecord
                {
                    Name = "Target2(Clone)",
                    Position = rangeOfMotionLimit.TargetPrefab.transform.position,
                    Rotation = rangeOfMotionLimit.TargetPrefab.transform.rotation,
                    ActionNames = new List<string> { "Action2" },
                    Timestamp = 0.0f,
                    DistanceCovered = 2.0f,
                    DistanceToCompare = 0.0f,
                };

                dataContainer.StoreData($"mages.analytics.transforms." + $"{rangeOfMotionLimit.TargetPrefab.gameObject.name}(Clone)", transformData);

                var entry = new MAGESAnalyticsEntry("Action2", 100);

                // Create error specification of type RangeOfMotionLimit.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new() { $"{rangeOfMotionLimit.TargetPrefab.gameObject.name}(Clone)" },
                    BaseData = rangeOfMotionLimit,
                    ProcessorTarget = rangeOfMotionLimit.MaxDistance,
                    ProcessorArguments = new() { "Action2" },
                    Processor = DefaultProcessors.ProcessorRangeOfMotion,
                    Iterator = DefaultIterators.EventIterator,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(init_score, 100);
                Assert.AreEqual(final_score, 80); // NOTE: The final score is 80 since the error triggered in the test case above.
            }

            // Test case where the range of motion limit error does not trigger. (No entry exists in the data container)
            {
                // Create range of motion error.
                AnalyticsTypes.RangeOfMotionLimit rangeOfMotionLimit2 = new AnalyticsTypes.RangeOfMotionLimit("ROML", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
                {
                    TargetPrefab = new GameObject("NonExistentTarget2"),
                    MaxDistance = 5.0f,
                    Name = "ROML",
                    Penalty = 20,
                    Info = "Moved object more than the allowed distance.",
                    TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
                };

                var entry = new MAGESAnalyticsEntry("Action2", 100);

                // Create error specification of type RangeOfMotionLimit.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new() { $"{rangeOfMotionLimit2.TargetPrefab.gameObject.name}(Clone)" },
                    BaseData = rangeOfMotionLimit2,
                    ProcessorTarget = rangeOfMotionLimit2.MaxDistance,
                    ProcessorArguments = new() { "Action2" },
                    Processor = DefaultProcessors.ProcessorRangeOfMotion,
                    Iterator = DefaultIterators.EventIterator,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(init_score, 100);
                Assert.AreEqual(final_score, 80); // NOTE: The final score is 80 since the error triggered in the test case above.
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Interaction error.
        /// </summary>
        [Test]
        public void TestInteractionError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            AnalyticsTypes.InteractionRestriction interactionRestriction = new AnalyticsTypes.InteractionRestriction("InteractionError", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
            {
                Interactable = new GameObject("Interactable"),
                Interactor = new GameObject("Interactor"),
                Penalty = 50,
                Info = "Interaction error occurred.",
                TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
            };

            // Test case where the interaction error triggers.
            {
                // Create and set up the position data object.
                InteractionRecord record = new()
                {
                    Interactor = interactionRestriction.Interactor.name,
                    Interactable = interactionRestriction.Interactable.name,
                    Timestamp = 0.0f,
                    ActionNames = new() { "Action1" },
                };

                // Store the data to the data container.
                Hub.Instance.Get<DataContainerModule>().StoreData($"mages.analytics.interactions.{record.Interactable}.{record.Interactor}", record);

                // Create error specification of type Interaction.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new() { $"{record.Interactable}.{record.Interactor}" },
                    BaseData = interactionRestriction,
                    ProcessorTarget = record.Interactor,
                    ProcessorArguments = new() { "Action1" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action1(1).InteractionError(1).Penalty(%)");
                string name = (string)dataContainer.GetData("mages.analytics.errors.Action1(1).InteractionError(1).Action");
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50, penalty);
                Assert.AreEqual("Action1", name);
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(25, final_score);
            }

            // Test case where the interaction error does not trigger.
            {
                // Create and set up the position data object.
                InteractionRecord record = new()
                {
                    Interactor = interactionRestriction.Interactor.name,
                    Interactable = interactionRestriction.Interactable.name,
                    Timestamp = 0.0f,
                    ActionNames = new() { "Action1" },
                };

                // Store the data to the data container.
                Hub.Instance.Get<DataContainerModule>().StoreData($"mages.analytics.interactions.{record.Interactable}.{record.Interactor}", record);

                // Create error specification of type Interaction.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action2", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new() { $"{record.Interactable}.{record.Interactor}" },
                    BaseData = interactionRestriction,
                    ProcessorTarget = record.Interactor,
                    ProcessorArguments = new() { "Action2" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(50, final_score); // NOTE: Here the error did not trigger since interaction is registered with Action1.
            }

            // Test case where the interaction error does not trigger. (No entry exists in the data container)
            {
                AnalyticsTypes.InteractionRestriction interactionRestriction2 = new AnalyticsTypes.InteractionRestriction("InteractionError", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
                {
                    Interactable = new GameObject("NonExistentInteractable"),
                    Interactor = new GameObject("NonExistentInteractor"),
                    Penalty = 50,
                    Info = "Interaction error occurred.",
                    TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
                };

                // Create error specification of type Interaction.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action2", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new() { $"NonExistentInteractable.NonExistentInteractor" },
                    BaseData = interactionRestriction2,
                    ProcessorTarget = "NonExistentInteractor",
                    ProcessorArguments = new() { "Action2" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(50, final_score); // NOTE: Here the error did not trigger since interaction is registered with Action1.
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Performed Wrong Action error.
        /// </summary>
        [Test]
        public void TestPerformedWrongActionError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f, ActiveActions = new List<string> { } });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f, ActiveActions = new List<string> { "Action1", "Action3" } });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f, ActiveActions = new List<string> { "Action1" } });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f, ActiveActions = new List<string> { } });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f, ActiveActions = new List<string> { "Action4" } });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f, ActiveActions = new List<string> { } });

            // Test case where the performed wrong action error triggers.
            {
                // Create error specification of type PerformedWrongAction.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action2-ID" },
                    BaseData = new AnalyticsTypes.PerformedWrongAction("PerformedWrongActionError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = "Action1",
                    ProcessorArguments = new() { "Action2" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorPerformedWrongAction,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action1(1).PerformedWrongActionError(1).Penalty(%)");
                string name = (string)dataContainer.GetData("mages.analytics.errors.Action1(1).PerformedWrongActionError(1).Action");
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50, penalty);
                Assert.AreEqual("Action1", name);
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(25, final_score);
            }

            // Test case where the performed wrong action error does not trigger.
            {
                // Create error specification of type PerformedWrongAction.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action2", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action3-ID" },
                    BaseData = new AnalyticsTypes.PerformedWrongAction("PerformedWrongActionError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = "Action2",
                    ProcessorArguments = new() { "Action3" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorPerformedWrongAction,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(50, final_score);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Did Not Perform Required Action error.
        /// </summary>
        [Test]
        public void TestDidNotPerformRequiredActionError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f, ActiveActions = new List<string> { } });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f, ActiveActions = new List<string> { "Action1", "Action3" } });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f, ActiveActions = new List<string> { "Action1" } });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f, ActiveActions = new List<string> { } });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f, ActiveActions = new List<string> { "Action4" } });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f, ActiveActions = new List<string> { } });

            // Test case where the did not perform required action error triggers.
            {
                // Create error specification of type DidNotPerformRequiredAction.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action2", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    BaseData = new AnalyticsTypes.DidNotPerformRequiredAction("DidNotPerformRequiredActionError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = "Action2",
                    ProcessorArguments = new() { "Action3" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorDidNotPerformRequiredAction,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action2(1).DidNotPerformRequiredActionError(1).Penalty(%)");
                string name = (string)dataContainer.GetData("mages.analytics.errors.Action2(1).DidNotPerformRequiredActionError(1).Action");
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(50, penalty);
                Assert.AreEqual("Action2", name);
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(25, final_score);
            }

            // Test case where the did not perform required action error does not trigger.
            {
                // Create error specification of type DidNotPerformRequiredAction.
                MAGESAnalyticsEntry entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                MAGESAnalyticsEntry.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    BaseData = new AnalyticsTypes.DidNotPerformRequiredAction("DidNotPerformRequiredActionError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = "Action1",
                    ProcessorArguments = new() { "Action2" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorDidNotPerformRequiredAction,
                    KeyValuePairs = new(),
                };

                // Process the error.
                entry.ProcessError(specification);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(50, final_score);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Wrong Answer error.
        /// </summary>
        [Test]
        public void TestWrongAnswerError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Test case where the wrong answer error triggers.
            {
                bool questionResult = false;

                // Create error specification of type WrongAnswer.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    BaseData = new AnalyticsTypes.WrongAnswer("WrongAnswer", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    Processor = questionResult ? DefaultProcessors.DummyFalse : DefaultProcessors.DummyTrue,
                };

                var entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                // Process the error.
                entry.ProcessError(specification, null, false, false);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action1(1).WrongAnswer(1).Penalty(%)");
                string name = (string)dataContainer.GetData("mages.analytics.errors.Action1(1).WrongAnswer(1).Action");
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50, penalty);
                Assert.AreEqual("Action1", name);
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(25, final_score);
            }

            // Test case where the wrong answer error does not trigger.
            {
                bool questionResult = true;

                // Create error specification of type WrongAnswer.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    BaseData = new AnalyticsTypes.WrongAnswer("WrongAnswer", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    Processor = questionResult ? DefaultProcessors.DummyFalse : DefaultProcessors.DummyTrue,
                };

                var entry = new MAGESAnalyticsEntry("Action2", 50.0f);

                // Process the error.
                entry.ProcessError(specification, null, false, false);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(50, final_score);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Insertion Angle error.
        /// </summary>
        [Test]
        public void TestInsertionAngleError()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Test case where the insertion angle error triggers.
            {
                // Create error specification of type InsertionAngle.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    BaseData = new AnalyticsTypes.ErrorAngle("AngleError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = 45.0f,
                    ProcessorArguments = new() { 40.0f },
                    Processor = DefaultProcessors.ProcessorAngleLimit,
                };

                var entry = new MAGESAnalyticsEntry("Action1", 50.0f);

                // Process the error.
                entry.ProcessError(specification, null, false, false);

                // Retrieve the error records from the data container.
                object penalty = dataContainer.GetData("mages.analytics.errors.Action1(1).AngleError(1).Penalty(%)");
                string name = (string)dataContainer.GetData("mages.analytics.errors.Action1(1).AngleError(1).Action");
                object init_score = dataContainer.GetData("mages.analytics.score.Action1(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action1(1).final");

                // Validate the error records.
                Assert.AreEqual(50, penalty);
                Assert.AreEqual("Action1", name);
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(25, final_score);
            }

            // Test case where the insertion angle error does not trigger.
            {
                // Create error specification of type InsertionAngle.
                MAGESAnalyticsEntry.Specification specification = new()
                {
                    BaseData = new AnalyticsTypes.ErrorAngle("AngleError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = 30.0f,
                    ProcessorArguments = new() { 40.0f },
                    Processor = DefaultProcessors.ProcessorAngleLimit,
                };

                var entry = new MAGESAnalyticsEntry("Action2", 50.0f);

                // Process the error.
                entry.ProcessError(specification, null, false, false);

                // Retrieve the error records from the data container.
                object init_score = dataContainer.GetData("mages.analytics.score.Action2(1).initial");
                object final_score = dataContainer.GetData("mages.analytics.score.Action2(1).final");

                // Validate the error records.
                Assert.AreEqual(50, init_score);
                Assert.AreEqual(50, final_score);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the scoring functionality.
        /// </summary>
        [Test]
        public void TestScoring()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Inject action records to data container.
            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            // Create the scoring entries.
            var entry1 = new MAGESAnalyticsEntry("Action1", 50.0f);
            var entry2 = new MAGESAnalyticsEntry("Action2", 10.0f);
            var entry3 = new MAGESAnalyticsEntry("Action3", 100.0f);
            var entry4 = new MAGESAnalyticsEntry("Action4", 200.0f);
            var entry5 = new MAGESAnalyticsEntry("Action5", 25.0f);
            var entry6 = new MAGESAnalyticsEntry("Action6", 150.0f);

            MAGESAnalytics magesAnalytics = analytics as MAGESAnalytics;

            CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");
            float finalScore = (float)CallPrivateMethod(magesAnalytics, "CalculateFinalScore");

            Assert.IsTrue(finalScore >= 0.0f && finalScore <= 100.0f);

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the scoring merge policy functionality.
        /// </summary>
        [Test]
        public void TestScoringMergePolicy()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Inject action records to data container.
            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            // Create the scoring entries.
            var entry1 = new MAGESAnalyticsEntry("Action1", 50.0f);
            var entry2 = new MAGESAnalyticsEntry("Action2", 10.0f);
            var entry3 = new MAGESAnalyticsEntry("Action3", 100.0f);
            var entry4 = new MAGESAnalyticsEntry("Action4", 200.0f);
            var entry5 = new MAGESAnalyticsEntry("Action5", 25.0f);
            var entry6 = new MAGESAnalyticsEntry("Action6", 150.0f);

            // Cause an error to change the score of the second action.
            MAGESAnalyticsEntry.Specification specification = new()
            {
                FilterPrefix = "mages.analytics.actions",
                FilterScope = new() { "Action1-ID" },
                BaseData = new AnalyticsTypes.TimeLimit("TimeLimitError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                ProcessorTarget = 4f,
                ProcessorArguments = new() { "Action1", "Upper bound" },
                Iterator = DefaultIterators.ListIterator,
                Processor = DefaultProcessors.ProcessorTimeLimit,
            };

            // Process the error.
            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 29.0f, EndTime = 37.0f });
            var entry1_1 = new MAGESAnalyticsEntry("Action1", 50.0f);
            entry1_1.ProcessError(specification);

            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 48.0f, EndTime = 53.0f });
            var entry1_2 = new MAGESAnalyticsEntry("Action1", 50.0f);
            entry1_2.ProcessError(specification);

            MAGESAnalytics magesAnalytics = analytics as MAGESAnalytics;

            // Test case where the scoring merge policy is set to Average.
            {
                magesAnalytics.ActionScoringMergePolicy = MAGESAnalytics.ScoringMergePolicy.Average;

                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");
                float finalScore = (float)CallPrivateMethod(magesAnalytics, "CalculateFinalScore");

                Assert.IsTrue(finalScore >= 0.0f && finalScore <= 100.0f);
                Assert.IsTrue(Mathf.Approximately(96.88474f, finalScore));
            }

            // Test case where the scoring merge policy is set to Max.
            {
                magesAnalytics.ActionScoringMergePolicy = MAGESAnalytics.ScoringMergePolicy.Max;

                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");
                float finalScore = (float)CallPrivateMethod(magesAnalytics, "CalculateFinalScore");

                Assert.IsTrue(finalScore >= 0.0f && finalScore <= 100.0f);
                Assert.AreEqual(100f, finalScore);
            }

            // Test case where the scoring merge policy is set to Min.
            {
                magesAnalytics.ActionScoringMergePolicy = MAGESAnalytics.ScoringMergePolicy.Min;

                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");
                float finalScore = (float)CallPrivateMethod(magesAnalytics, "CalculateFinalScore");

                Assert.IsTrue(finalScore >= 0.0f && finalScore <= 100.0f);
                Assert.IsTrue(Mathf.Approximately(95.3271f, finalScore));
            }

            // Test case where the scoring merge policy is set to First.
            {
                magesAnalytics.ActionScoringMergePolicy = MAGESAnalytics.ScoringMergePolicy.First;

                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");
                float finalScore = (float)CallPrivateMethod(magesAnalytics, "CalculateFinalScore");

                Assert.IsTrue(finalScore >= 0.0f && finalScore <= 100.0f);
                Assert.AreEqual(100f, finalScore);
            }

            // Test case where the scoring merge policy is set to Last.
            {
                magesAnalytics.ActionScoringMergePolicy = MAGESAnalytics.ScoringMergePolicy.Last;

                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");
                float finalScore = (float)CallPrivateMethod(magesAnalytics, "CalculateFinalScore");

                Assert.IsTrue(finalScore >= 0.0f && finalScore <= 100.0f);
                Assert.IsTrue(Mathf.Approximately(95.3271f, finalScore));
            }

            // Test case where the scoring merge policy is set to Aggregate.
            {
                magesAnalytics.ActionScoringMergePolicy = MAGESAnalytics.ScoringMergePolicy.Aggregate;

                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");
                float finalScore = (float)CallPrivateMethod(magesAnalytics, "CalculateFinalScore");

                Assert.IsTrue(Mathf.Approximately(109.3458f, finalScore));
            }

            GameObject.Destroy(go);
        }

        private GameObject InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer)
        {
            GameObject go = new GameObject("__test_go__");

            go.SetActive(false);

            Hub hub = go.AddComponent<Hub>();
            hub.AutoStart = false;
            hub.BaseBundle = BuildStubBundle();

            go.SetActive(true);

            AnalyticsLogger.Enable = false;

            hub.StartSystems();

            analytics = Hub.Instance.Get<AnalyticsModule>();
            Assert.IsNotNull(analytics);

            dataContainer = Hub.Instance.Get<DataContainerModule>();
            Assert.IsNotNull(dataContainer);

            return go;
        }

        private Bundle BuildStubBundle()
        {
            UserAuthenticationModule userAuthenticationModule = ScriptableObject.CreateInstance<StubUserAuthentication>();
            DataContainerModule dataContainerModule = ScriptableObject.CreateInstance<MAGESDataContainer>();
            InteractionSystemModule interactionSystemModule = ScriptableObject.CreateInstance<StubInteractionSystem>();
            NetworkingModule networkingModule = ScriptableObject.CreateInstance<StubNetworking>();
            AnalyticsModule analyticsModule = ScriptableObject.CreateInstance<MAGESAnalytics>();
            DeviceManagerModule deviceManagerModule = ScriptableObject.CreateInstance<StubDeviceManager>();
            SceneGraphModule sceneGraphModule = ScriptableObject.CreateInstance<StubSceneGraph>();

            Bundle bundle = ScriptableObject.CreateInstance<Bundle>();
            bundle.UserAuthentication = userAuthenticationModule;
            bundle.DataContainer = dataContainerModule;
            bundle.InteractionSystem = interactionSystemModule;
            bundle.Networking = networkingModule;
            bundle.DeviceManager = deviceManagerModule;
            bundle.SceneGraph = sceneGraphModule;
            bundle.Analytics = analyticsModule;

            return bundle;
        }

        private object CallPrivateMethod(object objectInstance, string methodName, object[] methodParameters = null)
        {
            MethodInfo privMethod = objectInstance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return privMethod.Invoke(objectInstance, methodParameters);
        }
    }
}
