namespace MAGES.NonWebMicrophone
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    /// Basically invokes the <see cref="Microphone"/> class in Unity.
    /// </summary>
    [Preserve]
    public class NonWebMicrophone : LocalMicrophone.IApi
    {
        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public IEnumerable<string> Devices => Microphone.devices;

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public void End(string deviceName)
        {
            Microphone.End(deviceName);
        }

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public void GetDeviceCaps(string deviceName, out int minFreq, out int maxFreq)
        {
            Microphone.GetDeviceCaps(deviceName, out minFreq, out maxFreq);
        }

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public int GetPosition(string deviceName)
        {
            return Microphone.GetPosition(deviceName);
        }

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public bool IsRecording(string deviceName = null)
        {
            return Microphone.IsRecording(deviceName);
        }

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency)
        {
            return Microphone.Start(deviceName, loop, lengthSec, frequency);
        }
    }
}
