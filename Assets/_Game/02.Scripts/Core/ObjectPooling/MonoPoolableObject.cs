using System;
using UnityEngine;

namespace _Game._02.Scripts.Core
{
    public class MonoPoolableObject : MonoBehaviour, IPoolableObject
    {
        public virtual void Active()
        {
            gameObject.SetActive(true);
        }
        public virtual void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}