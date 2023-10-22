using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    [Serializable]
    public class BaseGridData : IGridData
    {
        #region Properties
    
        public Dictionary<Vector3Int, IPlaceableData> GridData { get; protected set; } = new();

        #endregion

        #region Public Methods

        public void AddObjectAt(Vector3Int gridPosition, Vector2Int size, int objectID)
        {
            // We calculate the positions that the object will occupy in the grid.
            List<Vector3Int> positionToOccupy = CalculateOccupiedPositions(gridPosition, size);
        
            // We create the data of the object that will occupy the grid.
            IPlaceableData data = new BasePlaceableData(positionToOccupy, size, objectID);
        
            // We update the grid data with the new object and fill all occupied positions.
            foreach (var pos in positionToOccupy)
            {
                if (GridData.ContainsKey(pos))
                {
                    Debug.LogError($"Grid Data already contain this cell position: {gridPosition}");
                    return;
                }
                GridData[pos] = data;
            }
        }
    
        public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int size)
        {
            // We calculate the positions that the object will occupy in the grid.
            List<Vector3Int> positionToOccupy = CalculateOccupiedPositions(gridPosition, size);
        
            // If there is any position that is already occupied, we return false.
            foreach (var pos in positionToOccupy)
            {
                if (GridData.ContainsKey(pos))
                    return false;
            }
            return true;
        }

        public void RemoveObjectAt(Vector3Int gridPosition)
        {
            if (GridData.ContainsKey(gridPosition))
                GridData.Remove(gridPosition);
            else
                Debug.LogError($"Grid Data does not contain this cell position: {gridPosition}");
        }

        public Vector3Int GetObjectPosition(int objectID)
        {
            foreach (var (position, placeableData) in GridData)
            {
                if (placeableData.ObjectID == objectID)
                    return position;
            }
            throw new Exception($"Object with ID {objectID} not found in Grid Data");
        }

        public BaseGridSaveData GetSaveData()
        {
            var saveData = new BaseGridSaveData();
            foreach (var (gridPosition, placeableData) in GridData)
            {
                saveData.OccupiedGridPositions.Add(gridPosition);
                saveData.PlaceableData.Add(placeableData);
            }

            return saveData;
        }

        public void LoadSaveData(BaseGridSaveData saveData)
        {
            for (int i = 0; i < saveData.OccupiedGridPositions.Count; i++)
            {
                GridData.TryAdd(saveData.OccupiedGridPositions[i], saveData.PlaceableData[i]);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Use this method to calculate the positions that an object will occupy in the grid.
        /// </summary>
        /// <param name="gridPosition"></param>
        /// <param name="objectSize"></param>
        /// <returns></returns>
        private List<Vector3Int> CalculateOccupiedPositions(Vector3Int gridPosition, Vector2Int objectSize)
        {
            List<Vector3Int> occupiedPositions = new();
            for (int x = 0; x < objectSize.x; x++)
            {
                for (int y = 0; y < objectSize.y; y++)
                {
                    // We add the occupied position to the list based on the grid position and the size of the object.
                    occupiedPositions.Add(gridPosition + new Vector3Int(x, 0, y));
                }
            }
            return occupiedPositions;
        }

        #endregion
    
    
    }
}