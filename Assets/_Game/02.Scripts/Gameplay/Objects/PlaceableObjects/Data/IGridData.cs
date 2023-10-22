using System.Collections.Generic;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public interface IGridData
    {
        /// <summary>
        /// A dictionary that contains all the occupied grid positions and the data of the object that occupies it.
        /// </summary>
        public Dictionary<Vector3Int, IPlaceableData> GridData { get; }
        
        /// <summary>
        /// Use this method to add an object to the grid.
        /// </summary>
        /// <param name="gridPosition">Position in grid of object desire to add</param>
        /// <param name="size">Size that object will occupy in grid</param>
        /// <param name="objectID">Id of object in Object Dictionary</param>
        public void AddObjectAt(Vector3Int gridPosition, Vector2Int size, int objectID);
        
        /// <summary>
        /// Use this method to check if an object can be placed in the grid.
        /// </summary>
        /// <param name="gridPosition">Position in grid of object desire to check</param>
        /// <param name="size">Size that object will occupy in grid</param>
        /// <returns></returns>
        public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int size);
        
        /// <summary>
        /// Use this method to remove an object from the grid.
        /// </summary>
        /// <param name="gridPosition">Position in grid of object desire to remove</param>
        public void RemoveObjectAt(Vector3Int gridPosition);
        
        /// <summary>
        /// Use this method to get the position of an object in the grid.
        /// </summary>
        /// <param name="objectID">Id of object in Object Dictionary</param>
        /// <returns>Grid position of object</returns>
        public Vector3Int GetObjectPosition(int objectID);
        
        public BaseGridSaveData GetSaveData();
        
        public void LoadSaveData(BaseGridSaveData saveData);
    }
}