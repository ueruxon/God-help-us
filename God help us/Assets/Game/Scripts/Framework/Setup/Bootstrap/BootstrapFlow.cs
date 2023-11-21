using Cysharp.Threading.Tasks;
using Game.Scripts.Framework.Services.AssetManagement;
using Game.Scripts.Framework.Services.Config;
using VContainer.Unity;

namespace Game.Scripts.Framework.Setup.Bootstrap
{
    public class BootstrapFlow : IInitializable
    {
        private const string GameSceneName = "GameScene";
        
        private readonly SceneLoader _sceneLoader;
        private readonly IAssetProvider _assetProvider;
        private readonly IConfigProvider _configProvider;

        public BootstrapFlow(SceneLoader sceneLoader, IAssetProvider assetProvider, IConfigProvider configProvider)
        {
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _configProvider = configProvider;
        }

        public void Initialize()
        {
            InitializeSystems().Forget();
        }

        private async UniTaskVoid InitializeSystems()
        {
            await _assetProvider.Init();
            await _configProvider.Init();
            
            await LoadNextScene();
        }

        // public void Start() => 
        //     LoadNextScene().Forget();

        private async UniTask LoadNextScene() => 
            await _sceneLoader.Load(GameSceneName);
    }
}