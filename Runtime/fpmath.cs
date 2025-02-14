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

        public static fp Abs(fp x)
        {
            var t = x.m_value >> TotalBits - 1;
            return new fp((x.m_value ^ t) - t);
        }

        public static fp Floor(fp x)
        {
            return new fp(x.m_value & IntMask);
        }

        public static fp Ceiling(fp x)
        {
            return new fp((x.m_value + FracMask) & IntMask);
        }

        public static fp Round(fp x)
        {
            return new fp((x.m_value + (IntOne >> 1)) & IntMask);
        }

        public static fp Sign(fp x)
        {
            if (x > fp.Zero)
                return fp.One;
            if (x < fp.Zero)
                return -fp.One;
            return fp.Zero;
        }

        public static fp Max(fp x, fp y)
        {
            return x > y ? x : y;
        }

        public static fp Min(fp x, fp y)
        {
            return x < y ? x : y;
        }

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

        public static fp Clamp01(fp x)
        {
            return Clamp(x, fp.Zero, fp.One);
        }

        public static fp Lerp(fp x, fp y, fp t)
        {
            return x + (y - x) * t;
        }

        public static fp RadToDeg(fp radian)
        {
            return (fp)fp128math.RadToDeg(radian);
        }

        public static fp DegToRad(fp degree)
        {
            return (fp)fp128math.DegToRad(degree);
        }

        public static fp Rcp(fp x)
        {
            return (fp)fp128math.Rcp(x);
        }

        public static fp Sqrt(fp x)
        {
            return (fp)fp128math.Sqrt(x);
        }

        public static fp RSqrt(fp x)
        {
            return (fp)fp128math.RSqrt(x);
        }

        public static fp Exp2(fp x)
        {
            return (fp)fp128math.Exp2(x);
        }

        public static fp Exp(fp x)
        {
            return (fp)fp128math.Exp(x);
        }

        public static fp Exp10(fp x)
        {
            return (fp)fp128math.Exp10(x);
        }

        public static fp Pow(fp x, fp exponent)
        {
            return (fp)fp128math.Pow(x, exponent);
        }

        public static fp Log2(fp x)
        {
            return (fp)fp128math.Log2(x);
        }

        public static fp Log(fp x)
        {
            return (fp)fp128math.Log(x);
        }

        public static fp Log10(fp x)
        {
            return (fp)fp128math.Log10(x);
        }

        public static fp Log(fp x, fp newBase)
        {
            return (fp)fp128math.Log(x, newBase);
        }

        public static fp Sin(fp x)
        {
            return (fp)fp128math.Sin(x);
        }

        public static fp Cos(fp x)
        {
            return (fp)fp128math.Cos(x);
        }

        public static fp Tan(fp x)
        {
            return (fp)fp128math.Tan(x);
        }

        public static fp Atan2(fp y, fp x)
        {
            return (fp)fp128math.Atan2(y, x);
        }

        public static fp Atan(fp x)
        {
            return (fp)fp128math.Atan(x);
        }

        public static fp Asin(fp x)
        {
            return (fp)fp128math.Asin(x);
        }

        public static fp Acos(fp x)
        {
            return (fp)fp128math.Acos(x);
        }
    }
}