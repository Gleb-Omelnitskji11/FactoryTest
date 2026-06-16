using ConfigData;
using Core;
using Core.BusEvents;
using GameUnits;
using UnityEngine;
using Zenject;

namespace GameServices
{
    public class GameCarBuilder : MonoBehaviour
    {
        [SerializeField] private Vector3 _carStartPos = new Vector3(0, 0.5f, -48.6f);
        
        private PlayerCar _car;
        private GameConfig _gameConfig;

        private IEventBus _eventBus;
        private PlayerProgressSaver _playerProgressSaver;
        private PlayerProvider _playerProvider;

        [Inject]
        public void Construct(ConfigProvider configProvider, IEventBus eventBus, PlayerProgressSaver playerProgressSaver,
            PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
            _playerProgressSaver = playerProgressSaver;
            _eventBus = eventBus;
            _gameConfig = configProvider.GameConfig;
        }

        private void Start()
        {
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            var carModel = _gameConfig.GetPlayerUnitModel(_playerProgressSaver.CarData.CarType);
            _car = Instantiate(carModel.CarPrefab, _carStartPos, Quaternion.identity);
            var turretModel = _gameConfig.GetTurretModel(_playerProgressSaver.CarData.TurretType);
            _car.ReplaceTurret(turretModel);
            _playerProvider.PlayerCar = _car;
        }
    }
}
