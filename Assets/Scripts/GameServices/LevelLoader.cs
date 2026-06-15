using ConfigData;
using Core;
using Core.BusEvents;
using GameUnits;
using UI;
using UnityEngine;
using Zenject;

namespace GameServices
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Vector3 _carStartPos = new Vector3(0, 0.5f, -48.6f);

        private PlayerCar _car;
        private GameConfig _gameConfig;

        private ProgressBar _progressBar;
        private IEventBus _eventBus;
        private PlayerProgressSaver _progressSaver;
        private LevelModel _level;

        [Inject]
        public void Construct(PlayerCar playerCar, ConfigProvider configProvider, ProgressBar progressBar,
            IEventBus eventBus, PlayerProgressSaver progressSaver)
        {
            _progressSaver = progressSaver;
            _eventBus = eventBus;
            _progressBar = progressBar;
            _car = playerCar;
            _gameConfig = configProvider.GameConfig;
        }

        private void Start()
        {
            _eventBus.Subscribe<GameResultEvent>(OnGameEnd);
            _eventBus.Subscribe<RestartEvent>(Restart);
            _eventBus.Subscribe<PauseEvent>(OnPause);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameResultEvent>(OnGameEnd);
            _eventBus.Unsubscribe<RestartEvent>(Restart);
            _eventBus.Unsubscribe<PauseEvent>(OnPause);
        }

        private void Restart(RestartEvent restartEvent)
        {
            _level = _gameConfig.GetLevelModel(_progressSaver.CurrentLevel);
            ResetCar();
            _progressBar.Setup(_level, _carStartPos.z);
            _car.StartLevel(_level);
        }

        private void ResetCar()
        {
            _car.transform.position = _carStartPos;
            var carData = _progressSaver.CarData;
            var carModel = _gameConfig.GetPlayerUnitModel(carData.CarType);
            var turretModel = _gameConfig.GetTurretModel(carData.TurretType);
            
            _car.InitUnit(carModel.UnitModel, turretModel);
        }

        private void OnLose()
        {
            Pause();
        }

        private void OnGameEnd(GameResultEvent gameResultEvent)
        {
            if (gameResultEvent.IsWin)
                OnWin();
            else OnLose();
        }

        private void OnWin()
        {
            Pause();
        }

        private void OnPause(PauseEvent pauseEvent)
        {
            if (pauseEvent.IsPause) Pause();
            else Resume();
        }

        private void Pause()
        {
            _car.Stop();
        }

        private void Resume()
        {
            _car.Resume();
        }
    }
}