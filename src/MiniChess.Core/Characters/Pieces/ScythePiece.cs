using MiniChess.Core.Data;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 낫 / 그림자 분신 전투원 (명세 4).
    /// 스킬 1 일자 베기, 스킬 2 그림자 분신: 미구현.
    /// </summary>
    public class ScythePiece : PieceModule
    {
        public const string PieceId = "SCYTHE";

        public override string Id => PieceId;
        public override string Name => "낫 / 그림자 분신 전투원";
        public override CharacterRole Role => CharacterRole.Combat;
        public override string Skill1Id => null;
        public override string Skill2Id => null;
    }
}
