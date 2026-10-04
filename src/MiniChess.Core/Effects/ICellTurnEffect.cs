using MiniChess.Core.State;
using MiniChess.Core.Turns;

namespace MiniChess.Core.Effects
{
    /// <summary>턴 단계에도 반응하는 칸 효과(턴 종료 장판 피해, 턴 시작 회복 장판 등).</summary>
    public interface ICellTurnEffect : ICellEffect
    {
        /// <summary>
        /// 모든 턴 단계마다 호출된다. 어느 단계/누구의 턴에 반응할지는 효과가 context 로 판단한다.
        /// </summary>
        void OnTurnStep(CellTurnContext context);
    }

    public class CellTurnContext
    {
        public TurnContext Turn { get; }
        public BoardCell Cell { get; }

        public GameState State => Turn.State;

        public CellTurnContext(TurnContext turn, BoardCell cell)
        {
            Turn = turn;
            Cell = cell;
        }
    }
}
