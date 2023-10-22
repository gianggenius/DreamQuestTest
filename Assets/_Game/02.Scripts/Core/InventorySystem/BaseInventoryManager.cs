using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game._02.Scripts.Core
{
    public abstract class BaseInventoryManager : MonoBehaviour, IInventoryManager
    {
        public Dictionary<IItem, int> Inventory { get; protected set; } = new();
        
        public event Action<IInventoryManager> OnInventoryChanged;
        public virtual void AddItem(IItem item, int amount)
        {
            if (Inventory.ContainsKey(item))
            {
                Inventory[item] += amount;
            }
            else
            {
                Inventory.Add(item, amount);
            }
            OnInventoryChanged?.Invoke(this);
        }

        public virtual bool RemoveItem(IItem item, int amount)
        {
            if (Inventory.TryGetValue(item, out var currentAmount))
            {
                if(currentAmount >= amount)
                {
                    Inventory[item] -= amount;
                    OnInventoryChanged?.Invoke(this);
                    return true;
                }
            }
            return false;
        }

    }
}