namespace MiniChess.Core.Effects
{
    /// <summary>칸 효과가 발동한 뒤 진행 중인 이동을 어떻게 할지.</summary>
    public enum CellEnterResult
    {
        /// <summary>이동을 계속한다.</summary>
        Continue,

        /// <summary>이 칸에서 이동을 멈춘다.</summary>
        Stop,
    }
}
