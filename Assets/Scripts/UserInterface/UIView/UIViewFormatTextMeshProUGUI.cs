using TMPro;
using UniRx;
using UnityEngine;

namespace UIView
{
    public class UIViewFormatTextMeshProUGUI : UIViewBase<ReactiveProperty<object>>
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private string format;
        
        protected override void OnSetModel(ReactiveProperty<object> model)
        {
            model.Subscribe(val => textMeshProUGUI.text = string.Format(format, val)).AddTo(disposables);
        }

        public void SetModel<T>(ReactiveProperty<T> genericModel)
        {
            var model = new ReactiveProperty<object>();
            Model = model;
            if (disposables == default)
            {
                disposables = new();
                disposables.AddTo(this);
            }
            else
            {
                disposables.Clear();
            }
            genericModel.Subscribe(val => model.Value = val).AddTo(disposables);
            OnSetModel(model);
        }

        private void Reset()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }
    }
}