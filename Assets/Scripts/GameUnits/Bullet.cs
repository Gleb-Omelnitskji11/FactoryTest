using ConfigData;
using Core;
using DG.Tweening;
using UnityEngine;

namespace GameUnits
{
    public class Bullet : BasicProjectile
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private TrailRenderer _trailRenderer;
        
        private Tweener _tween;


        public void StartMovement(Vector3 forward)
        {
            _trailRenderer.Clear();
            _tween?.Kill();
            Vector3 newPos = transform.position + forward * ProjectileModel.ProjectSpeed * ProjectileModel.ProjectLifetime;
            _tween = transform.DOMove(newPos, ProjectileModel.ProjectLifetime).SetEase(Ease.Linear);
            _tween.OnComplete(Deactivate);
        }
        
        public override void Deactivate()
        {
            base.Deactivate();
            _tween?.Kill();
        }
        
        public override void Pause(bool pause)
        {
            base.Pause(pause);
            if (pause) _tween?.Pause();
            else _tween?.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Enemy))
            {
                if (other.TryGetComponent<BasicEnemy>(out BasicEnemy enemy))
                {
                    enemy.TakeDamage(ProjectileModel.DamageShoot);
                }

                Deactivate();
            }
        }
    }
}