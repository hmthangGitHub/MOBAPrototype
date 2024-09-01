using System;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigSkillEffectLevelContainer : ConfigContainer<ConfigSkillEffectLevelContainer.Config, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_skill_effect_level_key")]
            public int ConfigSkillEffectLevelKey { get; private set; }
                
            [JsonProperty("config_skill_effect_key")]
            public int ConfigSkillEffectKey { get; private set; }
                
            [JsonProperty("effect_value")]
            public int EffectValue { get; private set; }

            [JsonProperty("effect_duration")]
            public int EffectDuration { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigSkillEffectLevelKey;
    }
}