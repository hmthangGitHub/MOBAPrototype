using System.Collections.Generic;
using MobaPrototype.Hero;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer.Unity;

namespace MobaPrototype
{
    public class PlayerSelectionPresenter : IInitializable, ITickable
    {
        private Camera mainCamera;
        public ReactiveProperty<HeroEntityModel> CurrentSelectHeroEntityModel { get; set; } = new(default);
        public ReactiveProperty<HeroCommand> CurrentSelectHeroCommand { get; set; } = new(default);

        public void Initialize()
        {
            mainCamera = Camera.main;
        }
        
        public void Tick()
        {
            if (!Input.GetMouseButtonUp(0)) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out var raycastHit) && raycastHit.collider.TryGetComponent<IHeroController>(out var heroController))
            {
                CurrentSelectHeroEntityModel.Value = heroController.HeroEntityModel;
                CurrentSelectHeroCommand.Value = heroController.HeroCommand;
            }
        }
    }
}