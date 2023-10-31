using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Config;
using Game.Scripts.Infrastructure.Setup.EntryPoints;
using Game.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Infrastructure.Setup.Scopes
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("Configure from BootstrapScope");
            
            builder.Register<IAssetProvider, AssetProvider>(Lifetime.Singleton);
            builder.Register<IConfigProvider, ConfigProvider>(Lifetime.Singleton);

            builder.Register<SceneLoader>(Lifetime.Singleton);

            builder.RegisterComponentInNewPrefab(_loadingCurtainPrefab, Lifetime.Singleton).DontDestroyOnLoad();
            
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}
