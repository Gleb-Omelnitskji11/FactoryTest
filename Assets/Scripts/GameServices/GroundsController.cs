using Core;
using Core.BusEvents;
using GameUnits;
using UnityEngine;
using Zenject;

namespace GameServices
{
    public class GroundsController : MonoBehaviour
    {
        [SerializeField] private Transform[] _grounds;

        private const float GroundStart = 0f;
        private const float MoverOffset = 75f;
        private const float ZTriggerOffset = -30f;
    
        private Transform _car;
        private int _additionalGroundIndex = 1;
        private int _currentGroundIndex;
        private float _nextZPoint;
        private bool _active;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(PlayerCar car, IEventBus eventBus)
        {
            _car = car.transform;
            _eventBus = eventBus;
        }

        private void Start()
        {
            _eventBus.Subscribe<GameResultEvent>(OnGameResult);
            _eventBus.Subscribe<RestartEvent>(Restart);
            _eventBus.Subscribe<PauseEvent>(OnPauseResult);
        }
        
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameResultEvent>(OnGameResult);
            _eventBus.Unsubscribe<RestartEvent>(Restart);
            _eventBus.Unsubscribe<PauseEvent>(OnPauseResult);
        }
        
        private void Update()
        {
            if (!_active) return;

            if (_car.position.z >= _nextZPoint)
            {
                Vector3 requiredPosition = _grounds[_currentGroundIndex].position;
                requiredPosition.z += MoverOffset;
                _grounds[_additionalGroundIndex].position = requiredPosition;

                (_additionalGroundIndex, _currentGroundIndex) = (_currentGroundIndex, _additionalGroundIndex);
                _nextZPoint += MoverOffset;
            }
        }

        private void OnGameResult(GameResultEvent gameResultEvent)
        {
            OnPause(false);
        }
        
        private void OnPauseResult(PauseEvent pauseEvent)
        {
            OnPause(pauseEvent.IsPause);
        }

        private void Restart(RestartEvent restartEvent)
        {
            _grounds[0].position = new Vector3(0f, 0f, GroundStart);
            _grounds[1].position = new Vector3(0f, 0f, MoverOffset);
            _currentGroundIndex = 0;
            _additionalGroundIndex = 1;
            _nextZPoint = ZTriggerOffset;
            _active = true;
        }
    
        private void OnPause(bool active) => _active = !active;
    }
}