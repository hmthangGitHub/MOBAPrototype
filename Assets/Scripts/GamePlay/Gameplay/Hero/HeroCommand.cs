using UniRx;
using UnityEngine;

namespace MobaPrototype.Hero
{
    public class HeroCommand
    {
        public Subject<SkillCastingCommand> SkillCastingCommand { get; private set; } = new();
        public Subject<SkillPreviewCommand> SkillPreviewCommand { get; private set; } = new();
        public Subject<SkillPreviewCommand> SkillPreviewRangeCommand { get; private set; } = new();
        public Subject<SkillPreviewExitCommand> SkillPreviewExitCommand { get; private set; } = new();
    }

    public class SkillPreviewExitCommand
    {
        public int SkillIndex { get; set; }
    }

    public class SkillPreviewCommand
    {
        public int SkillIndex { get; set; }
    }

    public class SkillCastingCommand
    {
        public int SkillIndex { get; set; }
    }
}