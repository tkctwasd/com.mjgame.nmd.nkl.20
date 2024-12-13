
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

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }
        public void Setup(ItemAudio item, string key)
        {
            this.key = key;
            audioSource.clip = item.clip;
            audioSource.volume = DataManager.DataGameUtility.Profile.volume;
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