namespace MobaPrototype.Hero
{
    public interface IHeroSkillExecutor
    {
        void Execute(int skillIndex);
        void Preview(int skillIndex);
        void PreviewRange(int skillIndex);
        public void ExitPreview();
    }
}