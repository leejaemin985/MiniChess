using MiniChess.Core.State;

namespace MiniChess.Core.Combat
{
    /// <summary>
    /// HP 에 적용되기 전에 피해량을 바꾸는 규칙(호위 전가, 보호막 흡수 등).
    /// 등록된 순서(Order 오름차순)대로 앞 단계의 결과를 받아 처리한다.
    /// 계산 순서 자체가 정책이므로 Order 값으로 명시한다.
    /// </summary>
    public interface IDamageInterceptor
    {
        int Order { get; }

        /// <summary>현재까지의 피해량을 받아 HP 에 적용할 피해량을 돌려준다(0 이상).</summary>
        int Intercept(GameState state, DamageRequest request, int currentAmount);
    }
}
