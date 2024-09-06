using System;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigTalentTreeContainer : ConfigContainerGroup<ConfigTalentTreeContainer.Config, int, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_talent_tree_key")]
            public int ConfigTalentTreeKey { get; private set; }

            [JsonProperty("talent_tree_name")]
            public string TalentTreeName { get; private set; }

            [JsonProperty("config_hero_key")]
            public int ConfigHeroKey { get; private set; }

            [JsonProperty("description")]
            public string Description { get; private set; }

            [JsonProperty("hero_requirement_level")]
            public int HeroRequirementLevel { get; private set; }

            [JsonProperty("hero_requirement_level_2")]
            public int HeroRequirementLevel2 { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigTalentTreeKey;
        protected override Func<Config, int> ConfigToGroupKeyFactory => x => x.ConfigHeroKey;
    }
}
