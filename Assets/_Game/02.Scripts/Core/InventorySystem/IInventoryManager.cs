using System;
using System.Collections.Generic;

namespace _Game._02.Scripts.Core
{
    public interface IInventoryManager
    {
        /// <summary>
        /// Dictionary of items and their amount
        /// </summary>
        public Dictionary<IItem, int> Inventory { get; }
        
        /// <summary>
        /// Add item to inventory with amount
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void AddItem(IItem item, int amount);
        
        /// <summary>
        /// Remove item from inventory with amount
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns>Remove success or not</returns>
        public bool RemoveItem(IItem item, int amount);
        
        /// <summary>
        /// Event fire when inventory change
        /// </summary>
        public event Action<IInventoryManager> OnInventoryChanged;
        
    }
}