using System;
using System.Collections.Generic;

namespace MiniChess.Core.Skills.Areas
{
    /// <summary>
    /// 범위 모양. 문자열 그리드로 정의하며, 첫 줄이 위쪽(+Y)이다.
    /// <code>
    ///   @ = 기준점(범위 포함)   o = 기준점(범위 미포함)   # = 범위   . = 빈칸
    /// </code>
    /// 회전하는 범위는 위쪽을 바라보는 모양으로 작성한다.
    /// </summary>
    public class AreaShape
    {
        public const char AnchorIncluded = '@';
        public const char AnchorExcluded = 'o';
        public const char Cell = '#';
        public const char Empty = '.';

        public static readonly AreaShape SingleCell = Parse(new[] { "@" });

        /// <summary>기준점에서의 상대 좌표(위쪽을 바라볼 때).</summary>
        public IReadOnlyList<(int Dx, int Dy)> Offsets { get; }

        private AreaShape(IReadOnlyList<(int Dx, int Dy)> offsets)
        {
            Offsets = offsets;
        }

        /// <summary>그리드를 해석한다. rows 가 null 이면 null(미설정). 형식이 잘못되면 예외.</summary>
        public static AreaShape Parse(IReadOnlyList<string> rows)
        {
            if (rows == null)
                return null;
            if (rows.Count == 0)
                throw new ArgumentException("범위 모양이 비어 있음", nameof(rows));

            (int Row, int Col)? anchor = null;
            var cells = new List<(int Row, int Col)>();

            for (int row = 0; row < rows.Count; row++)
            {
                if (rows[row].Length != rows[0].Length)
                    throw new ArgumentException($"범위 모양: {row}번째 줄 길이 {rows[row].Length}, 기대값 {rows[0].Length}", nameof(rows));

                for (int col = 0; col < rows[row].Length; col++)
                {
                    char c = rows[row][col];
                    switch (c)
                    {
                        case AnchorIncluded:
                        case AnchorExcluded:
                            if (anchor != null)
                                throw new ArgumentException("범위 모양: 기준점이 둘 이상", nameof(rows));
                            anchor = (row, col);
                            if (c == AnchorIncluded)
                                cells.Add((row, col));
                            break;

                        case Cell:
                            cells.Add((row, col));
                            break;

                        case Empty:
                            break;

                        default:
                            throw new ArgumentException($"범위 모양: 알 수 없는 문자 '{c}'", nameof(rows));
                    }
                }
            }

            if (anchor == null)
                throw new ArgumentException("범위 모양: 기준점(@ 또는 o)이 없음", nameof(rows));

            var offsets = new List<(int Dx, int Dy)>();
            foreach ((int row, int col) in cells)
                offsets.Add((col - anchor.Value.Col, anchor.Value.Row - row));

            return new AreaShape(offsets);
        }

        /// <summary>facing 방향(상하좌우 단위 벡터)을 바라보도록 회전한 상대 좌표.</summary>
        public IEnumerable<(int Dx, int Dy)> GetOffsets(int facingX, int facingY)
        {
            // 위(0,1) 기준 모양을 (fx,fy) 방향으로 회전: 오른쪽이면 (dx,dy) → (dy,-dx)
            foreach ((int dx, int dy) in Offsets)
                yield return (dx * facingY + dy * facingX, dy * facingY - dx * facingX);
        }
    }
}
