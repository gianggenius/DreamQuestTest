namespace _Game._02.Scripts.Core
{
    /// <summary>
    /// Marker interface for poolable objects
    /// </summary>
    public interface IPoolableObject
    {
        public void Active();
        public void Destroy();
    }
}