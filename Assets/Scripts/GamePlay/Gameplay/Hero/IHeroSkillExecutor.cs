namespace MobaPrototype.Hero
{
    public interface IHeroSkillExecutor
    {
        void Execute(SkillCastingCommand skillCastingCommand);
        void Preview(SkillPreviewCommand skillPreviewCommand);
        public void ExitPreview(SkillPreviewExitCommand skillPreviewExitCommand);
    }
}