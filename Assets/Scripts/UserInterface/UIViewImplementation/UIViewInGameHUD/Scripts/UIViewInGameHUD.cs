using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UIView;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MobaPrototype.UIViewImplementation
{
    public class UIViewInGameHUD : UIViewBase<UIViewInGameHUD.UIModel>
    {
        [Serializable]
        public class UIModel
        {
            [field: SerializeField] public ReactiveProperty<string> HeroName { get; set; } = new();
            [field: SerializeField] public ReactiveCollection<UIViewSkill.UIModel> SkillLists { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<float> Hp { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<float> MaxHp { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<float> HpRegen { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<float> Mana { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<float> MaxMana { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<float> ManaRegen { get; set; } = new();
            [field: SerializeField] public ReactiveProperty<int> Level { get; set; } = new();
            [field: SerializeField] public Subject<Unit> ShowNoManaEvent { get; set; } = new();
            [field: SerializeField] public Subject<Unit> ShowCoolDownEvent { get; set; } = new();
            [field: SerializeField] public UIViewTalentTreeLevel.UIModel[] TalentTreeLevels { get; set; } = Array.Empty<UIViewTalentTreeLevel.UIModel>();
            [field: SerializeField] public UIViewButton.UIModel TalentTreeButton { get; set; } = new();
            [field: SerializeField] public Subject<ReactiveCollection<UIViewTalentTreePopUpContent.UIModel>> ShowTalentTreeContentEvent { get; set; } = new();
            [field: SerializeField] public Subject<UIViewSkillInfoPopUp.UIModel> ShowUIViewSkillInfoPopUpEvent { get; set; } = new();
            [field: SerializeField] public Subject<Unit> CloseSkillInfoPopUp { get; set; } = new();
        }

        [field: SerializeField] public UIViewSkillList SkillLists { get; set; }
        [field: SerializeField] public TextMeshProUGUI HeroName { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Hp { get; private set; }
        [field: SerializeField] public TextMeshProUGUI HpRegen { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Mana { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ManaRegen { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Level { get; private set; }
        [field: SerializeField] public Image HpBar { get; private set; }
        [field: SerializeField] public Image ManaBar { get; private set; }
        [field: SerializeField] public RectTransform NoManaNotification { get; private set; }
        [field: SerializeField] public RectTransform CoolDownNotification { get; private set; }
        [field: SerializeField] public UIViewTalentTreeLevel[] TalentTreeLevels { get; set; }
        [field: SerializeField] public UIViewButton TalentTreeButton { get; set; }
        [field: SerializeField] public UIViewTalentTreePopUp UIViewTalentTreePopUp { get; set; }
        [field: SerializeField] public UIViewSkillInfoPopUp UIViewSkillInfoPopUp { get; set; }
        

        private Tween noManaTween;
        private Tween coolDownTween;
        
        protected override void OnSetModel(UIModel model)
        {
            model.HeroName.Subscribe(val => HeroName.text = val).AddTo(disposables);
            SkillLists.SetModel(model.SkillLists);
            model.MaxHp.Subscribe(_ => { UpdateHp(); }).AddTo(disposables);
            model.Hp.Subscribe(_ => { UpdateHp(); }).AddTo(disposables);
            model.MaxMana.Subscribe(_ => { UpdateMana(); }).AddTo(disposables);
            model.Mana.Subscribe(_ => { UpdateMana(); }).AddTo(disposables);
            model.HpRegen.Subscribe(_ => HpRegen.text = $"+{model.HpRegen}").AddTo(disposables);
            model.ManaRegen.Subscribe(_ => ManaRegen.text = $"+{model.ManaRegen}").AddTo(disposables);
            model.Level.Subscribe(_ => Level.text = $"{model.Level}").AddTo(disposables);
            model.ShowNoManaEvent.Subscribe(_ =>
            {
                PlayTweenNotificationAnimation(ref noManaTween, NoManaNotification);
            }).AddTo(disposables);
            
            model.ShowCoolDownEvent.Subscribe(_ =>
            {
                PlayTweenNotificationAnimation(ref coolDownTween, CoolDownNotification);
            }).AddTo(disposables);

            for (var index = 0; index < model.TalentTreeLevels.Length; index++)
            {
                TalentTreeLevels[index].SetModel(model.TalentTreeLevels[index]);
            }
            TalentTreeButton.SetModel(model.TalentTreeButton);

            model.ShowTalentTreeContentEvent.Subscribe(contents =>
            {
                UIViewTalentTreePopUp.SetModel(contents);
                UIViewTalentTreePopUp.In();
            }).AddTo(disposables);
            
            model.ShowUIViewSkillInfoPopUpEvent.Subscribe(contents =>
            {
                UIViewSkillInfoPopUp.SetModel(contents);
                UIViewSkillInfoPopUp.In();
            }).AddTo(disposables);
            
            model.CloseSkillInfoPopUp.Subscribe(_ =>
            {
                UIViewSkillInfoPopUp.Out();
            }).AddTo(disposables);
        }

        private void PlayTweenNotificationAnimation(ref Tween tween, RectTransform notification)
        {
            tween?.Kill(true);

            notification.gameObject.SetActive(true);
            notification.localScale = Vector3.zero;
            var notificationAnchoredPosition = notification.anchoredPosition;
            var currentAnchorPosY = notificationAnchoredPosition.y;
            notification.anchoredPosition = new Vector2(notificationAnchoredPosition.x, currentAnchorPosY - 20.0f);
            tween = DOTween.Sequence()
                .Append(notification.DOScale(1.0f, 0.2f))
                .Join(notification.DOAnchorPosY(currentAnchorPosY, 0.2f).SetEase(Ease.OutBack, 2.0f))
                .AppendInterval(1.0f)
                .OnComplete(() => notification.gameObject.SetActive(false));
        }

        private void UpdateMana()
        {
            Mana.text = $"{Mathf.CeilToInt(Model.Mana.Value)}/{Model.MaxMana}";
            ManaBar.fillAmount = Model.Mana.Value / Model.MaxMana.Value;
        }

        private void UpdateHp()
        {
            Hp.text = $"{Mathf.CeilToInt(Model.Hp.Value)}/{Model.MaxHp}";
            HpBar.fillAmount = Model.Hp.Value / Model.MaxHp.Value;
        }
    }
}