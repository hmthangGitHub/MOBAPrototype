using MobaPrototype.Hero;

namespace MobaPrototype.Skills
{
    public interface ITargetAble
    {
        void HighLight(bool enable);
        void ApplySkillEffectToTarget(SkillEffectModel[] skillEffectModels);
    }
}