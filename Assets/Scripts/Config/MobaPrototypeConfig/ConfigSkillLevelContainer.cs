using System;
using System.Linq;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigSkillLevelContainer : ConfigContainerGroup<ConfigSkillLevelContainer.Config, int, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_skill_level_key")]
            public int ConfigSkillLevelKey { get; private set; }

            [JsonProperty("config_skill_key")]
            public int ConfigSkillKey { get; private set; }

            [JsonProperty("mana_cost")]
            public int ManaCost { get; private set; }

            [JsonProperty("cool_down")]
            public int CoolDown { get; private set; }

            [JsonProperty("cast_range")]
            public int CastRange { get; private set; }

            [JsonProperty("aoe")]
            public int Aoe { get; private set; }
            
            [JsonProperty("hero_requirement_level")]
            public int HeroRequirementLevel { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigSkillLevelKey;
        protected override Func<Config, int> ConfigToGroupKeyFactory => x => x.ConfigSkillKey;
    }
}