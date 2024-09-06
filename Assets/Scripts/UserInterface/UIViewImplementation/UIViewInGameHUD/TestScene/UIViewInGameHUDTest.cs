using System;
using System.Collections;
using System.Collections.Generic;
using UIView.Test;
using UniRx;
using UnityEngine;

namespace MobaPrototype.UIViewImplementation.Test
{
    public class UIViewInGameHUDTest : UIViewTestBase<UIViewInGameHUD, UIViewInGameHUD.UIModel>
    {
        [Serializable]
        private class UIViewSkillLevelTest
        {
            [SerializeField] public UIViewSkillLevel.UIModel[] skillLevelsList;
        }
        
        [SerializeField] private UIViewSkill.UIModel[] skills;
        [SerializeField] private UIViewSkillLevelTest[] skillLevels;
        [SerializeField] private UIViewTalentTreePopUpContent.UIModel[] talentTreePopUpContents;
        [SerializeField] private UIViewSkillInfoPopUp.UIModel skillInFoPopUp;
        [SerializeField] private UIViewSkillEffect.UIModel[] skillEffectTypes;

        protected override void OnSetTestModel()
        {
            for (var index = 0; index < skills.Length; index++)
            {
                var i = index;
                var skill = skills[i];
                skill.Button.OnClick = () => { Debug.Log($"OnClick skill {i}"); };
                skill.Button.OnHover = () => { Debug.Log($"Hover {i}"); };
                skill.Button.OnHoverExit = () => { Debug.Log($"OnHoverExit {i}"); };
                skill.SkillLevels = skillLevels[i].skillLevelsList.ToReactiveCollection();
            }

            uiModel.SkillLists = skills.ToReactiveCollection();
            base.OnSetTestModel();
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Show lack mana notification"))
            {
                uiModel.ShowNoManaEvent.OnNext(default);
            }
            
            if (GUILayout.Button("Show on cd notification"))
            {
                uiModel.ShowCoolDownEvent.OnNext(default);
            }
            
            if (GUILayout.Button("Show talent tree popup"))
            {
                uiModel.ShowTalentTreeContentEvent.OnNext(talentTreePopUpContents.ToReactiveCollection());
            }
            
            if (GUILayout.Button("Show skill info popup"))
            {
                skillInFoPopUp.EffectList = skillEffectTypes.ToReactiveCollection();
                uiModel.ShowUIViewSkillInfoPopUpEvent.OnNext(skillInFoPopUp);
            }
        }
    }
}