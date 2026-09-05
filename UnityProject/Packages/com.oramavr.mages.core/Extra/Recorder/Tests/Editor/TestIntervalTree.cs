namespace MAGES.Recorder.Editor.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using UnityEngine;

    /// <summary>
    /// Tests the interval tree implementation.
    /// </summary>
    public class TestIntervalTree
    {
        /// <summary>
        /// Tests that the interval tree can be constructed with the correct count of values.
        /// </summary>
        [Test]
        public void Given4ValuesTreeConstructsWithoutErrors()
        {
            IntervalTree<double, int> tree = new IntervalTree<double, int>();

            tree.Add(-10.0, 10.0, 0);
            tree.Add(-5.0, 5.0, 1);
            tree.Add(0.0, 10.0, 2);
            tree.Add(5.0, 15.0, 3);

            Assert.AreEqual(tree.Count, 4);
        }

        /// <summary>
        /// Tests that the interval tree behaves correctly with a closed range of a single value.
        /// </summary>
        [Test]
        public void GivenRangeOfSingleValueQueryRange()
        {
            IntervalTree<double, int> tree = new IntervalTree<double, int>();
            tree.Add(0.0, 0.0, 10);
            tree.Add(0.0, 1.0, 20);

            var results = tree.Q(0.0, 0.0);
            Assert.AreEqual(results.Count, 2);
        }

        /// <summary>
        /// Tests that the interval tree behaves correctly with multiple intersecting ranges of double values.
        /// </summary>
        [Test]
        public void GivenAccurateDoubleRangeWithRangeQueryMatchesCorrectly()
        {
            IntervalTree<double, int> tree = new IntervalTree<double, int>();

            tree.Add(0.068443, 1.468443, 1);
            tree.Add(0.078443, 0.088443, 2);

            var result = tree.Q(0.0, 1.0);
            Assert.AreEqual(result.Count, 2);
        }

        /// <summary>
        /// Tests that the range [0, 1] returns the correct count of values.
        /// </summary>
        [Test]
        public void GivenRange01Query0()
        {
            IntervalTree<double, int> tree = new IntervalTree<double, int>();
            tree.Add(0.0, 1.0, 10);

            Assert.AreEqual(tree.Q(0.0).Count, 1);
            Assert.AreEqual(tree.Q(0.0).First(), 10);
        }

        /// <summary>
        /// Tests that the interval tree can be constructed with the correct count of values.
        /// </summary>
        [Test]
        public void Given4IntercontainedValuesPointQueryReturnsAll()
        {
            IntervalTree<double, int> tree = new IntervalTree<double, int>();

            tree.Add(-10.0, 10.0, 0);
            tree.Add(-5.0, 5.0, 1);
            tree.Add(-2.0, 2.0, 2);
            tree.Add(-1.0, 1.0, 3);

            var results = tree.Q(0.0);

            Assert.AreEqual(results.Count, 4);
            Assert.AreEqual(results.Sum(), 6);
        }
    }
}