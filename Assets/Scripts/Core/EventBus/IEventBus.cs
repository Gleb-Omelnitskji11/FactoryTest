using System;

namespace Core
{
    public interface IEventBus
    {
        public void Subscribe<T>(Action<T> callback) where T : IEvent;
        public void Unsubscribe<T>(Action<T> callback) where T : IEvent;
        public void Publish<T>(T evt) where T : IEvent;
    }
}