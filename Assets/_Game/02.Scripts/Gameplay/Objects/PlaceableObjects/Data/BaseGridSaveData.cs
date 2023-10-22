using System.Collections.Generic;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public class BaseGridSaveData
    {
        public List<Vector3Int>     OccupiedGridPositions = new ();
        public List<IPlaceableData> PlaceableData         = new ();
    }
}