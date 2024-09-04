using System;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigSkillEffectLevelContainer : ConfigContainerGroup<ConfigSkillEffectLevelContainer.Config, int, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_skill_effect_level_key")]
            public int ConfigSkillEffectLevelKey { get; private set; }
                
            [JsonProperty("config_skill_effect_key")]
            public int ConfigSkillEffectKey { get; private set; }
                
            [JsonProperty("effect_value")]
            public float EffectValue { get; private set; }

            [JsonProperty("effect_duration")]
            public float EffectDuration { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigSkillEffectLevelKey;
        protected override Func<Config, int> ConfigToGroupKeyFactory => x => x.ConfigSkillEffectKey;
    }
}