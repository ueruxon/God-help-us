using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Scripts.Infrastructure.Services.AssetManagement
{
    public interface IAssetProvider
    {
        public UniTask Init();
        public T Load<T>(string path) where T : Object;
        public T[] LoadAll<T>(string path) where T : Object;
        public UniTask<T> LoadAsync<T>(string key) where T : class;
        public UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class;
        public UniTask<TAsset[]> LoadAllAsync<TAsset>(List<string> keys) where TAsset : class;
        public UniTask<List<TAsset>> LoadAssetsByLabel<TAsset>(string label) where TAsset : class;
        public T Instantiate<T>(string path, Vector3 at) where T : Object;
        public T Instantiate<T>(string path, Transform parent) where T : Object;
        public T Instantiate<T>(string path) where T : Object;
        T Instantiate<T>(T prefab, Vector3 at) where T : Object;
        public UniTask<T> InstantiateAsync<T>(string key, Vector3 at = default,
            Quaternion quaternion = default, Transform parent = null) where T : Object;
        public void ReleaseAsset(string key);
        public void Cleanup();
    }
}