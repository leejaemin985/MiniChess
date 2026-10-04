using MiniChess.Core.State;

namespace MiniChess.Core.Effects
{
    /// <summary>칸 효과(OnEnter / OnStop)에 전달되는 정보.</summary>
    public class CellEffectContext
    {
        /// <summary>경기 상태. 피해/회복 등은 이 상태를 통해 DamageSystem 으로 처리한다.</summary>
        public GameState State { get; }
        public BoardCell Cell { get; }
        public Unit Unit { get; }

        public Board Board => State.Board;

        public CellEffectContext(GameState state, BoardCell cell, Unit unit)
        {
            State = state;
            Cell = cell;
            Unit = unit;
        }
    }
}
