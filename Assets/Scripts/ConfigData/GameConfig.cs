using System;
using UnityEngine;

namespace ConfigData
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/UnitsConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private PlayerUnitModel[] _playerUnits = new PlayerUnitModel[1];
        [SerializeField] private TurretModel[] _turretModels = new TurretModel[1];
        [SerializeField] private EnemyModel[] _enemyModels = new EnemyModel[2];
        [SerializeField] private ProjectaleModel[] _projectileModels= new ProjectaleModel[1];
        [SerializeField] private LevelModel[] _levelModels = new LevelModel[0];
        [SerializeField] private LevelModel _defaultLevelModel;

        public LevelModel GetLevelModel(int level)
        {
            return _levelModels.Length <= level ? _defaultLevelModel : _levelModels[level];
        }

        public PlayerUnitModel GetPlayerUnitModel(CarType carType)
        {
            foreach (var car in _playerUnits)
            {
                if(car.CarType == carType)
                    return car;
            }
            
            throw new Exception($"No car found for car type {carType}");
        }
        
        public EnemyModel GetEnemyUnitModel(EnemyType enemyType)
        {
            foreach (var enemy in _enemyModels)
            {
                if(enemy.EnemyType == enemyType)
                    return enemy;
            }
            
            throw new Exception($"No enemy found for enemy type {enemyType}");
        }
        
        public TurretModel GetTurretModel(TurretType turretType)
        {
            foreach (var turret in _turretModels)
            {
                if(turret.TurretType == turretType)
                    return turret;
            }
            
            throw new Exception($"No turret found for type {turretType}");
        }    
        
        public ProjectaleModel GetProjectileModel(ProjectileType projectileType)
        {
            foreach (var projectile in _projectileModels)
            {
                if(projectile.ProjectileType == projectileType)
                    return projectile;
            }
            
            throw new Exception($"No projectile found for type {projectileType}");
        }
    }
}