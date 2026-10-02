using MiniChess.Core.State;

namespace MiniChess.Core.Effects
{
    /// <summary>칸 효과(OnEnter / OnStop)에 전달되는 정보.</summary>
    public class CellEffectContext
    {
        public Board Board { get; }
        public BoardCell Cell { get; }
        public Unit Unit { get; }

        public CellEffectContext(Board board, BoardCell cell, Unit unit)
        {
            Board = board;
            Cell = cell;
            Unit = unit;
        }
    }
}
