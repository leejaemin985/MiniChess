using MiniChess.Core.Turns;

namespace MiniChess.Core.Statuses.Library
{
    /// <summary>
    /// 여러 캐릭터가 공유하는 상태 정의. 수치가 하나라도 TBD(null)면 정의를 만들지 않고 null 을 돌려준다
    /// (이 상태를 쓰는 스킬은 설정 누락으로 사용 불가가 된다).
    /// </summary>
    public static class StatusLibrary
    {
        public const string RootId = "ROOT";
        public const string BurnId = "BURN";

        /// <summary>
        /// 속박(이동 불가). 사슬 속박과 속박 덫이 같은 개념을 쓴다(명세 9.2, 12.3).
        /// 지속은 "대상 소유자의 턴 종료" 횟수로 센다. 1 이면 대상이 다음 행동 턴 한 번을 속박 상태로 보낸다.
        /// 부여된 턴의 종료는 세지 않는다(대상이 자기 턴에 덫을 밟아도 다음 자기 턴까지 유지).
        /// </summary>
        public static StatusDefinition Root(int? targetTurns, bool? blocksExternalMoves)
        {
            if (targetTurns == null || blocksExternalMoves == null)
                return null;

            return new StatusDefinition(
                RootId,
                targetTurns.Value,
                StatusStackPolicy.Refresh,
                new StatusTiming(TurnStep.EndDurationTick, TimingOwner.TargetOwner, includeApplicationTurn: false),
                behavior: new RootBehavior(blocksExternalMoves.Value));
        }

        /// <summary>
        /// 화상(DoT). 대상 소유자의 Turn End 에 피해(명세 10.3). 같은 화상은 스택하지 않고 갱신한다([설계안]).
        /// </summary>
        public static StatusDefinition Burn(int? damagePerTrigger, int? triggers)
        {
            if (damagePerTrigger == null || triggers == null)
                return null;

            return new StatusDefinition(
                BurnId,
                triggers.Value,
                StatusStackPolicy.Refresh,
                new StatusTiming(TurnStep.EndDurationTick, TimingOwner.TargetOwner, includeApplicationTurn: false),
                new StatusTiming(TurnStep.EndDamageOverTime, TimingOwner.TargetOwner, includeApplicationTurn: false),
                new BurnBehavior(damagePerTrigger.Value));
        }
    }
}
