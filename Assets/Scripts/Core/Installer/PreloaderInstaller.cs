
using Core.ObjectPool;
using UnityEngine;
using Zenject;

namespace Core.Installer
{
    public class PreloaderInstaller : MonoInstaller<PreloaderInstaller>
    {
        [SerializeField] private ConfigProvider _configProvider;

        public override void InstallBindings()
        {
            DiContainer container = StaticContext.Container;
            container.Bind<ConfigProvider>().FromInstance(_configProvider).AsSingle();
            container.Bind<IEventBus>().To<EventBus>().AsSingle();
            container.Bind<IObjectPooler>().To<ObjectPooler>().AsSingle();
            container.Bind<PlayerProgressSaver>().AsSingle().NonLazy();
        }
    }
}