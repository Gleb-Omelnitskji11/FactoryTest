using ConfigData;
using Core;
using Core.BusEvents;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Gameplay.GameUnits
{
    public class PlayerCar : Unit
    {
        [SerializeField] private Turret _turret;
        [SerializeField] private Transform[] _wheels;
        [SerializeField] private TrailRenderer[] _trails;

        private Sequence _seq;
        private IEventBus _eventBus;
        private float _goalZ;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void ReplaceTurret(TurretModel turret)
        {
            _turret.CreateObject(turret);
        }

        public void InitUnit(UnitModel model, TurretModel turretModel)
        {
            base.InitUnit(model);
            _turret.Init(turretModel);
            _hpBar.Init(model.MaxHp);
        }

        public void ResetCar()
        {
            foreach (var trail in _trails)
            {
                trail.Clear();
            }

            _seq?.Kill();
            _turret.ResetRotation();
        }

        public void StartLevel(LevelModel levelModel)
        {
            UpdateDirection(levelModel.Distance);
            _turret.Resume();
        }

        public void Stop()
        {
            _seq?.Pause();
            _turret.Stop();
        }

        public void Resume()
        {
            _seq.Play();
            _turret.Resume();
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