namespace MiniChess.Core.Effects
{
    /// <summary>
    /// 칸 효과가 놓이는 레이어. 한 칸에는 레이어마다 효과가 최대 1개 존재한다.
    /// 선언 순서가 곧 발동 순서이며, 값은 0부터 연속이어야 한다(칸의 슬롯 인덱스로 사용).
    /// </summary>
    public enum CellEffectLayer
    {
        Trap = 0,
        AreaEffect = 1,

        /// <summary>이동을 막는 설치물(재봉사 장애물). 유닛이 들어갈 수 없으므로 진입 발동은 없다.</summary>
        Obstacle = 2,
    }
}
