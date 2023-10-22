using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    public struct PlacementSuccessEvent
    {
        public ObjectData ObjectData;
        public Vector3Int CellPosition;
        public bool       IsNewSpawn;
    }
}