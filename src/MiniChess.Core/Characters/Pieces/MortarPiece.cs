using MiniChess.Core.Data;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 마법공학 박격포 여학생 (명세 5).
    /// 스킬 1 설치/해체, 스킬 2 지연 포격: 미구현.
    /// </summary>
    public class MortarPiece : PieceModule
    {
        public const string PieceId = "MORTAR";

        public override string Id => PieceId;
        public override string Name => "마법공학 박격포 여학생";
        public override CharacterRole Role => CharacterRole.Control;
        public override string Skill1Id => null;
        public override string Skill2Id => null;
    }
}
