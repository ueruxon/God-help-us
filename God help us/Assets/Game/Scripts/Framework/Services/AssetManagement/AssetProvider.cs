using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Scripts.Framework.Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly AssetLoader _assetLoader;
        
        public AssetProvider()
        {
            _assetLoader = new AssetLoader();
        }

        public async UniTask Init() => 
            await _assetLoader.InitializeAsync();

        public async UniTask<T> LoadAsync<T>(string key) where T : class => 
            await _assetLoader.LoadAsync<T>(key);
        
        public async UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class => 
            await _assetLoader.LoadAsync<T>(assetReference);

        public async UniTask<TAsset[]> LoadAllAsync<TAsset>(List<string> keys) where TAsset : class => 
            await _assetLoader.LoadAllAsync<TAsset>(keys);

        public async UniTask<List<TAsset>> LoadAssetsByLabel<TAsset>(string label) where TAsset : class => 
            await _assetLoader.LoadAssetsByLabel<TAsset>(label);

        public T Load<T>(string path) where T : Object => 
            _assetLoader.Load<T>(path);

        public T[] LoadAll<T>(string path) where T : Object => 
            _assetLoader.LoadAll<T>(path);

        public T Instantiate<T>(string path, Vector3 at) where T : Object
        {
            var prefab = _assetLoader.Load<T>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public T Instantiate<T>(string path, Transform parent) where T : Object
        {
            var prefab = _assetLoader.Load<T>(path);
            return Object.Instantiate(prefab, parent);
        }

        public T Instantiate<T>(string path) where T : Object
        {
            T prefab = _assetLoader.Load<T>(path);
            return Object.Instantiate(prefab);
        }

        public T Instantiate<T>(T prefab, Vector3 at) where T : Object => 
            Object.Instantiate(prefab, at, Quaternion.identity);

        public async UniTask<T> InstantiateAsync<T>(string key, Vector3 at = default, 
            Quaternion quaternion = default, Transform parent = null) where T : Object
        {
            return parent is null
                ? Object.Instantiate(await LoadAsync<T>(key), at, Quaternion.identity)
                : Object.Instantiate(await LoadAsync<T>(key), at, Quaternion.identity, parent);
        }

        public void ReleaseAsset(string key) => 
            _assetLoader.ReleaseAssetByKey(key);

        public void Cleanup() => 
            _assetLoader.Dispose();
    }
}