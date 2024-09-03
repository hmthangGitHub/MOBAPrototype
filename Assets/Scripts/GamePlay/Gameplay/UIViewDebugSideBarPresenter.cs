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
                        playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Level.Value++;
                        playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillPointRemaining.Value++;
                    },
                },
                LevelMax = new()
                {
                    OnClick = () =>
                    {
                        Debug.Log("LevelMax");
                    },
                },
                ResetLevel = new()
                {
                    OnClick = () =>
                    {
                        Debug.Log("ResetLevel");
                    },
                },
                FreeSpellToggle = _freeSpellToggle,
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