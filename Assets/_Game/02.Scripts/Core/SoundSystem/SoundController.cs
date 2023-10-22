using UnityEngine;

namespace _Game._02.Scripts.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour
    {
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
        
        public bool IsPlaying { get; private set; }
        public bool IsStopped
        {
            get
            {
                if (AudioSource != null && !AudioSource.isPlaying)
                {
                    IsPlaying = false;
                }

                return !IsPlaying;
            }
        }
        
        #region Unity Events

        private void Awake()
        {
            if (AudioSource == null)
            {
                AudioSource             = GetComponent<AudioSource>();
                AudioSource.playOnAwake = false;
            }
        }

        #endregion

        #region Public Methods

        

        #endregion
        public void Play(AudioClip clip, float volume)
        {
            if (AudioSource != null)
            {
                AudioSource.clip         = clip;
                AudioSource.name         = clip.name;
                AudioSource.spatialBlend = 0f;
                AudioSource.playOnAwake  = false;
                AudioSource.volume       = volume;
                IsPlaying                = true;
                AudioSource.Play();
            }
        }
    }
}