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

        public virtual void SetJsonData(string jsonData)
        {
            ConfigList = JsonConvert.DeserializeObject<TConfig[]>(jsonData);
            configDictionary = ConfigList.ToDictionary(x => ConfigToKeyFactory(x), x => x);
        }
    }

    public abstract class ConfigContainerGroup<TConfig, TKey, TGroupKey> : ConfigContainer<TConfig, TKey>
    {
        protected abstract Func<TConfig, TGroupKey> ConfigToGroupKeyFactory { get; }

        public ILookup<TGroupKey, TConfig> GroupConfigLookUp { get; private set; }
        
        public override void SetJsonData(string jsonData)
        {
            base.SetJsonData(jsonData);
            GroupConfigLookUp = ConfigList.ToLookup(x => ConfigToGroupKeyFactory(x), x => x);
        }
    }
}

