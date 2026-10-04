using System;
using System.Collections.Generic;

namespace MiniChess.Core.Data
{
    /// <summary>
    /// 필수 설정 값이 없거나 잘못되어 기능을 실행할 수 없음.
    /// TBD 값을 임의로 채우지 않고 실행을 보류하기 위해 사용한다.
    /// </summary>
    public class GameConfigException : Exception
    {
        public IReadOnlyList<string> Issues { get; }

        public GameConfigException(IReadOnlyList<string> issues)
            : base("설정 오류:\n  - " + string.Join("\n  - ", issues))
        {
            Issues = issues;
        }
    }
}
