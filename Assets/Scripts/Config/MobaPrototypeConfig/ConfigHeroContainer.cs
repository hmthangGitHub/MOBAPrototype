using System;
using ConfigBase;
using Unity.Plastic.Newtonsoft.Json;

namespace MobaPrototype.Config
{
    public class ConfigHeroContainer : ConfigContainer<ConfigHeroContainer.Config, int>
    {
        [Serializable]
        public class Config
        {
            [JsonProperty("config_hero_key")] public int ConfigHeroKey { get; private set; }
            [JsonProperty("hero_name")] public string HeroName { get; private set; }
        }

        protected override Func<Config, int> ConfigToKeyFactory => x => x.ConfigHeroKey;
    }
    
    namespace MobaPrototype.Config
    {
    }

}