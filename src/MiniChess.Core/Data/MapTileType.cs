namespace MiniChess.Core.Data
{
    /// <summary>맵 문자열의 한 글자가 의미하는 칸 종류.</summary>
    public enum MapTileType
    {
        Empty,
        Wall,
        Capture,
        SpawnPlayer1,
        SpawnPlayer2,
    }
}
