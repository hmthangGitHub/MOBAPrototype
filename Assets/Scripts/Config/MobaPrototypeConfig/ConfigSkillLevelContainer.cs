using System;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigSkillLevelContainer : ConfigContainer<ConfigSkillLevelContainer.Config, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_skill_level_key")]
            public int ConfigSkillLevelKey { get; private set; }

            [JsonProperty("skill_name")]
            public string SkillName { get; private set; }

            [JsonProperty("mana_cost")]
            public int ManaCost { get; private set; }

            [JsonProperty("cool_down")]
            public int CoolDown { get; private set; }

            [JsonProperty("cast_range")]
            public int CastRange { get; private set; }

            [JsonProperty("aoe")]
            public int Aoe { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigSkillLevelKey;
    }
}