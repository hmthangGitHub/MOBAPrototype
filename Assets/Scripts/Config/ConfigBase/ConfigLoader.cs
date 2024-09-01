using System;
using System.Threading;
using ConfigBase;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Config
{
    public static class ConfigLoader
    {
        public static async UniTask<TContainer> LoadConfigContainer<TContainer>(string containerDataPath, CancellationToken token = default) where TContainer : IConfigContainer, new()
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(containerDataPath);
            try
            {
                await handle.ToUniTask(cancellationToken: token);
                var configContainer = new TContainer();
                configContainer.SetJsonData(handle.Result.text);
                return configContainer;
            }
            finally
            {
                Addressables.Release(handle);
            }
        }
        
        public static async UniTask<IConfigContainer> LoadConfigContainer(Type containerType, string containerDataPath, CancellationToken token = default)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(containerDataPath);
            try
            {
                await handle.ToUniTask(cancellationToken: token);
                var configContainer = (IConfigContainer)Activator.CreateInstance(containerType);
                configContainer.SetJsonData(handle.Result.text);
                return configContainer;
            }
            finally
            {
                Addressables.Release(handle);
            }
        }
    }
}
