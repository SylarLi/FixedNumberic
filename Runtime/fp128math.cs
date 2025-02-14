using System;

namespace Fixed.Numeric
{
    /// <summary>
    /// Mathematics functions for fp128
    /// 精度有限（与函数和参数相关，例如Pow幂底太小会导致误差放大）
    /// </summary>
    public static class fp128math
    {
        public static readonly fp128 PI = new fp128(3, 2611923443488327891);
        public static readonly fp128 E = new fp128(2, 13249961062380153451);
        public static readonly fp128 Epsilon = new fp128(0, 0x0000000100000000);
        public static readonly fp128 SqrEpsilon = fp128.Epsilon;

        internal static readonly fp128 RcpSqrt2 = new fp128(0, 13043817825332782212);

        private static readonly fp192 OnePI = new fp192(3, 2611923443488327891, 1376283091369227076);
        private static readonly fp192 Sqrt2 = new fp192(1, 7640891576956012808, 12896923290648804670);
        private static readonly fp192 HalfSqrt2 = new fp192(0, 13043817825332782212, 6448461645324402335);
        private static readonly fp192 LN2 = new fp192(0, 12786308645202655659, 14547668686819489455);
        private static readonly fp192 Log2E = new fp192(1, 8166282121979094257, 16131891943319421812);
        private static readonly fp192 Log2E10 = new fp192(3, 5938525176524057593, 2643573386881494324);
        private static readonly fp192 Log10E2 = new fp192(0, 5553023288523357132, 5171448307347507388);
        private static readonly fp192 Log2Sp1 = new fp192(0, 18428297329635842048, 0); // 0.999
        private static readonly fp192 Log2Sp2 = new fp192(1, 1844674407370956800, 0); // 1.1
        private static readonly fp192 RcpHalfPI = new fp192(0, 11743562013128004905, 18169587780923219393); // 2/PI
        private static readonly fp192 HalfPI = new fp192(1, 10529333758598939753, 9911513582539389346); // PI/2
        private static readonly fp192 Two = new fp192(2, 0, 0);
        private static readonly fp192 URM = new fp192(3, 18446744073709551615, 18446744073709551615); // 4-fp192.Epsilon
        private static readonly fp128 CFM = new fp128(0, 18446744073709551615);
        private static readonly fp192 RcpPIDiv180 = new fp192(57, 5456168980075999426, 11949421796649203136); // 180/PI
        private static readonly fp192 PIDiv180 = new fp192(0, 321956420358983237, 8103717027302354470); // PI/180

        public static fp128 Abs(fp128 x)
        {
            return x < fp128.Zero ? -x : x;
        }

        public static fp128 Floor(fp128 x)
        {
            return new fp128(x.m_hi, 0);
        }

        public static fp128 Ceiling(fp128 x)
        {
            return Floor(x + CFM);
        }

        public static fp128 Round(fp128 x)
        {
            return Floor(x + (fp128.One >> 1));
        }

        public static fp128 Sign(fp128 x)
        {
            if (x > fp128.Zero)
                return fp128.One;
            if (x < fp128.Zero)
                return -fp128.One;
            return fp128.Zero;
        }

        public static fp128 Max(fp128 x, fp128 y)
        {
            return x > y ? x : y;
        }

        public static fp128 Min(fp128 x, fp128 y)
        {
            return x < y ? x : y;
        }

        public static fp128 Clamp(fp128 x, fp128 min, fp128 max)
        {
            x = x < min ? min : x;
            x = x > max ? max : x;
            return x;
        }

        public static fp128 Clamp01(fp128 x)
        {
            x = x < fp128.Zero ? fp128.Zero : x;
            x = x > fp128.One ? fp128.One : x;
            return x;
        }

        public static fp128 Lerp(fp128 x, fp128 y, fp128 t)
        {
            return x + (y - x) * t;
        }

        public static fp128 RadToDeg(fp128 radian)
        {
            return (fp128)(radian * RcpPIDiv180);
        }

        public static fp128 DegToRad(fp128 degree)
        {
            return (fp128)(degree * PIDiv180);
        }

        public static fp128 Rcp(fp128 x)
        {
            return (fp128)Rcp((fp192)x);
        }

        public static fp128 Sqrt(fp128 x)
        {
            return (fp128)Sqrt((fp192)x);
        }

        public static fp128 RSqrt(fp128 x)
        {
            return (fp128)RSqrt((fp192)x);
        }

        public static fp128 Exp2(fp128 x)
        {
            return (fp128)Exp2((fp192)x);
        }

        public static fp128 Exp(fp128 x)
        {
            return (fp128)Exp2(Log2E * x);
        }

        public static fp128 Exp10(fp128 x)
        {
            return (fp128)Exp2(Log2E10 * x);
        }

        public static fp128 Pow(fp128 x, fp128 exponent)
        {
            return (fp128)Pow((fp192)x, exponent);
        }

        public static fp128 Log2(fp128 x)
        {
            return (fp128)Log2((fp192)x);
        }

        public static fp128 Log(fp128 x)
        {
            return (fp128)(LN2 * Log2((fp192)x));
        }

        public static fp128 Log10(fp128 x)
        {
            return (fp128)(Log10E2 * Log2((fp192)x));
        }

        public static fp128 Log(fp128 x, fp128 newBase)
        {
            return (fp128)Log((fp192)x, newBase);
        }

        public static fp128 Sin(fp128 x)
        {
            return (fp128)Sin((fp192)x);
        }

        public static fp128 Cos(fp128 x)
        {
            return (fp128)Cos((fp192)x);
        }

        public static fp128 Tan(fp128 x)
        {
            return (fp128)Tan((fp192)x);
        }

        public static fp128 Atan2(fp128 y, fp128 x)
        {
            return (fp128)Atan2(y, (fp192)x);
        }

        public static fp128 Atan(fp128 x)
        {
            return (fp128)Atan2(x, fp192.One);
        }

        public static fp128 Asin(fp128 x)
        {
            return (fp128)Asin((fp192)x);
        }

        public static fp128 Acos(fp128 x)
        {
            return (fp128)Acos((fp192)x);
        }

        private static fp192 Rcp(fp192 x)
        {
            if (x == fp192.Zero)
                throw new InvalidOperationException();
            var sign = x < fp192.Zero;
            x = sign ? -x : x;
            var offset = 63 - fputil.Clz(x);
            var n = offset >= 0 ? x >> offset : x << -offset;
            var y = fputil.RcpPoly8Lut64(n - fp192.One);
            y = sign ? -y : y;
            return offset >= 0 ? y >> offset : y << -offset;
        }

        private static fp192 Sqrt(fp192 x)
        {
            if (x < fp192.Zero)
                throw new InvalidOperationException();
            if (x == fp192.Zero)
                return fp192.Zero;
            var offset = 63 - fputil.Clz(x);
            var n = offset >= 0 ? x >> offset : x << -offset;
            var y = fputil.SqrtPoly7Lut64(n - fp192.One);
            if ((offset & 1) != 0)
                y *= Sqrt2;
            offset >>= 1;
            return offset >= 0 ? y << offset : y >> -offset;
        }

        private static fp192 RSqrt(fp192 x)
        {
            if (x <= fp192.Zero)
                throw new InvalidOperationException();
            var offset = 63 - fputil.Clz(x);
            var n = offset >= 0 ? x >> offset : x << -offset;
            var y = fputil.RSqrtPoly8Lut64(n - fp192.One);
            if ((offset & 1) != 0)
                y *= HalfSqrt2;
            offset >>= 1;
            return offset >= 0 ? y >> offset : y << -offset;
        }

        private static fp192 Exp2(fp192 x)
        {
            if (x >= 63)
                return fp192.MaxValue;
            if (x <= -63)
                return fp192.Zero;
            var xi = (int)x.m_hi;
            var ret = fputil.Exp2Poly6Lut16(new fp192(0, x.m_lo, x.m_oo));
            return xi >= 0 ? ret << xi : ret >> -xi;
        }

        private static fp192 Pow(fp192 x, fp192 exponent)
        {
            if (x <= fp192.Zero)
                throw new InvalidOperationException();
            if (x == fp192.One || exponent == fp192.Zero)
                return fp192.One;
            return Exp2(exponent * Log2(x));
        }

        private static fp192 Log2(fp192 x)
        {
            if (x <= fp192.Zero)
                throw new InvalidOperationException();
            // 在[0.999, 1.1]范围内用lut计算精度太低，改用递归逼近方式计算
            if (x > Log2Sp1 && x < Log2Sp2)
                return Log2R(x);
            var offset = 63 - fputil.Clz(x);
            var n = offset >= 0 ? x >> offset : x << -offset;
            var y = fputil.Log2Poly6Lut64(n - fp192.One);
            return offset + y;
        }

        private static fp192 Log2R(fp192 x)
        {
            var y = fp192.Zero;
            var offset = fputil.Clz(x) - 63;
            x = offset > 0 ? x << offset : x >> -offset;
            y -= offset;
            var b = fp192.One >> 1;
            for (var i = 0; i < 128; i++)
            {
                x *= x;
                if (x >= Two)
                {
                    x >>= 1;
                    y += b;
                }

                var oo = b.m_oo;
                var lo = b.m_lo;
                oo >>= 1;
                oo |= lo << 63;
                lo >>= 1;
                b = new fp192(b.m_hi, lo, oo);
            }

            return y;
        }

        private static fp192 Log(fp192 x, fp192 newBase)
        {
            if (x <= fp192.Zero || newBase <= fp192.Zero || newBase == fp192.One)
                throw new InvalidOperationException();
            return Log2(x) * Rcp(Log2(newBase));
        }

        private static fp192 Sin(fp192 x)
        {
            var sign = x < fp192.Zero;
            x = sign ? -x : x;
            x *= RcpHalfPI;
            x &= URM;
            if (x >= Two)
            {
                x -= Two;
                sign = !sign;
            }

            if (x >= fp192.One)
                x = Two - x;
            var y = fputil.SinPoly8Lut64(x);
            return sign ? -y : y;
        }

        private static fp192 Cos(fp192 x)
        {
            return Sin(x + HalfPI);
        }

        private static fp192 Tan(fp192 x)
        {
            var sinx = Sin(x);
            if (sinx == fp192.Zero)
                return fp192.Zero;
            var cosx = Cos(x);
            if (cosx == fp192.Zero)
                return sinx > 0 ? fp192.MaxValue : fp192.MinValue;
            return sinx * Rcp(cosx);
        }

        private static fp192 Atan2(fp192 y, fp192 x)
        {
            if (y == fp192.Zero)
                return fp192.Zero;
            if (x == fp192.Zero)
                return y > fp192.Zero ? HalfPI : -HalfPI;
            var sy = y < fp192.Zero;
            var sx = x < fp192.Zero;
            y = sy ? -y : y;
            x = sx ? -x : x;
            if (x >= y)
            {
                var k = y * Rcp(x);
                var z = fputil.AtanPoly8Lut64(k);
                z = sx != sy ? -z : z;
                if (!sx) return z;
                if (!sy) return z + OnePI;
                return z - OnePI;
            }
            else
            {
                var k = x * Rcp(y);
                var z = fputil.AtanPoly8Lut64(k);
                z = sx != sy ? -z : z;
                return (!sy ? HalfPI : -HalfPI) - z;
            }
        }

        private static fp192 Asin(fp192 x)
        {
            if (x < -fp192.One || x > fp192.One)
                throw new InvalidOperationException();
            return Atan2(x, Sqrt((fp192.One + x) * (fp192.One - x)));
        }

        private static fp192 Acos(fp192 x)
        {
            if (x < -fp192.One || x > fp192.One)
                throw new InvalidOperationException();
            return Atan2(Sqrt((fp192.One + x) * (fp192.One - x)), x);
        }
    }
}