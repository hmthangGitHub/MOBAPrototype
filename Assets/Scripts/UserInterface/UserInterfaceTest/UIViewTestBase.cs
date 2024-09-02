using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIView.Test
{
    public class UIViewTestBase<TUIView, TUIModel> : MonoBehaviour where TUIView : UIViewBase<TUIModel>
    {
        [SerializeField] protected TUIView uiView;
        [SerializeField] protected TUIModel uiModel;

        protected virtual void OnGUI()
        {
            if (GUILayout.Button("Set model"))
            {
                OnSetTestModel();
            }

            if (GUILayout.Button("In"))
            {
                uiView.In();
            }
            
            if (GUILayout.Button("Out"))
            {
                uiView.Out();
            }
        }

        protected virtual void OnSetTestModel()
        {
            uiView.SetModel(uiModel);
        }
    }
}