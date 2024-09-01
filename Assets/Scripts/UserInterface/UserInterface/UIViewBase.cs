using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIView
{
    public abstract class UIViewBaseBase<TModel> : MonoBehaviour
    {
        protected TModel uiModel;

        public void SetModel(TModel uiModel)
        {
            this.uiModel = uiModel;
            OnSetModel(uiModel);
        }

        protected abstract void OnSetModel(TModel model);
    }
}