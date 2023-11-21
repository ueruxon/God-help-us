using Game.Scripts.Framework.Services.AssetManagement;
using Game.Scripts.Framework.Services.Config;
using Game.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Framework.Setup.Bootstrap
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
