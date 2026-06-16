using Core;
using Core.BusEvents;
using UnityEngine;
using Zenject;

namespace Gameplay.GameServices
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Vector3 _gamePositionOffSet = new Vector3(0f, 10.13f, -8.14f);
        
        private Vector3 _currentPosition;
        private Transform _car;
        private PlayerProvider _playerProvider;
        private IEventBus _eventBus;
        private bool _follow;

        [Inject]
        public void Construct(PlayerProvider playerProvider, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _playerProvider = playerProvider;
            _eventBus.Subscribe<RestartEvent>(UpdatePlayerCar);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<RestartEvent>(UpdatePlayerCar);
        }

        private void UpdatePlayerCar(RestartEvent restartEvent)
        {
            _car = _playerProvider.PlayerCar.transform;
            _follow = true;
        }

        private void LateUpdate()
        {
            if (_follow)
            {
                _currentPosition = _car.position + _gamePositionOffSet;
                _cameraTransform.position = _currentPosition;
            }
        }
    }
}