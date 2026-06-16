using Core;
using Core.BusEvents;
using UnityEngine;
using Zenject;

namespace Gameplay.GameServices
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
        private PlayerProvider _playerProvider;
        private GameInstance _gameInstance;

        [Inject]
        public void Construct(PlayerProvider playerProvider, IEventBus eventBus, GameInstance gameInstance)
        {
            _gameInstance = gameInstance;
            _playerProvider = playerProvider;
            _eventBus = eventBus;
        }

        private void Start()
        {
            _eventBus.Subscribe<GameResultEvent>(OnGameResult);
            _eventBus.Subscribe<StartGameClickedEvent>(ResetData);
            _eventBus.Subscribe<PauseEvent>(OnPauseResult);
            
            _gameInstance.OnUpdate += OnUpdate;
        }
        
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameResultEvent>(OnGameResult);
            _eventBus.Unsubscribe<StartGameClickedEvent>(ResetData);
            _eventBus.Unsubscribe<PauseEvent>(OnPauseResult);
            _gameInstance.OnUpdate -= OnUpdate;
        }
        
        private void OnUpdate()
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
            _active = false;
        }
        
        private void OnPauseResult(PauseEvent pauseEvent)
        {
            OnPause(pauseEvent.IsPause);
        }

        private void ResetData(StartGameClickedEvent eventData)
        {
            _car = _playerProvider.PlayerCar.transform;
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