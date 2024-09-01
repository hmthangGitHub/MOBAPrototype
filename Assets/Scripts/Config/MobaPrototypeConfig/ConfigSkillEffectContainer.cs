using System;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigSkillEffectContainer : ConfigContainer<ConfigSkillEffectContainer.Config, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_skill_effect_key")]
            public int ConfigSkillEffectKey { get; private set; }

            [JsonProperty("skill_effect_type")]
            public SkillEffectType SkillEffectType { get; private set; }

            [JsonProperty("config_skill_key")]
            public int ConfigSkillKey { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigSkillEffectKey;
    }
}