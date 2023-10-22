using System;
using System.Linq;
using _Game._02.Scripts.Core;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public class PlacementManager : MonoBehaviour, IEventListener<PlacementEvent>
    {
        #region Serialize Fields

        [SerializeField] private Camera    sceneCamera;
        [SerializeField] private Grid      grid;
        [SerializeField] private LayerMask placementLayerMask;
        [SerializeField] private LayerMask objectLayerMask;

        #endregion

        #region Private Fields

        // A simple state machine to separate logic of spawner's different states
        private BasicStateMachine<PlacementState> _stateMachine;
        private IGridData                         _gridData;

        // Temporary Data
        private ObjectData       _selectedObjectData;
        private bool             _isNewSpawn;
        private ObjectController _spawnedObject;
        private Vector3Int       _lastCellPosition;

        [Serializable]
        public enum PlacementState
        {
            Initialize,
            Dragging,
            Spawning,
            Spawned
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            // We initialize the state machine with the initial state
            _stateMachine = new BasicStateMachine<PlacementState>(true);
            _stateMachine.ChangeState(PlacementState.Initialize);

            // Get the grid data from the game manager
            _gridData = GameManager.Instance.GridData;
            if (_gridData.GridData.Count > 0)
            {
                SpawnSavedData();
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
        }

        private void Update()
        {
            switch (_stateMachine.CurrentState)
            {
                case PlacementState.Dragging:
                    HandleDraggingLogic();
                    break;
                case PlacementState.Spawning:
                    HandleSpawningLogic();
                    break;
                case PlacementState.Spawned:
                    HandleSpawnedLogic();
                    break;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handle the logic of spawning object.
        /// In this demo, we simply change the state to Spawned but we can handle animations, vfx, timing, etc. here.
        /// </summary>
        private void HandleSpawningLogic()
        {
            _stateMachine.ChangeState(PlacementState.Spawned);
        }

        /// <summary>
        /// Handle the logic after player decide position of the object.
        /// We spawn the object on the grid then reset temporary data and change state to Initialize.
        /// </summary>
        private void HandleSpawnedLogic()
        {
            // We spawn the object on the grid logically
            _gridData.AddObjectAt(_lastCellPosition, _selectedObjectData.Size, _selectedObjectData.ID);
            EventManager.TriggerEvent(new PlacementSuccessEvent()
            {
                ObjectData   = _selectedObjectData,
                CellPosition = _lastCellPosition,
                IsNewSpawn   = _isNewSpawn
            });

            // Change the layer of the object to Object layer so that we can interact with it
            _spawnedObject.gameObject.layer = LayerMask.NameToLayer("Object");
            _spawnedObject.SetCellPosition(_lastCellPosition);
            
            // Play spawn sound
            SoundManager.Instance.PlaySound(SoundManager.SoundType.ObjectSpawn);
            
            // Reset temporary data and change state to Initialize
            ResetTemporaryData();
            _stateMachine.ChangeState(PlacementState.Initialize);
        }

        /// <summary>
        /// Handle the logic of dragging object.
        /// </summary>
        private void HandleDraggingLogic()
        {
            // If we don't have any spawned object, we will spawn one
            if (_spawnedObject == null)
            {
                _spawnedObject = ObjectPoolingManager.Instance.GetObjectFromPool(_selectedObjectData.ID) as ObjectController;
                if (_spawnedObject == null) return;
                _spawnedObject.Populate(_selectedObjectData);
            }

            if (_gridData.GridData.ContainsKey(_spawnedObject.CellPosition))
                _gridData.RemoveObjectAt(_spawnedObject.CellPosition);
            // Change the layer of the object to Default layer so that it won't interact with itself
            _spawnedObject.gameObject.layer = LayerMask.NameToLayer("Default");
            var position = GetPositionOfPointerByLayerMask(objectLayerMask);
            // If pointer is on other object, we will spawn object on top of that object
            if (!position.Equals(Vector3.negativeInfinity))
            {
                position = new Vector3(position.x, position.y, position.z + 1);
                PlaceObjectVisualAtPosition(position);
                return;
            }

            // If pointer is not on other object, we will spawn object on grid
            position = GetPositionOfPointerByLayerMask(placementLayerMask);
            if (!position.Equals(Vector3.negativeInfinity))
            {
                PlaceObjectVisualAtPosition(position);
            }
        }

        /// <summary>
        /// Reset the temporary data to handle logic of other object spawning.
        /// </summary>
        private void ResetTemporaryData()
        {
            _spawnedObject      = null;
            _selectedObjectData = null;
            _lastCellPosition   = Vector3Int.zero;
            _isNewSpawn         = false;
        }

        /// <summary>
        /// Place the spawned object to available cell position and save the last cell position.
        /// </summary>
        /// <param name="position">World position that player clicked on</param>
        private void PlaceObjectVisualAtPosition(Vector3 position)
        {
            var cellPosition = grid.WorldToCell(position);
            // Check if we can place object at this position
            if (_gridData.CanPlaceObjectAt(cellPosition, _selectedObjectData.Size))
            {
                _spawnedObject.transform.position = grid.CellToWorld(cellPosition);
                _lastCellPosition                 = cellPosition;
            }
        }

        /// <summary>
        /// Get the position of Pointer clicked on specific Layermask
        /// </summary>
        /// <param name="layerMask">Layer Mask to Ray cast</param>
        /// <returns>Position on Layermask that pointer clicked
        /// Return Vector3.negativeInfinity if pointer is not on Layermask
        /// </returns>
        private Vector3 GetPositionOfPointerByLayerMask(LayerMask layerMask)
        {
            var        lastPosition = Vector3.negativeInfinity;
            Vector3    mousePos     = Input.mousePosition;
            Ray        ray          = sceneCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                lastPosition = hit.point;
            }

            return lastPosition;
        }

        /// <summary>
        /// Spawn objects that already in save file
        /// </summary>
        private void SpawnSavedData()
        {
            foreach (var (gridPosition, placeableData) in _gridData.GridData)
            {
                // Get the object data from the object database
                var objectData = GameManager.Instance.ObjectsData.FirstOrDefault(obj => obj.ID == placeableData.ObjectID);
                if (objectData == null) continue;

                // Get the object from the object pool
                var objectController = ObjectPoolingManager.Instance.GetObjectFromPool(objectData.ID) as ObjectController;
                if (objectController == null) continue;

                // Populate the object with data and set its position
                objectController.Populate(objectData);
                objectController.SetCellPosition(gridPosition);
                objectController.gameObject.layer   = LayerMask.NameToLayer("Object");
                objectController.transform.position = grid.CellToWorld(gridPosition);
            }
        }

        #endregion

        #region Events Handler

        public void OnReceiveEvent(PlacementEvent placementEvent)
        {
            _selectedObjectData = placementEvent.ObjectData;
            _isNewSpawn         = placementEvent.IsNewSpawn;
            switch (placementEvent.IsDragging)
            {
                case false:
                    // If we stop dragging and current state is dragging, we change state to spawning
                    if (_stateMachine.CurrentState == PlacementState.Dragging)
                        _stateMachine.ChangeState(PlacementState.Spawning);
                    break;
                case true:
                    // If we start dragging and current state is initialize, we change state to dragging
                    if (_stateMachine.CurrentState == PlacementState.Initialize)
                    {
                        _stateMachine.ChangeState(PlacementState.Dragging);
                        _spawnedObject = placementEvent.TargetObject;
                    }

                    break;
            }
        }

        #endregion
    }
}