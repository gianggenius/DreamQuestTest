using System;
using _Game._02.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game._02.Scripts.Gameplay
{
    public class ObjectSpawnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Serialized Fields

        [Header("Debug Data")] [SerializeField]
        private ObjectData data;

        [Header("References")] [SerializeField]
        private Transform objectParent;

        [SerializeField] private TextMeshProUGUI objectQuantity;

        [Header("Settings")] [SerializeField] private float objectScaleFactor;

        private bool             _isPointerDown;
        private InventoryManager _inventoryManager;
        private bool             _canSpawn => _inventoryManager.Inventory[data] > 0;

        #endregion


        #region Public Methods

        public void Populate(ObjectData objectData)
        {
            data = objectData;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!_canSpawn) return;
            _isPointerDown = true;
            EventManager.TriggerEvent(new PlacementEvent()
            {
                ObjectData = data,
                IsDragging = true,
                IsNewSpawn = true
            });
            SoundManager.Instance.PlaySound(SoundManager.SoundType.ButtonClick);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPointerDown = false;
            EventManager.TriggerEvent(new PlacementEvent()
            {
                ObjectData = data,
                IsDragging = false,
                IsNewSpawn = true
            });
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            _inventoryManager                    =  GameManager.Instance.InventoryManager;
            _inventoryManager.OnInventoryChanged += OnInventoryChanged;
            if (data != null)
            {
                SetupButtonData();
            }
        }

        private void OnDestroy()
        {
            _inventoryManager.OnInventoryChanged -= OnInventoryChanged;
        }

        #endregion

        #region Private Methods

        private void SetupButtonData()
        {
            objectParent.DestroyAllChildren();
            var uiObject = Instantiate(data.Prefab, objectParent);
            uiObject.gameObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            uiObject.transform.localScale *= objectScaleFactor;
            uiObject.gameObject.AddComponent<AutoRotate>();
            Destroy(uiObject);
            UpdateQuantityText();
        }

        private void UpdateQuantityText()
        {
            objectQuantity.text = _inventoryManager.Inventory[data].ToString();
        }

        private void OnInventoryChanged(IInventoryManager inventory)
        {
            UpdateQuantityText();
        }

        #endregion
    }
}