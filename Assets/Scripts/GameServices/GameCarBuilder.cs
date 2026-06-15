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

        [Inject]
        public void Construct(ConfigProvider configProvider, IEventBus eventBus, PlayerCar playerCar)
        {
            _eventBus = eventBus;
            _gameConfig = configProvider.GameConfig;
            _car = playerCar;
        }

        // private void Start()
        // {
        //     _eventBus.Subscribe<RestartEvent>(Restart);
        // }
        //
        // private void OnDestroy()
        // {
        //     _eventBus.Unsubscribe<RestartEvent>(Restart);
        // }

        // private void Restart(RestartEvent restartEvent)
        // {
        //     ResetCar();
        //     _car.StartLevel();
        // }

        // private void ResetCar()
        // {
        //     _car.transform.position = _carStartPos;
        //     var carModel = _gameConfig.GetUnitModel(UnitType.Player);
        //     var turretModel = _gameConfig.GetTurretModel(0);
        //     _car.InitUnit(carModel, turretModel, _gameConfig.GetDefaultLevelModel);
        // }
    }
}
