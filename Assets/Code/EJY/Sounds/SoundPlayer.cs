using RuddnjsPool;
using UnityEngine;
using UnityEngine.Audio;

namespace Code.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioSource audioSource;
        [field: SerializeField] public PoolingItemSO PoolingType { get; private set; }
        public PoolTypeSO PoolType { get; set; }
        public GameObject GameObject => gameObject;

        private Pool _myPool;

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
        }

        public async void PlaySound(SoundSO clipData)
        {
            if (clipData.audioType == SoundSO.AudioTypes.SFX)
            {
                audioSource.outputAudioMixerGroup = sfxGroup;
            }
            else if (clipData.audioType == SoundSO.AudioTypes.Music)
            {
                audioSource.outputAudioMixerGroup = musicGroup;
            }

            audioSource.volume = clipData.volume;
            audioSource.pitch = clipData.pitch;

            if (clipData.randomizePitch)
            {
                audioSource.pitch += Random.Range(-clipData.randomPitchModifier, clipData.randomPitchModifier);
            }

            audioSource.clip = clipData.clip;
            audioSource.loop = clipData.loop;

            audioSource.Play();

            if (clipData.loop == false)
            {
                await Awaitable.WaitForSecondsAsync(clipData.clip.length + 0.2f);
                _myPool.Push(this);
            }
        }

        public void StopAndReturnPool()
        {
            audioSource.Stop();
            _myPool.Push(this);
        }
    }
}