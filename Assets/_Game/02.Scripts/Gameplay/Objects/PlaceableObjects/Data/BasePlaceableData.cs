using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    [Serializable]
    public class BasePlaceableData : IPlaceableData
    {
        public int              ObjectID              { get; protected set; }
        public Vector2Int       Size                  { get; protected set; }
        public List<Vector3Int> OccupiedGridPositions { get; protected set; }
        
        public BasePlaceableData(List<Vector3Int> occupiedGridPositions, Vector2Int size, int objectID)
        {
            OccupiedGridPositions = occupiedGridPositions;
            ObjectID              = objectID;
            Size                  = size;
        }
    }
}