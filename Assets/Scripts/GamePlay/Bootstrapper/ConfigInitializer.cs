using System;
using System.Collections.Generic;
using System.Threading;
using ConfigBase;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype
{
    public class ConfigInitializer : ICustomAsyncStart
    {
        private readonly IReadOnlyList<IConfigContainer> configContainers;

        [Inject]
        public ConfigInitializer(IReadOnlyList<IConfigContainer> configContainers)
        {
            this.configContainers = configContainers;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            await configContainers.Select(x => LoadConfigContainer(x, GetContainerDataPath(x), cancellation));
        }

        private static async UniTask LoadConfigContainer(IConfigContainer container, string containerDataPath, CancellationToken token = default)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(containerDataPath);
            try
            {
                await handle.ToUniTask(cancellationToken: token);
                container.SetJsonData(handle.Result.text);
            }
            finally
            {
                Addressables.Release(handle);
            }
        }
        
        private static string GetContainerDataPath(IConfigContainer container)
        {
            var typeName = container.GetType().Name;
            typeName = typeName.Replace("Container", string.Empty);
            typeName = typeName.Replace("Config", string.Empty);
            return $"ConfigJsonData/{typeName}.json";
        }
    }
}