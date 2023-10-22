using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace _Game._02.Scripts.Core
{
    /// <summary>
    /// Simple class to manage object pooling by using Unity's ObjectPool and saving different pools in a dictionary
    /// </summary>
    public class ObjectPoolingManager : MonoPersistentSingleton<ObjectPoolingManager>
    {
        #region Fields

        private Dictionary<int, ObjectPool<MonoPoolableObject>> _objectPoolDictionary = new();
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize a new ObjectPool and add it to the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size"></param>
        /// <param name="prefab"></param>
        public void AddObjectPoolToDictionary(int key, int size, MonoPoolableObject prefab)
        {
            var poolParent = new GameObject($"{prefab.name}_Pool");
            poolParent.transform.parent = transform;
            var pool = new ObjectPool<MonoPoolableObject>(() => Instantiate(prefab, poolParent.transform), OnGetPooledObject, OnPutBackInPool, defaultCapacity: size);
            _objectPoolDictionary.TryAdd(key, pool);
        }

        /// <summary>
        /// Get an object from the pool by object key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public MonoPoolableObject GetObjectFromPool(int key)
        {
            return _objectPoolDictionary[key].Get();
        }

        /// <summary>
        /// Return an object to the pool by object key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prefab"></param>
        public void ReturnObjectToPool(int key, MonoPoolableObject prefab)
        {
            _objectPoolDictionary[key].Release(prefab);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Callback for when the object is retrieved from the pool
        /// </summary>
        /// <param name="pooledObj"></param>
        private void OnGetPooledObject(MonoPoolableObject pooledObj)
        {
            pooledObj.Active();
        }

        /// <summary>
        /// Callback for when the object is returned to the pool
        /// </summary>
        /// <param name="pooledObj"></param>
        private void OnPutBackInPool(MonoPoolableObject pooledObj)
        {
            pooledObj.Destroy();
        }

        #endregion
       
    }
}