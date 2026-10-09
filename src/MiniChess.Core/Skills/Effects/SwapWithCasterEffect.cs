using MiniChess.Core.Movement;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Effects
{
    /// <summary>시전자와 지정 칸의 유닛 자리를 맞바꾼다. 상대 쪽 위치 변경은 시전자가 일으킨 외부 이동이다.</summary>
    public class SwapWithCasterEffect : SkillEffect
    {
        public override void Apply(SkillContext context)
        {
            Unit other = context.State.Board.GetCell(context.Target).Occupant;
            MovementResolver.ResolveSwap(context.State, context.Caster, other, context.Caster);
        }
    }
}
