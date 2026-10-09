using MiniChess.Core.Skills.Areas;

namespace MiniChess.Core.Characters.Shared
{
    /// <summary>
    /// 범위 스킬의 범위 정보. 기준점에 따라 지정 방식도 정해진다(SkillBuildUtil.AreaTargeting).
    ///   Target          → 사거리 안의 칸 지정
    ///   Target + Rotate → 사거리 안의 상하좌우 직선 칸 지정, 시전자 → 선택 칸 방향으로 회전
    ///   Caster          → 지정 없음(자기 자신)
    ///   Caster + Rotate → 방향 지정
    /// </summary>
    public class AreaTuning
    {
        /// <summary>범위 모양 그리드(AreaShape 참고). null 이면 미설정.</summary>
        public string[] Shape { get; set; }

        public AreaAnchor Anchor { get; set; } = AreaAnchor.Target;

        /// <summary>시전자 → 지정 칸 방향으로 모양을 회전할지. 모양은 위쪽을 바라보는 것으로 작성한다.</summary>
        public bool Rotate { get; set; }
    }
}
