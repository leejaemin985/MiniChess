namespace MiniChess.Core.Skills
{
    /// <summary>스킬이 유닛의 턴 행동 중 무엇에 해당하는지(명세 3.2).</summary>
    public enum SkillActionKind
    {
        /// <summary>전투 행동. 턴당 1회이며 사용 후 자발적 이동이 잠긴다.</summary>
        Combat,

        /// <summary>이동을 포함한 전투 행동(돌진 등). 전투 행동 1회를 사용한다.</summary>
        CombatWithMovement,

        /// <summary>
        /// 이동 계열 기능(낫의 분신 교환 등). 전투 행동을 쓰지 않으며, 전투 행동 후(자발적 이동 잠금)에는 사용할 수 없다.
        /// </summary>
        Movement,
    }
}
