using System.Collections;
using System.Collections.Generic;
using MobaPrototype.UIViewImplementation;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype
{
    public class GamePlayScope : LifetimeScope
    {
        [SerializeField] private UIViewDebugSideBar uiViewDebugSideBar;
        [SerializeField] private UIViewInGameHUD uiViewInGameHUD;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterInstance(uiViewDebugSideBar);
            builder.RegisterInstance(uiViewInGameHUD);
            builder.RegisterEntryPoint<UIViewDebugSideBarPresenter>();
            builder.RegisterEntryPoint<UIViewInGameHUDPresenter>();
        }
    }
}