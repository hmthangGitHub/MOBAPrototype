using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UIView
{
    public abstract class UIViewBase<TModel> : MonoBehaviour
    {
        protected TModel uiModel;
        protected CompositeDisposable disposables;

        public void SetModel(TModel uiModel)
        {
            this.uiModel = uiModel;
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