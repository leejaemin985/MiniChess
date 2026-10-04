using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Turns
{
    /// <summary>턴 단계 처리 중 훅에 전달되는 정보.</summary>
    public class TurnContext
    {
        public GameState State { get; }
        public TurnStep Step { get; }

        /// <summary>지금 턴을 진행 중인(시작/종료 중인) 팀.</summary>
        public Team ActiveTeam { get; }

        public TurnContext(GameState state, TurnStep step, Team activeTeam)
        {
            State = state;
            Step = step;
            ActiveTeam = activeTeam;
        }
    }
}
