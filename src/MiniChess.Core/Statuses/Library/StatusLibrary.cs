using MiniChess.Core.Turns;

namespace MiniChess.Core.Statuses.Library
{
    /// <summary>
    /// 공용 상태효과 정의 모음. 스킬, 덫, 보상 등 어디서든 같은 상태를 이 정의로 건다.
    /// 수치가 하나라도 null 이면 null 을 반환한다(그 상태를 쓰는 기능은 설정 누락으로 사용 불가).
    /// </summary>
    public static class StatusLibrary
    {
        public const string RootId = "ROOT";
        public const string BurnId = "BURN";
        public const string DistanceLimitId = "DISTANCE_LIMIT";

        // 대상 소유자의 턴 종료 단계. 걸린 그 턴은 세지 않아, 1 이면 대상의 다음 자기 턴까지 유지된다.
        private static readonly StatusTiming TargetDurationTick =
            new StatusTiming(TurnStep.EndDurationTick, TimingOwner.TargetOwner, includeApplicationTurn: false);

        private static readonly StatusTiming TargetDamageOverTime =
            new StatusTiming(TurnStep.EndDamageOverTime, TimingOwner.TargetOwner, includeApplicationTurn: false);

        /// <summary>
        /// 속박: 자발적 이동 불가. 공격/스킬은 가능하다.
        /// 다시 걸면 남은 횟수를 갱신한다.
        /// </summary>
        /// <param name="targetTurns">지속 횟수(대상 소유자의 턴 종료 횟수).</param>
        /// <param name="blocksExternalMoves">넉백/당기기/워프 같은 외부 강제 이동도 막는지.</param>
        public static StatusDefinition Root(int? targetTurns, bool? blocksExternalMoves)
        {
            if (targetTurns == null || blocksExternalMoves == null)
                return null;

            return new StatusDefinition(
                RootId,
                targetTurns.Value,
                StatusStackPolicy.Refresh,
                TargetDurationTick,
                behavior: new RootBehavior(blocksExternalMoves.Value));
        }

        /// <summary>
        /// 화상: 대상 소유자의 턴 종료마다 DoT 피해. 출처는 상태를 건 유닛.
        /// 다시 걸면 피해를 쌓지 않고 남은 횟수만 갱신한다.
        /// </summary>
        /// <param name="damagePerTrigger">1 회 발동 피해량.</param>
        /// <param name="triggers">발동 횟수.</param>
        public static StatusDefinition Burn(int? damagePerTrigger, int? triggers)
        {
            if (damagePerTrigger == null || triggers == null)
                return null;

            return new StatusDefinition(
                BurnId,
                triggers.Value,
                StatusStackPolicy.Refresh,
                TargetDurationTick,
                TargetDamageOverTime,
                new BurnBehavior(damagePerTrigger.Value));
        }

        /// <summary>
        /// 이동 거리 제한: 한 턴의 자발적 이동을 누적 maxCells 칸으로 제한. 외부 강제 이동은 막지 않는다.
        /// 다시 걸면 남은 횟수를 갱신한다.
        /// </summary>
        /// <param name="maxCells">한 턴에 자발적으로 이동할 수 있는 최대 칸 수(누적).</param>
        /// <param name="targetTurns">지속 횟수(대상 소유자의 턴 종료 횟수).</param>
        public static StatusDefinition DistanceLimit(int? maxCells, int? targetTurns)
        {
            if (maxCells == null || targetTurns == null)
                return null;

            return new StatusDefinition(
                DistanceLimitId,
                targetTurns.Value,
                StatusStackPolicy.Refresh,
                TargetDurationTick,
                behavior: new DistanceLimitBehavior(maxCells.Value));
        }
    }
}
