using System;
using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// 4x4 matrix with fp
    /// </summary>
    public struct fpmatrix4x4 : IEquatable<fpmatrix4x4>
    {
        public static readonly fpmatrix4x4 zero = new fpmatrix4x4(fpvec4.zero, fpvec4.zero, fpvec4.zero, fpvec4.zero);

        public static readonly fpmatrix4x4 identity = new fpmatrix4x4(
            new fpvec4(fp.One, fp.Zero, fp.Zero, fp.Zero),
            new fpvec4(fp.Zero, fp.One, fp.Zero, fp.Zero),
            new fpvec4(fp.Zero, fp.Zero, fp.One, fp.Zero),
            new fpvec4(fp.Zero, fp.Zero, fp.Zero, fp.One));

        public fp m00;
        public fp m10;
        public fp m20;
        public fp m30;
        public fp m01;
        public fp m11;
        public fp m21;
        public fp m31;
        public fp m02;
        public fp m12;
        public fp m22;
        public fp m32;
        public fp m03;
        public fp m13;
        public fp m23;
        public fp m33;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpmatrix4x4(fp m00, fp m01, fp m02, fp m03, fp m10, fp m11, fp m12, fp m13,
            fp m20, fp m21, fp m22, fp m23, fp m30, fp m31, fp m32, fp m33)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpmatrix4x4(fpvec4 column0, fpvec4 column1, fpvec4 column2, fpvec4 column3)
        {
            m00 = column0.x;
            m01 = column1.x;
            m02 = column2.x;
            m03 = column3.x;
            m10 = column0.y;
            m11 = column1.y;
            m12 = column2.y;
            m13 = column3.y;
            m20 = column0.z;
            m21 = column1.z;
            m22 = column2.z;
            m23 = column3.z;
            m30 = column0.w;
            m31 = column1.w;
            m32 = column2.w;
            m33 = column3.w;
        }

        public override int GetHashCode()
        {
            return GetColumn(0).GetHashCode() ^ GetColumn(1).GetHashCode() << 2 ^
                   GetColumn(2).GetHashCode() >> 2 ^ GetColumn(3).GetHashCode() >> 1;
        }

        public override bool Equals(object other)
        {
            if (!(other is fpmatrix4x4))
                return false;
            return Equals((fpmatrix4x4)other);
        }

        public bool Equals(fpmatrix4x4 other)
        {
            return GetColumn(0).Equals(other.GetColumn(0)) && GetColumn(1).Equals(other.GetColumn(1)) &&
                   GetColumn(2).Equals(other.GetColumn(2)) && GetColumn(3).Equals(other.GetColumn(3));
        }

        public override string ToString()
        {
            return
                $"{m00}\t{m01}\t{m02}\t{m03}\n" +
                $"{m10}\t{m11}\t{m12}\t{m13}\n" +
                $"{m20}\t{m21}\t{m22}\t{m23}\n" +
                $"{m30}\t{m31}\t{m32}\t{m33}\n";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ValidTRS()
        {
            if (m30 != fp.Zero ||
                m31 != fp.Zero ||
                m32 != fp.Zero ||
                m33 != fp.One)
                return false;
            if (fpmath.Abs(determinant) <= fpmath.Epsilon)
                return false;
            return true;
        }

        public fpvec3 translate => new fpvec3(m03, m13, m23);

        public fpquat rotation
        {
            get
            {
                // This may not be consistent with unity's implementation.(don't know why -_-#)
                fpquat q;
                var sx = (fp128)m00 * m00 + (fp128)m10 * m10 + (fp128)m20 * m20;
                var sy = (fp128)m01 * m01 + (fp128)m11 * m11 + (fp128)m21 * m21;
                var sz = (fp128)m02 * m02 + (fp128)m12 * m12 + (fp128)m22 * m22;
                var inv_sx = fp128math.RSqrt(sx);
                inv_sx = determinant < fp128.Zero ? -inv_sx : inv_sx;
                var inv_sy = fp128math.RSqrt(sy);
                var inv_sz = fp128math.RSqrt(sz);
                var mm00 = m00 * inv_sx;
                var mm10 = m10 * inv_sx;
                var mm20 = m20 * inv_sx;
                var mm01 = m01 * inv_sy;
                var mm11 = m11 * inv_sy;
                var mm21 = m21 * inv_sy;
                var mm02 = m02 * inv_sz;
                var mm12 = m12 * inv_sz;
                var mm22 = m22 * inv_sz;
                var tr = mm00 + mm11 + mm22;
                if (tr > fp128.Zero)
                {
                    var s = fp128math.Sqrt(fp128.One + tr);
                    var rs = fp128math.Rcp(s << 1);
                    q.w = (fp)(s >> 1);
                    q.x = (fp)((mm21 - mm12) * rs);
                    q.y = (fp)((mm02 - mm20) * rs);
                    q.z = (fp)((mm10 - mm01) * rs);
                }
                else if (mm00 > mm11 && mm00 > mm22)
                {
                    var s = fp128math.Sqrt(fp128.One + mm00 - mm11 - mm22);
                    var rs = fp128math.Rcp(s << 1);
                    q.w = (fp)((mm21 - mm12) * rs);
                    q.x = (fp)(s >> 1);
                    q.y = (fp)((mm01 + mm10) * rs);
                    q.z = (fp)((mm02 + mm20) * rs);
                }
                else if (mm11 > mm22)
                {
                    var s = fp128math.Sqrt(fp128.One + mm11 - mm00 - mm22);
                    var rs = fp128math.Rcp(s << 1);
                    q.w = (fp)((mm02 - mm20) * rs);
                    q.x = (fp)((mm01 + mm10) * rs);
                    q.y = (fp)(s >> 1);
                    q.z = (fp)((mm12 + mm21) * rs);
                }
                else
                {
                    var s = fp128math.Sqrt(fp128.One + mm22 - mm00 - mm11);
                    var rs = fp128math.Rcp(s << 1);
                    q.w = (fp)((mm10 - mm01) * rs);
                    q.x = (fp)((mm02 + mm20) * rs);
                    q.y = (fp)((mm12 + mm21) * rs);
                    q.z = (fp)(s >> 1);
                }

                return q;
            }
        }

        public fpvec3 lossyScale
        {
            get
            {
                var scale = fpvec3.zero;
                scale.x = new fpvec3(m00, m10, m20).magnitude;
                scale.y = new fpvec3(m01, m11, m21).magnitude;
                scale.z = new fpvec3(m02, m12, m22).magnitude;
                if (determinant < fp.Zero)
                    scale.x = -scale.x;
                return scale;
            }
        }

        public bool isIdentity => this == identity;

        public fp determinant =>
                (fp)(m00 * ((fp128)m11 * (m22 * m33 - m23 * m32) - (fp128)m12 * (m21 * m33 - m23 * m31) + (fp128)m13 * (m21 * m32 - m22 * m31)) -
                m01 * ((fp128)m10 * (m22 * m33 - m23 * m32) - (fp128)m12 * (m20 * m33 - m23 * m30) + (fp128)m13 * (m20 * m32 - m22 * m30)) +
                m02 * ((fp128)m10 * (m21 * m33 - m23 * m31) - (fp128)m11 * (m20 * m33 - m23 * m30) + (fp128)m13 * (m20 * m31 - m21 * m30)) -
                m03 * ((fp128)m10 * (m21 * m32 - m22 * m31) - (fp128)m11 * (m20 * m32 - m22 * m30) + (fp128)m12 * (m20 * m31 - m21 * m30)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Determinant(fpmatrix4x4 m)
        {
            return m.determinant;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 TRS(fpvec3 t, fpquat q, fpvec3 s)
        {
            var ret = Rotate(q);
            ret.m00 *= s.x;
            ret.m10 *= s.x;
            ret.m20 *= s.x;
            ret.m01 *= s.y;
            ret.m11 *= s.y;
            ret.m21 *= s.y;
            ret.m02 *= s.z;
            ret.m12 *= s.z;
            ret.m22 *= s.z;
            ret.m03 = t.x;
            ret.m13 = t.y;
            ret.m23 = t.z;
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 TRS(fpvec2 t, fp q, fpvec2 s)
        {
            return TRS(t, fpquat.AngleAxis(q, fpvec3.forward), new fpvec3(s.x, s.y, fp.One));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTRS(fpvec3 pos, fpquat q, fpvec3 s)
        {
            this = TRS(pos, q, s);
        }

        public fpmatrix4x4 inverse
        {
            get
            {
                var det2x2_20_31 = (fp128)m20 * m31 - (fp128)m21 * m30;
                var det2x2_20_32 = (fp128)m20 * m32 - (fp128)m22 * m30;
                var det2x2_20_33 = (fp128)m20 * m33 - (fp128)m23 * m30;
                var det2x2_21_32 = (fp128)m21 * m32 - (fp128)m22 * m31;
                var det2x2_21_33 = (fp128)m21 * m33 - (fp128)m23 * m31;
                var det2x2_22_33 = (fp128)m22 * m33 - (fp128)m23 * m32;
                var det2x2_10_21 = (fp128)m10 * m21 - (fp128)m11 * m20;
                var det2x2_10_22 = (fp128)m10 * m22 - (fp128)m12 * m20;
                var det2x2_10_23 = (fp128)m10 * m23 - (fp128)m13 * m20;
                var det2x2_11_22 = (fp128)m11 * m22 - (fp128)m12 * m21;
                var det2x2_11_23 = (fp128)m11 * m23 - (fp128)m13 * m21;
                var det2x2_12_23 = (fp128)m12 * m23 - (fp128)m13 * m22;
                var det2x2_10_31 = (fp128)m10 * m31 - (fp128)m11 * m30;
                var det2x2_10_32 = (fp128)m10 * m32 - (fp128)m12 * m30;
                var det2x2_10_33 = (fp128)m10 * m33 - (fp128)m13 * m30;
                var det2x2_11_32 = (fp128)m11 * m32 - (fp128)m12 * m31;
                var det2x2_11_33 = (fp128)m11 * m33 - (fp128)m13 * m31;
                var det2x2_12_33 = (fp128)m12 * m33 - (fp128)m13 * m32;

                var det3x3_00 = m11 * det2x2_22_33 - m12 * det2x2_21_33 + m13 * det2x2_21_32;
                var det3x3_01 = m10 * det2x2_22_33 - m12 * det2x2_20_33 + m13 * det2x2_20_32;
                var det3x3_02 = m10 * det2x2_21_33 - m11 * det2x2_20_33 + m13 * det2x2_20_31;
                var det3x3_03 = m10 * det2x2_21_32 - m11 * det2x2_20_32 + m12 * det2x2_20_31;
                var det3x3_10 = m01 * det2x2_22_33 - m02 * det2x2_21_33 + m03 * det2x2_21_32;
                var det3x3_11 = m00 * det2x2_22_33 - m02 * det2x2_20_33 + m03 * det2x2_20_32;
                var det3x3_12 = m00 * det2x2_21_33 - m01 * det2x2_20_33 + m03 * det2x2_20_31;
                var det3x3_13 = m00 * det2x2_21_32 - m01 * det2x2_20_32 + m02 * det2x2_20_31;
                var det3x3_20 = m01 * det2x2_12_33 - m02 * det2x2_11_33 + m03 * det2x2_11_32;
                var det3x3_21 = m00 * det2x2_12_33 - m02 * det2x2_10_33 + m03 * det2x2_10_32;
                var det3x3_22 = m00 * det2x2_11_33 - m01 * det2x2_10_33 + m03 * det2x2_10_31;
                var det3x3_23 = m00 * det2x2_11_32 - m01 * det2x2_10_32 + m02 * det2x2_10_31;
                var det3x3_30 = m01 * det2x2_12_23 - m02 * det2x2_11_23 + m03 * det2x2_11_22;
                var det3x3_31 = m00 * det2x2_12_23 - m02 * det2x2_10_23 + m03 * det2x2_10_22;
                var det3x3_32 = m00 * det2x2_11_23 - m01 * det2x2_10_23 + m03 * det2x2_10_21;
                var det3x3_33 = m00 * det2x2_11_22 - m01 * det2x2_10_22 + m02 * det2x2_10_21;

                var det = m00 * det3x3_00 - m01 * det3x3_01 + m02 * det3x3_02 - m03 * det3x3_03;
                if (fp128math.Abs(det) < fpmath.Epsilon) return zero;
                var invDet = fp128math.Rcp(det);
                var inv = new fpmatrix4x4();
                inv.m00 = (fp)(det3x3_00 * invDet);
                inv.m01 = (fp)(-det3x3_10 * invDet);
                inv.m02 = (fp)(det3x3_20 * invDet);
                inv.m03 = (fp)(-det3x3_30 * invDet);
                inv.m10 = (fp)(-det3x3_01 * invDet);
                inv.m11 = (fp)(det3x3_11 * invDet);
                inv.m12 = (fp)(-det3x3_21 * invDet);
                inv.m13 = (fp)(det3x3_31 * invDet);
                inv.m20 = (fp)(det3x3_02 * invDet);
                inv.m21 = (fp)(-det3x3_12 * invDet);
                inv.m22 = (fp)(det3x3_22 * invDet);
                inv.m23 = (fp)(-det3x3_32 * invDet);
                inv.m30 = (fp)(-det3x3_03 * invDet);
                inv.m31 = (fp)(det3x3_13 * invDet);
                inv.m32 = (fp)(-det3x3_23 * invDet);
                inv.m33 = (fp)(det3x3_33 * invDet);
                return inv;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 Transpose(fpmatrix4x4 m)
        {
            fpmatrix4x4 ret;
            ret.m00 = m.m00;
            ret.m01 = m.m10;
            ret.m02 = m.m20;
            ret.m03 = m.m30;
            ret.m10 = m.m01;
            ret.m11 = m.m11;
            ret.m12 = m.m21;
            ret.m13 = m.m31;
            ret.m20 = m.m02;
            ret.m21 = m.m12;
            ret.m22 = m.m22;
            ret.m23 = m.m32;
            ret.m30 = m.m03;
            ret.m31 = m.m13;
            ret.m32 = m.m23;
            ret.m33 = m.m33;
            return ret;
        }

        public fpmatrix4x4 transpose => Transpose(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 Ortho(fp left, fp right, fp bottom, fp top, fp zNear, fp zFar)
        {
            var rcpdx = fp128math.Rcp(right - left);
            var rcpdy = fp128math.Rcp(top - bottom);
            var rcpdz = fp128math.Rcp(zFar - zNear);
            return new fpmatrix4x4(
                (fp)(rcpdx << 1), fp.Zero, fp.Zero, (fp)(-(right + left) * rcpdx),
                fp.Zero, (fp)(rcpdy << 1), fp.Zero, (fp)(-(top + bottom) * rcpdy),
                fp.Zero, fp.Zero, (fp)(-rcpdz << 1), (fp)(-(zFar + zNear) * rcpdz),
                fp.Zero, fp.Zero, fp.Zero, fp.One
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 Perspective(fp fov, fp aspect, fp zNear, fp zFar)
        {
            var cotangent = fp128math.Rcp(fp128math.Tan(fp128math.DegToRad(fov) >> 1));
            var rcpdz = fp128math.Rcp(zNear - zFar);
            return new fpmatrix4x4(
                (fp)(cotangent * fp128math.Rcp(aspect)), fp.Zero, fp.Zero, fp.Zero,
                fp.Zero, (fp)cotangent, fp.Zero, fp.Zero,
                fp.Zero, fp.Zero, (fp)((zFar + zNear) * rcpdz), (fp)(zNear * zFar * rcpdz << 1),
                fp.Zero, fp.Zero, -fp.One, fp.Zero
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 LookAt(fpvec3 from, fpvec3 to, fpvec3 up)
        {
            var forward = (to - from).normalized;
            var side = fpvec3.Cross(up, forward).normalized;
            var upward = fpvec3.Cross(forward, side);
            return new fpmatrix4x4(side, upward, forward, new fpvec4(from.x, from.y, from.z, fp.One));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 Frustum(fp left, fp right, fp bottom, fp top, fp zNear, fp zFar)
        {
            var rcpdz = fp128math.Rcp(zNear - zFar);
            var rcpWidth = fp128math.Rcp(right - left);
            var rcpHeight = fp128math.Rcp(top - bottom);
            return new fpmatrix4x4(
                (fp)(zNear * rcpWidth << 1), fp.Zero, (fp)((left + right) * rcpWidth), fp.Zero,
                fp.Zero, (fp)(zNear * rcpHeight << 1), (fp)((bottom + top) * rcpHeight), fp.Zero,
                fp.Zero, fp.Zero, (fp)((zFar + zNear) * rcpdz), (fp)(zNear * zFar * rcpdz << 1),
                fp.Zero, fp.Zero, -fp.One, fp.Zero
            );
        }

        public fp this[int row, int column]
        {
            get => this[row + column * 4];
            set => this[row + column * 4] = value;
        }

        public fp this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return m00;
                    case 1:
                        return m10;
                    case 2:
                        return m20;
                    case 3:
                        return m30;
                    case 4:
                        return m01;
                    case 5:
                        return m11;
                    case 6:
                        return m21;
                    case 7:
                        return m31;
                    case 8:
                        return m02;
                    case 9:
                        return m12;
                    case 10:
                        return m22;
                    case 11:
                        return m32;
                    case 12:
                        return m03;
                    case 13:
                        return m13;
                    case 14:
                        return m23;
                    case 15:
                        return m33;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        m00 = value;
                        break;
                    case 1:
                        m10 = value;
                        break;
                    case 2:
                        m20 = value;
                        break;
                    case 3:
                        m30 = value;
                        break;
                    case 4:
                        m01 = value;
                        break;
                    case 5:
                        m11 = value;
                        break;
                    case 6:
                        m21 = value;
                        break;
                    case 7:
                        m31 = value;
                        break;
                    case 8:
                        m02 = value;
                        break;
                    case 9:
                        m12 = value;
                        break;
                    case 10:
                        m22 = value;
                        break;
                    case 11:
                        m32 = value;
                        break;
                    case 12:
                        m03 = value;
                        break;
                    case 13:
                        m13 = value;
                        break;
                    case 14:
                        m23 = value;
                        break;
                    case 15:
                        m33 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 operator *(fpmatrix4x4 lhs, fpmatrix4x4 rhs)
        {
            fpmatrix4x4 matrix;
            matrix.m00 = (fp)((fp128)lhs.m00 * rhs.m00 + (fp128)lhs.m01 * rhs.m10 + (fp128)lhs.m02 * rhs.m20 +
                               (fp128)lhs.m03 * rhs.m30);
            matrix.m01 = (fp)((fp128)lhs.m00 * rhs.m01 + (fp128)lhs.m01 * rhs.m11 + (fp128)lhs.m02 * rhs.m21 +
                               (fp128)lhs.m03 * rhs.m31);
            matrix.m02 = (fp)((fp128)lhs.m00 * rhs.m02 + (fp128)lhs.m01 * rhs.m12 + (fp128)lhs.m02 * rhs.m22 +
                               (fp128)lhs.m03 * rhs.m32);
            matrix.m03 = (fp)((fp128)lhs.m00 * rhs.m03 + (fp128)lhs.m01 * rhs.m13 + (fp128)lhs.m02 * rhs.m23 +
                               (fp128)lhs.m03 * rhs.m33);
            matrix.m10 = (fp)((fp128)lhs.m10 * rhs.m00 + (fp128)lhs.m11 * rhs.m10 + (fp128)lhs.m12 * rhs.m20 +
                               (fp128)lhs.m13 * rhs.m30);
            matrix.m11 = (fp)((fp128)lhs.m10 * rhs.m01 + (fp128)lhs.m11 * rhs.m11 + (fp128)lhs.m12 * rhs.m21 +
                               (fp128)lhs.m13 * rhs.m31);
            matrix.m12 = (fp)((fp128)lhs.m10 * rhs.m02 + (fp128)lhs.m11 * rhs.m12 + (fp128)lhs.m12 * rhs.m22 +
                               (fp128)lhs.m13 * rhs.m32);
            matrix.m13 = (fp)((fp128)lhs.m10 * rhs.m03 + (fp128)lhs.m11 * rhs.m13 + (fp128)lhs.m12 * rhs.m23 +
                               (fp128)lhs.m13 * rhs.m33);
            matrix.m20 = (fp)((fp128)lhs.m20 * rhs.m00 + (fp128)lhs.m21 * rhs.m10 + (fp128)lhs.m22 * rhs.m20 +
                               (fp128)lhs.m23 * rhs.m30);
            matrix.m21 = (fp)((fp128)lhs.m20 * rhs.m01 + (fp128)lhs.m21 * rhs.m11 + (fp128)lhs.m22 * rhs.m21 +
                               (fp128)lhs.m23 * rhs.m31);
            matrix.m22 = (fp)((fp128)lhs.m20 * rhs.m02 + (fp128)lhs.m21 * rhs.m12 + (fp128)lhs.m22 * rhs.m22 +
                               (fp128)lhs.m23 * rhs.m32);
            matrix.m23 = (fp)((fp128)lhs.m20 * rhs.m03 + (fp128)lhs.m21 * rhs.m13 + (fp128)lhs.m22 * rhs.m23 +
                               (fp128)lhs.m23 * rhs.m33);
            matrix.m30 = (fp)((fp128)lhs.m30 * rhs.m00 + (fp128)lhs.m31 * rhs.m10 + (fp128)lhs.m32 * rhs.m20 +
                               (fp128)lhs.m33 * rhs.m30);
            matrix.m31 = (fp)((fp128)lhs.m30 * rhs.m01 + (fp128)lhs.m31 * rhs.m11 + (fp128)lhs.m32 * rhs.m21 +
                               (fp128)lhs.m33 * rhs.m31);
            matrix.m32 = (fp)((fp128)lhs.m30 * rhs.m02 + (fp128)lhs.m31 * rhs.m12 + (fp128)lhs.m32 * rhs.m22 +
                               (fp128)lhs.m33 * rhs.m32);
            matrix.m33 = (fp)((fp128)lhs.m30 * rhs.m03 + (fp128)lhs.m31 * rhs.m13 + (fp128)lhs.m32 * rhs.m23 +
                               (fp128)lhs.m33 * rhs.m33);
            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 operator *(fpmatrix4x4 lhs, fpvec4 vector)
        {
            fpvec4 vec;
            vec.x = (fp)((fp128)lhs.m00 * vector.x + (fp128)lhs.m01 * vector.y + (fp128)lhs.m02 * vector.z +
                          (fp128)lhs.m03 * vector.w);
            vec.y = (fp)((fp128)lhs.m10 * vector.x + (fp128)lhs.m11 * vector.y + (fp128)lhs.m12 * vector.z +
                          (fp128)lhs.m13 * vector.w);
            vec.z = (fp)((fp128)lhs.m20 * vector.x + (fp128)lhs.m21 * vector.y + (fp128)lhs.m22 * vector.z +
                          (fp128)lhs.m23 * vector.w);
            vec.w = (fp)((fp128)lhs.m30 * vector.x + (fp128)lhs.m31 * vector.y + (fp128)lhs.m32 * vector.z +
                          (fp128)lhs.m33 * vector.w);
            return vec;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fpmatrix4x4 lhs, fpmatrix4x4 rhs)
        {
            return lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) &&
                   lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fpmatrix4x4 lhs, fpmatrix4x4 rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec4 GetColumn(int index)
        {
            switch (index)
            {
                case 0:
                    return new fpvec4(m00, m10, m20, m30);
                case 1:
                    return new fpvec4(m01, m11, m21, m31);
                case 2:
                    return new fpvec4(m02, m12, m22, m32);
                case 3:
                    return new fpvec4(m03, m13, m23, m33);
                default:
                    throw new IndexOutOfRangeException("Invalid column index!");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec4 GetRow(int index)
        {
            switch (index)
            {
                case 0:
                    return new fpvec4(m00, m01, m02, m03);
                case 1:
                    return new fpvec4(m10, m11, m12, m13);
                case 2:
                    return new fpvec4(m20, m21, m22, m23);
                case 3:
                    return new fpvec4(m30, m31, m32, m33);
                default:
                    throw new IndexOutOfRangeException("Invalid row index!");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetColumn(int index, fpvec4 column)
        {
            this[0, index] = column.x;
            this[1, index] = column.y;
            this[2, index] = column.z;
            this[3, index] = column.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRow(int index, fpvec4 row)
        {
            this[index, 0] = row.x;
            this[index, 1] = row.y;
            this[index, 2] = row.z;
            this[index, 3] = row.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec3 MultiplyPoint(fpvec3 point)
        {
            var vx = (fp128)m00 * point.x + (fp128)m01 * point.y + (fp128)m02 * point.z + m03;
            var vy = (fp128)m10 * point.x + (fp128)m11 * point.y + (fp128)m12 * point.z + m13;
            var vz = (fp128)m20 * point.x + (fp128)m21 * point.y + (fp128)m22 * point.z + m23;
            var num = fp128math.Rcp((fp128)m30 * point.x + (fp128)m31 * point.y + (fp128)m32 * point.z + m33);
            vx *= num;
            vy *= num;
            vz *= num;
            return new fpvec3((fp)vx, (fp)vy, (fp)vz);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec3 MultiplyPoint3x4(fpvec3 point)
        {
            fpvec3 vec;
            vec.x = (fp)((fp128)m00 * point.x + (fp128)m01 * point.y + (fp128)m02 * point.z + m03);
            vec.y = (fp)((fp128)m10 * point.x + (fp128)m11 * point.y + (fp128)m12 * point.z + m13);
            vec.z = (fp)((fp128)m20 * point.x + (fp128)m21 * point.y + (fp128)m22 * point.z + m23);
            return vec;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fpvec3 MultiplyVector(fpvec3 vector)
        {
            fpvec3 vec;
            vec.x = (fp)((fp128)m00 * vector.x + (fp128)m01 * vector.y + (fp128)m02 * vector.z);
            vec.y = (fp)((fp128)m10 * vector.x + (fp128)m11 * vector.y + (fp128)m12 * vector.z);
            vec.z = (fp)((fp128)m20 * vector.x + (fp128)m21 * vector.y + (fp128)m22 * vector.z);
            return vec;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 Scale(fpvec3 vector)
        {
            fpmatrix4x4 matrix;
            matrix.m00 = vector.x;
            matrix.m01 = fp.Zero;
            matrix.m02 = fp.Zero;
            matrix.m03 = fp.Zero;
            matrix.m10 = fp.Zero;
            matrix.m11 = vector.y;
            matrix.m12 = fp.Zero;
            matrix.m13 = fp.Zero;
            matrix.m20 = fp.Zero;
            matrix.m21 = fp.Zero;
            matrix.m22 = vector.z;
            matrix.m23 = fp.Zero;
            matrix.m30 = fp.Zero;
            matrix.m31 = fp.Zero;
            matrix.m32 = fp.Zero;
            matrix.m33 = fp.One;
            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 Translate(fpvec3 vector)
        {
            fpmatrix4x4 matrix;
            matrix.m00 = fp.One;
            matrix.m01 = fp.Zero;
            matrix.m02 = fp.Zero;
            matrix.m03 = vector.x;
            matrix.m10 = fp.Zero;
            matrix.m11 = fp.One;
            matrix.m12 = fp.Zero;
            matrix.m13 = vector.y;
            matrix.m20 = fp.Zero;
            matrix.m21 = fp.Zero;
            matrix.m22 = fp.One;
            matrix.m23 = vector.z;
            matrix.m30 = fp.Zero;
            matrix.m31 = fp.Zero;
            matrix.m32 = fp.Zero;
            matrix.m33 = fp.One;
            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 Rotate(fpquat q)
        {
            var xx = (fp128)q.x * q.x;
            var xy = (fp128)q.x * q.y;
            var xz = (fp128)q.x * q.z;
            var xw = (fp128)q.x * q.w;
            var yy = (fp128)q.y * q.y;
            var yz = (fp128)q.y * q.z;
            var yw = (fp128)q.y * q.w;
            var zz = (fp128)q.z * q.z;
            var zw = (fp128)q.z * q.w;
            fpmatrix4x4 ret;
            ret.m00 = (fp)(fp128.One - (yy + zz << 1));
            ret.m01 = (fp)(xy - zw << 1);
            ret.m02 = (fp)(xz + yw << 1);
            ret.m10 = (fp)(xy + zw << 1);
            ret.m11 = (fp)(fp128.One - (xx + zz << 1));
            ret.m12 = (fp)(yz - xw << 1);
            ret.m20 = (fp)(xz - yw << 1);
            ret.m21 = (fp)(yz + xw << 1);
            ret.m22 = (fp)(fp128.One - (xx + yy << 1));
            ret.m03 = ret.m13 = ret.m23 = ret.m30 = ret.m31 = ret.m32 = fp.Zero;
            ret.m33 = fp.One;
            return ret;
        }
    }
}