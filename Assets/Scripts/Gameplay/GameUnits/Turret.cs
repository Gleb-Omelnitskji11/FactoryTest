using ConfigData;
using Gameplay.GameServices;
using UnityEngine;
using Zenject;

namespace Gameplay.GameUnits
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Transform _bulletSpawnPoint;
        private GameObject _turretObj;
    
        private TurretModel _turretModel;
        private Quaternion _targetRotation = Quaternion.identity;
        private float _shootTimer;
        private bool _stop = true;
        
        private IInputProvider _input;
        private ProjectileSpawner _projectileSpawner;
        private GameInstance _gameInstance;

        [Inject]
        private void Construct(IInputProvider input, ProjectileSpawner projectileSpawner, GameInstance gameInstance)
        {
            _gameInstance = gameInstance;
            _input = input;
            _projectileSpawner = projectileSpawner;
        }

        private void Start()
        {
            _gameInstance.OnUpdate += OnUpdate;
        }

        private void OnDestroy()
        {
            _gameInstance.OnUpdate -= OnUpdate;
        }

        public void CreateObject(TurretModel turretModel)
        {
            if (turretModel.TurretType == _turretModel?.TurretType && _turretObj != null) return;
            if(_turretObj != null) Destroy(_turretObj.gameObject);

            _turretObj = Instantiate(turretModel.TurretPrefab, transform);
            _turretObj.transform.localPosition = Vector3.zero;
            _turretObj.transform.rotation = Quaternion.identity;
            _turretModel = turretModel;

        }

        public void Init(TurretModel model)
        {
            _turretModel = model;
            _projectileSpawner.SetDataForLevel(_turretModel.ProjectileTypes);
        }

        public void Stop()
        {
            _stop = true;
        }

        public void Resume()
        {
            _stop = false;
        }

        public void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
        }

        private void OnUpdate()
        {
            if (_stop)
                return;
        
            HandleRotation();
            HandleShooting();
        }

        private void HandleShooting()
        {
            _shootTimer += Time.deltaTime;

            if (_shootTimer < _turretModel.FireDelay)
                return;

            Shot();
            _shootTimer = 0f;
        }

        protected virtual void Shot()
        {
            Bullet bullet = _projectileSpawner.SpawnBullet<Bullet>();
            bullet.transform.position = _bulletSpawnPoint.position;
            bullet.transform.rotation = transform.rotation;
            bullet.Activate();
            bullet.StartMovement(_bulletSpawnPoint.forward);
        }

        private void HandleRotation()
        {
            if (!_input.TryGetInputPosition(out Vector3 targetPoint))
                return;

            Vector3 direction = targetPoint - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude < 0.01f)
                return;

            _targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                _targetRotation,
                _turretModel.RotationSpeed * Time.deltaTime
            );
        }
    }
}