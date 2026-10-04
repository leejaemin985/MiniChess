using System;
using MiniChess.Core.Common;
using MiniChess.Core.Data;
using MiniChess.Core.Events;
using MiniChess.Core.State;

namespace MiniChess.Core.Turns
{
    /// <summary>Battle 단계의 턴 진행(시작/종료/교대)을 담당한다.</summary>
    public static class TurnSystem
    {
        /// <summary>Setup 단계를 끝내고 Battle 단계로 넘어가 선공 팀의 첫 턴을 시작한다.</summary>
        public static void StartBattle(GameState state)
        {
            if (state.Phase != GamePhase.Setup)
                throw new InvalidOperationException($"Battle 은 Setup 단계에서만 시작할 수 있음. 현재: {state.Phase}");

            state.Phase = GamePhase.Battle;
            state.CurrentTeam = state.Rules.Match.FirstTeam;
            state.TurnNumber = 1;

            BeginTurn(state);
        }

        /// <summary>현재 팀의 턴을 끝내고 상대 팀의 턴을 시작한다.</summary>
        internal static void EndTurn(GameState state)
        {
            if (state.Phase != GamePhase.Battle)
                throw new InvalidOperationException($"턴 종료는 Battle 단계에서만 가능. 현재: {state.Phase}");

            // TODO: 턴 종료 처리(회복 → DoT → 장판 → 지속시간 감소 → 점령 판정)는 해당 시스템 구현 시 추가.

            state.Events.Record(new TurnEndedEvent(state.CurrentTeam, state.TurnNumber));

            state.CurrentTeam = GetOpponent(state.CurrentTeam);
            state.TurnNumber++;

            BeginTurn(state);
        }

        public static Team GetOpponent(Team team)
        {
            return team == Team.Player1 ? Team.Player2 : Team.Player1;
        }

        /// <summary>
        /// 현재 팀의 턴 시작 처리.
        /// 첫 턴은 StartAp 만 적용하고, 두 번째 턴부터 AP 를 회복한다.
        /// </summary>
        private static void BeginTurn(GameState state)
        {
            PlayerState player = state.CurrentPlayer;

            state.Events.Record(new TurnStartedEvent(state.CurrentTeam, state.TurnNumber));

            if (player.TurnsStarted > 0)
                RecoverAp(player.Ap, state.Rules.Ap);

            player.TurnsStarted++;

            foreach (Unit unit in player.Units)
                unit.ResetTurnState();
        }

        private static void RecoverAp(ApPool ap, ApRuleData rule)
        {
            if (!rule.CarryOver)
                ap.Clear();

            ap.Recover(rule.TurnRecoveryAp);
        }
    }
}
