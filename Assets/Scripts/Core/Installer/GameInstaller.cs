using GameServices;
using GameUnits;
using UI;
using UnityEngine;
using Zenject;

namespace Core.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private GroundsController _groundsController;
        [SerializeField] private PlayerCar _player;
        [SerializeField] private LevelLoader _levelLoader;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerInputProvider _playerInputProvider;
        [SerializeField] private ProgressBar _progressBar;
    
        public override void InstallBindings()
        {
            Container.Bind<PlayerCar>().FromInstance(_player).AsCached();
            Container.Bind<ProjectileSpawner>().AsCached();
            Container.Bind<GroundsController>().FromInstance(_groundsController).AsCached();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsCached();
            Container.Bind<EnemySpawner>().FromInstance(_enemySpawner).AsCached();
            Container.Bind<LevelLoader>().FromInstance(_levelLoader).AsCached();
            Container.Bind<IInputProvider>().FromInstance(_playerInputProvider).AsCached();
            Container.Bind<ProgressBar>().FromInstance(_progressBar).AsCached();
        }
    }
}