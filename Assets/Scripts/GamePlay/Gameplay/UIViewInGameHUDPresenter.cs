using System;
using System.Linq;
using MobaPrototype.Config;
using MobaPrototype.Hero;
using MobaPrototype.UIViewImplementation;
using UIView;
using UniRx;
using UnityEngine;
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
            playerSelectionPresenter.CurrentSelectHeroEntityModel.Subscribe(heroEntityModel =>
            {
                if (heroEntityModel == default) return;
                uiViewInGameHUD.SetModel(new()
                {
                    HeroName = new(heroEntityModel.HeroConfig.HeroName),
                    SkillLists = CreateSkillListUIViewModel().ToReactiveCollection(),
                    Hp = heroEntityModel.Hp,
                    Mana = heroEntityModel.Mana,
                    HpRegen = heroEntityModel.HpRegen,
                    ManaRegen = heroEntityModel.ManaRegen,
                    MaxHp = heroEntityModel.MaxHp,
                    MaxMana = heroEntityModel.MaxMana,
                });
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
            return playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillModels.Select((skillModel, skillIndex) => new UIViewSkill.UIModel()
            {
                Button = new UIViewButton.UIModel()
                {
                    OnHover = () =>
                    {
                        playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillPreviewRangeCommand.OnNext(new()
                        {
                            SkillIndex = skillIndex
                        });
                    },
                    OnHoverExit = () => playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillPreviewExitCommand.OnNext(new ()
                    {
                        SkillIndex = skillIndex
                    }),
                    OnClick = () =>
                    {
                        if (!ValidatedSkillUsage(skillIndex)) return;
                        
                        if (skillModel.SkillCastType.Value == SkillCastType.Direction || skillModel.SkillCastType.Value == SkillCastType.Target)
                        {
                            playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillPreviewCommand.OnNext(new()
                            {
                                SkillIndex = skillIndex
                            });
                        }

                        if (skillModel.SkillCastType.Value == SkillCastType.Aoe)
                        {
                            playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillCastingCommand.OnNext(new()
                            {
                                SkillIndex = skillIndex
                            });
                        }
                    },
                },
                HotKey = hotKeyConfiguration.HotKeys[skillIndex],
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
                },
                CoolDownTimeStamp = skillModel.CoolDownTimeStamp,
                CoolDownTotalTime = skillModel.CoolDown
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

        private bool ValidatedSkillUsage(int skillIndex)
        {
            if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillModels[skillIndex].Level.Value < 0) return false;
            if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillModels[skillIndex].ManaCost.Value >
                playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Mana.Value)
            {
                uiViewInGameHUD.Model.ShowNoManaEvent.OnNext(default);
                return false;
            }
            if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillModels[skillIndex].CoolDownTimeStamp.Value > Time.time)
            {
                uiViewInGameHUD.Model.ShowCoolDownEvent.OnNext(default);
                return false;
            }
            return true;
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