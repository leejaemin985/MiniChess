using System.Collections.Generic;

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

        /// <summary>수치가 비어 있는 9개 캐릭터 정의를 새로 만든다.</summary>
        public static List<CharacterDefinition> Create()
        {
            return new List<CharacterDefinition>
            {
                new CharacterDefinition(Scythe, "낫 / 그림자 분신 전투원", CharacterRole.Combat),
                new CharacterDefinition(Mortar, "마법공학 박격포 여학생", CharacterRole.Control),
                new CharacterDefinition(Gardener, "원예부 선배 수호자", CharacterRole.Guardian),
                new CharacterDefinition(Warrior, "대검 전사", CharacterRole.Combat),
                new CharacterDefinition(Archer, "궁수 / 볼라 사냥꾼", CharacterRole.Combat),
                new CharacterDefinition(Chemist, "화학공학 덫 전문가", CharacterRole.Control),
                new CharacterDefinition(Flame, "소형 중화기 화염 딜러", CharacterRole.Combat),
                new CharacterDefinition(Seamstress, "공간 재봉사", CharacterRole.Control),
                new CharacterDefinition(ChainGuard, "사슬 수호기사", CharacterRole.Guardian),
            };
        }
    }
}
