using System;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigSkillContainer : ConfigContainerGroup<ConfigSkillContainer.Config, int, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_skill_key")]
            public int ConfigSkillKey { get; private set; }

            [JsonProperty("config_hero_key")]
            public int ConfigHeroKey { get; private set; }

            [JsonProperty("skill_name")]
            public string SkillName { get; private set; }

            [JsonProperty("skill_cast_type")]
            public SkillCastType SkillCastType { get; private set; }

            [JsonProperty("description")]
            public string Description { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigSkillKey;
        protected override Func<Config, int> ConfigToGroupKeyFactory => x => x.ConfigHeroKey;
    }
}