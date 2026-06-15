using System;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class UnitModel
    {
        [SerializeField] private int _maxHp;
        [SerializeField] private int _collisionDamage;
        [SerializeField] private float _speed;

        public int MaxHp => _maxHp;
        public int CollisionDamage => _collisionDamage;
        public float Speed => _speed;
    }

    [Serializable]
    public class EnemyUnitModel : UnitModel
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private int _points = 1;
        public EnemyType EnemyType => _type;
        public int Points => _points;
    }
}