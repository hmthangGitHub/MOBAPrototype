using UniRx;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UIView
{
    public class UIViewToggle : UIViewBase<ReactiveProperty<bool>>
    {
        [field: SerializeField] public Toggle Toggle { get; private set; }
        
        protected override void OnSetModel(ReactiveProperty<bool> value)
        {
            value.Subscribe(x => { Toggle.isOn = x; }).AddTo(disposables);
            Toggle.onValueChanged.AsObservable().Subscribe(x => value.Value = x).AddTo(disposables);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            Toggle = GetComponent<Toggle>();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}