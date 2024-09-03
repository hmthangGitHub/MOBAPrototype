using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype.Scope
{
    public class CustomAsyncStartService : IAsyncStartable
    {
        private IReadOnlyList<ICustomAsyncStart> customAsyncStarts;
        private IReadOnlyList<ICustomPostAsyncStart> customPostAsyncStarts;
        private IReadOnlyList<IPostAsyncStart> postAsyncStarts;

        [Inject]
        public CustomAsyncStartService(IReadOnlyList<ICustomAsyncStart> customAsyncStarts, IReadOnlyList<IPostAsyncStart> postAsyncStarts, IReadOnlyList<ICustomPostAsyncStart> customPostAsyncStarts)
        {
            this.postAsyncStarts = postAsyncStarts;
            this.customPostAsyncStarts = customPostAsyncStarts;
            this.customAsyncStarts = customAsyncStarts;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            await customAsyncStarts.Select(x => x.StartAsync(cancellation));
            await customPostAsyncStarts.Select(x => x.PostStartAsync(cancellation));
            foreach (var postAsyncStart in this.postAsyncStarts)
            {
                postAsyncStart.PostAsyncStart();
            }
        }
    }
}