using System;
using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// Quaternion with fp
    /// </summary>
    public struct fpquat : IEquatable<fpquat>
    {
        public static readonly fpquat identity = new fpquat(fp.Zero, fp.Zero, fp.Zero, fp.One);

        public fp w;

        public fp x;

        public fp y;

        public fp z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpquat(fp x, fp y, fp z, fp w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public void Set(fp x, fp y, fp z, fp w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public fpquat normalized
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
            var value = fp128math.RSqrt((fp128)x * x + (fp128)y * y + (fp128)z * z + (fp128)w * w);
            x = (fp)(x * value);
            y = (fp)(y * value);
            z = (fp)(z * value);
            w = (fp)(w * value);
        }

        public fpvec3 eulerAngles
        {
            get => ToEulerAngles(this);
            set
            {
                var q = Euler(value);
                x = q.x;
                y = q.y;
                z = q.z;
                w = q.w;
            }
        }

        public void SetLookRotation(fpvec3 forward, fpvec3 upwards)
        {
            this = LookRotation(forward, upwards);
        }

        public void SetFromToRotation(fpvec3 fromDirection, fpvec3 toDirection)
        {
            this = FromToRotation(fromDirection, toDirection);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2 ^ w.GetHashCode() >> 1;
        }

        public override bool Equals(object other)
        {
            return other is fpquat a && this == a;
        }

        public bool Equals(fpquat other)
        {
            return this == other;
        }

        public override string ToString()
        {
            return $"fpquat({x}, {y}, {z}, {w})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat operator +(fpquat lhs, fpquat rhs)
        {
            return new fpquat(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat operator -(fpquat lhs, fpquat rhs)
        {
            return new fpquat(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat operator *(fpquat lhs, fpquat rhs)
        {
            var x = (fp128)lhs.w * rhs.x + (fp128)lhs.x * rhs.w + (fp128)lhs.y * rhs.z - (fp128)lhs.z * rhs.y;
            var y = (fp128)lhs.w * rhs.y + (fp128)lhs.y * rhs.w + (fp128)lhs.z * rhs.x - (fp128)lhs.x * rhs.z;
            var z = (fp128)lhs.w * rhs.z + (fp128)lhs.z * rhs.w + (fp128)lhs.x * rhs.y - (fp128)lhs.y * rhs.x;
            var w = (fp128)lhs.w * rhs.w - (fp128)lhs.x * rhs.x - (fp128)lhs.y * rhs.y - (fp128)lhs.z * rhs.z;
            return new fpquat((fp)x, (fp)y, (fp)z, (fp)w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat operator *(fpquat q, fp scalar)
        {
            return new fpquat(q.x * scalar, q.y * scalar, q.z * scalar, q.w * scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat operator *(fp scalar, fpquat q)
        {
            return q * scalar;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 operator *(fpquat q, fpvec3 point)
        {
            var tx = (fp128)q.y * point.z - (fp128)q.z * point.y << 1;
            var ty = (fp128)q.z * point.x - (fp128)q.x * point.z << 1;
            var tz = (fp128)q.x * point.y - (fp128)q.y * point.x << 1;
            var cx = q.y * tz - q.z * ty;
            var cy = q.z * tx - q.x * tz;
            var cz = q.x * ty - q.y * tx;
            return new fpvec3((fp)(point.x + q.w * tx + cx),
                (fp)(point.y + q.w * ty + cy),
                (fp)(point.z + q.w * tz + cz));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat operator /(fpquat q, fp scalar)
        {
            return new fpquat(q.x / scalar, q.y / scalar, q.z / scalar, q.w / scalar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fpquat lhs, fpquat rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fpquat lhs, fpquat rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Angle(fpquat a, fpquat b)
        {
            // a, b should be unit quaternion.
            var dot = (fp128)a.x * b.x + (fp128)a.y * b.y + (fp128)a.z * b.z + (fp128)a.w * b.w;
            dot = fp128math.Min(fp128math.Abs(dot), fp128.One);
            var rad = fp128math.Acos(dot) << 1;
            return (fp)fp128math.RadToDeg(rad);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat AngleAxis(fp angle, fpvec3 axis)
        {
            var rad2 = fp128math.DegToRad(angle) >> 1;
            var sina = fp128math.Sin(rad2);
            var cosa = fp128math.Cos(rad2);
            var rsqrt = fp128math.RSqrt((fp128)axis.x * axis.x + (fp128)axis.y * axis.y + (fp128)axis.z * axis.z);
            return new fpquat((fp)(sina * axis.x * rsqrt),
                (fp)(sina * axis.y * rsqrt),
                (fp)(sina * axis.z * rsqrt),
                (fp)cosa);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Dot(fpquat a, fpquat b)
        {
            return (fp)((fp128)a.x * b.x + (fp128)a.y * b.y + (fp128)a.z * b.z + (fp128)a.w * b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat Euler(fpvec3 euler)
        {
            return Euler(euler.x, euler.y, euler.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat Euler(fp x, fp y, fp z)
        {
            var ex = fp128math.DegToRad(x) >> 1;
            var ey = fp128math.DegToRad(y) >> 1;
            var ez = fp128math.DegToRad(z) >> 1;
            var sinex = fp128math.Sin(ex);
            var siney = fp128math.Sin(ey);
            var sinez = fp128math.Sin(ez);
            var cosex = fp128math.Cos(ex);
            var cosey = fp128math.Cos(ey);
            var cosez = fp128math.Cos(ez);
            var xx = cosez * sinex * cosey + sinez * cosex * siney;
            var yy = cosez * cosex * siney - sinez * sinex * cosey;
            var zz = sinez * cosex * cosey - cosez * sinex * siney;
            var ww = cosez * cosex * cosey + sinez * sinex * siney;
            var mm = fp128math.RSqrt(xx * xx + yy * yy + zz * zz + ww * ww);
            return new fpquat((fp)(xx * mm), (fp)(yy * mm), (fp)(zz * mm), (fp)(ww * mm));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 ToEulerAngles(fpquat q)
        {
            ToEulerRadians(q, out var x, out var y, out var z);
            x = fp128math.RadToDeg(x);
            y = fp128math.RadToDeg(y);
            z = fp128math.RadToDeg(z);
            return new fpvec3((fp)NormalizeAngle(x), (fp)NormalizeAngle(y), (fp)NormalizeAngle(z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat FromToRotation(fpvec3 from, fpvec3 to)
        {
            var axisX = (fp128)from.y * to.z - (fp128)from.z * to.y;
            var axisY = (fp128)from.z * to.x - (fp128)from.x * to.z;
            var axisZ = (fp128)from.x * to.y - (fp128)from.y * to.x;
            var axisSqr = axisX * axisX + axisY * axisY + axisZ * axisZ;
            if (axisSqr <= fp128math.SqrEpsilon)
                return identity;
            var axisRSqrt = fp128math.RSqrt(axisSqr);
            var value = ((fp128)from.x * from.x + (fp128)from.y * from.y + (fp128)from.z * from.z) *
                        ((fp128)to.x * to.x + (fp128)to.y * to.y + (fp128)to.z * to.z);
            if (value <= fp128math.SqrEpsilon)
                return identity;
            value = fp128math.RSqrt(value);
            value *= (fp128)from.x * to.x + (fp128)from.y * to.y + (fp128)from.z * to.z;
            value = fp128math.Clamp(value, -fp128.One, fp128.One);
            value = fp128math.Acos(value);
            value = fp128math.RadToDeg(value);
            value = fp128math.DegToRad(value) >> 1;
            var sina = fp128math.Sin(value);
            var cosa = fp128math.Cos(value);
            return new fpquat((fp)(sina * axisX * axisRSqrt),
                (fp)(sina * axisY * axisRSqrt),
                (fp)(sina * axisZ * axisRSqrt),
                (fp)cosa);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat LookRotation(fpvec3 forward)
        {
            return LookRotation(forward, fpvec3.up);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat LookRotation(fpvec3 forward, fpvec3 upwards)
        {
            var axisZ = forward;
            var axisY = upwards;
            fpvec3.OrthoNormalize(ref axisZ, ref axisY);
            var axisX = fpvec3.Cross(axisY, axisZ);
            var w = fp128math.Sqrt(fp.One + axisX.x + axisY.y + axisZ.z);
            var w4r = fp128math.Rcp(w << 1);
            w >>= 1;
            var x = (axisY.z - axisZ.y) * w4r;
            var y = (axisZ.x - axisX.z) * w4r;
            var z = (axisX.y - axisY.x) * w4r;
            return new fpquat((fp)x, (fp)y, (fp)z, (fp)w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat RotateTowards(fpquat from, fpquat to, fp maxDegreesDelta)
        {
            // from, to should be unit quaternion
            var angle = Angle(from, to);
            if (angle <= fpmath.Epsilon)
                return to;
            return Slerp(from, to, maxDegreesDelta / angle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat Lerp(fpquat from, fpquat to, fp t)
        {
            t = fpmath.Clamp01(t);
            return LerpUnclamped(from, to, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat LerpUnclamped(fpquat from, fpquat to, fp t)
        {
            var ret = from + (to - from) * t;
            ret.Normalize();
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat Slerp(fpquat a, fpquat b, fp t)
        {
            // a, b should be unit quaternion
            t = fpmath.Clamp01(t);
            return SlerpUnclamped(a, b, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat SlerpUnclamped(fpquat a, fpquat b, fp t)
        {
            // a, b should be unit quaternion
            var dot = (fp128)a.x * b.x + (fp128)a.y * b.y + (fp128)a.z * b.z + (fp128)a.w * b.w;
            if (dot < fp128.Zero)
            {
                dot = -dot;
                b.x = -b.x;
                b.y = -b.y;
                b.z = -b.z;
                b.w = -b.w;
            }

            if (fp128.One - dot <= fp128math.Epsilon)
                return LerpUnclamped(a, b, t);
            var o = fp128math.Acos(dot);
            var so = fp128math.Sin(o);
            var rso = fp128math.Rcp(so);
            var to = o * t;
            var t1 = fp128math.Sin(o - to);
            var t2 = fp128math.Sin(to);
            var qx = rso * (t1 * a.x + t2 * b.x);
            var qy = rso * (t1 * a.y + t2 * b.y);
            var qz = rso * (t1 * a.z + t2 * b.z);
            var qw = rso * (t1 * a.w + t2 * b.w);
            var qq = fp128math.RSqrt(qx * qx + qy * qy + qz * qz + qw * qw);
            qx *= qq;
            qy *= qq;
            qz *= qq;
            qw *= qq;
            return new fpquat((fp)qx, (fp)qy, (fp)qz, (fp)qw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat Inverse(fpquat q)
        {
            // q should be unit quaternion.
            return new fpquat(-q.x, -q.y, -q.z, q.w); // * fp128math.Rcp(Dot(q, q))
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ToEulerRadians(fpquat q, out fp128 ex, out fp128 ey, out fp128 ez)
        {
            // Rotate sequence: Z->X->Y
            fp128 x = q.x;
            fp128 y = q.y;
            fp128 z = q.z;
            fp128 w = q.w;
            var sqw = w * w;
            var sqx = x * x;
            var sqy = y * y;
            var sqz = z * z;
            var u = sqx + sqy + sqz + sqw;
            var test = x * w - y * z;
            if (test > (u >> 1) - fp128math.SqrEpsilon)
            {
                ex = fp128math.PI >> 1;
                ey = fp128math.Atan2(y, x) << 1;
                ez = 0;
                return;
            }

            if (test < (-u >> 1) + fp128math.SqrEpsilon)
            {
                ex = -fp128math.PI >> 1;
                ey = -fp128math.Atan2(y, x) << 1;
                ez = 0;
                return;
            }

            u = fp128math.Rcp(u);
            ex = fp128math.Asin(((x * w - y * z) << 1) * u);
            ey = fp128math.Atan2((q.y * q.w + q.x * q.z) << 1, -sqx - sqy + sqz + sqw);
            ez = fp128math.Atan2((q.x * q.y + q.z * q.w) << 1, -sqx + sqy - sqz + sqw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static fp128 NormalizeAngle(fp128 angle)
        {
            if (angle < fp128.Zero)
                angle += 360;
            else if (angle >= 360)
                angle -= 360;
            return angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat Normalize(fpquat q)
        {
            return q.normalized;
        }
    }
}