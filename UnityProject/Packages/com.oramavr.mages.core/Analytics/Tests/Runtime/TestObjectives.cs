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
    /// Class for testing the analytics objectives.
    /// </summary>
    public class TestObjectives
    {
        /// <summary>
        /// Test for the Collide objective.
        /// </summary>
        [Test]
        public void TestCollideObjective()
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

            // Test case where the collision objective is not successful (Entire Operation).
            {
                var objective = new MAGESObjective("O1");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel.Cauterizer" },
                    ProcessorTarget = "Cauterizer",
                    ProcessorArguments = new() { "*", AnalyticsTypes.ErrorCollider.CollisionRule.Collide },
                    Description = "Do not collide the cauterizer with the scalpel.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the collision objective is successful (Entire Operation).
            {
                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel2.Cauterizer2" },
                    ProcessorTarget = "Cauterizer2",
                    ProcessorArguments = new() { "*", AnalyticsTypes.ErrorCollider.CollisionRule.Collide },
                    Description = "Do not collide the cauterizer2 with the scalpel2.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the collision objective is not successful (Action1).
            {
                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel.Cauterizer" },
                    ProcessorTarget = "Cauterizer",
                    ProcessorArguments = new() { "Action1", AnalyticsTypes.ErrorCollider.CollisionRule.Collide },
                    Description = "Do not collide the cauterizer with the scalpel.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the collision objective is successful (Action2).
            {
                var objective = new MAGESObjective("O4");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel.Cauterizer" },
                    ProcessorTarget = "Cauterizer",
                    ProcessorArguments = new() { "Action2", AnalyticsTypes.ErrorCollider.CollisionRule.Collide },
                    Description = "Do not collide the cauterizer with the scalpel.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the collision objective is successful (Action1).
            {
                var objective = new MAGESObjective("O5");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "exit.Cauterizer.Leg" },
                    ProcessorTarget = "Leg",
                    ProcessorArguments = new() { "Action1", AnalyticsTypes.ErrorCollider.CollisionRule.Collide },
                    Description = "Do not collide the cauterizer with the leg.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Do Not Collide objective.
        /// </summary>
        [Test]
        public void TestDoNotCollideObjective()
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

            // Test case where the collision objective is not successful (Entire Operation).
            {
                var objective = new MAGESObjective("O1");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel.Cauterizer" },
                    ProcessorTarget = "Cauterizer",
                    ProcessorArguments = new() { "*", AnalyticsTypes.ErrorCollider.CollisionRule.DoNotCollide },
                    Description = "Do not collide the cauterizer with the scalpel.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the collision objective is successful (Entire Operation).
            {
                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel2.Cauterizer2" },
                    ProcessorTarget = "Cauterizer2",
                    ProcessorArguments = new() { "*", AnalyticsTypes.ErrorCollider.CollisionRule.DoNotCollide },
                    Description = "Do not collide the cauterizer2 with the scalpel2.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the collision objective is not successful (Action1).
            {
                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel.Cauterizer" },
                    ProcessorTarget = "Cauterizer",
                    ProcessorArguments = new() { "Action1", AnalyticsTypes.ErrorCollider.CollisionRule.DoNotCollide },
                    Description = "Do not collide the cauterizer with the scalpel.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the collision objective is successful (Action2).
            {
                var objective = new MAGESObjective("O4");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "enter.Scalpel.Cauterizer" },
                    ProcessorTarget = "Cauterizer",
                    ProcessorArguments = new() { "Action2", AnalyticsTypes.ErrorCollider.CollisionRule.DoNotCollide },
                    Description = "Do not collide the cauterizer with the scalpel.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the collision objective is successful (Action1).
            {
                var objective = new MAGESObjective("O5");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.collisions",
                    FilterScope = new List<string> { "exit.Cauterizer.Leg" },
                    ProcessorTarget = "Leg",
                    ProcessorArguments = new() { "Action1", AnalyticsTypes.ErrorCollider.CollisionRule.DoNotCollide },
                    Description = "Do not collide the cauterizer with the leg.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorCollision,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Interact objective.
        /// </summary>
        [Test]
        public void TestInteractObjective()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            AnalyticsTypes.InteractionRestriction interactionRestriction1 = new AnalyticsTypes.InteractionRestriction("InteractionError1", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
            {
                Interactable = new GameObject("Interactable1"),
                Interactor = new GameObject("Interactor1"),
                Penalty = 50,
                Info = "Interaction error occurred.",
                TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
            };

            AnalyticsTypes.InteractionRestriction interactionRestriction2 = new AnalyticsTypes.InteractionRestriction("InteractionError2", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
            {
                Interactable = new GameObject("Interactable2"),
                Interactor = new GameObject("Interactor2"),
                Penalty = 50,
                Info = "Interaction error occurred.",
                TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
            };

            // Create and set up the position data object.
            InteractionRecord record1 = new()
            {
                Interactor = interactionRestriction1.Interactor.name,
                Interactable = interactionRestriction1.Interactable.name,
                Timestamp = 2.0f,
                ActionNames = new() { "Action1" },
            };

            // Store the data to the data container.
            Hub.Instance.Get<DataContainerModule>().StoreData($"mages.analytics.interactions.{record1.Interactable}.{record1.Interactor}", record1);

            // Create and set up the position data object.
            InteractionRecord record2 = new()
            {
                Interactor = interactionRestriction2.Interactor.name,
                Interactable = interactionRestriction2.Interactable.name,
                Timestamp = 4.0f,
                ActionNames = new() { "Action2" },
            };

            // Store the data to the data container.
            Hub.Instance.Get<DataContainerModule>().StoreData($"mages.analytics.interactions.{record2.Interactable}.{record2.Interactor}", record2);

            // Test case where the interaction objective is successful (Entire Operation).
            {
                var objective = new MAGESObjective("O1");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new List<string> { $"{record1.Interactable}.{record1.Interactor}" },
                    ProcessorTarget = record1.Interactor,
                    ProcessorArguments = new() { "*", AnalyticsTypes.InteractionRestriction.InteractionRule.Interact },
                    Description = "Do not interact with the interactor.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the interaction objective is successful (Action1).
            {
                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new List<string> { $"{record1.Interactable}.{record1.Interactor}" },
                    ProcessorTarget = record1.Interactor,
                    ProcessorArguments = new() { "Action1", AnalyticsTypes.InteractionRestriction.InteractionRule.Interact },
                    Description = "Do not interact with the interactor.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the interaction objective is not successful (Action2).
            {
                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new List<string> { $"{record1.Interactable}.{record1.Interactor}" },
                    ProcessorTarget = record1.Interactor,
                    ProcessorArguments = new() { "Action2", AnalyticsTypes.InteractionRestriction.InteractionRule.Interact },
                    Description = "Do not interact with the interactor.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the interaction objective is successful (Action1).
            {
                var objective = new MAGESObjective("O4");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new List<string> { $"Interactable11.Interactor11" },
                    ProcessorTarget = "Interactor11",
                    ProcessorArguments = new() { "Action1", AnalyticsTypes.InteractionRestriction.InteractionRule.Interact },
                    Description = "Do not interact with the interactor.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Do Not Interact objective.
        /// </summary>
        [Test]
        public void TestDoNotInteractObjective()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            AnalyticsTypes.InteractionRestriction interactionRestriction1 = new AnalyticsTypes.InteractionRestriction("InteractionError1", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
            {
                Interactable = new GameObject("Interactable1"),
                Interactor = new GameObject("Interactor1"),
                Penalty = 50,
                Info = "Interaction error occurred.",
                TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
            };

            AnalyticsTypes.InteractionRestriction interactionRestriction2 = new AnalyticsTypes.InteractionRestriction("InteractionError2", string.Empty, string.Empty, AnalyticsTypes.Type.Error)
            {
                Interactable = new GameObject("Interactable2"),
                Interactor = new GameObject("Interactor2"),
                Penalty = 50,
                Info = "Interaction error occurred.",
                TypeOfError = MAGESAnalyticsEntry.ErrorType.Procedural.ToString(),
            };

            // Create and set up the position data object.
            InteractionRecord record1 = new()
            {
                Interactor = interactionRestriction1.Interactor.name,
                Interactable = interactionRestriction1.Interactable.name,
                Timestamp = 2.0f,
                ActionNames = new() { "Action1" },
            };

            // Store the data to the data container.
            Hub.Instance.Get<DataContainerModule>().StoreData($"mages.analytics.interactions.{record1.Interactable}.{record1.Interactor}", record1);

            // Create and set up the position data object.
            InteractionRecord record2 = new()
            {
                Interactor = interactionRestriction2.Interactor.name,
                Interactable = interactionRestriction2.Interactable.name,
                Timestamp = 4.0f,
                ActionNames = new() { "Action2" },
            };

            // Store the data to the data container.
            Hub.Instance.Get<DataContainerModule>().StoreData($"mages.analytics.interactions.{record2.Interactable}.{record2.Interactor}", record2);

            // Test case where the interaction objective is successful (Entire Operation).
            {
                var objective = new MAGESObjective("O1");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new List<string> { $"{record1.Interactable}.{record1.Interactor}" },
                    ProcessorTarget = record1.Interactor,
                    ProcessorArguments = new() { "*", AnalyticsTypes.InteractionRestriction.InteractionRule.DoNotInteract },
                    Description = "Do not interact with the interactor.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the interaction objective is successful (Action1).
            {
                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new List<string> { $"{record1.Interactable}.{record1.Interactor}" },
                    ProcessorTarget = record1.Interactor,
                    ProcessorArguments = new() { "Action1", AnalyticsTypes.InteractionRestriction.InteractionRule.DoNotInteract },
                    Description = "Do not interact with the interactor.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the interaction objective is not successful (Action2).
            {
                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new List<string> { $"{record1.Interactable}.{record1.Interactor}" },
                    ProcessorTarget = record1.Interactor,
                    ProcessorArguments = new() { "Action2", AnalyticsTypes.InteractionRestriction.InteractionRule.DoNotInteract },
                    Description = "Do not interact with the interactor.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the interaction objective is successful (Action1).
            {
                var objective = new MAGESObjective("O4");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.interactions",
                    FilterScope = new List<string> { $"Interactable11.Interactor11" },
                    ProcessorTarget = "Interactor11",
                    ProcessorArguments = new() { "Action1", AnalyticsTypes.InteractionRestriction.InteractionRule.DoNotInteract },
                    Description = "Do not interact with the interactor.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorInteractionRestriction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Time Limit objective.
        /// </summary>
        [Test]
        public void TestTimeLimitObjective()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Inject action records to data container.
            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            // Test case where the time limit objective is successful (Entire Operation, Upper bound).
            {
                var objective = new MAGESObjective("O1");

                MAGESObjective.Specification specification = new()
                {
                    ProcessorTarget = 60.0f,
                    ProcessorArguments = new() { 54.0, "Upper bound" },
                    Description = "Do not exceed the time limit.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorOperationDuration,
                };

                bool result = objective.ProcessObjective(specification, false);

                Assert.AreEqual(true, result);
            }

            // Test case where the time limit objective is successful (Entire Operation, Lower bound).
            {
                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    ProcessorTarget = 30.0f,
                    ProcessorArguments = new() { 54.0, "Lower bound" },
                    Description = "Play for at least 30 seconds.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorOperationDuration,
                };

                bool result = objective.ProcessObjective(specification, false);

                Assert.AreEqual(true, result);
            }

            // Test case where the time limit objective is not successful (Entire Operation, Upper bound).
            {
                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    ProcessorTarget = 60.0f,
                    ProcessorArguments = new() { 90.0, "Upper bound" },
                    Description = "Do not exceed the time limit.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorOperationDuration,
                };

                bool result = objective.ProcessObjective(specification, false);

                Assert.AreEqual(false, result);
            }

            // Test case where the time limit objective is not successful (Entire Operation, Lower bound).
            {
                var objective = new MAGESObjective("O4");

                MAGESObjective.Specification specification = new()
                {
                    ProcessorTarget = 30.0f,
                    ProcessorArguments = new() { 25.0, "Lower bound" },
                    Description = "Play for at least 30 seconds.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorOperationDuration,
                };

                bool result = objective.ProcessObjective(specification, false);

                Assert.AreEqual(false, result);
            }

            // Test case where the time limit objective is successful (Action1, Upper bound).
            {
                var objective = new MAGESObjective("O5");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    ProcessorTarget = 15.0f,
                    ProcessorArguments = new() { "Action1", "Upper bound" },
                    Description = "Do not exceed the time limit.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the time limit objective is successful (Action1, Lower bound).
            {
                var objective = new MAGESObjective("O6");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    ProcessorTarget = 5.0f,
                    ProcessorArguments = new() { "Action1", "Lower bound" },
                    Description = "Play for at least 5 seconds.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the time limit objective is not successful (Action1, Upper bound).
            {
                var objective = new MAGESObjective("O7");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    ProcessorTarget = 10.0f,
                    ProcessorArguments = new() { "Action1", "Upper bound" },
                    Description = "Do not exceed the time limit.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the time limit objective is not successful (Action1, Lower bound).
            {
                var objective = new MAGESObjective("O8");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    ProcessorTarget = 15.0f,
                    ProcessorArguments = new() { "Action1", "Lower bound" },
                    Description = "Play for at least 15 seconds.",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Score Limit objective.
        /// </summary>
        [Test]
        public void TestScoreLimitObjective()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            // Inject action records to data container.
            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f });

            var entry1 = new MAGESAnalyticsEntry("Action1", 50f);
            var entry2 = new MAGESAnalyticsEntry("Action2", 50f);
            var entry3 = new MAGESAnalyticsEntry("Action3", 50f);
            var entry4 = new MAGESAnalyticsEntry("Action4", 50f);
            var entry5 = new MAGESAnalyticsEntry("Action5", 50f);
            var entry6 = new MAGESAnalyticsEntry("Action6", 50f);

            MAGESAnalytics magesAnalytics = analytics as MAGESAnalytics;

            // Test case where the score limit objective is successful (Entire Operation).
            {
                // Normalize the score.
                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");

                var objective = new MAGESObjective("O1");

                float finalScore = 0;

                // Count score.
                foreach (var entry in Hub.Instance.Get<MAGESAnalytics>().Events)
                {
                    if (entry.Value is Analytics.Event eventValue)
                    {
                        if (eventValue.GetTag() == "Score")
                        {
                            finalScore += eventValue.GetData<float>("final");
                        }
                    }
                }

                MAGESObjective.Specification specification = new()
                {
                    ProcessorTarget = 90.0f,
                    ProcessorArguments = new() { finalScore },
                    Description = "Finish with score 90% or higher.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorOperationScoreLimit,
                };

                bool result = objective.ProcessObjective(specification, false);

                Assert.AreEqual(true, result);
            }

            // Test case where the score limit objective is successful (Action1).
            {
                // Normalize the score.
                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");

                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.score",
                    FilterScope = new() { $"Action1(1)" },
                    ProcessorTarget = 90.0f,
                    ProcessorArguments = new() { },
                    Description = "Finish Action1 with score 90% or higher.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorOperationScoreLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the score limit objective is not successful (Action1).
            {
                // Cause an error to change the score of the second action.
                MAGESAnalyticsEntry.Specification errorSpec = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    BaseData = new AnalyticsTypes.TimeLimit("TimeLimitError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = 4f,
                    ProcessorArguments = new() { "Action1", "Upper bound" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };
                entry1.ProcessError(errorSpec);

                // Normalize the score.
                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");

                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.score",
                    FilterScope = new() { $"Action1(1)" },
                    ProcessorTarget = 95.0f,
                    ProcessorArguments = new() { },
                    Description = "Finish Action1 with score 95% or higher.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorOperationScoreLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the score limit objective is not successful (Entire Operation).
            {
                // Cause an error to change the score of the second action.
                MAGESAnalyticsEntry.Specification errorSpec = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    BaseData = new AnalyticsTypes.TimeLimit("TimeLimitError", string.Empty, string.Empty, AnalyticsTypes.Type.Error),
                    ProcessorTarget = 4f,
                    ProcessorArguments = new() { "Action1", "Upper bound" },
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorTimeLimit,
                };
                entry1.ProcessError(errorSpec);

                // Normalize the score.
                CallPrivateMethod(magesAnalytics, "NormalizeActionScoring");

                var objective = new MAGESObjective("O4");

                float finalScore = 0;

                // Count score.
                foreach (var entry in Hub.Instance.Get<MAGESAnalytics>().Events)
                {
                    if (entry.Value is Analytics.Event eventValue)
                    {
                        if (eventValue.GetTag() == "Score")
                        {
                            finalScore += eventValue.GetData<float>("final");
                        }
                    }
                }

                MAGESObjective.Specification specification = new()
                {
                    ProcessorTarget = 99.0f,
                    ProcessorArguments = new() { finalScore },
                    Description = "Finish with score 99% or higher.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorOperationScoreLimit,
                };

                bool result = objective.ProcessObjective(specification, false);

                Assert.AreEqual(false, result);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Do Not Rotate objective.
        /// </summary>
        [Test]
        public void TestDoNotRotateObjective()
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

            // Test case where the rotation objective is successful.
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

                var objective = new MAGESObjective("O1");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new List<string> { "Target(Clone)" },
                    ProcessorTarget = rotationRestriction.MaxRotation,
                    Description = "Do not rotate the target.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorRotationLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the rotation objective is not successful.
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

                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new List<string> { "Target(Clone)" },
                    ProcessorTarget = rotationRestriction.MaxRotation,
                    Description = "Do not rotate the target.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorRotationLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            {
                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.transforms",
                    FilterScope = new List<string> { "NonExistentTarget(Clone)" },
                    ProcessorTarget = rotationRestriction.MaxRotation,
                    Description = "Do not rotate the target.",
                    Iterator = DefaultIterators.EventIterator,
                    Processor = DefaultProcessors.ProcessorRotationLimit,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Perform Required Action objective.
        /// </summary>
        [Test]
        public void TestPerformedWrongActionObjective()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f, ActiveActions = new List<string> { } });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f, ActiveActions = new List<string> { "Action1", "Action3" } });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f, ActiveActions = new List<string> { "Action1" } });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f, ActiveActions = new List<string> { } });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f, ActiveActions = new List<string> { "Action4" } });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f, ActiveActions = new List<string> { } });

            // Test case where the do not perform objective is successful (Entire Operation).
            {
                var objective = new MAGESObjective("O1");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    ProcessorTarget = true,
                    ProcessorArguments = new() { "Action2" },
                    Description = "Perform Action2",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorActionPerformedState,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the do not perform objective is not successful (Entire Operation).
            {
                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    ProcessorTarget = true,
                    ProcessorArguments = new() { "Action7" },
                    Description = "Perform Action7",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorActionPerformedState,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the do not perform objective is successful (Action1).
            {
                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    ProcessorTarget = "Action1",
                    ProcessorArguments = new() { "Action2" },
                    Description = "Perform Action2 before performing Action1",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorDidNotPerformRequiredAction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the do not perform objective is not successful (Action1).
            {
                var objective = new MAGESObjective("O4");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    ProcessorTarget = "Action2",
                    ProcessorArguments = new() { "Action1" },
                    Description = "Perform Action1 before performing Action2",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorDidNotPerformRequiredAction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the do not perform objective is not successful (Action7).
            {
                var objective = new MAGESObjective("O5");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    ProcessorTarget = "Action2",
                    ProcessorArguments = new() { "Action7" },
                    Description = "Perform Action7 before performing Action2",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorDidNotPerformRequiredAction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the do not perform objective is not successful (Action7).
            {
                var objective = new MAGESObjective("O6");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    ProcessorTarget = "Action7",
                    ProcessorArguments = new() { "Action1" },
                    Description = "Perform Action1 before performing Action7",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorDidNotPerformRequiredAction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            GameObject.Destroy(go);
        }

        /// <summary>
        /// Test for the Do Not Perform objective.
        /// </summary>
        [Test]
        public void TestDidNotPerformRequiredActionObjective()
        {
            GameObject go = InitializeModules(out AnalyticsModule analytics, out DataContainerModule dataContainer);

            dataContainer.StoreData($"mages.analytics.actions.Action1-ID", new ActionRecord { Name = "Action1", BeginTime = 0.0f, EndTime = 12.0f, ActiveActions = new List<string> { } });
            dataContainer.StoreData($"mages.analytics.actions.Action2-ID", new ActionRecord { Name = "Action2", BeginTime = 0.0f, EndTime = 5.0f, ActiveActions = new List<string> { "Action1", "Action3" } });
            dataContainer.StoreData($"mages.analytics.actions.Action3-ID", new ActionRecord { Name = "Action3", BeginTime = 0.0f, EndTime = 8.0f, ActiveActions = new List<string> { "Action1" } });
            dataContainer.StoreData($"mages.analytics.actions.Action4-ID", new ActionRecord { Name = "Action4", BeginTime = 13.0f, EndTime = 47.0f, ActiveActions = new List<string> { } });
            dataContainer.StoreData($"mages.analytics.actions.Action5-ID", new ActionRecord { Name = "Action5", BeginTime = 13.0f, EndTime = 28.0f, ActiveActions = new List<string> { "Action4" } });
            dataContainer.StoreData($"mages.analytics.actions.Action6-ID", new ActionRecord { Name = "Action6", BeginTime = 48.0f, EndTime = 54.0f, ActiveActions = new List<string> { } });

            // Test case where the do not perform objective is not successful (Entire Operation).
            {
                var objective = new MAGESObjective("O1");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    ProcessorTarget = false,
                    ProcessorArguments = new() { "Action2" },
                    Description = "Do not perform Action2",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorActionPerformedState,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the do not perform objective is successful (Entire Operation).
            {
                var objective = new MAGESObjective("O2");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "*" },
                    ProcessorTarget = false,
                    ProcessorArguments = new() { "Action7" },
                    Description = "Do not perform Action7",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorActionPerformedState,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the do not perform objective is not successful (Action1).
            {
                var objective = new MAGESObjective("O3");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action2-ID" },
                    ProcessorTarget = "Action1",
                    ProcessorArguments = new() { "Action2" },
                    Description = "Do not perform Action2 before performing Action1",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorPerformedWrongAction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(false, result);
            }

            // Test case where the do not perform objective is successful (Action1).
            {
                var objective = new MAGESObjective("O4");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action1-ID" },
                    ProcessorTarget = "Action2",
                    ProcessorArguments = new() { "Action1" },
                    Description = "Do not perform Action1 before performing Action2",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorPerformedWrongAction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
            }

            // Test case where the do not perform objective is successful (Action1).
            {
                var objective = new MAGESObjective("O5");

                MAGESObjective.Specification specification = new()
                {
                    FilterPrefix = "mages.analytics.actions",
                    FilterScope = new() { "Action7-ID" },
                    ProcessorTarget = "Action2",
                    ProcessorArguments = new() { "Action7" },
                    Description = "Do not perform Action7 before performing Action2",
                    Iterator = DefaultIterators.ListIterator,
                    Processor = DefaultProcessors.ProcessorPerformedWrongAction,
                };

                bool result = objective.ProcessObjective(specification);

                Assert.AreEqual(true, result);
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
