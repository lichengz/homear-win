namespace HomeAR.Events
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}