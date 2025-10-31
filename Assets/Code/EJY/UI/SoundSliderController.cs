using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Code.UI
{
    [Serializable]
    public struct SoundSlider
    {
        [SerializeField] private string VolumeName;
        [SerializeField] private Slider Bar;

        private AudioMixer _audioMixer;
        private float Volume => Bar.value;

        public void Init(AudioMixer mixer)
        {
            _audioMixer = mixer;

            // Audio Mixer에서 기본값이 0이라, 슬라이더의 값이 0이 되지 않게하기 위해 처리
            Bar.minValue = 0.0001f;
            Bar.maxValue = 1f;
            Bar.onValueChanged.AddListener(OnValueChange);

            Bar.value = PlayerPrefs.GetFloat(VolumeName, 1f);
            OnValueChange(Volume);
        }

        public void OnDestroy()
        {
            PlayerPrefs.SetFloat(VolumeName, Volume);
        }

        private void OnValueChange(float value)
        {
            _audioMixer.SetFloat(VolumeName, Mathf.Log10(value) * 20);
        }
    }
    
    public class SoundSliderController : MonoBehaviour
    {
        [SerializeField] private List<SoundSlider> soundSliders;
        [SerializeField] private AudioMixer audioMixer;
        
        private void Start()
        {
            foreach (var soundSlider in soundSliders)
            {
                soundSlider.Init(audioMixer);
            }
        }

        private void OnDestroy()
        {
            foreach (var soundSlider in soundSliders)
            {
                soundSlider.OnDestroy();
            }
        }
    }
}