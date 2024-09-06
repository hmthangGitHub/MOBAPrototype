using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIView;
using UnityEngine;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewTalentTreePopUpContent : UIViewBase<UIViewTalentTreePopUpContent.UIModel>
    {
        [Serializable]
        public class UIModel
        {
            [field : SerializeField] public UIViewTalentTreeLevel.UIModel TalentTreeLevel { get; set; }
            [field : SerializeField] public UIViewButton.UIModel LeftButton { get; set; }
            [field : SerializeField] public UIViewButton.UIModel RightButton { get; set; }
            [field : SerializeField] public int Level { get; set; }
            [field : SerializeField] public string DescriptionLeft { get; set; }
            [field : SerializeField] public string DescriptionRight { get; set; }
        }

        [field : SerializeField] public UIViewTalentTreeLevel TalentTreeLevel { get; set; }
        [field : SerializeField] public UIViewButton LeftButton { get; set; }
        [field : SerializeField] public UIViewButton RightButton { get; set; }
        [field : SerializeField] public TextMeshProUGUI Level { get; set; }
        [field : SerializeField] public TextMeshProUGUI DescriptionLeft { get; set; }
        [field : SerializeField] public TextMeshProUGUI DescriptionRight { get; set; }
        
        protected override void OnSetModel(UIModel model)
        {
            TalentTreeLevel.SetModel(model.TalentTreeLevel);
            LeftButton.SetModel(model.LeftButton);
            RightButton.SetModel(model.RightButton);
            Level.text = model.Level.ToString();
            DescriptionLeft.text = model.DescriptionLeft;
            DescriptionRight.text = model.DescriptionRight;
        }
    }
}