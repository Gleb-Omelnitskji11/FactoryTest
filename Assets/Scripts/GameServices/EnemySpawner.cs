using System.Collections.Generic;
using ConfigData;
using Core;
using Core.BusEvents;
using Core.ObjectPool;
using GameUnits;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameServices
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Vector3 _enemyOffSet = new Vector3(2f, 4f, 16f);
        [SerializeField] private float _zEnemyMinDistance = 6f;
        [SerializeField] private float _spawnEnemyDelay = 2f;
        
        private GameConfig _gameConfig;
        private PlayerCar _playerCar;
        private IEventBus _eventBus;

        private bool _isPaused = true;
        private float _timer;
        private LevelModel _levelModel;
        private IObjectPooler _pooler;
        private int _initialSize;
        private List<BasicEnemy> _enemies = new List<BasicEnemy>();
        private PlayerProgressSaver _progressSaver;

        [Inject]
        public void Construct(IObjectPooler pooler, ConfigProvider configProvider, PlayerCar playerCar, IEventBus eventBus, PlayerProgressSaver progressSaver)
        {
            _progressSaver = progressSaver;
            _pooler = pooler;
            _gameConfig = configProvider.GameConfig;
            _playerCar = playerCar;
            _eventBus = eventBus;
        }

        private void Start()
        {
            Subscribe();
        }
        
        private void Subscribe()
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
            if (_isPaused)
                return;

            _timer += Time.deltaTime;

            if (_timer < _spawnEnemyDelay)
                return;
            
            SpawnNewEnemy();
            _timer = 0f;
        }

        private void StartGame()
        {
            SetLevel();
            InitPools();
            SetNearestEnemies();
            ManagePaused(false);
        }
        
        private void RealiseAll()
        {
            while (_enemies.Count > 0)
            {
                _enemies[0].Deactivate();
            }
        }
        
        private void InitPools()
        {
            foreach (EnemyType type in _levelModel.EnemyTypes)
            {
                EnemyModel model = _gameConfig.GetEnemyUnitModel(type);
                string key = GetKey(type);
                _pooler.CreatePool<BasicEnemy, EnemyModel>(key, model, factory: CreateNewEnemyObj,
                    onGet: OnGetFromPool, onRelease: OnRealiseToPool,
                    prewarmCount: 0);
            }
        }

        private BasicEnemy CreateNewEnemyObj(EnemyModel model)
        {
            BasicEnemy enemy = Instantiate(model.EnemyPrefab);
            EnemyType enemyType = model.EnemyType;
            string key = GetKey(enemyType);
            enemy.SetPoolData(_pooler, key);
            enemy.InitEnemyModel(model.UnitModel, _playerCar.transform);
            return enemy;
        }
        
        private void OnGetFromPool(BasicEnemy enemy)
        {
            _enemies.Add(enemy);
            enemy.gameObject.SetActive(true);
            enemy.Reset();
            enemy.Activate();
        }

        private void OnRealiseToPool(BasicEnemy enemy)
        {
            enemy.Reset();
            _enemies.Remove(enemy);
            if(!_isPaused)
                _eventBus.Publish<EnemyDiedEvent>(new EnemyDiedEvent(enemy.EnemyUnitModel));
        }
        
        private string GetKey(EnemyType type) => $"Enemy_{type}";

        private void SetLevel()
        {
            var level = _gameConfig.GetLevelModel(_progressSaver.CurrentLevel);
            _initialSize = level.StartEnemyCount;
            _spawnEnemyDelay = level.EnemyDelay;
            _levelModel = level;
        }

        private void ManagePaused(bool isPaused, bool resetTimer = true)
        {
            _isPaused = isPaused;
            if (resetTimer) _timer = 0f;
            foreach (var enemy in _enemies)
            {
                enemy.PauseChanged(isPaused);
            }
        }

        private void SetNearestEnemies()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                SpawnNewEnemy();
            }
        }

        private void SpawnNewEnemy()
        {
            EnemyType enemyType = GetRandomEnemyType();
            Vector3 pos = GetRandomPosition();
            string key = GetKey(enemyType);
            BasicEnemy enemy = _pooler.Get<BasicEnemy, EnemyModel>(key);
            enemy.transform.position = pos;
        }

        private EnemyType GetRandomEnemyType()
        {
            int max = _levelModel.EnemyTypes.Count;
            int random = Random.Range(0, max);
            return _levelModel.EnemyTypes[random];
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 carPos = _playerCar.transform.position;
            float xRandom = Random.Range(-_enemyOffSet.x, _enemyOffSet.x);
            float zRandom = Random.Range(_zEnemyMinDistance, _enemyOffSet.z);
            Vector3 pos = new Vector3(xRandom, _enemyOffSet.y, carPos.z + zRandom);
            return pos;
        }
        
        private void OnGameResult(GameResultEvent gameResultEvent)
        {
            ManagePaused(true);
        }
        
        private void OnPauseResult(PauseEvent pauseEvent)
        {
            if (pauseEvent.IsPause) 
                ManagePaused(true, false);
            else ManagePaused(false, false);
        }

        private void Restart(RestartEvent restartEvent)
        {
            if(!restartEvent.IsFirstGame) RealiseAll();
            StartGame();
        }
    }
}