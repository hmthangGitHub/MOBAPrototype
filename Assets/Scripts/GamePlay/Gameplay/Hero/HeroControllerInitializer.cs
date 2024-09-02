using System.Linq;
using MobaPrototype.Config;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype.Hero
{
    public class HeroControllerInitializer : IInitializable
    {
        private LifetimeScope scope;
        private HeroController HeroController => (HeroController)scope;
        private ConfigHeroContainer configHeroContainer;
        private ConfigSkillContainer configSkillContainer;

        [Inject]
        public HeroControllerInitializer(LifetimeScope scope, ConfigHeroContainer configHeroContainer, ConfigSkillContainer configSkillContainer)
        {
            this.scope = scope;
            this.configHeroContainer = configHeroContainer;
            this.configSkillContainer = configSkillContainer;
        }

        public void Initialize()
        {
            HeroController.HeroEntityModel = new()
            {
                Level = new(1),
                TalentTree = new(2),
                HeroConfig = configHeroContainer.ConfigDictionary[HeroController.ConfigHeroKey],
                SkillCastingModel = new(),
                SkillModels = configSkillContainer.ConfigList.Select(x => new SkillModel()
                {
                    Level = new(0),
                }).ToArray(),
            };
        }
    }
}