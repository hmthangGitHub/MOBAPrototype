namespace MobaPrototype.SkillEntity
{
    public interface IGetAttackAble
    {
        void GetDamage(float valueEffectValue);
        void GetSlow(float effectValue, float valueEffectValue);
        void GetDamagePerSecond(float valueEffectValue, float valueEffectDuration);
        void GetStunned(float valueEffectValue, float valueEffectDuration);
    }
}