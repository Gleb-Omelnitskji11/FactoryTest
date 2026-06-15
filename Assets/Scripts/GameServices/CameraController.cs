using GameUnits;
using UnityEngine;
using Zenject;

namespace GameServices
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private bool _follow = true;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Vector3 _gamePositionOffSet = new Vector3(0f, 10.13f, -8.14f);
        private Vector3 _currentPosition;
        private Transform _car;
    
        [Inject]
        public void Construct(PlayerCar playerCar)
        {
            _car = playerCar.transform;
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