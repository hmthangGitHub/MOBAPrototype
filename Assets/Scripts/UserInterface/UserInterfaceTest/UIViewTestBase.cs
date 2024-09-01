using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIView.Test
{
    public class UIViewTestBase<TUIView, TUIModel> : MonoBehaviour where TUIView : UIViewBase<TUIModel>
    {
        [SerializeField] private TUIView uiView;
        [SerializeField] private TUIModel uiModel;

        private void OnGUI()
        {
            if (GUILayout.Button("Set model"))
            {
                uiView.SetModel(uiModel);
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
    }
}