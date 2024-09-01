using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;

namespace ConfigBase
{
    public interface IConfigContainer
    {
        void SetJsonData(string jsonData);
    }
    
    
    public abstract class ConfigContainer<TConfig, TKey> : IConfigContainer
    {
        private Dictionary<TKey, TConfig> configDictionary;
        protected abstract Func<TConfig, TKey> ConfigToKeyFactory { get; }
        public TConfig[] ConfigList { get; private set; }
        public IReadOnlyDictionary<TKey, TConfig> ConfigDictionary => configDictionary;

        public void SetJsonData(string jsonData)
        {
            ConfigList = JsonConvert.DeserializeObject<TConfig[]>(jsonData);
            configDictionary = ConfigList.ToDictionary(x => ConfigToKeyFactory(x), x => x);
        }
    }
}

