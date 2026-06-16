using Gameplay.GameServices;
using UI;
using UnityEngine;
using Zenject;

namespace Core.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameInstance _enemyInstance;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private GroundsController _groundsController;
        [SerializeField] private LevelLoader _levelLoader;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerInputProvider _playerInputProvider;
        [SerializeField] private ProgressBar _progressBar;
    
        public override void InstallBindings()
        {
            Container.Bind<GameInstance>().FromInstance(_enemyInstance).AsCached();
            Container.Bind<ProjectileSpawner>().AsCached();
            Container.Bind<PlayerProvider>().AsCached();
            Container.Bind<GroundsController>().FromInstance(_groundsController).AsCached();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsCached();
            Container.Bind<EnemySpawner>().FromInstance(_enemySpawner).AsCached();
            Container.Bind<LevelLoader>().FromInstance(_levelLoader).AsCached();
            Container.Bind<IInputProvider>().FromInstance(_playerInputProvider).AsCached();
            Container.Bind<ProgressBar>().FromInstance(_progressBar).AsCached();
        }
    }
}