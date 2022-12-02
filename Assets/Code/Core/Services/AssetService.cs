using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Code.Core.Services
{
    public interface IAssetService
    {
        void GetGameObjectWithComponentAsync<T>(string id, Action<T> cb) where T : Component;
        void GetTextAssetAsync(string id, Action<TextAsset> cb);
        void GetSceneAsync(string id, Action<SceneInstance> cb, LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activateOnLoad = true);
    }

    public class AssetService : BaseService, IAssetService
    {
        public const string SERVICE_NAME = nameof(AssetService);
        
        public override void Init(IServicesAggregator services, Action initializedCb)
        {
            initializedCb?.Invoke();
        }
        
        #region IAssetService
    
        void IAssetService.GetGameObjectWithComponentAsync<T>(string id, Action<T> cb)
        {
            var loadAssetAsync = Addressables.LoadAssetAsync<GameObject>(id);
            loadAssetAsync.Completed += (operation) =>
            {
                if (!operation.IsDone)
                {
                    Debug.LogError($"[{SERVICE_NAME}] Loading asset {id} with component {typeof(T)} failed");
                    return;
                }
                var result = operation.Result;
                cb?.Invoke(result.GetComponent<T>());
            };
        }

        void IAssetService.GetTextAssetAsync(string id, Action<TextAsset> cb)
        {
            var loadAssetAsync = Addressables.LoadAssetAsync<TextAsset>(id);
            loadAssetAsync.Completed += (operation) =>
            {
                if (!operation.IsDone)
                {
                    Debug.LogError($"[{SERVICE_NAME}] Loading text asset {id} failed");
                    return;
                }
                var result = operation.Result;
                cb?.Invoke(result);
            };
        }

        void IAssetService.GetSceneAsync(string id, Action<SceneInstance> cb, LoadSceneMode loadMode, bool activateOnLoad)
        {
            var loadAssetAsync = Addressables.LoadSceneAsync(id, loadMode, activateOnLoad);
            loadAssetAsync.Completed += (operation) =>
            {
                if (!operation.IsDone)
                {
                    Debug.LogError($"[{SERVICE_NAME}] Loading scene {id} failed");
                    return;
                }
                var result = operation.Result;
                cb?.Invoke(result);
            };
        }

        #endregion
    }
}