using System;
using System.Linq;
using MobaPrototype.Config;
using MobaPrototype.Hero;
using MobaPrototype.UIViewImplementation;
using UIView;
using UniRx;
using VContainer.Unity;

namespace MobaPrototype
{
    public class UIViewInGameHUDPresenter : IInitializable, IDisposable
    {
        private readonly UIViewInGameHUD uiViewInGameHUD;
        private readonly PlayerSelectionPresenter playerSelectionPresenter;
        private readonly ConfigHeroContainer configHeroContainer;
        private readonly HotKeyConfiguration hotKeyConfiguration;
        private readonly ConfigSkillLevelContainer configSkillLevelContainer;
        private CompositeDisposable disposables = new();

        public UIViewInGameHUDPresenter(UIViewInGameHUD uiViewInGameHUD, PlayerSelectionPresenter playerSelectionPresenter, ConfigHeroContainer configHeroContainer, HotKeyConfiguration hotKeyConfiguration, ConfigSkillLevelContainer configSkillLevelContainer)
        {
            this.uiViewInGameHUD = uiViewInGameHUD;
            this.playerSelectionPresenter = playerSelectionPresenter;
            this.configHeroContainer = configHeroContainer;
            this.hotKeyConfiguration = hotKeyConfiguration;
            this.configSkillLevelContainer = configSkillLevelContainer;
        }

        public void Initialize()
        {
            uiViewInGameHUD.SetModel(new());
            playerSelectionPresenter.CurrentSelectHeroEntityModel.Subscribe(heroEntityModel =>
            {
                if (heroEntityModel == default) return;
                uiViewInGameHUD.Model.HeroName.Value = heroEntityModel.HeroConfig.HeroName;
                uiViewInGameHUD.Model.SkillLists.Clear();
                var skillListUIViewModel = CreateSkillListUIViewModel();
                foreach (var skillUIModel in skillListUIViewModel)
                {
                    uiViewInGameHUD.Model.SkillLists.Add(skillUIModel);
                }
            }).AddTo(disposables);
        }

        private UIViewSkill.UIModel[] CreateSkillListUIViewModel()
        {
            return playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillModels.Select((skillModel, i) => new UIViewSkill.UIModel()
            {
                Button = new UIViewButton.UIModel()
                {
                    OnHover = () => playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillPreviewCommand.OnNext(new ()
                    {
                        SkillIndex = i
                    }),
                    OnHoverExit = () => playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillPreviewExitCommand.OnNext(new ()
                    {
                        SkillIndex = i
                    }),
                    OnClick = () => playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillCastingCommand.OnNext(new ()
                    {
                        SkillIndex = i
                    }),
                },
                HotKey = hotKeyConfiguration.HotKeys[i],
                SkillLevels = configSkillLevelContainer.GroupConfigLookUp[skillModel.ConfigSkill.ConfigSkillKey].Select((skillLevelConfig, index) =>
                {
                    var canBeUpgrade = CanBeUpgrade(skillModel, index, skillLevelConfig);
                    var upgraded = new ReactiveProperty<bool>(IsSkillUpgraded(skillModel, index));
                    
                    var reactiveProperty = new ReactiveProperty<bool>(canBeUpgrade);
                    playerSelectionPresenter
                        .CurrentSelectHeroEntityModel
                        .Value
                        .SkillPointRemaining
                        .Subscribe(_ => reactiveProperty.Value = CanBeUpgrade(skillModel, index, skillLevelConfig))
                        .AddTo(disposables);

                    skillModel
                        .Level
                        .Subscribe(_ => upgraded.Value = IsSkillUpgraded(skillModel, index))
                        .AddTo(disposables);

                    return new UIViewSkillLevel.UIModel()
                    {
                        Upgraded = upgraded,
                        CanUpgrade = reactiveProperty,
                    };
                }).ToReactiveCollection(),
                UpgradeButton = new UIViewButton.UIModel()
                {
                    OnClick = () =>
                    {
                        OnUpgradeSkillLevel(skillModel);
                    }
                }
            }).ToArray();

            bool CanBeUpgrade(SkillModel skillModel, int index, ConfigSkillLevelContainer.Config skillLevelConfig)
            {
                return skillModel.Level.Value == index
                       && playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillPointRemaining.Value > 0
                       && skillLevelConfig.HeroRequirementLevel <= playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Level.Value;
            }

            bool IsSkillUpgraded(SkillModel skillModel, int index)
            {
                return skillModel.Level.Value >= (index + 1);
            }
        }

        private void OnUpgradeSkillLevel(SkillModel skillModel)
        {
            skillModel.Level.Value++;
            playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillPointRemaining.Value--;
        }

        public void Dispose()
        {
            disposables.Dispose();
            disposables = default;
        }
    }
}