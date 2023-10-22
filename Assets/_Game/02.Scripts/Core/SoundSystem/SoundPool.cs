using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game._02.Scripts.Core
{
    public class SoundPool : MonoBehaviour
    {
        #region Fields

        [SerializeField] private SoundController _soundPrefab;

        private List<SoundController> _playingList = new();
        private List<SoundController> _freeList    = new();

        private List<string> _stoppedIds = new();

        public IReadOnlyList<SoundController> PlayingList => _playingList;

        #endregion

        #region Public Methods

        public void Initialize(int initialSpawn = 10)
        {
            for (int i = 0; i < initialSpawn; i++)
            {
                _freeList.Add(SpawnController());
            }
        }

        public List<SoundController> GetPlayingSound(string id)
        {
            return _playingList.Where(x => x.AudioSource.clip.name == id).ToList();
        }
        
        public SoundController GetFree(bool spawnIfNotFound = true)
        {
            if (_freeList.Count > 0)
            {
                return _freeList[^1];
            }

            if (!spawnIfNotFound)
                return null;
            
            _freeList.Add(SpawnController());
            return _freeList[^1];
        }

        public void SetPlaying(SoundController controller)
        {
            _freeList.Remove(controller);
            _playingList.Add(controller);
            controller.gameObject.SetActive(true);
        }

        public void Update()
        {
            for (var index = _playingList.Count - 1; index >= 0; index--)
            {
                var item = _playingList[index];
                if (item == null || item.gameObject == null)
                {
                    _playingList.RemoveAt(index);
                    continue;
                }

                if (!item.IsStopped) continue;
                _freeList.Add(item);
                _playingList.RemoveAt(index);
                item.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Private Methods

        private SoundController SpawnController()
        {
            if (_soundPrefab != null)
            {
                var controller = GameObject.Instantiate(_soundPrefab, transform);
                controller.transform.localPosition = Vector3.zero;

                return controller;
            }
            else
            {
                var go = new GameObject("[Sound Controller]");
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;

                return go.AddComponent<SoundController>(); 
            }
        }

        #endregion
    }
}