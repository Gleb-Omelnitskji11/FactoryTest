using Core.ObjectPool;

namespace GameUnits
{
    public interface IPooledObject
    {
        public bool IsActive { get;}
        public string PoolKey { get;}
        public IObjectPooler Pooler { get;}
        public void SetPoolData(IObjectPooler pooler, string poolKey);

        public void Deactivate();
        public void Activate();
    }
}