using System;
using MobaPrototype.UIViewImplementation;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace MobaPrototype
{
    public class UIViewDebugSideBarPresenter : IInitializable, IDisposable
    {
        private UIViewDebugSideBar uiViewDebugSideBar;
        private CompositeDisposable disposables = new();
        private ReactiveProperty<bool> _freeSpellToggle = new (false);

        public UIViewDebugSideBarPresenter(UIViewDebugSideBar uiViewDebugSideBar)
        {
            this.uiViewDebugSideBar = uiViewDebugSideBar;
        }

        public void Initialize()
        {
            uiViewDebugSideBar.SetModel(new UIViewDebugSideBar.UIModel()
            {
                LevelUp = new()
                {
                    OnClick = () =>
                    {
                        Debug.Log("LevelUp");
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