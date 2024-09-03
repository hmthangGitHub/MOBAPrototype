using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MobaPrototype.Config;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using VContainer;
using Object = UnityEngine.Object;

namespace MobaPrototype.Scope
{
    public class GameObjectPoolContainer : ICustomPostAsyncStart, IDisposable
    {
        private readonly ConfigSkillContainer configSkillContainer;
        private readonly Dictionary<string, IObjectPool<GameObjectEventPool>> gameObjectPools = new();

        private CompositeDisposable compositeDisposable = new();

        [Inject]
        public GameObjectPoolContainer(ConfigSkillContainer configSkillContainer)
        {
            this.configSkillContainer = configSkillContainer;
        }

        public async UniTask PostStartAsync(CancellationToken cancellation = default)
        {
            var allSkillEntityPath = configSkillContainer
                .ConfigList
                .Select(x => x.SkillPrefabPath)
                .Where(x => !string.IsNullOrEmpty(x)).Distinct()
                .ToArray();

            await InitializeGameObjectPoolsAsync(cancellation, allSkillEntityPath);
            PrewarmGameObjectPool(5);
        }

        private void PrewarmGameObjectPool(int quantity)
        {
            foreach (var pool in gameObjectPools)
            {
                var list = new List<GameObjectEventPool>();
                for (int i = 0; i < quantity; i++)
                {
                    list.Add(pool.Value.Get());
                }

                foreach (var instance in list)
                {
                    pool.Value.Release(instance);
                }
            }
        }

        private async Task InitializeGameObjectPoolsAsync(CancellationToken cancellation, string[] allSkillEntityPath)
        {
            foreach (var skillEntityPath in allSkillEntityPath)
            {
                var op = Addressables.LoadAssetAsync<GameObject>(skillEntityPath);
                await op.ToUniTask(cancellationToken: cancellation);
                var prefab = op.Task.Result.GetComponent<GameObjectEventPool>();
                Addressables.Release(op);
                gameObjectPools.Add(skillEntityPath, new ObjectPool<GameObjectEventPool>(() =>
                    {
                        var gameObjectEventPool = Object.Instantiate(prefab);
                        gameObjectEventPool.OnReturnToPool.Subscribe(_ =>
                        {
                            gameObjectPools[skillEntityPath].Release(gameObjectEventPool);
                        }).AddTo(compositeDisposable);
                        return gameObjectEventPool;
                    }, x => x.gameObject.SetActive(true),
                    x => x.gameObject.SetActive(false),
                    x =>
                    {
                        if (x != default && x.gameObject != default)
                        {
                            Object.Destroy(x.gameObject);
                        }
                    }));
            }
        }

        public T GetObject<T>(string key) where T : MonoBehaviour
        {
            return gameObjectPools[key].Get().GetComponent<T>();
        }

        public void Dispose()
        {
            compositeDisposable.Dispose();
            compositeDisposable = default;
            foreach (var gameObjectPool in gameObjectPools)
            {
                gameObjectPool.Value.Clear();
            }
        }
    }
}