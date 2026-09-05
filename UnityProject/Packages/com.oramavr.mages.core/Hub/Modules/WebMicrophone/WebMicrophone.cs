namespace MAGES.WebMicrophone
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    /// <summary>
    /// Web microphone implementation.
    /// </summary>
    public class WebMicrophone : LocalMicrophone.IApi
    {
        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public IEnumerable<string> Devices => new string[] { "Web Microphone" };

#pragma warning disable SA1600 // Elements should be documented
        [DllImport("__Internal")]
        public static extern bool JS_Microphone_InitOrResumeContext();

        [DllImport("__Internal")]
        public static extern int JS_Microphone_GetSampleRate(int deviceIndex);

        [DllImport("__Internal")]
        public static extern int JS_Microphone_GetPosition(int deviceIndex);

        [DllImport("__Internal")]
        public static extern bool JS_Microphone_IsRecording(int deviceIndex);

        [DllImport("__Internal")]
        public static extern int JS_Microphone_GetBufferInstanceOfLastAudioClip();

        [DllImport("__Internal")]
        public static extern void JS_Microphone_Start(int deviceIndex, int bufferInstance, int samplesPerUpdate);

        [DllImport("__Internal")]
        public static extern void JS_Microphone_End(int deviceIndex);
#pragma warning restore SA1600 // Elements should be documented

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public void End(string deviceName)
        {
            JS_Microphone_End(0);
        }

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public void GetDeviceCaps(string deviceName, out int minFreq, out int maxFreq)
        {
            JS_Microphone_InitOrResumeContext();
            minFreq = maxFreq = JS_Microphone_GetSampleRate(0);
        }

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public int GetPosition(string deviceName)
        {
            JS_Microphone_InitOrResumeContext();
            return JS_Microphone_GetPosition(0);
        }

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public bool IsRecording(string deviceName = null)
        {
            JS_Microphone_InitOrResumeContext();
            return JS_Microphone_IsRecording(0);
        }

        /// <inheritdoc cref="LocalMicrophone.IApi"/>
        public AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency)
        {
            if (!JS_Microphone_InitOrResumeContext() || JS_Microphone_IsRecording(0))
            {
                return null;
            }

            const int SAMPLES_PER_UPDATE_HIGHFREQ = 1024;
            const int SAMPLES_PER_UPDATE_LOWFREQ = 512;
            const int LENGTH_SAMPLES_MULTIPLIER = 32;

            var sampleRate = JS_Microphone_GetSampleRate(0);
            var samplesPerUpdate = sampleRate > 32000
                ? SAMPLES_PER_UPDATE_HIGHFREQ
                : SAMPLES_PER_UPDATE_LOWFREQ;
            var clip = AudioClip.Create(
                name: "Microphone AudioClip",
                lengthSamples: samplesPerUpdate * LENGTH_SAMPLES_MULTIPLIER,
                channels: 1,
                frequency: sampleRate,
                stream: false);

            var clipIndex = JS_Microphone_GetBufferInstanceOfLastAudioClip();
            JS_Microphone_Start(0, clipIndex, samplesPerUpdate);

            return clip;
        }
    }
}
