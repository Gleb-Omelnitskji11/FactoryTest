using System;
using GameUnits;

namespace Core.ObjectPool
{
    public interface IObjectPooler
    {
        void CreatePool<T, Y>(string key, Y prefab, Func<Y, T> factory, Action<T> onGet = null, Action<T> onRelease = null, int prewarmCount = 0) where T : IPooledObject;
        T Get<T, Y>(string key) where T : IPooledObject;
        void Release<T, Y>(string key, T instance) where T : IPooledObject;
        void Clear(string key);
        void ClearAll();
    }
}
