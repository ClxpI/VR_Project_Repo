namespace MAGES.Recorder.Dissonance
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using global::Dissonance;
    using global::Dissonance.Networking;
    using NAudio.Wave;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    /// Captures audio from Dissonance.
    /// </summary>
    [Preserve]
    public class DissonanceCaptor : ICaptor
    {
        private RecorderModule rm;
        private List<AudioVPS> audioVPS = new List<AudioVPS>();

        /// <summary>
        /// Gets a value indicating whether this captor is polling.
        /// </summary>
        public bool IsPolling => true;

        /// <summary>
        /// Deinitialize the captor.
        /// </summary>
        public void Deinit()
        {
        }

        /// <summary>
        /// Initializes the captor.
        /// </summary>
        public void Init()
        {
            rm = Hub.Instance.Get<RecorderModule>();
            if (Hub.Instance.Get<NetworkingModule>().IsInitialized)
            {
                InitSingleAudioStream();
            }
        }

        /// <summary>
        /// Polls the captor for data.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        public void Poll(double timestamp)
        {
        }

        private void InitSingleAudioStream()
        {
            DissonanceComms comms = GameObject.FindObjectOfType<DissonanceComms>();
            if (!comms)
            {
                Debug.LogWarning("[Recorder.Dissonance] DissonanceComms not found.");
                return;
            }

            var sub = comms.gameObject.AddComponent<DissonanceSubscriber>();
            sub.SourceCategory = MicrophoneEvent.SourceCategory.RemotePlayer;
            comms.SubscribeToRecordedAudio(sub);

            sub = comms.gameObject.AddComponent<DissonanceSubscriber>();
            sub.SourceCategory = MicrophoneEvent.SourceCategory.LocalPlayer;
            comms.MicrophoneCapture.Subscribe(sub);
        }

        private void InitSeparateAudioStreams()
        {
            DissonanceComms comms = GameObject.FindObjectOfType<DissonanceComms>();
            var commNetwork = comms.GetComponent<ICommsNetwork>();
            comms.OnPlayerJoinedSession += OnPlayerJoinedSession;
            comms.OnPlayerLeftSession += OnPlayerLeftSession;

            foreach (var player in comms.Players)
            {
                CreateStream(player);
            }
        }

        private void OnPlayerJoinedSession(VoicePlayerState state)
        {
            CreateStream(state);
        }

        private void OnPlayerLeftSession(VoicePlayerState state)
        {
            var vp = audioVPS.Where(x => x.VoicePlayerState == state).FirstOrDefault();
            if (vp == null)
            {
                return;
            }

            rm.RecorderInstance.Audio.CloseStream(vp.AudioStream);
        }

        private void CreateStream(VoicePlayerState player)
        {
            if (!(player.Playback is MonoBehaviour))
            {
                return;
            }

            var source = (player.Playback as MonoBehaviour).gameObject.GetComponentInChildren<AudioSource>();
            if (source == null)
            {
                return;
            }

            var audio = rm.RecorderInstance.Audio.AddStreamByAudioSource(source);
            audioVPS.Add(new AudioVPS
            {
                AudioStream = audio,
                VoicePlayerState = player,
            });
            Debug.Log($"[Recorder.Dissonance] Added {player.Name} to the audio streams.");
        }

        private class DissonanceSubscriber : BaseMicrophoneSubscriber
        {
            private RecorderModule rm;
            private AudioCapture.AudioStream audioStream = null;
            private int currentSampleRate;
            private int currentChannels;

            private MicrophoneEvent.SourceCategory sourceCategory;

            public MicrophoneEvent.SourceCategory SourceCategory { get => sourceCategory; set => sourceCategory = value; }

            protected override void ProcessAudio(ArraySegment<float> data)
            {
                rm.RecorderInstance.Audio.WriteSamples(data.Array, data.Offset, data.Count, currentSampleRate, currentChannels, audioStream);
            }

            protected override void ResetAudioStream(WaveFormat waveFormat)
            {
                if (audioStream == null)
                {
                    audioStream = Hub.Instance.Get<RecorderModule>().RecorderInstance.Audio.AddStreamByManual(waveFormat.SampleRate, sourceCategory);
                }

                currentSampleRate = waveFormat.SampleRate;
                currentChannels = waveFormat.Channels;
            }

            private void Awake()
            {
                rm = Hub.Instance.Get<RecorderModule>();
            }
        }

        private class AudioVPS
        {
            private AudioCapture.AudioStream audioStream;
            private VoicePlayerState voicePlayerState;

            public AudioCapture.AudioStream AudioStream { get => audioStream; set => audioStream = value; }

            public VoicePlayerState VoicePlayerState { get => voicePlayerState; set => voicePlayerState = value; }
        }
    }
}