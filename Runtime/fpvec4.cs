using System;
using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// Vector4 with fp
    /// </summary>
    public struct fpvec4 : IEquatable<fpvec4>
    {
        public static readonly fpvec4 zero = new fpvec4(fp.Zero, fp.Zero, fp.Zero, fp.Zero);
        public static readonly fpvec4 one = new fpvec4(fp.One, fp.One, fp.One, fp.One);

        public fp x;

        public fp y;

        public fp z;

        public fp w;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec4(fp x, fp y, fp z, fp w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec4(fp x, fp y, fp z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            w = fp.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec4(fp x, fp y)
        {
            this.x = x;
            this.y = y;
            z = fp.Zero;
            w = fp.Zero;
        }

        public fp sqrMagnitude => (fp) ((fp128) x * x + (fp128) y * y + (fp128) z * z + (fp128) w * w);

        public fp magnitude => (fp) fp128math.Sqrt((fp128) x * x + (fp128) y * y + (fp128) z * z + (fp128) w * w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            var sqr = (fp128) x * x + (fp128) y * y + (fp128) z * z + (fp128) w * w;
            if (sqr > fp128math.SqrEpsilon)
            {
                var value = fp128math.RSqrt(sqr);
                x = (fp) (x * value);
                y = (fp) (y * value);
                z = (fp) (z * value);
                w = (fp) (w * value);
            }
            else
            {
                x = fp.Zero;
                y = fp.Zero;
                z = fp.Zero;
                w = fp.Zero;
            }
        }

        public fpvec4 normalized
        {
            get
            {
                var v = this;
                v.Normalize();
                return v;
            }
        }

        public fp this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid fpvec4 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid fpvec4 index!");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(fp newX, fp newY, fp newZ, fp newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(fpvec4 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
            w *= scale.w;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2 ^ w.GetHashCode() >> 1;
        }

        public override bool Equals(object other)
        {
            return other is fpvec4 a && this == a;
        }

        public bool Equals(fpvec4 other)
        {
            return this == other;
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator +(fpvec4 a, fpvec4 b)
        {
            return new fpvec4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator -(fpvec4 a, fpvec4 b)
        {
            return new fpvec4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator -(fpvec4 a)
        {
            return new fpvec4(-a.x, -a.y, -a.z, -a.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator *(fpvec4 a, fp d)
        {
            return new fpvec4(a.x * d, a.y * d, a.z * d, a.w * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator *(fp d, fpvec4 a)
        {
            return new fpvec4(a.x * d, a.y * d, a.z * d, a.w * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator /(fpvec4 a, fp d)
        {
            return new fpvec4(a.x / d, a.y / d, a.z / d, a.w / d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator >>(fpvec4 a, int shift)
        {
            a.x >>= shift;
            a.y >>= shift;
            a.z >>= shift;
            a.w >>= shift;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator <<(fpvec4 a, int shift)
        {
            a.x <<= shift;
            a.y <<= shift;
            a.z <<= shift;
            a.w <<= shift;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fpvec4 lhs, fpvec4 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fpvec4 lhs, fpvec4 rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fpvec4(fpvec3 v)
        {
            return new fpvec4(v.x, v.y, v.z, fp.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fpvec3(fpvec4 v)
        {
            return new fpvec3(v.x, v.y, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fpvec4(fpvec2 v)
        {
            return new fpvec4(v.x, v.y, fp.Zero, fp.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fpvec2(fpvec4 v)
        {
            return new fpvec2(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 Lerp(fpvec4 a, fpvec4 b, fp t)
        {
            t = fpmath.Clamp01(t);
            return LerpUnclamped(a, b, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 LerpUnclamped(fpvec4 a, fpvec4 b, fp t)
        {
            return a + (b - a) * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 MoveTowards(fpvec4 current, fpvec4 target, fp maxDistanceDelta)
        {
            var diff = target - current;
            var magnitude = fp128math.Sqrt((fp128) diff.x * diff.x + (fp128) diff.y * diff.y + (fp128) diff.z * diff.z +
                                           (fp128) diff.w * diff.w);
            if (magnitude <= maxDistanceDelta || magnitude <= fp128math.Epsilon)
                return target;
            var value = fp128math.Rcp(magnitude) * maxDistanceDelta;
            return new fpvec4((fp) (current.x + diff.x * value),
                (fp) (current.y + diff.y * value),
                (fp) (current.z + diff.z * value),
                (fp) (current.w + diff.w * value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 Scale(fpvec4 a, fpvec4 b)
        {
            return new fpvec4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Dot(fpvec4 a, fpvec4 b)
        {
            return (fp) ((fp128) a.x * b.x + (fp128) a.y * b.y + (fp128) a.z * b.z + (fp128) a.w * b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 Project(fpvec4 vector, fpvec4 on)
        {
            var value = (fp128) on.x * on.x + (fp128) on.y * on.y + (fp128) on.z * on.z + (fp128) on.w * on.w;
            if (value <= fp128math.SqrEpsilon)
                return zero;
            value = fp128math.Rcp(value);
            value *= (fp128) vector.x * on.x + (fp128) vector.y * on.y + (fp128) vector.z * on.z +
                     (fp128) vector.w * on.w;
            return new fpvec4((fp) (value * on.x), (fp) (value * on.y), (fp) (value * on.z), (fp) (value * on.w));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Distance(fpvec4 a, fpvec4 b)
        {
            return (a - b).magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 Min(fpvec4 lhs, fpvec4 rhs)
        {
            return new fpvec4(fpmath.Min(lhs.x, rhs.x), fpmath.Min(lhs.y, rhs.y), fpmath.Min(lhs.z, rhs.z),
                fpmath.Min(lhs.w, rhs.w));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 Max(fpvec4 lhs, fpvec4 rhs)
        {
            return new fpvec4(fpmath.Max(lhs.x, rhs.x), fpmath.Max(lhs.y, rhs.y), fpmath.Max(lhs.z, rhs.z),
                fpmath.Max(lhs.w, rhs.w));
        }
    }
}