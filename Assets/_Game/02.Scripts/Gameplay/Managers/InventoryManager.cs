using System;
using System.Collections.Generic;
using _Game._02.Scripts.Core;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public class InventoryManager : BaseInventoryManager, IEventListener<ObjectItemChangedEvent>, IEventListener<PlacementSuccessEvent>, IEventListener<DestructionEvent>
    {
        [SerializeField] private List<ObjectItem> defaultItems = new();

        private List<ObjectData> _objectData;

        private void OnEnable()
        {
            EventManager.AddListener<ObjectItemChangedEvent>(this);
            EventManager.AddListener<PlacementSuccessEvent>(this);
            EventManager.AddListener<DestructionEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<ObjectItemChangedEvent>(this);
            EventManager.RemoveListener<PlacementSuccessEvent>(this);
            EventManager.RemoveListener<DestructionEvent>(this);
        }

        public void Initialize(InventorySaveData saveData)
        {
            _objectData = GameManager.Instance.ObjectsData;

            if (saveData == default)
            {
                foreach (var objectItem in defaultItems)
                {
                    var objectData = _objectData.Find(data => data.ID == objectItem.ObjectID);
                    if (objectData != null)
                        AddItem(objectData, objectItem.Amount);
                }
            }
            else
            {
                for (var index = 0; index < saveData.ObjectIDs.Count; index++)
                {
                    var objectID   = saveData.ObjectIDs[index];
                    var objectData = _objectData.Find(data => data.ID == objectID);
                    if (objectData != null)
                        AddItem(objectData, saveData.Amounts[index]);
                }
            }
        }

        public InventorySaveData GetSaveData()
        {
            var inventorySaveData = new InventorySaveData();
            foreach (var (item, amount) in Inventory)
            {
                var objectData = item as ObjectData;
                if (objectData == null) continue;
                inventorySaveData.ObjectIDs.Add(objectData.ID);
                inventorySaveData.Amounts.Add(amount);
            }

            return inventorySaveData;
        }

        public void OnReceiveEvent(ObjectItemChangedEvent itemChangedEvent)
        {
            if (itemChangedEvent.Amount > 0)
            {
                AddItem(itemChangedEvent.ObjectItemData, itemChangedEvent.Amount);
            }
            else
            {
                RemoveItem(itemChangedEvent.ObjectItemData, itemChangedEvent.Amount);
            }
        }

        public void OnReceiveEvent(PlacementSuccessEvent placementEvent)
        {
            if (placementEvent is { IsNewSpawn: true})
            {
                RemoveItem(placementEvent.ObjectData, 1);
            }
        }

        public void OnReceiveEvent(DestructionEvent destructionEvent)
        {
            AddItem(destructionEvent.ObjectData, 1);
        }
    }
}