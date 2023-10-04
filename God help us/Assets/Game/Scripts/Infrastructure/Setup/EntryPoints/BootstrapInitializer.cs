using Game.Scripts.Infrastructure.Services.Config;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.Infrastructure.Setup.EntryPoints
{
    public class BootstrapInitializer : IInitializable, IStartable
    {
        private const string GameSceneName = "GameScene";
        
        private readonly SceneLoader _sceneLoader;
        private readonly IConfigProvider _configProvider;

        public BootstrapInitializer(SceneLoader sceneLoader, IConfigProvider configProvider)
        {
            _sceneLoader = sceneLoader;
            _configProvider = configProvider;
        }

        public void Initialize()
        {
            InitializeSystems();
        }

        private void InitializeSystems()
        {
            _configProvider.Load();
        }

        public void Start() => 
            LoadNextScene();

        private void LoadNextScene()
        {
#pragma warning disable 4014
            _sceneLoader.Load(GameSceneName);
#pragma warning disable 4014
        }
    }
}