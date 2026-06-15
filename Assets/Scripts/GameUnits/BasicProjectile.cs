using ConfigData;
using Core.ObjectPool;
using UnityEngine;

namespace GameUnits
{
    public class BasicProjectile : MonoBehaviour, IPooledObject
    {
        protected ProjectaleModel ProjectileModel;
        public ProjectileType ProjectileType => ProjectileModel.ProjectileType;
        public bool IsActive { get; protected set; }
        public string PoolKey { get; protected set; }
        public IObjectPooler Pooler { get; protected set; }
        
        public void SetPoolData(IObjectPooler pooler, string poolKey)
        {
            PoolKey = poolKey;
            Pooler = pooler;
        }

        public void SetProjectileModel(ProjectaleModel model)
        {
            ProjectileModel = model;
        }

        public virtual void Deactivate()
        {
            Pooler.Release<BasicProjectile, ProjectaleModel>(PoolKey, this);
            IsActive = false;
            gameObject.SetActive(false);
        }

        public virtual void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
        }
        
        public virtual void Pause(bool pause)
        {
        }

        public virtual void Reset(){}
    }
}