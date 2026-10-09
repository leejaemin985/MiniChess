using MiniChess.Core.Skills.Areas;

namespace MiniChess.Core.Characters.Shared
{
    /// <summary>
    /// 범위 스킬의 범위 정보. 기준점에 따라 지정 방식도 정해진다(SkillBuildUtil.AreaTargeting).
    ///   Target          → 사거리 안의 칸 지정
    ///   Caster          → 지정 없음(자기 자신)
    ///   Caster + Rotate → 방향 지정
    /// </summary>
    public class AreaTuning
    {
        /// <summary>범위 모양 그리드(AreaShape 참고). null 이면 미설정.</summary>
        public string[] Shape { get; set; }

        public AreaAnchor Anchor { get; set; } = AreaAnchor.Target;

        /// <summary>시전자 기준일 때 지정한 방향으로 모양을 회전할지.</summary>
        public bool Rotate { get; set; }
    }
}
