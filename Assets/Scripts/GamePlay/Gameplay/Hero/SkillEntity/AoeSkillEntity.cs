using System;
using UnityEngine;

namespace MobaPrototype.Skills
{
    public class AoeSkillEntity : MonoBehaviour, IAoeSkillEntity
    {
        public class AoeSkillModel
        {
            public SkillEntityModel SkillEntityModel { get; set; }
            public float Aoe { get; set; }
            public Vector3 Position { get; set; }
        }
        
        [SerializeField] private SkillEntity skillEntity;
        [SerializeField] private Transform skillAoeContainer;
        
        public void SetModel(AoeSkillModel aoeSkillModel)
        {
            skillEntity.SkillEntityModel = aoeSkillModel.SkillEntityModel;
            skillAoeContainer.localScale = Vector3.one * aoeSkillModel.Aoe / 100.0f;
            transform.position = aoeSkillModel.Position;
        }
    }
}