using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game._02.Scripts.Core
{
    public class SoundManager : MonoPersistentSingleton<SoundManager>
    {
        [field: SerializeField] public  SoundPool         SoundPool { get; private set; }
        [SerializeField]        private List<SoundConfig> soundConfigs;

        [Serializable]
        public class SoundConfig
        {
            public SoundType SoundType;
            public AudioClip AudioClip;
        }

        public enum SoundType
        {
            ButtonClick,
            ObjectSpawn,
            ObjectDestroy,
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (SoundPool != null)
            {
                SoundPool.Initialize();
            }
        }

        public void PlaySound(SoundType soundType)
        {
            var soundConfig = soundConfigs.Find(obj => obj.SoundType == soundType);
            if (soundConfig == null) return;
            var soundController = SoundPool.GetFree();
            if (soundController == null) return;
            soundController.Play(soundConfig.AudioClip, 1f);
        }
    }
}