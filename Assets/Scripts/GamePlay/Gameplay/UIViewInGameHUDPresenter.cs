using System;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        private readonly ConfigTalentTreeContainer configTalentTreeContainer;
        private readonly ConfigSkillEffectContainer configSkillEffectContainer;
        private readonly ConfigSkillEffectLevelContainer configSkillEffectLevelContainer;
        
        private CompositeDisposable disposables = new();
        private CompositeDisposable changeHeroDisposables = new();

        public UIViewInGameHUDPresenter(UIViewInGameHUD uiViewInGameHUD,
            PlayerSelectionPresenter playerSelectionPresenter, ConfigHeroContainer configHeroContainer,
            HotKeyConfiguration hotKeyConfiguration, ConfigSkillLevelContainer configSkillLevelContainer,
            ConfigTalentTreeContainer configTalentTreeContainer, ConfigSkillEffectContainer configSkillEffectContainer, ConfigSkillEffectLevelContainer configSkillEffectLevelContainer)
        {
            this.uiViewInGameHUD = uiViewInGameHUD;
            this.playerSelectionPresenter = playerSelectionPresenter;
            this.configHeroContainer = configHeroContainer;
            this.hotKeyConfiguration = hotKeyConfiguration;
            this.configSkillLevelContainer = configSkillLevelContainer;
            this.configTalentTreeContainer = configTalentTreeContainer;
            this.configSkillEffectContainer = configSkillEffectContainer;
            this.configSkillEffectLevelContainer = configSkillEffectLevelContainer;
        }

        public void Initialize()
        {
            playerSelectionPresenter.CurrentSelectHeroEntityModel.Subscribe(heroEntityModel =>
            {
                if (heroEntityModel == default) return;
                changeHeroDisposables.Clear();
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
                    TalentTreeButton = new UIViewButton.UIModel()
                    {
                        OnClick = () =>
                        {
                            uiViewInGameHUD.Model.ShowTalentTreeContentEvent.OnNext(uiViewInGameHUD.Model
                                .TalentTreeLevels.Select((x, i) =>
                                {
                                    var leftTreeConfig = playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTreeConfigs[i * 2];
                                    var rightTreeConfig = playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTreeConfigs[i * 2 + 1];
                                    
                                    return new UIViewTalentTreePopUpContent.UIModel()
                                    {
                                        TalentTreeLevel = x,
                                        Level = leftTreeConfig.HeroRequirementLevel,
                                        DescriptionLeft = leftTreeConfig.Description,
                                        DescriptionRight = rightTreeConfig.Description,
                                        LeftButton = new UIViewButton.UIModel()
                                        {
                                            OnClick = () =>
                                            {
                                                if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTree.Contains(leftTreeConfig.ConfigTalentTreeKey)) return;
                                                if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Level.Value < leftTreeConfig.HeroRequirementLevel ) return;
                                                if(playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillPointRemaining.Value <= 0) return;
                                                playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTree.Add(leftTreeConfig.ConfigTalentTreeKey);
                                            }
                                        },
                                        RightButton = new UIViewButton.UIModel()
                                        {
                                            OnClick = () =>
                                            {
                                                if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTree.Contains(rightTreeConfig.ConfigTalentTreeKey)) return;
                                                if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Level.Value < leftTreeConfig.HeroRequirementLevel ) return;
                                                if(playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillPointRemaining.Value <= 0) return;
                                                playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTree.Add(rightTreeConfig.ConfigTalentTreeKey);
                                            }
                                        },
                                    };
                                }).ToReactiveCollection());
                        }
                    },
                    TalentTreeLevels = CreateTalentTreeLevels(),
                    Level = heroEntityModel.Level,
                });
                
                uiViewInGameHUD.In();
            }).AddTo(disposables);
        }

        private UIViewTalentTreeLevel.UIModel[] CreateTalentTreeLevels()
        {
            return playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTreeConfigs
                .Select((x, i) => (talentTreeConfig: x, index: i))
                .GroupBy(x => x.index / 2)
                .Select(x =>
                {
                    var uiModel = new UIViewTalentTreeLevel.UIModel();
                    OnTalentTreeChange(uiModel, x);
                    
                    playerSelectionPresenter.CurrentSelectHeroEntityModel
                        .Value
                        .TalentTree
                        .ObserveAdd()
                        .Subscribe(_ => OnTalentTreeChange(uiModel, x))
                        .AddTo(changeHeroDisposables);
                    return uiModel;
                })
                .ToArray();
        }

        private void OnTalentTreeChange(UIViewTalentTreeLevel.UIModel uiModel, IGrouping<int, (ConfigTalentTreeContainer.Config talentTreeConfig, int index)> x)
        {
            uiModel.LeftBranchUpgraded.Value = playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTree.Contains(x.ElementAt(0).talentTreeConfig.ConfigTalentTreeKey);
            uiModel.RightBranchUpgraded.Value = playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.TalentTree.Contains(x.ElementAt(1).talentTreeConfig.ConfigTalentTreeKey);
            uiModel.MainBranchUpgraded.Value = uiModel.RightBranchUpgraded.Value || uiModel.LeftBranchUpgraded.Value;
        }

        private UIViewSkill.UIModel[] CreateSkillListUIViewModel()
        {
            var heroEntityModel = playerSelectionPresenter.CurrentSelectHeroEntityModel.Value;
            return heroEntityModel.SkillModels.Select(
                (skillModel, skillIndex) =>
                {
                    var skillLevelConfigs = configSkillLevelContainer.GroupConfigLookUp[skillModel.ConfigSkill.ConfigSkillKey].ToArray();
                    return new UIViewSkill.UIModel()
                    {
                        Button = new UIViewButton.UIModel()
                        {
                            OnHover = () =>
                            {
                                playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillPreviewRangeCommand.OnNext(
                                    new()
                                    {
                                        SkillIndex = skillIndex
                                    });

                                uiViewInGameHUD.Model.ShowUIViewSkillInfoPopUpEvent.OnNext(
                                    new UIViewSkillInfoPopUp.UIModel()
                                    {
                                        Level = heroEntityModel.SkillModels[skillIndex].Level,
                                        Description = new(heroEntityModel.SkillModels[skillIndex].ConfigSkill
                                            .Description),
                                        CoolDown = new(string.Join('/', skillLevelConfigs.Select((x, skillLevelIndex) => GetValueLevelString(skillModel,
                                                    skillLevelIndex, x.CoolDown.ToString())))),
                                        ManaCost = new(string.Join('/', skillLevelConfigs.Select((x, skillLevelIndex) => GetValueLevelString(skillModel,
                                                    skillLevelIndex, x.ManaCost.ToString())))),
                                        CastType = new(
                                            (UIViewSkillInfoPopUp.SkillCastType)((int)skillModel.SkillCastType.Value)),
                                        Name = new(skillModel.ConfigSkill.SkillName),
                                        EffectList = CreateEffectListModel(skillModel)
                                    });
                            },
                            OnHoverExit = () =>
                            {
                                playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillPreviewExitCommand.OnNext(
                                    new()
                                    {
                                        SkillIndex = skillIndex
                                    });

                                uiViewInGameHUD.Model.CloseSkillInfoPopUp.OnNext(default);
                            },
                            OnClick = () =>
                            {
                                if (!ValidatedSkillUsage(skillIndex)) return;

                                if (skillModel.SkillCastType.Value == SkillCastType.Direction ||
                                    skillModel.SkillCastType.Value == SkillCastType.Target)
                                {
                                    playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillPreviewCommand.OnNext(
                                        new()
                                        {
                                            SkillIndex = skillIndex
                                        });
                                }

                                if (skillModel.SkillCastType.Value == SkillCastType.Aoe)
                                {
                                    playerSelectionPresenter.CurrentSelectHeroCommand.Value.SkillCastingCommand.OnNext(
                                        new()
                                        {
                                            SkillIndex = skillIndex
                                        });
                                }
                            },
                        },
                        HotKey = hotKeyConfiguration.HotKeys[skillIndex],
                        SkillLevels = skillLevelConfigs
                            .Select((skillLevelConfig, index) =>
                            {
                                var canBeUpgrade = CanBeUpgrade(skillModel, index, skillLevelConfig);
                                var upgraded = new ReactiveProperty<bool>(IsSkillUpgraded(skillModel, index));

                                var reactiveProperty = new ReactiveProperty<bool>(canBeUpgrade);
                                heroEntityModel
                                    .SkillPointRemaining
                                    .Subscribe(_ =>
                                        reactiveProperty.Value = CanBeUpgrade(skillModel, index, skillLevelConfig))
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
                            OnClick = () => { OnUpgradeSkillLevel(skillModel); }
                        },
                        CoolDownTimeStamp = skillModel.CoolDownTimeStamp,
                        CoolDownTotalTime = skillModel.CoolDown,
                        ManaCost = skillModel.ManaCost
                    };
                }).ToArray();

            bool CanBeUpgrade(SkillModel skillModel, int index, ConfigSkillLevelContainer.Config skillLevelConfig)
            {
                return skillModel.Level.Value == index
                       && heroEntityModel.SkillPointRemaining.Value > 0
                       && skillLevelConfig.HeroRequirementLevel <=
                       heroEntityModel.Level.Value;
            }

            bool IsSkillUpgraded(SkillModel skillModel, int index)
            {
                return skillModel.Level.Value >= (index + 1);
            }
        }

        private ReactiveCollection<UIViewSkillEffect.UIModel> CreateEffectListModel(SkillModel skillModel)
        {
            return configSkillEffectContainer.GroupConfigLookUp[skillModel.ConfigSkill.ConfigSkillKey].Select(x =>
            {
                var values = configSkillEffectLevelContainer.GroupConfigLookUp[x.ConfigSkillEffectKey].Select((levelConfig, skillEffectLevelIndex) =>
                {
                    var value = x.SkillEffectType == SkillEffectType.Stun
                        ? levelConfig.EffectDuration.ToString()
                        : levelConfig.EffectValue.ToString();
                    value = GetValueLevelString(skillModel, skillEffectLevelIndex, value);
                    return value;
                });

                return new UIViewSkillEffect.UIModel()
                {
                    SkillEffectType = new((UIViewSkillEffect.SkillEffectType)(object)x.SkillEffectType),
                    Value = new(string.Join('/', values))
                };
            }).ToReactiveCollection();
        }

        private static string GetValueLevelString(SkillModel skillModel, int skillEffectLevelIndex, string value)
        {
            if (skillModel.Level.Value == skillEffectLevelIndex + 1)
            {
                value = $"<color=#4D74D4>{value}</color>";
            }

            return value;
        }

        private bool ValidatedSkillUsage(int skillIndex)
        {
            if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillModels[skillIndex].Level.Value <
                0) return false;
            if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillModels[skillIndex].ManaCost.Value >
                playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.Mana.Value)
            {
                uiViewInGameHUD.Model.ShowNoManaEvent.OnNext(default);
                return false;
            }

            if (playerSelectionPresenter.CurrentSelectHeroEntityModel.Value.SkillModels[skillIndex].CoolDownTimeStamp
                    .Value > Time.time)
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
            changeHeroDisposables.Dispose();
            changeHeroDisposables = default;
        }
    }
}