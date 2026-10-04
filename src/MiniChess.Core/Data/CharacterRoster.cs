using System.Collections.Generic;
using MiniChess.Core.Characters;

namespace MiniChess.Core.Data
{
    /// <summary>
    /// 캐릭터 및 스킬 구현 명세 v0.1 의 9개 캐릭터.
    /// 명세에 있는 정보(ID, 임시 이름, 역할)만 담으며 수치는 모두 TBD(null)다.
    /// 플레이 테스트용 수치는 Presets 에서 따로 채운다.
    /// </summary>
    public static class CharacterRoster
    {
        public const string Scythe = "SCYTHE";
        public const string Mortar = "MORTAR";
        public const string Gardener = "GARDENER";
        public const string Warrior = "WARRIOR";
        public const string Archer = "ARCHER";
        public const string Chemist = "CHEMIST";
        public const string Flame = "FLAME";
        public const string Seamstress = "SEAMSTRESS";
        public const string ChainGuard = "CHAIN_GUARD";

        /// <summary>
        /// 수치가 비어 있는 9개 캐릭터 정의를 새로 만든다.
        /// 스킬 슬롯은 명세의 스킬 1/스킬 2 순서이며, 아직 구현하지 않은 스킬 슬롯은 비워 둔다(null).
        /// </summary>
        public static List<CharacterDefinition> Create()
        {
            return new List<CharacterDefinition>
            {
                // 스킬 1 일자 베기, 스킬 2 그림자 분신: 미구현
                Define(Scythe, "낫 / 그림자 분신 전투원", CharacterRole.Combat, null, null),

                // 스킬 1 설치/해체, 스킬 2 지연 포격: 미구현
                Define(Mortar, "마법공학 박격포 여학생", CharacterRole.Control, null, null),

                // 스킬 2 단일 보호막: 미구현
                Define(Gardener, "원예부 선배 수호자", CharacterRole.Guardian, SkillIds.GardenerHealingMeadow, null),

                // 스킬 2 돌진: 미구현
                Define(Warrior, "대검 전사", CharacterRole.Combat, SkillIds.WarriorSmash, null),

                // 스킬 2 볼라 투척: 미구현
                Define(Archer, "궁수 / 볼라 사냥꾼", CharacterRole.Combat, SkillIds.ArcherAimedShot, null),

                Define(Chemist, "화학공학 덫 전문가", CharacterRole.Control, SkillIds.ChemistRootTrap, SkillIds.ChemistPoisonGas),

                // 스킬 1 화염지대: 미구현
                Define(Flame, "소형 중화기 화염 딜러", CharacterRole.Combat, null, SkillIds.FlameCompressedShell),

                // 스킬 1 워프/위치 교환, 스킬 2 장애물 생성: 미구현
                Define(Seamstress, "공간 재봉사", CharacterRole.Control, null, null),

                // 스킬 1 호위: 미구현
                Define(ChainGuard, "사슬 수호기사", CharacterRole.Guardian, null, SkillIds.ChainGuardChainBind),
            };
        }

        private static CharacterDefinition Define(string id, string name, CharacterRole role, string skill1, string skill2)
        {
            var definition = new CharacterDefinition(id, name, role);
            definition.SkillSlots[0] = skill1;
            definition.SkillSlots[1] = skill2;
            return definition;
        }
    }
}
