namespace MiniChess.Core.Turns
{
    /// <summary>
    /// 턴 처리 단계. 선언 순서가 처리 순서다(기획 v0.7 3.3).
    ///   Turn Start: AP 회복 → 긍정 시작 효과 → 행동 제한 확인 → 시작 시점 예약 효과
    ///   Turn End:   종료형 회복 → DoT → 현재 위치의 종료형 장판 → 지속시간 감소·제거 → 점령 판정
    /// 상태효과/칸 효과는 자신이 반응할 단계와 기준 소유자를 데이터로 지정한다.
    /// </summary>
    public enum TurnStep
    {
        StartApRecovery,
        StartPositiveEffects,
        StartRestrictionCheck,
        StartScheduledEffects,

        EndHeal,
        EndDamageOverTime,
        EndAreaEffects,
        EndDurationTick,
        EndCapture,
    }
}
