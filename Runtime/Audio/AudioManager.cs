using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    readonly Dictionary<string, List<AudioSource>> MusicPool = new();
    readonly Dictionary<string, List<AudioSource>> SoundPool = new();
    readonly Dictionary<string, List<AudioSource>> SpeechPool = new();
    readonly Dictionary<AudioSource, Coroutine> FadeCoroutines = new();

    [SerializeField] private AudioConfig config;

    public event Action<bool> OnMusicToggled;
    public event Action<bool> OnSoundToggled;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;

        if (config == null)
        {
            Debug.LogError("AudioManager prefab is missing AudioConfig.");
            return;
        }

        ApplySavedVolumes();
    }

    AudioSource PlayInternal(Dictionary<string, List<AudioSource>> pool,
        List<AudioData> dataList,
        string name,
        int max)
    {
        var data = dataList.Find(x => x.name == name);
        if (data == null)
        {
            Debug.LogError($"Audio not found: {name}");
            return null;
        }

        var src = GetSource(pool, name, data, max);

        src.clip = data.GetClip();
        src.loop = data.isLoop;
        src.volume = data.volume;
        src.pitch = data.GetPitch();

        if (data.mixerGroup != null)
            src.outputAudioMixerGroup = data.mixerGroup;

        Apply3D(src, data);

        src.Play();
        return src;
    }

    public AudioSource PlayMusic(string name)
    {
        if (config == null) return null;
        if (!IsMusicEnabled()) return null;
        var existing = GetPlaying(MusicPool, name);
        if (existing != null) return existing;
        return PlayInternal(MusicPool, config.AudioMusicData, name, config.MaxSourcesPerMusicKey);
    }

    public AudioSource PlaySoundOne(string name)
    {
        if (config == null) return null;
        if (!IsSoundEnabled()) return null;
        return PlayInternal(SoundPool, config.AudioSoundData, name, config.MaxSourcesPerSoundKey);
    }

    public AudioSource PlaySoundOneIfNotPlaying(string name)
    {
        if (GetSoundIsPlaying(name)) return null;
        return PlaySoundOne(name);
    }

    public AudioSource PlaySound3D(string name, Vector3 pos)
    {
        var src = PlaySoundOne(name);
        if (src == null) return null;

        src.transform.position = pos;
        src.spatialBlend = 1f;

        return src;
    }

    public void AudioClickBtn()
    {
        PlaySoundOne(Consts.Au_ClickButton);
    }

    void StartFade(AudioSource src, IEnumerator routine)
    {
        if (src == null) return;

        if (FadeCoroutines.TryGetValue(src, out var existing))
        {
            StopCoroutine(existing);
            FadeCoroutines.Remove(src);
        }

        var coroutine = StartCoroutine(RunFade(src, routine));
        FadeCoroutines[src] = coroutine;
    }

    IEnumerator RunFade(AudioSource src, IEnumerator routine)
    {
        yield return routine;
        FadeCoroutines.Remove(src);
    }

    public void FadeOutMusic(string name, float duration = 1f)
    {
        var src = GetPlaying(MusicPool, name);
        if (src != null)
            StartFade(src, CoFadeOut(src, duration, true));
    }

    public void CrossfadeMusic(string from, string to, float duration = 1f)
    {
        if (from == to) return;

        var fromSrc = GetPlaying(MusicPool, from);
        if (fromSrc != null)
            StartFade(fromSrc, CoFadeOut(fromSrc, duration, true));

        var toSrc = PlayMusic(to);
        if (toSrc != null)
        {
            float target = toSrc.volume;
            toSrc.volume = 0;
            StartFade(toSrc, CoFadeIn(toSrc, target, duration));
        }
    }

    public void CrossfadeTo(string newMusic, float duration = 1f)
    {
        foreach (var list in MusicPool.Values)
        {
            foreach (var src in list)
            {
                if (src.isPlaying)
                    StartFade(src, CoFadeOut(src, duration, true));
            }
        }

        var newSrc = PlayMusic(newMusic);
        if (newSrc != null)
        {
            float target = newSrc.volume;
            newSrc.volume = 0;
            StartFade(newSrc, CoFadeIn(newSrc, target, duration));
        }
    }

    public void StopAll() { StopAll(MusicPool); StopAll(SoundPool); StopAll(SpeechPool); }
    public void StopMusic(string name) => Stop(MusicPool, name);
    public void StopSounds(string name) => Stop(SoundPool, name);
    public void StopAllMusic() => StopAll(MusicPool);
    public void StopAllSounds() => StopAll(SoundPool);
    public void StopAllSoundsSpeech() => StopAll(SpeechPool);
    public void PauseAllMusic() { PauseAll(MusicPool); PauseAll(SoundPool); PauseAll(SpeechPool); }
    public void ContinueAll() { ResumeAll(MusicPool); ResumeAll(SoundPool); ResumeAll(SpeechPool); }

    public bool GetSoundIsPlaying(string name) => IsPlaying(SoundPool, name);
    public bool GetMusicIsPlaying(string name) => IsPlaying(MusicPool, name);

    public void SetMusicVolume(float linear)
    {
        if (config != null) SetMixer(config.MusicVolumeParam, linear);
    }

    public void SetSFXVolume(float linear)
    {
        if (config != null) SetMixer(config.SFXVolumeParam, linear);
    }

    public void ToggleSound(bool enabled)
    {
        DataSetting.Current.Sound = enabled;
        SetSFXVolume(enabled ? 1f : 0f);

        if (!enabled)
            StopAllSounds();

        OnSoundToggled?.Invoke(enabled);
    }

    public void ToggleMusic(bool enabled)
    {
        DataSetting.Current.Music = enabled;
        SetMusicVolume(enabled ? 1f : 0f);

        if (!enabled)
            StopAllMusic();

        OnMusicToggled?.Invoke(enabled);
    }

    bool IsMusicEnabled() => DataSetting.Current.Music;
    bool IsSoundEnabled() => DataSetting.Current.Sound;


    AudioSource GetSource(Dictionary<string, List<AudioSource>> pool,
        string name, AudioData data, int max)
    {
        if (!pool.TryGetValue(name, out var list))
        {
            list = new List<AudioSource>();
            pool[name] = list;
        }

        foreach (var s in list)
            if (!s.isPlaying) return s;

        if (list.Count < max)
        {
            var s = CreateSource(name, data);
            list.Add(s);
            return s;
        }

        // steal oldest playing
        var steal = list[0];
        steal.Stop();
        return steal;
    }

    AudioSource CreateSource(string name, AudioData data)
    {
        var go = new GameObject($"[Au]{name}");
        go.transform.SetParent(transform);

        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;

        if (data.mixerGroup != null)
            src.outputAudioMixerGroup = data.mixerGroup;

        return src;
    }

    void Apply3D(AudioSource src, AudioData data)
    {
        src.spatialBlend = data.is3D ? 1f : 0f;
        src.minDistance = data.minDistance;
        src.maxDistance = data.maxDistance;
    }

    void Stop(Dictionary<string, List<AudioSource>> pool, string name)
    {
        if (!pool.TryGetValue(name, out var list)) return;
        foreach (var s in list) if (s != null) s.Stop();
    }

    void StopAll(Dictionary<string, List<AudioSource>> pool)
    {
        foreach (var list in pool.Values)
            foreach (var s in list) if (s != null) s.Stop();
    }

    void PauseAll(Dictionary<string, List<AudioSource>> pool)
    {
        foreach (var list in pool.Values)
            foreach (var s in list) if (s != null) s.Pause();
    }

    void ResumeAll(Dictionary<string, List<AudioSource>> pool)
    {
        foreach (var list in pool.Values)
            foreach (var s in list) if (s != null) s.UnPause();
    }

    bool IsPlaying(Dictionary<string, List<AudioSource>> pool, string name)
    {
        if (!pool.TryGetValue(name, out var list)) return false;
        foreach (var s in list)
            if (s != null && s.isPlaying) return true;
        return false;
    }

    AudioSource GetPlaying(Dictionary<string, List<AudioSource>> pool, string name)
    {
        if (!pool.TryGetValue(name, out var list)) return null;
        foreach (var s in list)
            if (s != null && s.isPlaying) return s;
        return null;
    }

    void SetMixer(string param, float linear)
    {
        if (config?.AudioMixer == null) return;
        float db = linear > 0.0001f ? 20f * Mathf.Log10(linear) : -80f;
        config.AudioMixer.SetFloat(param, db);
    }

    void ApplySavedVolumes()
    {
        SetMusicVolume(DataSetting.Current.Music ? 1f : 0f);
        SetSFXVolume(DataSetting.Current.Sound ? 1f : 0f);
    }
    public void FadeOutSound(string name, float duration = 1f)
    {
        PlaySoundOne(name);
        var src = GetPlaying(SoundPool, name);
        if (src != null)
            StartFade(src, CoFadeOut(src, duration, true));
    }
    IEnumerator CoFadeOut(AudioSource src, float duration, bool stop)
    {
        float start = src.volume;
        float t = 0;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            src.volume = Mathf.Lerp(start, 0, t / duration);
            yield return null;
        }

        if (stop) src.Stop();
        src.volume = start;
    }

    IEnumerator CoFadeIn(AudioSource src, float target, float duration)
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            src.volume = Mathf.Lerp(0, target, t / duration);
            yield return null;
        }

        src.volume = target;
    }
}
