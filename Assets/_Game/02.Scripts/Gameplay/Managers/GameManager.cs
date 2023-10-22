using System;
using System.Collections.Generic;
using _Game._02.Scripts.Core;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public class GameManager : MonoPersistentSingleton<GameManager>, IEventListener<DestructionEvent>
    {
        #region Fields
        
        [SerializeField] private ObjectsDatabaseSO objectsDatabaseSo;
        [SerializeField] private InventoryManager  inventoryManager;

        // Contain data of grid that define which cell is occupied by which object
        private       IGridData    _gridData;
        private       ISaveManager _saveManager;
        private const string       SavePath = "SaveData";

        #endregion

        #region Properties

        public IGridData        GridData         => _gridData;
        public List<ObjectData> ObjectsData      => objectsDatabaseSo.objectsData;
        public InventoryManager InventoryManager => inventoryManager;

        #endregion

        #region Unity Methods

        // In this Test Project, we control the flow of system initialization from GameManager in this Method
        // In a scalable project, we can use Boostrap & Service Locator pattern to control the flow of system initialization
        private void OnEnable()
        {
            InitializeSaveSystem();
            InitializeGridData();
            InitializeInventorySystem();
            InitializeObjectPool();
            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _saveManager.Save(_gridData.GetSaveData());
            _saveManager.Save(inventoryManager.GetSaveData());
        }

        private void OnApplicationQuit()
        {
            _saveManager.Save(_gridData.GetSaveData());
            _saveManager.Save(inventoryManager.GetSaveData());
        }

        private void Update()
        {
            // Close the game when click escape button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// We initialize the save system here with the path to save data and the stream type
        /// </summary>
        private void InitializeSaveSystem()
        {
            _saveManager = new SaveManager();
            _saveManager.Initialize(Application.persistentDataPath + "/" + SavePath, new JsonStream());
        }
        
        /// <summary>
        /// We initialize the grid data with the save data from the save manager
        /// </summary>
        private void InitializeGridData()
        {
            _gridData = new BaseGridData();
            if (_saveManager.Load<BaseGridSaveData>() != default)
                _gridData.LoadSaveData(_saveManager.Load<BaseGridSaveData>());
        }

        /// <summary>
        /// We initialize the inventory system with the save data from the save manager
        /// </summary>
        private void InitializeInventorySystem()
        {
            inventoryManager.Initialize(_saveManager.Load<InventorySaveData>());
        }
        
        /// <summary>
        /// We initialize the object pool with the data from inventory manager after inventory manager is initialized
        /// </summary>
        private void InitializeObjectPool()
        {
            foreach (var (item, amount) in inventoryManager.Inventory)
            {
                var objectData = item as ObjectData;
                if (objectData == null) continue;
                ObjectPoolingManager.Instance.AddObjectPoolToDictionary(objectData.ID, amount, objectData.Prefab);
            }
        }
        
        #endregion

        #region Events

        /// <summary>
        /// We listen to the destruction event to remove the object from the grid data logically
        /// </summary>
        /// <param name="destructionEvent"></param>
        public void OnReceiveEvent(DestructionEvent destructionEvent)
        {
            if (_gridData.GridData.TryGetValue(destructionEvent.CellPosition, out var objectData))
            {
                if (objectData.ObjectID != destructionEvent.ObjectData.ID) return;
                _gridData.RemoveObjectAt(destructionEvent.CellPosition);
            }
        }

        #endregion
    }
}