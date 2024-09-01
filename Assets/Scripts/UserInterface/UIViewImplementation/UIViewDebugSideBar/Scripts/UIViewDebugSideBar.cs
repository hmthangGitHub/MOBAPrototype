using System;
using UIView;
using UnityEngine;
using UniRx;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewDebugSideBar : UIViewBase<UIViewDebugSideBar.UIModel>
    {
        [Serializable]
        public class UIModel
        {
            [field: SerializeField] public UIViewButton.UIModel  LevelUp { get; set; } = new();
            [field: SerializeField] public UIViewButton.UIModel  LevelMax { get; set; } = new();
            [field: SerializeField] public UIViewButton.UIModel ResetLevel { get; set; } = new();
            [field: SerializeField] public UIViewButton.UIModel  ResetManaAndCd { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<bool> FreeSpellToggle { get; set; } = new();
        }
        
        [field: SerializeField] public UIViewButton LevelUp { get; set; }
        [field: SerializeField] public UIViewButton LevelMax { get; set; }
        [field: SerializeField] public UIViewButton ResetLevel { get; set; }
        [field: SerializeField] public UIViewButton ResetManaAndCd { get; set; }
        [field: SerializeField] public UIViewToggle FreeSpellToggle { get; set; }
        
        protected override void OnSetModel(UIModel model)
        {
            LevelUp.SetModel(uiModel.LevelUp);
            LevelMax.SetModel(uiModel.LevelMax);
            ResetLevel.SetModel(uiModel.ResetLevel);
            ResetManaAndCd.SetModel(uiModel.ResetManaAndCd);
            FreeSpellToggle.SetModel(uiModel.FreeSpellToggle);
        }
    }
}