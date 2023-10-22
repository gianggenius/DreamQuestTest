namespace _Game._02.Scripts.Core
{
    public interface IEventListener
    {
        
    }
    public interface IEventListener<T> : IEventListener
    {
        void OnReceiveEvent( T eventType );
    }
}