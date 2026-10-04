using System;

namespace MiniChess.Core.Common
{
    /// <summary>게임 규칙이 쓰는 무작위 값의 출처. 테스트에서 결과를 고정할 수 있도록 분리한다.</summary>
    public interface IRandomSource
    {
        /// <summary>0 이상 maxExclusive 미만의 정수.</summary>
        int Next(int maxExclusive);
    }

    /// <summary>System.Random 기반 기본 구현.</summary>
    public class SystemRandomSource : IRandomSource
    {
        private readonly Random _random;

        public SystemRandomSource()
        {
            _random = new Random();
        }

        public SystemRandomSource(int seed)
        {
            _random = new Random(seed);
        }

        public int Next(int maxExclusive)
        {
            return _random.Next(maxExclusive);
        }
    }
}
