using ConfigData;
using Core;
using Gameplay.GameUnits;
using UnityEngine;
using Zenject;

namespace Gameplay.GameServices
{
    public class GameCarBuilder : MonoBehaviour
    {
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
            _car = Instantiate(carModel.CarPrefab);
            _car.transform.rotation = Quaternion.identity;
            var turretModel = _gameConfig.GetTurretModel(_playerProgressSaver.CarData.TurretType);
            _car.ReplaceTurret(turretModel);
            _playerProvider.PlayerCar = _car;
        }
    }
}
