using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MobaPrototype.Hero
{
    public interface IHeroController
    {
        public HeroEntityModel HeroEntityModel { get; }
    }
    
    public class HeroController : LifetimeScope, IHeroController
    {
        [field: SerializeField] public int ConfigHeroKey { get; private set; } = 0;
        [field: SerializeField] public HeroEntityModel HeroEntityModel { get; set; }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterEntryPoint<HeroControllerInitializer>();
        }
    }
}