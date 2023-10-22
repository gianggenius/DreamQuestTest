using System;
using System.Collections.Generic;
using _Game._02.Scripts.Core;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public class InventoryManager : BaseInventoryManager, IEventListener<ObjectItemChangedEvent>, IEventListener<PlacementSuccessEvent>, IEventListener<DestructionEvent>
    {
        /// <summary>
        /// Default items that will be added to the inventory when the game starts and no save data is found
        /// </summary>
        [SerializeField] private List<ObjectItem> defaultItems = new();

        // Reference of all object data in the game loaded after GameManager is initialized
        private List<ObjectData> _objectData;

        private void OnEnable()
        {
            // We will need to update inventory when an object is placed or destroyed
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

        /// <summary>
        /// Initialize inventory with save data or default items
        /// </summary>
        /// <param name="saveData"></param>
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

        /// <summary>
        /// Get save data from inventory
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// When receive an item changed event, we will add or remove quantity of item from inventory
        /// </summary>
        /// <param name="itemChangedEvent"></param>
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

        /// <summary>
        /// When receive a placement success event, we will remove the item from inventory
        /// </summary>
        /// <param name="placementEvent"></param>
        public void OnReceiveEvent(PlacementSuccessEvent placementEvent)
        {
            if (placementEvent is { IsNewSpawn: true})
            {
                RemoveItem(placementEvent.ObjectData, 1);
            }
        }

        /// <summary>
        /// When receive a destruction event, we will add the item back to inventory
        /// </summary>
        /// <param name="destructionEvent"></param>
        public void OnReceiveEvent(DestructionEvent destructionEvent)
        {
            AddItem(destructionEvent.ObjectData, 1);
        }
    }
}