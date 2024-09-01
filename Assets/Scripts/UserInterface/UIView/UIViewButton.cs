using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UIView
{
    public class UIViewButton : UIViewBase<UIViewButton.UIModel>
    {
        [Serializable]
        public class UIModel
        {
            [field: SerializeField] public Action OnClick { get; set; } = EmptyAction.Empty;
            [field: SerializeField] public Action OnHover { get; set; } = EmptyAction.Empty;
            [field: SerializeField] public Action OnHoverExit { get; set; } = EmptyAction.Empty;
        }

        [field : SerializeField] private UIViewButtonExtension Button { get; set; }
        
        protected override void OnSetModel(UIModel model)
        {
            Button.onClick.AsObservable().Subscribe(_ => model.OnClick()).AddTo(disposables);
            Button.OnHoverEvent.AsObservable().Subscribe(_ => model.OnHover()).AddTo(disposables);
            Button.OnHoverExitEvent.AsObservable().Subscribe(_ => model.OnHoverExit()).AddTo(disposables);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            var button = GetComponent<Button>();
            if (button != default && button is not UIViewButtonExtension)
            {
                DestroyImmediate(button);
            }
            Button = gameObject.GetOrAddComponent<UIViewButtonExtension>();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}