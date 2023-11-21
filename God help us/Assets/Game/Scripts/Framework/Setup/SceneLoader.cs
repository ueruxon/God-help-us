using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Framework.Setup
{
    public class SceneLoader
    {
        private readonly LoadingCurtain _loadingCurtain;
        
        public SceneLoader(LoadingCurtain loadingCurtain) => 
            _loadingCurtain = loadingCurtain;

        public async UniTask Load(string sceneName, Action onLoadedCallback = null)
        {
            _loadingCurtain.Show();
            await LoadScene(sceneName, onLoadedCallback);
            _loadingCurtain.Hide();
        }

        private async UniTask LoadScene(string nextScene, Action onLoadedCallback = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoadedCallback?.Invoke();
                await UniTask.Yield();
            }

            AsyncOperationHandle<SceneInstance> handler = Addressables.LoadSceneAsync(nextScene);

            while (!handler.IsDone)
            {
                _loadingCurtain.SetProgress(handler.PercentComplete);
                await UniTask.Yield();
            }

            onLoadedCallback?.Invoke();
        }
    }
}