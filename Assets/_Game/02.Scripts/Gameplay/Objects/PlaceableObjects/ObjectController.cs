using _Game._02.Scripts.Core;
using _Game._02.Scripts.Gameplay;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public class ObjectController : MonoPoolableObject
    {
        #region Serialized Fields

        [SerializeField] private Color highlightColor;

        #endregion
    
        #region Fields

        private bool       _isDestroyed;
        private bool       _isDragging;
        private ObjectData _objectData;
        private Vector3Int _cellPosition;
        private Renderer   _renderer;

        #endregion

        #region Properties

        public Vector3Int CellPosition => _cellPosition;

        #endregion

        #region Public Methods

        public override void Active()
        {
            base.Active();
            _isDestroyed = false;
            _isDragging  = false;
        }

        public void Populate(ObjectData data)
        {
            _objectData = data;
            ToggleHighlight(true);
        }

        public void SetCellPosition(Vector3Int cellPosition)
        {
            _cellPosition = cellPosition;
            ToggleHighlight(false);
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _renderer = GetComponentInChildren<Renderer>();
        }

        private void OnMouseDown()
        {
            if (_isDragging) return;
            _isDragging = true;
            ToggleHighlight(true);
            EventManager.TriggerEvent(new PlacementEvent()
            {
                IsDragging   = true,
                ObjectData   = _objectData,
                TargetObject = this,
                IsNewSpawn   = false
            });
        }

        private void OnMouseUp()
        {
            if (!_isDragging) return;
            ToggleHighlight(false);
            EventManager.TriggerEvent(new PlacementEvent()
            {
                IsDragging   = false,
                ObjectData   = _objectData,
                TargetObject = this,
                IsNewSpawn = false
            });
            _isDragging = false;
        }

        private void OnMouseOver()
        {
            if (_isDestroyed) return;
            if (!Input.GetMouseButtonDown(1)) return;
            DestroyObject();
        }

        #endregion

        #region Private Methods

        private void DestroyObject()
        {
            _isDestroyed = true;
            EventManager.TriggerEvent(new DestructionEvent()
            {
                ObjectData   = _objectData,
                CellPosition = _cellPosition
            });
            ObjectPoolingManager.Instance.ReturnObjectToPool(_objectData.ID, this);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.ObjectDestroy);
        }
        private void ToggleHighlight(bool isOn)
        {
            if (isOn)
            {
                foreach (var material in _renderer.materials)
                {
                    //We need to enable the EMISSION
                    material.EnableKeyword("_EMISSION");
                    //before we can set the color
                    material.SetColor("_EmissionColor", highlightColor);
                }
            }
            else
            {
                foreach (var material in _renderer.materials)
                {
                    //we can just disable the EMISSION
                    //if we don't use emission color anywhere else
                    material.DisableKeyword("_EMISSION");
                }
            }
        }
        #endregion
    
    }
}