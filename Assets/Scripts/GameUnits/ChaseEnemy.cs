using ConfigData;
using UnityEngine;

namespace GameUnits
{
    public class ChaseEnemy : BasicEnemy
    {
        private static readonly int Chase = Animator.StringToHash("Chase");
        private static readonly int Agro = Animator.StringToHash("Agro");

        [SerializeField] private float _agroDistance = 10f;
        [SerializeField] private float _moveSpeed = 4f;

        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _rotateTransform;

        private bool _isChasing;

        public override void InitEnemyModel(EnemyUnitModel model, Transform carTransform)
        {
            base.InitEnemyModel(model, carTransform);
            SetIdle();
        }

        public override void Reset()
        {
            base.Reset();
            _isChasing = false;
            SetIdle();
        }

        protected override void OnUpdate()
        {
            if (!IsActive) return;

            if (IsFar())
            {
                Deactivate();
                return;
            }

            if (!_isChasing)
            {
                float sqrDistance = (Player.position - transform.position).sqrMagnitude;
                if (sqrDistance <= _agroDistance * _agroDistance)
                {
                    StartChase();
                }
            }

            if (_isChasing)
            {
                MoveToPlayer();
            }
        }

        public override void PauseChanged(bool paused)
        {
            base.PauseChanged(paused);
            _animator.speed = IsActive ? 1f : 0f;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            if (IsActive && !_isChasing)
            {
                StartChase();
            }
        }

        private void StartChase()
        {
            _isChasing = true;
            _animator.SetBool(Chase, true);
            if(_animator.GetCurrentAnimatorStateInfo(0).tagHash != Agro)
                _animator.Play(Agro);
        }

        private void SetIdle()
        {
            _isChasing = false;
            _animator.SetBool(Chase, false);
        }

        private void MoveToPlayer()
        {
            Vector3 direction = Player.position - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude < 0.01f)
                return;

            direction.Normalize();

            transform.position += direction * _moveSpeed * Time.deltaTime;

            Vector3 localDirection = transform.InverseTransformDirection(direction);
            _rotateTransform.localRotation = Quaternion.LookRotation(localDirection);
        }

        protected override void Died()
        {
            SetIdle();
            base.Died();
        }
    }
}