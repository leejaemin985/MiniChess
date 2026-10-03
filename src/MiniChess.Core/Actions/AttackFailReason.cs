namespace MiniChess.Core.Actions
{
    public enum AttackFailReason
    {
        None,
        NotBattlePhase,
        NotYourTurn,

        /// <summary>공격하는 유닛이 보드 위에 없거나 사망.</summary>
        AttackerNotOnBoard,

        AlreadyActed,

        /// <summary>대상이 보드 위에 없거나 사망.</summary>
        InvalidTarget,

        TargetNotEnemy,
        OutOfRange,
        NotEnoughAp,
    }
}
