using System;
using System.Collections;
using System.Collections.Generic;
using UIView;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewSkillLevel : UIViewBase<UIViewSkillLevel.UIModel>
    {
        [Serializable]
        public class UIModel
        {
            [field : SerializeField] public ReactiveProperty<bool> Upgraded { get; set; }
            [field : SerializeField] public ReactiveProperty<bool> CanUpgrade { get; set; }
        }

        [SerializeField] private GameObject upgradedMark;
        [SerializeField] private GameObject canUpgradeMark;

        protected override void OnSetModel(UIModel model)
        {
            model.Upgraded.Subscribe(val => upgradedMark.SetActive(val)).AddTo(disposables);
            model.CanUpgrade.Subscribe(val => canUpgradeMark.SetActive(val)).AddTo(disposables);
        }
    }
}