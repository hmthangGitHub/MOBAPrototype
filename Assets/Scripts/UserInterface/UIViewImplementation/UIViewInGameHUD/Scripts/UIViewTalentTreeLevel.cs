using System;
using System.Collections;
using System.Collections.Generic;
using UIView;
using UniRx;
using UnityEngine;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewTalentTreeLevel : UIViewBase<UIViewTalentTreeLevel.UIModel>
    {
        [Serializable]
        public class UIModel
        {
            [field : SerializeField] public ReactiveProperty<bool> LeftBranchUpgraded { get; set; } = new();
            [field : SerializeField] public ReactiveProperty<bool> RightBranchUpgraded { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<bool> MainBranchUpgraded { get; set; } = new();
        }

        [SerializeField] private GameObject leftBranchUpgraded;
        [SerializeField] private GameObject rightBranchUpgraded;
        [SerializeField] private GameObject mainBranchUpgraded;
    
        protected override void OnSetModel(UIModel model)
        {
            model.LeftBranchUpgraded.Subscribe(leftBranchUpgraded.SetActive).AddTo(disposables);
            model.RightBranchUpgraded.Subscribe(rightBranchUpgraded.SetActive).AddTo(disposables);
            model.MainBranchUpgraded.Subscribe(mainBranchUpgraded.SetActive).AddTo(disposables);
        }
    }
}