using System;
using MobaPrototype.Config;
using MobaPrototype.UIViewImplementation;
using UniRx;
using VContainer.Unity;

namespace MobaPrototype
{
    public class UIViewInGameHUDPresenter : IInitializable, IDisposable
    {
        private readonly UIViewInGameHUD uiViewInGameHUD;
        private readonly PlayerSelectionPresenter playerSelectionPresenter;
        private readonly ConfigHeroContainer configHeroContainer;
        private CompositeDisposable disposables = new();

        public UIViewInGameHUDPresenter(UIViewInGameHUD uiViewInGameHUD, PlayerSelectionPresenter playerSelectionPresenter, ConfigHeroContainer configHeroContainer)
        {
            this.uiViewInGameHUD = uiViewInGameHUD;
            this.playerSelectionPresenter = playerSelectionPresenter;
            this.configHeroContainer = configHeroContainer;
        }

        public void Initialize()
        {
            uiViewInGameHUD.SetModel(new());
            playerSelectionPresenter.CurrentSelectHeroEntityModel.Subscribe(heroEntityModel =>
            {
                uiViewInGameHUD.Model.HeroName.Value = heroEntityModel == default ? string.Empty : heroEntityModel.HeroConfig.HeroName;
            }).AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
            disposables = default;
        }
    }
}