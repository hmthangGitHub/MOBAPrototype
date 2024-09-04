namespace MobaPrototype.Skills
{
    public interface IGetAttackAble
    {
        void GetDamage(float valueEffectValue);
        void GetSlow(float effectValue, float duration);
        void GetDamagePerSecond(float valueEffectValue, float valueEffectDuration);
        void GetStunned(float duration);
    }
}