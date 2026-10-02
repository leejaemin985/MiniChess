using System;
using MiniChess.Core.Common;

namespace MiniChess.Core.Data
{
    /// <summary>
    /// 문자열로 작성하는 맵 구조. Rows 는 화면에 보이는 모양 그대로 위쪽 줄부터 적는다.
    /// 첫 줄이 보드의 가장 위(y = Height - 1), 마지막 줄이 가장 아래(y = 0)이다.
    ///
    /// 기호
    ///   .  빈 칸
    ///   #  벽
    ///   C  점령 칸
    ///   1  Player1 시작 위치
    ///   2  Player2 시작 위치
    /// </summary>
    public class MapData
    {
        public const char EmptySymbol = '.';
        public const char WallSymbol = '#';
        public const char CaptureSymbol = 'C';
        public const char SpawnPlayer1Symbol = '1';
        public const char SpawnPlayer2Symbol = '2';

        public string Name { get; set; }
        public string[] Rows { get; set; }

        public int Width => Rows[0].Length;
        public int Height => Rows.Length;

        public MapTileType GetTile(Position position)
        {
            int rowIndex = Height - 1 - position.Y;
            char symbol = Rows[rowIndex][position.X];
            return ToTileType(symbol);
        }

        /// <summary>모든 줄의 길이가 같고, 알 수 없는 기호가 없는지 확인한다.</summary>
        public void Validate()
        {
            if (Rows == null || Rows.Length == 0)
                throw new InvalidOperationException($"맵 '{Name}': Rows 가 비어 있음");

            for (int i = 0; i < Rows.Length; i++)
            {
                string row = Rows[i];

                if (row.Length != Width)
                    throw new InvalidOperationException($"맵 '{Name}': {i}번째 줄 길이 {row.Length}, 기대값 {Width}");

                foreach (char symbol in row)
                    ToTileType(symbol);
            }
        }

        private MapTileType ToTileType(char symbol)
        {
            switch (symbol)
            {
                case EmptySymbol: return MapTileType.Empty;
                case WallSymbol: return MapTileType.Wall;
                case CaptureSymbol: return MapTileType.Capture;
                case SpawnPlayer1Symbol: return MapTileType.SpawnPlayer1;
                case SpawnPlayer2Symbol: return MapTileType.SpawnPlayer2;
                default: throw new InvalidOperationException($"맵 '{Name}': 알 수 없는 기호 '{symbol}'");
            }
        }
    }
}
