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


        private       IGridData    _gridData;
        private       ISaveManager _saveManager;
        private const string       SavePath = "SaveData";

        #endregion

        #region Properties

        public IGridData        GridData    => _gridData;
        public List<ObjectData> ObjectsData => objectsDatabaseSo.objectsData;

        public InventoryManager InventoryManager => inventoryManager;

        #endregion

        #region Unity Methods

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

        private void InitializeObjectPool()
        {
            foreach (var (item, amount) in inventoryManager.Inventory)
            {
                var objectData = item as ObjectData;
                if (objectData == null) continue;
                ObjectPoolingManager.Instance.AddObjectPoolToDictionary(objectData.ID, amount, objectData.Prefab);
            }
        }

        private void InitializeGridData()
        {
            _gridData = new BaseGridData();
            if (_saveManager.Load<BaseGridSaveData>() != default)
                _gridData.LoadSaveData(_saveManager.Load<BaseGridSaveData>());
        }

        private void InitializeInventorySystem()
        {
            inventoryManager.Initialize(_saveManager.Load<InventorySaveData>());
        }

        private void InitializeSaveSystem()
        {
            _saveManager = new SaveManager();
            _saveManager.Initialize(Application.persistentDataPath + "/" + SavePath, new JsonStream());
        }

        #endregion

        #region Events

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