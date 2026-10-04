using System;
using System.Collections.Generic;

namespace MiniChess.Core.Skills
{
    /// <summary>경기에서 쓰는 스킬 정의 모음(Id → 정의).</summary>
    public class SkillCatalog
    {
        private readonly Dictionary<string, SkillDefinition> _skills = new Dictionary<string, SkillDefinition>();

        public static SkillCatalog Empty => new SkillCatalog();

        public IEnumerable<SkillDefinition> All => _skills.Values;

        public SkillCatalog Add(SkillDefinition skill)
        {
            if (skill == null) throw new ArgumentNullException(nameof(skill));
            if (_skills.ContainsKey(skill.Id)) throw new ArgumentException($"스킬 Id 중복: {skill.Id}", nameof(skill));

            _skills.Add(skill.Id, skill);
            return this;
        }

        public SkillDefinition Find(string id)
        {
            return id != null && _skills.TryGetValue(id, out SkillDefinition skill) ? skill : null;
        }
    }
}
