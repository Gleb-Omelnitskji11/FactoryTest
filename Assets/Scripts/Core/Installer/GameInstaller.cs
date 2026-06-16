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
            Container.Bind<GameInstance>().FromInstance(_enemyInstance).AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsSingle();
            Container.Bind<PlayerProvider>().AsSingle();
            Container.Bind<GroundsController>().FromInstance(_groundsController).AsSingle();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();
            Container.Bind<EnemySpawner>().FromInstance(_enemySpawner).AsSingle();
            Container.Bind<LevelLoader>().FromInstance(_levelLoader).AsSingle();
            Container.Bind<IInputProvider>().FromInstance(_playerInputProvider).AsSingle();
            Container.Bind<ProgressBar>().FromInstance(_progressBar).AsSingle();
        }
    }
}