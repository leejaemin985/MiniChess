namespace MiniChess.Core.Characters
{
    /// <summary>
    /// 캐릭터 스킬의 수치 묶음. 명세상 모두 TBD 이며 기본값은 전부 null(미설정)이다.
    /// 값은 프리셋 등 외부 데이터에서 채운다. 0 은 "없음"을 뜻한다(예: 즉시 피해 0 = 즉시 피해 없음).
    /// </summary>
    public class SkillTuning
    {
        public RootPolicyTuning Root { get; } = new RootPolicyTuning();
        public SmashTuning WarriorSmash { get; } = new SmashTuning();
        public AimedShotTuning ArcherAimedShot { get; } = new AimedShotTuning();
        public CompressedShellTuning FlameCompressedShell { get; } = new CompressedShellTuning();
        public RootTrapTuning ChemistRootTrap { get; } = new RootTrapTuning();
        public PoisonGasTuning ChemistPoisonGas { get; } = new PoisonGasTuning();
        public ChainBindTuning ChainGuardChainBind { get; } = new ChainBindTuning();
        public HealingMeadowTuning GardenerHealingMeadow { get; } = new HealingMeadowTuning();
    }

    /// <summary>속박 공통 정책(사슬 속박과 속박 덫이 공유).</summary>
    public class RootPolicyTuning
    {
        /// <summary>속박 중 외부 강제 이동(넉백/워프 등)도 막는지(명세 12.3).</summary>
        public bool? BlocksExternalMoves { get; set; }
    }

    /// <summary>대검 전사 — 강타(명세 7.2).</summary>
    public class SmashTuning
    {
        public int? ApCost { get; set; }
        public int? Damage { get; set; }
    }

    /// <summary>궁수 — 장거리 조준(명세 8.2).</summary>
    public class AimedShotTuning
    {
        public int? ApCost { get; set; }
        public int? Range { get; set; }
        public int? Damage { get; set; }
    }

    /// <summary>화염 딜러 — 압축열탄(명세 10.3).</summary>
    public class CompressedShellTuning
    {
        public int? ApCost { get; set; }
        public int? Range { get; set; }

        /// <summary>명중 즉시 피해. 0 이면 없음.</summary>
        public int? ImpactDamage { get; set; }

        /// <summary>화상 1회 피해.</summary>
        public int? BurnDamage { get; set; }

        /// <summary>화상 발동 횟수(대상 소유자 턴 종료 횟수).</summary>
        public int? BurnTriggers { get; set; }
    }

    /// <summary>화학 덫 전문가 — 속박 덫(명세 9.2).</summary>
    public class RootTrapTuning
    {
        public int? ApCost { get; set; }
        public int? Range { get; set; }

        /// <summary>덫 피해. 0 이면 없음.</summary>
        public int? Damage { get; set; }

        /// <summary>속박 기간(대상 소유자 턴 종료 횟수).</summary>
        public int? RootTurns { get; set; }

        /// <summary>시전자당 동시에 유지할 수 있는 덫 수.</summary>
        public int? MaxActive { get; set; }

        /// <summary>덫 수명(설치한 쪽 소유자 턴 종료 횟수).</summary>
        public int? Lifetime { get; set; }

        /// <summary>발동 후 사라지는지.</summary>
        public bool? SingleUse { get; set; }
    }

    /// <summary>화학 덫 전문가 — 독가스 설치(명세 9.3).</summary>
    public class PoisonGasTuning
    {
        public int? ApCost { get; set; }
        public int? Range { get; set; }

        /// <summary>구역 모양(가로×세로). [설계안] 2×2.</summary>
        public int? Width { get; set; }
        public int? Height { get; set; }

        /// <summary>적 소유자 턴 종료마다 주는 장판 피해.</summary>
        public int? Damage { get; set; }

        /// <summary>구역 수명(설치한 쪽 소유자 턴 종료 횟수).</summary>
        public int? Lifetime { get; set; }

        /// <summary>시전자당 동시 구역 수. [설계안] 1.</summary>
        public int? MaxActive { get; set; }
    }

    /// <summary>사슬 수호기사 — 사슬 속박(명세 12.3). 지속은 "한 턴"으로 확정.</summary>
    public class ChainBindTuning
    {
        public int? ApCost { get; set; }
        public int? Range { get; set; }

        /// <summary>피해. 0 이면 없음.</summary>
        public int? Damage { get; set; }
    }

    /// <summary>원예부 선배 — 회복 초원(명세 6.2).</summary>
    public class HealingMeadowTuning
    {
        public int? ApCost { get; set; }

        /// <summary>장판 중심을 지정할 수 있는 거리.</summary>
        public int? Range { get; set; }

        /// <summary>중심 기준 반경. [설계안] 3×3 = 1.</summary>
        public int? Radius { get; set; }

        /// <summary>턴 시작마다 회복량.</summary>
        public int? Heal { get; set; }

        /// <summary>장판 수명(설치한 쪽 소유자 턴 종료 횟수).</summary>
        public int? Lifetime { get; set; }
    }
}
