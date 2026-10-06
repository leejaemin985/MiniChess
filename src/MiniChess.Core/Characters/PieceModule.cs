using System;
using System.Collections.Generic;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;

namespace MiniChess.Core.Characters
{
    /// <summary>
    /// 캐릭터(체스말) 하나의 구조 정의. 캐릭터마다 파일 하나(Pieces/)에 Id, 스킬 칸, 스킬 정의, 수치 항목을 모은다.
    /// 수치(밸런스 값)는 여기에 두지 않고 SkillTuning 으로 받는다. 실제 값은 Presets 에서 채운다.
    /// </summary>
    public abstract class PieceModule
    {
        public abstract string Id { get; }

        /// <summary>임시 기능명(고유 이름 아님).</summary>
        public abstract string Name { get; }

        public abstract CharacterRole Role { get; }

        /// <summary>스킬 1 칸의 스킬 Id. 아직 구현하지 않았으면 null.</summary>
        public abstract string Skill1Id { get; }

        /// <summary>스킬 2 칸의 스킬 Id. 아직 구현하지 않았으면 null.</summary>
        public abstract string Skill2Id { get; }

        /// <summary>수치가 비어 있는 캐릭터 정의를 새로 만든다.</summary>
        public CharacterDefinition CreateDefinition()
        {
            var definition = new CharacterDefinition(Id, Name, Role);
            definition.SkillSlots[0] = Skill1Id;
            definition.SkillSlots[1] = Skill2Id;
            return definition;
        }

        /// <summary>이 캐릭터가 가진 구현된 스킬 정의. 수치가 비어 있으면 설정 누락으로 사용할 수 없는 정의가 된다.</summary>
        public virtual IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            return Array.Empty<SkillDefinition>();
        }
    }
}
