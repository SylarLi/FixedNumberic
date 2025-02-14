using System;
using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// 32/64bit random number generator
    /// Implemented with Xorshift RNGs by George Marsaglia
    /// </summary>
    public class fprandom
    {
        private uint state1;

        private ulong state2;
        
        public fprandom(uint seed1, ulong seed2)
        {
            SetSeed(seed1, seed2);
        }

        public void SetSeed(uint seed1, ulong seed2)
        {
            if (seed1 == 0 || seed2 == 0)
                throw new InvalidOperationException("seed can not set to zero.");
            state1 = seed1;
            state2 = seed2;
            NextState1();
            NextState2();
        }

        /// <summary>
        /// Generate random uint
        /// </summary>
        /// <returns>Ranges: [0, 2^32 - 2]</returns>
        public uint NextUInt()
        {
            return NextState1() - 1;
        }

        /// <summary>
        /// Generate random uint
        /// </summary>
        /// <param name="min">min number</param>
        /// <param name="max">max number</param>
        /// <returns></returns>
        public uint NextUInt(uint min, uint max)
        {
            var minimum = min < max ? min : max;
            var maximum = min > max ? min : max;
            var range = maximum - minimum;
            return (uint) (NextState1() * (ulong) range >> 32) + minimum;
        }

        /// <summary>
        /// Generate random int
        /// </summary>
        /// <returns>Ranges: [-2^31 + 1, 2^31 - 1]</returns>
        public int NextInt()
        {
            return (int) NextState1() ^ -2147483648;
        }

        /// <summary>
        /// Generate random int
        /// </summary>
        /// <param name="min">min number</param>
        /// <param name="max">max number</param>
        /// <returns>Ranges: [min, max)</returns>
        public int NextInt(int min, int max)
        {
            var minimum = min < max ? min : max;
            var maximum = min > max ? min : max;
            var range = maximum - minimum;
            return (int) (NextState1() * (ulong) range >> 32) + minimum;
        }

        /// <summary>
        /// Generate random ulong
        /// </summary>
        /// <returns>Ranges: [0, 2^64 - 2]</returns>
        public ulong NextULong()
        {
            return NextState2() - 1;
        }

        /// <summary>
        /// Generate random ulong
        /// </summary>
        /// <param name="min">min number</param>
        /// <param name="max">max number</param>
        /// <returns>Ranges: [min, max)</returns>
        public ulong NextUlong(ulong min, ulong max)
        {
            var minimum = min < max ? min : max;
            var maximum = min > max ? min : max;
            var range = maximum - minimum;
            var dist = NextULong();
            var li = range >> 32;
            var lf = range & 0xFFFFFFFF;
            var ri = dist >> 32;
            var rf = dist & 0xFFFFFFFF;
            var pd = (li * ri << 32) +
                     (li * rf + ri * lf) +
                     (lf * rf >> 32);
            return pd + minimum;
        }

        /// <summary>
        /// Generate random long
        /// </summary>
        /// <returns>Ranges: [-2^63 + 1, 2^63 - 1]</returns>
        public long NextLong()
        {
            return (long) NextState2() ^ -9223372036854775808;
        }

        /// <summary>
        /// Generate random long
        /// </summary>
        /// <param name="min">min number</param>
        /// <param name="max">max number</param>
        /// <returns>Ranges: [min, max)</returns>
        public long NextLong(long min, long max)
        {
            var minimum = min < max ? min : max;
            var maximum = min > max ? min : max;
            var range = maximum - minimum;
            var dist = NextLong();
            var li = range >> 32;
            var lf = (ulong) (range & 0xFFFFFFFF);
            var ri = dist >> 32;
            var rf = (ulong) (dist & 0xFFFFFFFF);
            var pd = (li * ri << 32) +
                     (li * (long) rf + ri * (long) lf) +
                     (long) (lf * rf >> 32);
            return pd + minimum;
        }

        /// <summary>
        /// Generate random fixed point
        /// </summary>
        /// <returns>Ranges: [0, 1)</returns>
        public fp Next64()
        {
            return new fp(NextUInt());
        }

        /// <summary>
        /// Generate random fixed point
        /// </summary>
        /// <param name="min">min number</param>
        /// <param name="max">max number</param>
        /// <returns>Ranges: [min, max)</returns>
        public fp Next64(fp min, fp max)
        {
            var minimum = fpmath.Min(min, max);
            var maximum = fpmath.Max(min, max);
            return fpmath.Lerp(minimum, maximum, Next64());
        }

        /// <summary>
        /// Generate random fixed point
        /// </summary>
        /// <returns>Ranges: [0, 1)</returns>
        public fp128 Next128()
        {
            return new fp128(0, NextULong());
        }

        /// <summary>
        /// Generate random fixed point
        /// </summary>
        /// <param name="min">min number</param>
        /// <param name="max">max number</param>
        /// <returns>Ranges: [min, max)</returns>
        public fp128 Next128(fp128 min, fp128 max)
        {
            var minimum = fp128math.Min(min, max);
            var maximum = fp128math.Max(min, max);
            return fp128math.Lerp(minimum, maximum, Next128());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint NextState1()
        {
            // [1, 2^32 - 1]
            var t = state1;
            state1 ^= state1 << 11;
            state1 ^= state1 >> 7;
            state1 ^= state1 << 16;
            return t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong NextState2()
        {
            // [1, 2^64 - 1]
            var t = state2;
            state2 ^= state2 << 11;
            state2 ^= state2 >> 29;
            state2 ^= state2 << 14;
            return t;
        }
    }
}