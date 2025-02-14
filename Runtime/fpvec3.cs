using System;
using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// Vector3 with fp
    /// </summary>
    public struct fpvec3 : IEquatable<fpvec3>
    {
        public static readonly fpvec3 zero = new fpvec3(fp.Zero, fp.Zero, fp.Zero);
        public static readonly fpvec3 one = new fpvec3(fp.One, fp.One, fp.One);
        public static readonly fpvec3 up = new fpvec3(fp.Zero, fp.One, fp.Zero);
        public static readonly fpvec3 down = new fpvec3(fp.Zero, -fp.One, fp.Zero);
        public static readonly fpvec3 left = new fpvec3(-fp.One, fp.Zero, fp.Zero);
        public static readonly fpvec3 right = new fpvec3(fp.One, fp.Zero, fp.Zero);
        public static readonly fpvec3 forward = new fpvec3(fp.Zero, fp.Zero, fp.One);
        public static readonly fpvec3 back = new fpvec3(fp.Zero, fp.Zero, -fp.One);

        public fp x;

        public fp y;

        public fp z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec3(fp x, fp y, fp z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec3(fp x, fp y)
        {
            this.x = x;
            this.y = y;
            z = fp.Zero;
        }

        public fp sqrMagnitude => (fp) ((fp128) x * x + (fp128) y * y + (fp128) z * z);

        public fp magnitude => (fp) fp128math.Sqrt((fp128) x * x + (fp128) y * y + (fp128) z * z);

        public fpvec3 normalized
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
            var sqr = (fp128) x * x + (fp128) y * y + (fp128) z * z;
            if (sqr > fp128math.SqrEpsilon)
            {
                var value = fp128math.RSqrt(sqr);
                x = (fp) (x * value);
                y = (fp) (y * value);
                z = (fp) (z * value);
            }
            else
            {
                x = fp.Zero;
                y = fp.Zero;
                z = fp.Zero;
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
                    default:
                        throw new IndexOutOfRangeException("Invalid fpvec3 index!");
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
                    default:
                        throw new IndexOutOfRangeException("Invalid fpvec3 index!");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(fp newX, fp newY, fp newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(fpvec3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;
        }

        public override bool Equals(object other)
        {
            return other is fpvec3 a && this == a;
        }

        public bool Equals(fpvec3 other)
        {
            return this == other;
        }

        public override string ToString()
        {
            return $"fpvec3({x}, {y}, {z})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fpvec3 lhs, fpvec3 rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fpvec3 lhs, fpvec3 rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator +(fpvec3 a, fpvec3 b)
        {
            return new fpvec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator -(fpvec3 a, fpvec3 b)
        {
            return new fpvec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator -(fpvec3 a)
        {
            return new fpvec3(-a.x, -a.y, -a.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator *(fpvec3 a, fp d)
        {
            return new fpvec3(a.x * d, a.y * d, a.z * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator *(fp d, fpvec3 a)
        {
            return new fpvec3(a.x * d, a.y * d, a.z * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator /(fpvec3 a, fp d)
        {
            return new fpvec3(a.x / d, a.y / d, a.z / d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator >>(fpvec3 a, int shift)
        {
            a.x >>= shift;
            a.y >>= shift;
            a.z >>= shift;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator <<(fpvec3 a, int shift)
        {
            a.x <<= shift;
            a.y <<= shift;
            a.z <<= shift;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fpvec3(fpvec2 v)
        {
            return new fpvec3(v.x, v.y, fp.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fpvec2(fpvec3 v)
        {
            return new fpvec2(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 Slerp(fpvec3 a, fpvec3 b, fp t)
        {
            t = fpmath.Clamp01(t);
            return SlerpUnclamped(a, b, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 SlerpUnclamped(fpvec3 a, fpvec3 b, fp t)
        {
            var sqrA = (fp128) a.x * a.x + (fp128) a.y * a.y + (fp128) a.z * a.z;
            var sqrB = (fp128) b.x * b.x + (fp128) b.y * b.y + (fp128) b.z * b.z;
            var value = sqrA * sqrB;
            if (value <= fp128math.SqrEpsilon)
                return Lerp(a, b, t);
            value = fp128math.RSqrt(value);
            value *= (fp128) a.x * b.x + (fp128) a.y * b.y + (fp128) a.z * b.z;
            value = fp128math.Clamp(value, -fp128.One, fp128.One);
            var o = fp128math.Acos(value);
            var so = fp128math.Sin(o);
            if (so <= fp128math.Epsilon)
                return Lerp(a, b, t);
            var rso = fp128math.Rcp(so);
            var to = o * t;
            var t1 = fp128math.Sin(o - to);
            var t2 = fp128math.Sin(to);
            return new fpvec3((fp) (rso * (t1 * a.x + t2 * b.x)),
                (fp) (rso * (t1 * a.y + t2 * b.y)),
                (fp) (rso * (t1 * a.z + t2 * b.z)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 Lerp(fpvec3 a, fpvec3 b, fp t)
        {
            t = fpmath.Clamp01(t);
            return LerpUnclamped(a, b, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 LerpUnclamped(fpvec3 a, fpvec3 b, fp t)
        {
            return a + (b - a) * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 MoveTowards(fpvec3 current, fpvec3 target, fp maxDistanceDelta)
        {
            var diff = target - current;
            var magnitude = fp128math.Sqrt((fp128) diff.x * diff.x + (fp128) diff.y * diff.y + (fp128) diff.z * diff.z);
            if (magnitude <= maxDistanceDelta || magnitude <= fp128math.Epsilon)
                return target;
            var value = fp128math.Rcp(magnitude) * maxDistanceDelta;
            return new fpvec3((fp) (current.x + diff.x * value),
                (fp) (current.y + diff.y * value),
                (fp) (current.z + diff.z * value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 Scale(fpvec3 a, fpvec3 b)
        {
            return new fpvec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 Cross(fpvec3 lhs, fpvec3 rhs)
        {
            return new fpvec3((fp) ((fp128) lhs.y * rhs.z - (fp128) lhs.z * rhs.y),
                (fp) ((fp128) lhs.z * rhs.x - (fp128) lhs.x * rhs.z),
                (fp) ((fp128) lhs.x * rhs.y - (fp128) lhs.y * rhs.x));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 Reflect(fpvec3 inDirection, fpvec3 inNormal)
        {
            var dot = (fp128) inNormal.x * inDirection.x + (fp128) inNormal.y * inDirection.y +
                      (fp128) inNormal.z * inDirection.z;
            dot = -(dot << 1);
            return new fpvec3((fp) (dot * inNormal.x + inDirection.x),
                (fp) (dot * inNormal.y + inDirection.y),
                (fp) (dot * inNormal.z + inDirection.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Dot(fpvec3 lhs, fpvec3 rhs)
        {
            return (fp) ((fp128) lhs.x * rhs.x + (fp128) lhs.y * rhs.y + (fp128) lhs.z * rhs.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 Project(fpvec3 vector, fpvec3 on)
        {
            var value = (fp128) on.x * on.x + (fp128) on.y * on.y + (fp128) on.z * on.z;
            if (value <= fp128math.SqrEpsilon)
                return zero;
            value = fp128math.Rcp(value);
            value *= (fp128) vector.x * on.x + (fp128) vector.y * on.y + (fp128) vector.z * on.z;
            return new fpvec3((fp) (value * on.x), (fp) (value * on.y), (fp) (value * on.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 ProjectOnPlane(fpvec3 vector, fpvec3 planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Angle(fpvec3 from, fpvec3 to)
        {
            var value = ((fp128) from.x * from.x + (fp128) from.y * from.y + (fp128) from.z * from.z) *
                        ((fp128) to.x * to.x + (fp128) to.y * to.y + (fp128) to.z * to.z);
            if (value <= fp128math.SqrEpsilon)
                return fp.Zero;
            value = fp128math.RSqrt(value);
            value *= (fp128) from.x * to.x + (fp128) from.y * to.y + (fp128) from.z * to.z;
            value = fp128math.Clamp(value, -fp128.One, fp128.One);
            value = fp128math.Acos(value);
            return (fp) fp128math.RadToDeg(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp SignedAngle(fpvec3 from, fpvec3 to, fpvec3 axis)
        {
            return Angle(from, to) * fpmath.Sign(Dot(axis, Cross(from, to)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Distance(fpvec3 a, fpvec3 b)
        {
            return (a - b).magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 ClampMagnitude(fpvec3 vector, fp maxLength)
        {
            var sqrMaxLength = (fp128) maxLength * maxLength;
            var sqrMagnitude = (fp128) vector.x * vector.x + (fp128) vector.y * vector.y + (fp128) vector.z * vector.z;
            if (sqrMagnitude <= sqrMaxLength)
                return vector;
            if (sqrMagnitude <= fp128math.SqrEpsilon)
                return zero;
            var value = fp128math.RSqrt(sqrMagnitude) * maxLength;
            return new fpvec3((fp) (vector.x * value), (fp) (vector.y * value), (fp) (vector.z * value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 Min(fpvec3 lhs, fpvec3 rhs)
        {
            return new fpvec3(fpmath.Min(lhs.x, rhs.x), fpmath.Min(lhs.y, rhs.y), fpmath.Min(lhs.z, rhs.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 Max(fpvec3 lhs, fpvec3 rhs)
        {
            return new fpvec3(fpmath.Max(lhs.x, rhs.x), fpmath.Max(lhs.y, rhs.y), fpmath.Max(lhs.z, rhs.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OrthoNormalize(ref fpvec3 normal, ref fpvec3 tangent)
        {
            normal.Normalize();
            if (normal == zero)
                normal = right;
            var proj = Dot(tangent, normal);
            tangent -= proj * normal;
            tangent.Normalize();
            if (tangent == zero)
                tangent = OrthoNormalizeCollinear(normal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OrthoNormalize(ref fpvec3 normal, ref fpvec3 tangent, ref fpvec3 binormal)
        {
            normal.Normalize();
            if (normal == zero)
                normal = right;
            var proj = Dot(tangent, normal);
            tangent -= proj * normal;
            tangent.Normalize();
            if (tangent == zero)
                tangent = OrthoNormalizeCollinear(normal);
            var proj1 = Dot(tangent, binormal);
            var proj2 = Dot(normal, binormal);
            binormal -= proj1 * tangent + proj2 * normal;
            binormal.Normalize();
            if (binormal == zero)
                binormal = Cross(normal, tangent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static fpvec3 OrthoNormalizeCollinear(fpvec3 normal)
        {
            var x = normal.x;
            var y = normal.y;
            var z = normal.z;
            if (fp128math.Abs(z) > fp128math.RcpSqrt2)
            {
                var a = (fp128) y * y + (fp128) z * z;
                var k = fp128math.RSqrt(a);
                normal.x = fp.Zero;
                normal.y = (fp) (-z * k);
                normal.z = (fp) (y * k);
            }
            else
            {
                var a = (fp128) x * x + (fp128) y * y;
                var k = fp128math.RSqrt(a);
                normal.x = (fp) (-y * k);
                normal.y = (fp) (x * k);
                normal.z = fp.Zero;
            }

            return normal;
        }
    }
}