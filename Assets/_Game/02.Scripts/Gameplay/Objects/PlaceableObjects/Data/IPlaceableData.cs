using System.Collections.Generic;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public interface IPlaceableData
    {
        /// <summary>
        /// Id of Object in Object Dictionary
        /// </summary>
        public int ObjectID { get; }
        
        /// <summary>
        /// Size that object will occupy in grid
        /// Ex: Cube with size 2x2 will occupy 4 cells in grid
        /// </summary>
        public Vector2Int Size { get; }
        
        /// <summary>
        /// List of all grid positions that object will occupy
        /// </summary>
        public List<Vector3Int> OccupiedGridPositions { get; }
    }
}