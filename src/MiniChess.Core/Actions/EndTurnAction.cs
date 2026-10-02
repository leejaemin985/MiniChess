using System;
using MiniChess.Core.Common;
using MiniChess.Core.State;
using MiniChess.Core.Turns;

namespace MiniChess.Core.Actions
{
    /// <summary>플레이어가 자기 턴을 끝내는 명령. AP 가 남아 있어도 언제든 가능하다.</summary>
    public class EndTurnAction
    {
        public Team Team { get; }

        public EndTurnAction(Team team)
        {
            Team = team;
        }

        public EndTurnFailReason Validate(GameState state)
        {
            if (state.Phase != GamePhase.Battle) return EndTurnFailReason.NotBattlePhase;
            if (Team != state.CurrentTeam) return EndTurnFailReason.NotYourTurn;

            return EndTurnFailReason.None;
        }

        public void Execute(GameState state)
        {
            EndTurnFailReason reason = Validate(state);
            if (reason != EndTurnFailReason.None)
                throw new InvalidOperationException($"턴 종료 불가: {reason}");

            TurnSystem.EndTurn(state);
        }
    }
}
