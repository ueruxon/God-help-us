using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Game.Scripts.Infrastructure.Services.AssetManagement
{
    public class AssetLoader : IDisposable
    {
        private Dictionary<string, List<AsyncOperationHandle>> _assetsByLabel;
        private Dictionary<string, AsyncOperationHandle> _requestedAssets;

        public async UniTask InitializeAsync()
        {
            _requestedAssets = new Dictionary<string, AsyncOperationHandle>();
            _assetsByLabel = new Dictionary<string, List<AsyncOperationHandle>>();
            
            await Addressables.InitializeAsync().ToUniTask();
        }

        public async UniTask<TAsset> LoadAsync<TAsset>(string key) where TAsset : class
        {
            if (_requestedAssets.TryGetValue(key, out AsyncOperationHandle handle))
                return handle.Result as TAsset;

            AsyncOperationHandle<TAsset> asyncHandle = Addressables.LoadAssetAsync<TAsset>(key);
            _requestedAssets.Add(key, asyncHandle);

            return await asyncHandle.ToUniTask();
        }

        public async UniTask<TAsset> LoadAsync<TAsset>(AssetReference assetReference) where TAsset : class => 
            await LoadAsync<TAsset>(assetReference.AssetGUID);

        public async UniTask<TAsset[]> LoadAllAsync<TAsset>(List<string> keys) where TAsset : class
        {
            List<UniTask<TAsset>> tasks = new List<UniTask<TAsset>>(keys.Count);

            foreach (var key in keys) 
                tasks.Add(LoadAsync<TAsset>(key));

            return await UniTask.WhenAll(tasks);
        }

        public async UniTask<List<TAsset>> LoadAssetsByLabel<TAsset>(string label) where TAsset : class
        {
            AsyncOperationHandle<IList<IResourceLocation>> handle = 
                Addressables.LoadResourceLocationsAsync(label, typeof(TAsset));
            IList<IResourceLocation> locations = await handle.ToUniTask();
            
            List<string> assetKeys = new List<string>(locations.Count);
            foreach (IResourceLocation location in locations) 
                assetKeys.Add(location.PrimaryKey);

            var assets = await LoadAllAsync<TAsset>(assetKeys);
            
            Addressables.Release(handle);
            return assets.ToList();
        }

        public T Load<T>(string filepath) where T : Object
        {
            var data = Resources.Load<T>(filepath);
            if(data is null)
                throw new NullReferenceException($"Asset of type {typeof(T)} from {filepath} can't be loaded");

            return data;
        }
        
        public T[] LoadAll<T>(string path) where T : Object
        {
            var data = Resources.LoadAll<T>(path);
            if(data is null)
                throw new NullReferenceException($"Assets of type {typeof(T)} from {path} can't be loaded");

            return data;
        }

        public void ReleaseAssetByKey(string key)
        {
            if (_requestedAssets.TryGetValue(key, out AsyncOperationHandle handle))
            {
                Addressables.Release(handle);
                _requestedAssets.Remove(key);
            }
        }

        public void Dispose()
        {
            foreach (AsyncOperationHandle handle in _requestedAssets.Values) 
                Addressables.Release(handle);
            
            _requestedAssets.Clear();
        }
    }
}