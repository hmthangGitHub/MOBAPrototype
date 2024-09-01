using System.Threading;
using Cysharp.Threading.Tasks;

namespace MobaPrototype
{
    public interface ICustomAsyncStart
    {
        UniTask StartAsync(CancellationToken cancellation = default);
    }
}