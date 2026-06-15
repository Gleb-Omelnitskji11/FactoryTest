using System.Collections;
using ConfigData;
using Core;
using Core.BusEvents;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace GameUnits
{
    public class PlayerCar : Unit
    {
        [SerializeField] private Turret _turret;
        [SerializeField] private Transform[] _wheels;
        [SerializeField] private TrailRenderer[] _trails;

        private IEnumerator _moveCoroutine;

        private Vector3 _carPosition;
        private Sequence _seq;
        private IEventBus _eventBus;
        private float _goalZ;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void InitUnit(UnitModel model, TurretModel turretModel)
        {
            base.InitUnit(model);
            _turret.Init(turretModel);
            _hpBar.Init(model.MaxHp);
        }

        public void StartLevel(LevelModel levelModel)
        {
            foreach (var trail in _trails)
            {
                trail.Clear();
            }

            _seq?.Kill();
            UpdateDirection(levelModel.Distance);
            _turret.Activate();
        }

        public void Stop()
        {
            _seq?.Pause();
            _turret.Stop();
        }

        public void Resume()
        {
            _seq.Play();
            _turret.Activate();
        }

        public override void TakeDamage(int damageTaken)
        {
            base.TakeDamage(damageTaken);
        }

        protected override void Died()
        {
            GameResultEvent gameResultEvent = new GameResultEvent(false);
            _eventBus.Publish<GameResultEvent>(gameResultEvent);
            Stop();
        }

        private void UpdateDirection(float distance)
        {
            _goalZ = distance + transform.position.z;
            float duration = distance / UnitModel.Speed;
            _seq = DOTween.Sequence();
            _seq.Join(transform.DOMoveZ(_goalZ, duration).SetEase(Ease.Linear));
            foreach (var wheel in _wheels)
            {
                _seq.Join(wheel.DORotate(new Vector3(360f, 0f, 0f), 2f, RotateMode.FastBeyond360));
            }
        }
    }
}