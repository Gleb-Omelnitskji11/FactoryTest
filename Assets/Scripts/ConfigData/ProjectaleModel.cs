using System;
using GameUnits;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class ProjectaleModel
    {
        [SerializeField] private int _damageShoot;
        [SerializeField] private float _projectSpeed;
        [SerializeField] private float _projectLifetime;
        [SerializeField] private ProjectileType _projectileType;
        [SerializeField] private BasicProjectile _bulletPrefab;

        public int DamageShoot => _damageShoot;
        public float ProjectSpeed => _projectSpeed;
        public float ProjectLifetime => _projectLifetime;

        public BasicProjectile BulletPrefab => _bulletPrefab;
        public ProjectileType ProjectileType => _projectileType;
    }
}