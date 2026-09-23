using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Common/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [Header("Mixer")]
    public AudioMixer AudioMixer;
    public string MasterVolumeParam = "Master";
    public string MusicVolumeParam = "Music";
    public string SFXVolumeParam = "SFX";
    public string SpeechVolumeParam = "Speech";

    [Header("Pool Limit")]
    [Range(1, 8)] public int MaxSourcesPerMusicKey = 2;
    [Range(1, 8)] public int MaxSourcesPerSoundKey = 4;
    [Range(1, 4)] public int MaxSourcesPerSpeechKey = 2;

    [Header("Audio Data")]
    public List<AudioData> AudioMusicData;
    public List<AudioData> AudioSoundData;
    public List<AudioData> AudioSpeechData;
}

[Serializable]
public class AudioData
{
    public string name;
    public List<AudioClip> clips;

    public bool isLoop;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Header("Pitch")]
    [Range(0.5f, 2f)] public float pitchMin = 1f;
    [Range(0.5f, 2f)] public float pitchMax = 1f;

    [Header("3D")]
    public bool is3D;
    [ShowIf(nameof(is3D), 1)] public float minDistance = 1f;
    [ShowIf(nameof(is3D), 1)] public float maxDistance = 10f;

    [Header("Mixer")]
    public AudioMixerGroup mixerGroup;

    public AudioClip GetClip()
    {
        if (clips == null || clips.Count == 0) return null;
        return clips.Count == 1 ? clips[0] : clips[UnityEngine.Random.Range(0, clips.Count)];
    }

    public float GetPitch() => UnityEngine.Random.Range(pitchMin, pitchMax);
}