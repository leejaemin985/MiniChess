using MiniChess.Core.Data;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 공간 재봉사 (명세 11).
    /// 스킬 1 워프/위치 교환, 스킬 2 장애물 생성: 미구현.
    /// </summary>
    public class SeamstressPiece : PieceModule
    {
        public const string PieceId = "SEAMSTRESS";

        public override string Id => PieceId;
        public override string Name => "공간 재봉사";
        public override CharacterRole Role => CharacterRole.Control;
        public override string Skill1Id => null;
        public override string Skill2Id => null;
    }
}
