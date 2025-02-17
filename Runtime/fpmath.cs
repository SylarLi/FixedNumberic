using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// Mathematics functions for fp
    /// 精度有限（与函数和参数相关，例如Pow幂底太小会导致误差放大）
    /// </summary>
    public static class fpmath
    {
        public static readonly fp Epsilon = new fp(0x00010000);
        public static readonly fp SqrEpsilon = fp.Epsilon;

        private const int TotalBits = 64;
        private const long FracMask = 0xFFFFFFFF;
        private const long IntMask = ~FracMask;
        private const long IntOne = 0x100000000;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Abs(fp x)
        {
            var t = x.m_value >> TotalBits - 1;
            return new fp((x.m_value ^ t) - t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Floor(fp x)
        {
            return new fp(x.m_value & IntMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Ceiling(fp x)
        {
            return new fp((x.m_value + FracMask) & IntMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Round(fp x)
        {
            return new fp((x.m_value + (IntOne >> 1)) & IntMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Sign(fp x)
        {
            if (x > fp.Zero)
                return fp.One;
            if (x < fp.Zero)
                return -fp.One;
            return fp.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Max(fp x, fp y)
        {
            return x > y ? x : y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Min(fp x, fp y)
        {
            return x < y ? x : y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Clamp(fp x, fp min, fp max)
        {
            if (x < min)
            {
                x = min;
            }
            else if (x > max)
            {
                x = max;
            }

            return x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Clamp01(fp x)
        {
            return Clamp(x, fp.Zero, fp.One);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Lerp(fp x, fp y, fp t)
        {
            return x + (y - x) * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp RadToDeg(fp radian)
        {
            return (fp)fp128math.RadToDeg(radian);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp DegToRad(fp degree)
        {
            return (fp)fp128math.DegToRad(degree);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Rcp(fp x)
        {
            return (fp)fp128math.Rcp(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Sqrt(fp x)
        {
            return (fp)fp128math.Sqrt(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp RSqrt(fp x)
        {
            return (fp)fp128math.RSqrt(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Exp2(fp x)
        {
            return (fp)fp128math.Exp2(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Exp(fp x)
        {
            return (fp)fp128math.Exp(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Exp10(fp x)
        {
            return (fp)fp128math.Exp10(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Pow(fp x, fp exponent)
        {
            return (fp)fp128math.Pow(x, exponent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Log2(fp x)
        {
            return (fp)fp128math.Log2(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Log(fp x)
        {
            return (fp)fp128math.Log(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Log10(fp x)
        {
            return (fp)fp128math.Log10(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Log(fp x, fp newBase)
        {
            return (fp)fp128math.Log(x, newBase);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Sin(fp x)
        {
            return (fp)fp128math.Sin(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Cos(fp x)
        {
            return (fp)fp128math.Cos(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Tan(fp x)
        {
            return (fp)fp128math.Tan(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Atan2(fp y, fp x)
        {
            return (fp)fp128math.Atan2(y, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Atan(fp x)
        {
            return (fp)fp128math.Atan(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Asin(fp x)
        {
            return (fp)fp128math.Asin(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Acos(fp x)
        {
            return (fp)fp128math.Acos(x);
        }
    }
}