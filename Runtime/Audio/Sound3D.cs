using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound3D : MonoBehaviour
{
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        AudioManager.Instance.OnSoundToggled += OnToggle;
        OnToggle(DataSetting.Current.Sound);
    }

    void OnDisable()
    {
        if (AudioManager.HasInstance) AudioManager.Instance.OnSoundToggled -= OnToggle;
    }

    void OnToggle(bool enabled)
    {
        source.mute = !enabled;
    }
}