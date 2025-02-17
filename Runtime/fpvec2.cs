using System;
using System.Runtime.CompilerServices;
using Unity.Burst;

namespace Fixed.Numeric
{
    /// <summary>
    /// Vector2 with fp
    /// </summary>
    public struct fpvec2 : IEquatable<fpvec2>
    {
        public static readonly fpvec2 zero = new fpvec2();
        public static readonly fpvec2 one = new fpvec2(fp.One, fp.One);
        public static readonly fpvec2 left = new fpvec2(-fp.One, fp.Zero);
        public static readonly fpvec2 right = new fpvec2(fp.One, fp.Zero);
        public static readonly fpvec2 up = new fpvec2(fp.Zero, fp.One);
        public static readonly fpvec2 down = new fpvec2(fp.Zero, -fp.One);

        public fp x;

        public fp y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec2(fp x, fp y)
        {
            this.x = x;
            this.y = y;
        }

        public fp sqrMagnitude => (fp)((fp128)x * x + (fp128)y * y);

        public fp magnitude => (fp)fp128math.Sqrt((fp128)x * x + (fp128)y * y);

        public fpvec2 normalized
        {
            get
            {
                var v = this;
                v.Normalize();
                return v;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            var sqr = (fp128)x * x + (fp128)y * y;
            if (sqr > fp128math.SqrEpsilon)
            {
                var value = fp128math.RSqrt(sqr);
                x = (fp)(x * value);
                y = (fp)(y * value);
            }
            else
            {
                x = fp.Zero;
                y = fp.Zero;
            }
        }

        public fp this[int index]
        {
            get
            {
                if (index == 0)
                    return x;
                if (index == 1)
                    return y;
                throw new IndexOutOfRangeException("Invalid fpvec2 index!");
            }
            set
            {
                if (index == 0)
                    x = value;
                if (index == 1)
                    y = value;
                throw new IndexOutOfRangeException("Invalid fpvec2 index!");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(fp newX, fp newY)
        {
            x = newX;
            y = newY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(fpvec2 scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2;
        }

        public override bool Equals(object obj)
        {
            return obj is fpvec2 a && this == a;
        }

        public bool Equals(fpvec2 other)
        {
            return this == other;
        }

        public override string ToString()
        {
            return $"fpvec2({x}, {y})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fpvec2 lhs, fpvec2 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fpvec2 lhs, fpvec2 rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 operator +(fpvec2 a, fpvec2 b)
        {
            return new fpvec2(a.x + b.x, a.y + b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 operator -(fpvec2 a, fpvec2 b)
        {
            return new fpvec2(a.x - b.x, a.y - b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 operator -(fpvec2 a)
        {
            return new fpvec2(-a.x, -a.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 operator *(fp d, fpvec2 a)
        {
            return a * d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 operator *(fpvec2 a, fp d)
        {
            return new fpvec2(a.x * d, a.y * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 operator /(fpvec2 a, fp d)
        {
            return new fpvec2(a.x / d, a.y / d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 operator >>(fpvec2 a, int shift)
        {
            a.x >>= shift;
            a.y >>= shift;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 operator <<(fpvec2 a, int shift)
        {
            a.x <<= shift;
            a.y <<= shift;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Angle(fpvec2 from, fpvec2 to)
        {
            var value = ((fp128)from.x * from.x + (fp128)from.y * from.y) *
                        ((fp128)to.x * to.x + (fp128)to.y * to.y);
            if (value <= fp128math.SqrEpsilon)
                return fp.Zero;
            value = fp128math.RSqrt(value);
            value *= (fp128)from.x * to.x + (fp128)from.y * to.y;
            value = fp128math.Clamp(value, -fp128.One, fp128.One);
            value = fp128math.Acos(value);
            return (fp)fp128math.RadToDeg(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp SignedAngle(fpvec2 from, fpvec2 to)
        {
            return Angle(from, to) * fpmath.Sign(Cross(from, to));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 ClampMagnitude(fpvec2 vector, fp maxLength)
        {
            var sqrMaxLength = (fp128)maxLength * maxLength;
            var sqrMagnitude = (fp128)vector.x * vector.x + (fp128)vector.y * vector.y;
            if (sqrMagnitude <= sqrMaxLength)
                return vector;
            if (sqrMagnitude <= fp128math.SqrEpsilon)
                return zero;
            var value = fp128math.RSqrt(sqrMagnitude) * maxLength;
            return new fpvec2((fp)(vector.x * value), (fp)(vector.y * value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Distance(fpvec2 a, fpvec2 b)
        {
            return (a - b).magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Dot(fpvec2 lhs, fpvec2 rhs)
        {
            return (fp)((fp128)lhs.x * rhs.x + (fp128)lhs.y * rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Cross(fpvec2 lhs, fpvec2 rhs)
        {
            return (fp)((fp128)lhs.x * rhs.y - (fp128)lhs.y * rhs.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 Lerp(fpvec2 from, fpvec2 to, fp t)
        {
            t = fpmath.Clamp01(t);
            return LerpUnclamped(from, to, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 LerpUnclamped(fpvec2 from, fpvec2 to, fp t)
        {
            return from + (to - from) * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 Min(fpvec2 lhs, fpvec2 rhs)
        {
            return new fpvec2(fpmath.Min(lhs.x, rhs.x), fpmath.Min(lhs.y, rhs.y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 Max(fpvec2 lhs, fpvec2 rhs)
        {
            return new fpvec2(fpmath.Max(lhs.x, rhs.x), fpmath.Max(lhs.y, rhs.y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 MoveTowards(fpvec2 current, fpvec2 target, fp maxDistanceDelta)
        {
            var diff = target - current;
            var magnitude = fp128math.Sqrt((fp128)diff.x * diff.x + (fp128)diff.y * diff.y);
            if (magnitude <= maxDistanceDelta || magnitude <= fp128math.Epsilon)
                return target;
            var value = fp128math.Rcp(magnitude) * maxDistanceDelta;
            return new fpvec2((fp)(current.x + diff.x * value), (fp)(current.y + diff.y * value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 Perpendicular(fpvec2 inDirection)
        {
            return new fpvec2(-inDirection.y, inDirection.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 Reflect(fpvec2 inDirection, fpvec2 inNormal)
        {
            var dot = (fp128)inNormal.x * inDirection.x + (fp128)inNormal.y * inDirection.y;
            dot = -(dot << 1);
            return new fpvec2((fp)(dot * inNormal.x + inDirection.x), (fp)(dot * inNormal.y + inDirection.y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 Scale(fpvec2 a, fpvec2 b)
        {
            return new fpvec2(a.x * b.x, a.y * b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 Project(fpvec2 vector, fpvec2 on)
        {
            var value = (fp128)on.x * on.x + (fp128)on.y * on.y;
            if (value <= fp128math.SqrEpsilon)
                return zero;
            value = fp128math.Rcp(value);
            value *= (fp128)vector.x * on.x + (fp128)vector.y * on.y;
            return new fpvec2((fp)(value * on.x), (fp)(value * on.y));
        }
    }
}