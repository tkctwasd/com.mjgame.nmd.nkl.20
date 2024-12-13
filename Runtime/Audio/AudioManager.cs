using System;
using System.Collections.Generic;
using UnityEngine;

namespace MJGame
{

    public class AudioManager : SingletonComponent<AudioManager>
    {
        private Dictionary<string, Pool<AudioPool>> audioPools = new();
        private AudioConfigSO audioConfigSO;
        private GameObject prefabAudioPool;
        Dictionary<string, ItemAudio> vfxAudioClips = new();
        Dictionary<string, ItemAudio> bgAudioClips = new();
        Dictionary<string, ItemAudio> sfxAudioClips = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            GameObject audioManager = new("AudioManager");
            audioManager.AddComponent<AudioManager>();
            Debug.Log("AudioManager has been initialized and added to the scene.");
        }

        protected override void Awake()
        {
            base.Awake();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            Prefab();
            InitializeAudioPools();
            LoadConfig();
        }
        private void Prefab()
        {
            prefabAudioPool = new GameObject("AudioPool");
            prefabAudioPool.transform.SetParent(transform);
            prefabAudioPool.AddComponent<AudioPool>();
            prefabAudioPool.GetComponent<AudioPool>().LoadComponent();
        }
        private void InitializeAudioPools()
        {
            CreateAudioPool("SFX", 0);
            CreateAudioPool("VFX", 0);
            CreateAudioPool("BGMusic", 0);
        }

        private void LoadConfig()
        {
            audioConfigSO = Resources.Load<AudioConfigSO>("AudioConfig");
            if (audioConfigSO == null)
            {
                Debug.Log("AudioConfig not found in Resources folder");
                return;
            }
            InitAudioClipDictionary(audioConfigSO);
        }

        private void InitAudioClipDictionary(AudioConfigSO audioConfig)
        {
            List<ItemAudio> vfxClips = audioConfig.vfxClips;
            List<ItemAudio> bgClips = audioConfig.bgClips;
            List<ItemAudio> sfxClips = audioConfig.sfxClips;

            foreach (ItemAudio item in vfxClips)
            {
                vfxAudioClips.Add(item.name, item);
            }

            foreach (ItemAudio item in bgClips)
            {
                bgAudioClips.Add(item.name, item);
            }

            foreach (ItemAudio item in sfxClips)
            {
                sfxAudioClips.Add(item.name, item);
            }
        }

        private void CreateAudioPool(string poolName, int size)
        {
            GameObject poolObject = new GameObject(poolName);
            poolObject.transform.SetParent(transform);

            Pool<AudioPool> pool = new Pool<AudioPool>(
            new PoolItem
            {
                name = poolName,
                size = size,
                prefab = prefabAudioPool
            }, poolObject.transform);
            audioPools.Add(poolName, pool);
        }

        public void PlaySFX(string sfxName)
        {
            AudioPool audioPool = GetAudioFromPool("SFX");
            if (audioPool != null)
            {
                audioPool.Setup(sfxAudioClips[sfxName], "SFX");
            }
        }

        public void PlayVFX(string vfxName)
        {
            AudioPool audioPool = GetAudioFromPool("VFX");
            if (audioPool != null)
            {
                audioPool.Setup(vfxAudioClips[vfxName], "VFX");
            }
        }

        public void PlayBGMusic(string bgName)
        {
            AudioPool audioPool = GetAudioFromPool("BGMusic");
            if (audioPool != null)
            {
                audioPool.Setup(bgAudioClips[bgName], "BGMusic");
            }
        }

        private AudioPool GetAudioFromPool(string poolName)
        {
            if (audioPools.ContainsKey(poolName))
            {
                return audioPools[poolName].GetObject();
            }
            Debug.LogWarning("No pool found for: " + poolName);
            return null;
        }

        public void ReturnAudioToPool(AudioPool audioPool, string poolName)
        {
            if (audioPools.ContainsKey(poolName))
            {
                audioPools[poolName].ReturnObject(audioPool);
            }
        }

        public void PauseAllAudio()
        {
            PauseAudioBGMusic();
            PauseAudioSFX();
            PauseAudioVFX();
        }

        public void PauseAudioVFX()
        {
            Pool<AudioPool> pool = audioPools["VFX"];
            PauseAudio(pool);
        }


        public void PauseAudioSFX()
        {
            Pool<AudioPool> pool = audioPools["SFX"];
            PauseAudio(pool);
        }

        public void PauseAudioBGMusic()
        {
            Pool<AudioPool> pool = audioPools["BGMusic"];
            PauseAudio(pool);
        }
        private void PauseAudio(Pool<AudioPool> pool)
        {
            foreach (AudioPool audioPool in pool)
            {
                if (audioPool != null && audioPool.audioSource != null)
                {
                    audioPool.audioSource.Pause();
                }
            }
        }

        public void ContinueAllAudio()
        {
            ContinueAudioBGMusic();
            ContinueAudioSFX();
            ContinueAudioVFX();
        }

        public void ContinueAudioBGMusic()
        {
            Pool<AudioPool> pool = audioPools["BGMusic"];
            ContinueAudio(pool);
        }

        public void ContinueAudioSFX()
        {
            Pool<AudioPool> pool = audioPools["SFX"];
            ContinueAudio(pool);
        }

        public void ContinueAudioVFX()
        {
            Pool<AudioPool> pool = audioPools["VFX"];
            ContinueAudio(pool);
        }


        private void ContinueAudio(Pool<AudioPool> pool)
        {
            foreach (AudioPool audioPool in pool)
            {
                if (audioPool != null && audioPool.audioSource != null)
                {
                    audioPool.audioSource.Play();
                }
            }
        }

        public void UpdateAllAudioVolumes()
        {
            foreach (var poolEntry in audioPools)
            {
                Pool<AudioPool> pool = poolEntry.Value;
                foreach (AudioPool audioPool in pool)
                {
                    if (audioPool != null && audioPool.audioSource != null)
                    {
                        audioPool.SetVolume(DataManager.DataGameUtility.Profile.volume);
                    }
                }
            }
        }
    }

    [Serializable]
    public class ItemAudio
    {
        public string name;
        public AudioClip clip;
        public bool loop = false;
        public float volume = 1f;
    }

}