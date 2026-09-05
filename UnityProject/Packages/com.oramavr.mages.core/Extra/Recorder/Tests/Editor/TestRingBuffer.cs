namespace MAGES.Recorder.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;

    /// <summary>
    /// Tests various aspects of the ring buffer.
    /// </summary>
    public class TestRingBuffer
    {
        /// <summary>
        /// Case: When the buffer is empty, reading should return null, and writing should be allowed.
        /// </summary>
        [Test]
        public void WhenReadWriteEqualThenOnlyWriteIsAllowed()
        {
            Utilities.RingBuffer<int> buffer = new Utilities.RingBuffer<int>(10);

            int[] result = buffer.Read(1);
            Assert.IsNull(result, "Reading from an empty buffer should return null.");

            Assert.IsTrue(buffer.TryWrite(new int[] { 1 }), "Write must be allowed");
        }

        /// <summary>
        /// Case: When the whole buffer is written to, reading should return the correct values.
        /// </summary>
        [Test]
        public void WhenWriteToEndThenReadToEndIsCorrect()
        {
            Utilities.RingBuffer<int> buffer = new Utilities.RingBuffer<int>(10);

            // Write 10 values.
            Assert.IsTrue(buffer.TryWrite(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }), "Write must be allowed");

            int[] result = buffer.Read(10);
            Assert.IsNotNull(result, "Reading from a full buffer should return a non-null value.");

            Assert.AreEqual(10, result.Length, "Reading from a full buffer should return the correct number of values.");

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(i + 1, result[i], "Reading from a full buffer should return the correct values.");
            }
        }
    }
}