using System;
using GameUnits;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class EnemyModel
    {
        [SerializeField] private BasicEnemy _enemyPrefab;
        [SerializeField] private EnemyUnitModel _unitModel;
        
        public BasicEnemy EnemyPrefab => _enemyPrefab;
        public EnemyType EnemyType => _unitModel.EnemyType;
        public EnemyUnitModel UnitModel => _unitModel;
    }
}