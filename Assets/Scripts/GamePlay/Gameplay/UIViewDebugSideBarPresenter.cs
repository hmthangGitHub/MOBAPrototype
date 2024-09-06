using System;
using MobaPrototype.UIViewImplementation;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace MobaPrototype
{
    public class UIViewDebugSideBarPresenter : IInitializable, IDisposable
    {
        private readonly PlayerSelectionPresenter playerSelectionPresenter;
        
        private UIViewDebugSideBar uiViewDebugSideBar;
        private CompositeDisposable disposables = new();
        private ReactiveProperty<bool> _freeSpellToggle = new (false);

        public UIViewDebugSideBarPresenter(UIViewDebugSideBar uiViewDebugSideBar, PlayerSelectionPresenter playerSelectionPresenter)
        {
            this.uiViewDebugSideBar = uiViewDebugSideBar;
            this.playerSelectionPresenter = playerSelectionPresenter;
        }

        public void Initialize()
        {
            uiViewDebugSideBar.SetModel(new UIViewDebugSideBar.UIModel()
            {
                LevelUp = new()
                {
                    OnClick = () =>
                    {
                        if(playerSelectionPresenter.CurrentSelectHeroEntityModel.Value == default) return;
                        if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Level.Value >= 25) return;
                        playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Level.Value++;
                        playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillPointRemaining.Value++;
                    },
                },
                LevelMax = new()
                {
                    OnClick = () =>
                    {
                        if(playerSelectionPresenter.CurrentSelectHeroEntityModel.Value == default) return;
                        playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillPointRemaining.Value += 25 - playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Level.Value;
                        playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Level.Value = 25;
                    },
                },
                ResetManaAndCd = new()
                {
                    OnClick = () =>
                    {
                        playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Mana.Value = playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.MaxMana.Value;
                    }
                }
            });

            _freeSpellToggle.Subscribe(val => { }).AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
            disposables = default;
        }
    }
}