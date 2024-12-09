
using System.Collections;
using UnityEngine;

namespace MJGame
{
    public class AudioPool : ObjectPool
    {
        public AudioSource audioSource;
        private string key;
        private void OnValidate()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void LoadComponent()
        {
            gameObject.AddComponent<AudioSource>();
            audioSource = GetComponent<AudioSource>();
        }
        public void Setup(ItemAudio item, string key)
        {
            float volume = 1;
            volume = PlayerPrefs.GetFloat("Volume", 1);
            this.key = key;
            audioSource.clip = item.clip;
            audioSource.volume = volume != 1 ? volume : item.volume;
            audioSource.loop = item.loop;
            audioSource.Play();
            if (!item.loop)
            {
                StartCoroutine(Delay());
            }
        }
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            Return();
        }
        [Button("Return")]
        public override void Return()
        {
            SingletonComponent<AudioManager>.Instance.ReturnAudioToPool(this, key);
        }
    }
}