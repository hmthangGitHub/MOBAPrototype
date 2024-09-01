using System.Collections;
using System.Collections.Generic;
using Config;
using ConfigBase;
using MobaPrototype.Config;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype.Scope
{
    public class GameBootstrapper : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            RegisterConfigs(builder);
            builder.RegisterEntryPoint<ConfigInitializer>();
            builder.RegisterEntryPoint<CustomAsyncStartService>();
            builder.RegisterEntryPoint<GameSceneLoader>();
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.Register<ConfigHeroContainer>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ConfigSkillContainer>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ConfigSkillEffectContainer>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ConfigSkillLevelContainer>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ConfigSkillEffectLevelContainer>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private static string GetContainerDataPath<TContainer>() where TContainer : IConfigContainer
        {
            var typeName = typeof(TContainer).Name;
            typeName = typeName.Replace("Container", string.Empty);
            typeName = typeName.Replace("Config", string.Empty);
            return $"ConfigJsonData/{typeName}.json";
        }
    }
}