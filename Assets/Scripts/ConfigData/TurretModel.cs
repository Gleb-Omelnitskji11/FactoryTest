using System;
using GameUnits;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class TurretModel
    {
        [SerializeField] private Turret _turretView;
        [SerializeField] private float _fireDelay;
        [SerializeField] private float _damageShoot;
        [SerializeField] private float _rotationSpeed;

        [SerializeField] private ProjectileType _projectileType;
        [SerializeField] private TurretType _turretType;

        public float FireDelay => _fireDelay;

        public float RotationSpeed => _rotationSpeed;
        public ProjectileType ProjectileTypes => _projectileType;
        public TurretType TurretType => _turretType;
    }
}