using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Community.Helpers
{

    public class RandomColorCreator : RandomCreator<Color>
    {
        private readonly Random _random;

        public RandomColorCreator()
        {
            var tick = DateTime.Now.Ticks;
            _random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        }

        public override Color Next =>
            Color.FromRgb((byte)_random.Next(255), (byte)_random.Next(255), (byte)_random.Next(255));
    }

    public class RandomHSBColorCreator : RandomCreator<Color>
    {
        private readonly Random _random;

        private readonly int _mins = 0;
        private readonly int _maxs = 100;
        private readonly int _minb = 0;
        private readonly int _maxb = 100;

        public RandomHSBColorCreator(int MinS, int MaxS, int MinB, int MaxB)
        {
            var tick = DateTime.Now.Ticks;
            _random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            _mins = MinS < 0 ? 0 : MinS > 100 ? 100 : MinS > MaxS ? MaxS : MinS;
            _maxs = MaxS < 0 ? 0 : MaxS > 100 ? 100 : MaxS < MinS ? MinS : MaxS;
            _minb = MinB < 0 ? 0 : MinB > 100 ? 100 : MinB > MaxB ? MaxB : MinB;
            _maxb = MaxB < 0 ? 0 : MaxB > 100 ? 100 : MaxB < MinB ? MinB : MaxB;
        }

        public RandomHSBColorCreator()
        {
            var tick = DateTime.Now.Ticks;
            _random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        }

        public override Color Next =>
            ColorHelpers.HSBtoRGB((float)(_random.NextDouble() + _random.Next(0, 360)), _random.Next(_mins, _maxs) / 100f, _random.Next(_minb, _maxb) / 100f);
    }

    public class RandomDoubleCreator : RandomCreator<double>
    {
        private readonly Random _random = new Random((int)(DateTime.Now.Ticks & 0xffffffffL) | (int)(DateTime.Now.Ticks >> 32));

        public override double Next => _random.NextDouble() * _random.Next(Convert.ToInt32(Math.Round(Max)));
    }
}
