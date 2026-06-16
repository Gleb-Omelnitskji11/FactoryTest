using System;
using System.Collections.Generic;
using ConfigData;
using Core;
using Core.BusEvents;
using Core.ObjectPool;
using Gameplay.GameUnits;
using UnityEngine;
using Zenject;

namespace Gameplay.GameServices
{
    public class ProjectileSpawner : IDisposable
    {
        private GameConfig _gameConfig;
        private IObjectPooler _pooler;
        
        private ProjectileModel _projectileModel;
        private ProjectileType _projectileType;
        private IEventBus _eventBus;
        
        private readonly List<BasicProjectile> _projectiles = new List<BasicProjectile>();

        [Inject]
        public void Construct(IObjectPooler pooler, ConfigProvider configProvider, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _pooler = pooler;
            _gameConfig = configProvider.GameConfig;

            Subscribe();
        }
        
        private void Subscribe()
        {
            _eventBus.Subscribe<GameResultEvent>(DeactivateAll);
            _eventBus.Subscribe<PauseEvent>(OnPause);
        }
        public void Dispose()
        {
            _eventBus.Unsubscribe<GameResultEvent>(DeactivateAll);
            _eventBus.Unsubscribe<PauseEvent>(OnPause);
        }

        public void SetDataForLevel(ProjectileType projectileType)
        {
            _projectileType = projectileType;
            InitPool();
        }

        public T SpawnBullet<T>() where T : BasicProjectile
        {
            string key = GetKey(_projectileType);
            BasicProjectile projectile = _pooler.Get<BasicProjectile, ProjectileModel>(key);
            return projectile as T;
        }

        private void OnPause(PauseEvent pauseEvent)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.Pause(pauseEvent.IsPause);
            }
        }

        private void DeactivateAll(GameResultEvent startGameEvent)
        {
            while (_projectiles.Count > 0)
            {
                _projectiles[0].Deactivate();
            }
        }
        
        private void InitPool()
        {
            ProjectileModel model = _gameConfig.GetProjectileModel(_projectileType);
            string key = GetKey(_projectileType);

            _pooler.CreatePool<BasicProjectile, ProjectileModel>(key, model, factory: CreateNewProjectileObj,
                onGet: OnGetFromPool, onRelease: OnRealiseToPool,
                prewarmCount: 0);
        }
        
        private BasicProjectile CreateNewProjectileObj(ProjectileModel model)
        {
            BasicProjectile projectile = GameObject.Instantiate(model.BulletPrefab);
            ProjectileType projectileType = model.ProjectileType;
            string key = GetKey(projectileType);
            projectile.SetProjectileModel(model);
            projectile.SetPoolData(_pooler, key);
            return projectile;
        }
        
        private void OnGetFromPool(BasicProjectile projectile)
        {
            projectile.Reset();
            _projectiles.Add(projectile);
        }
        
        private void OnRealiseToPool(BasicProjectile projectile)
        {
            _projectiles.Remove(projectile);
        }
        
        private string GetKey(ProjectileType type) => $"Projectile_{type}";
    }
}