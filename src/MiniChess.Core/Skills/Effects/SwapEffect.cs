using MiniChess.Core.Movement;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Effects
{
    /// <summary>
    /// 지정한 두 칸의 유닛 자리를 맞바꾼다(첫 번째, 두 번째 지정). 시전자가 일으킨 외부 이동이며,
    /// 옮겨진 유닛의 행동 상태(이미 쓴 전투 행동 등)는 바뀌지 않는다.
    /// </summary>
    public class SwapEffect : SkillEffect
    {
        public override void Apply(SkillContext context)
        {
            Board board = context.State.Board;
            Unit a = board.GetCell(context.Targets[0]).Occupant;
            Unit b = board.GetCell(context.Targets[1]).Occupant;

            MovementResolver.ResolveSwap(context.State, a, b, context.Caster);
        }
    }
}
