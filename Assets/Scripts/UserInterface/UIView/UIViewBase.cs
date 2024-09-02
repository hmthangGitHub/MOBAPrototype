using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace UIView
{
    public abstract class UIViewBase<TModel> : MonoBehaviour
    {
        protected CompositeDisposable disposables;
        public TModel Model { get; private set; }

        public void SetModel(TModel uiModel)
        {
            Model = uiModel;
            if (disposables == default)
            {
                disposables = new();
                disposables.AddTo(this);
            }
            else
            {
                disposables.Clear();
            }
            OnSetModel(uiModel);
        }

        public virtual void In()
        {
            gameObject.SetActive(true);
        }

        public virtual void Out()
        {
            gameObject.SetActive(false);
        }
        
        protected abstract void OnSetModel(TModel model);
    }
}