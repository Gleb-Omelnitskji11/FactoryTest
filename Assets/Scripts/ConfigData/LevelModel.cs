using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public struct LevelModel
    {
        [SerializeField] private float _distance;
        [SerializeField] private EnemyType[] _enemyTypes;
        [SerializeField] private int _startCount;
        [SerializeField] private float _enemyDelay;
    
        public float Distance => _distance;
        public int StartEnemyCount => _startCount;
        public float EnemyDelay => _enemyDelay;
        public ReadOnlyCollection<EnemyType> EnemyTypes => Array.AsReadOnly(_enemyTypes);
    }
}