namespace _Game._02.Scripts.Gameplay
{
    public struct PlacementEvent
    {
        public ObjectData       ObjectData;
        public bool             IsDragging;
        public bool             IsNewSpawn;
        public ObjectController TargetObject;
    }
}