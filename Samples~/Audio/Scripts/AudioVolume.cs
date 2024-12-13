using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MJGame
{
    public class AudioVolume : TickBehaviour
    {
        private Slider sliderVolume;
        public bool IsAnimationCurve = false;
        [ShowIf("IsAnimationCurve")]
        [SerializeField] private AnimationCurve volumeCurve;

        protected override void Awake()
        {
            base.Awake();
            sliderVolume = GetComponent<Slider>();
            sliderVolume.value = ReverseValueVolume();
            sliderVolume.onValueChanged.AddListener(ChangeVolume);
        }
        public void ChangeVolume(float volume)
        {
            DataManager.DataGameUtility.Profile.volume = CalculateVolume(volume);
            SingletonComponent<AudioManager>.Instance.UpdateAllAudioVolumes();
        }

        #region Calculate Volume
        private float ValueVolume(float volume)
        {
            return Mathf.Log10(volume + 1) / Mathf.Log10(2);
        }

        private float ReverseValueVolume(float volume)
        {
            return Mathf.Pow(10, volume * Mathf.Log10(2)) - 1;
        }

        private float CalculateVolumeAnimationCurve(float volume)
        {
            return volumeCurve.Evaluate(volume);
        }
        private float ReverseCalculateVolumeAnimationCurve(float volume)
        {
            float precision = 0.001f;
            float sliderValue = 0f;
            float closestValue = Mathf.Infinity;
            float closestSliderValue = 0f;

            while (sliderValue <= 1f)
            {
                float evaluatedValue = volumeCurve.Evaluate(sliderValue);

                if (Mathf.Abs(evaluatedValue - volume) < closestValue)
                {
                    closestValue = Mathf.Abs(evaluatedValue - volume);
                    closestSliderValue = sliderValue;
                }

                sliderValue += precision;
            }
            return closestSliderValue;
        }
        private float CalculateVolume(float volume)
        {
            if (IsAnimationCurve)
            {
                return CalculateVolumeAnimationCurve(volume);
            }
            return ValueVolume(volume);
        }
        private float ReverseValueVolume()
        {
            float volume = DataManager.DataGameUtility.Profile.volume;
            if (IsAnimationCurve)
            {
                return ReverseCalculateVolumeAnimationCurve(volume);
            }
            return ReverseValueVolume(volume);
        }
        #endregion
    }
}