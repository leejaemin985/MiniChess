using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Combat
{
    /// <summary>
    /// 유닛 사망과 경기 종료를 처리한다. 피해 원인(공격/덫/장판 등)과 무관하게 이곳 한 곳에서 처리한다.
    /// </summary>
    public static class DeathSystem
    {
        /// <summary>
        /// 유닛이 죽었으면 보드에서 제거하고 경기 종료 여부를 판정한다.
        /// 죽은 유닛이면 true. 플레이어의 유닛 목록에는 남겨둔다(부활 대상 보존).
        /// </summary>
        internal static bool HandleIfDead(GameState state, Unit unit)
        {
            if (unit.IsAlive)
                return false;

            if (unit.IsPlaced)
                state.Board.Remove(unit);

            CheckGameEnd(state);
            return true;
        }

        /// <summary>
        /// 전멸한 팀이 있으면 경기를 끝낸다.
        /// 한쪽만 전멸 → 상대 승리, 양쪽 동시 전멸 → 무승부(Winner = null).
        /// </summary>
        internal static void CheckGameEnd(GameState state)
        {
            if (state.Phase != GamePhase.Battle)
                return;

            bool player1Eliminated = state.GetPlayer(Team.Player1).IsEliminated;
            bool player2Eliminated = state.GetPlayer(Team.Player2).IsEliminated;

            if (!player1Eliminated && !player2Eliminated)
                return;

            state.Phase = GamePhase.Ended;

            if (player1Eliminated && player2Eliminated)
                state.Winner = null;
            else
                state.Winner = player1Eliminated ? Team.Player2 : Team.Player1;
        }
    }
}
