using System;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigTalentTreeEffectContainer : ConfigContainerGroup<ConfigTalentTreeEffectContainer.Config, int, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_talent_tree_effect_key")]
            public int ConfigTalentTreeEffectKey { get; private set; }

            [JsonProperty("config_talent_tree_key")]
            public int ConfigTalentTreeKey { get; private set; }

            [JsonProperty("talent_tree_effect_type")]
            public TalentTreeEffectType TalentTreeEffectType { get; private set; }

            [JsonProperty("value")]
            public float Value { get; private set; }
            
            [JsonProperty("duration")]
            public float Duration { get; private set; }

            [JsonProperty("string_value")]
            public string StringValue { get; private set; }

            [JsonProperty("config_skill_key_target")]
            public int ConfigSkillKeyTarget { get; private set; }

            [JsonProperty("config_skill_effect_target")]
            public int ConfigSkillEffectTarget { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigTalentTreeEffectKey;
        protected override Func<Config, int> ConfigToGroupKeyFactory => x => x.ConfigTalentTreeKey;
    }
}