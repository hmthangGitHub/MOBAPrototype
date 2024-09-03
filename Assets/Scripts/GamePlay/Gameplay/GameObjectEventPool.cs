using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameObjectEventPool : MonoBehaviour
{
    private Subject<Unit> onReturnToPool = new();
    public IObservable<Unit> OnReturnToPool => onReturnToPool;

    public void ReturnToPool()
    {
        onReturnToPool.OnNext(default);
    }
}
