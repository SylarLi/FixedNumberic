using System;
using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// Fixed point(S1Q63.128)
    /// 范围: [-9223372036854775809, 9223372036854775808)
    /// 精度: 1e-39
    /// </summary>
    internal struct fp192 : IComparable, IFormattable, IConvertible, IComparable<fp192>, IEquatable<fp192>
    {
        public static readonly fp192 MaxValue = new fp192(long.MaxValue, ulong.MaxValue, ulong.MaxValue);
        public static readonly fp192 MinValue = new fp192(0x8000000000000000, 0, 0);
        public static readonly fp192 Epsilon = new fp192(0, 0, 1);
        public static readonly fp192 Zero = new fp192(0, 0, 0);
        public static readonly fp192 One = new fp192(1, 0, 0);
        public static readonly fp192 Half = new fp192(1, 0x8000000000000000, 0);
        public static readonly fp192 Two = new fp192(2, 0, 0);

        private const double MaxRaw = 9223372036854775808.0;
        private const double MinRaw = -9223372036854775809.0;
        private const double P64Of2 = 18446744073709551616.0;
        private const double P128Of2 = 340282366920938463463374607431768211456.0;

        internal readonly ulong m_hi;
        internal readonly ulong m_lo;
        internal readonly ulong m_oo;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp192(ulong hi, ulong lo, ulong oo)
        {
            m_hi = hi;
            m_lo = lo;
            m_oo = oo;
        }

        public static implicit operator fp192(byte value)
        {
            return new fp192(value, 0, 0);
        }

        public static explicit operator byte(fp192 value)
        {
            return (byte) value.m_hi;
        }

        public static implicit operator fp192(sbyte value)
        {
            return new fp192((ulong) value, 0, 0);
        }

        public static explicit operator sbyte(fp192 value)
        {
            return (sbyte) value.m_hi;
        }

        public static implicit operator fp192(ushort value)
        {
            return new fp192(value, 0, 0);
        }

        public static explicit operator ushort(fp192 value)
        {
            return (ushort) value.m_hi;
        }

        public static implicit operator fp192(short value)
        {
            return new fp192((ulong) value, 0, 0);
        }

        public static explicit operator short(fp192 value)
        {
            return (short) value.m_hi;
        }

        public static implicit operator fp192(uint value)
        {
            return new fp192(value, 0, 0);
        }

        public static explicit operator uint(fp192 value)
        {
            return (uint) value.m_hi;
        }

        public static implicit operator fp192(int value)
        {
            return new fp192((ulong) value, 0, 0);
        }

        public static explicit operator int(fp192 value)
        {
            return (int) value.m_hi;
        }

        public static implicit operator fp192(ulong value)
        {
            return new fp192(value, 0, 0);
        }

        public static explicit operator ulong(fp192 value)
        {
            return value.m_hi;
        }

        public static explicit operator fp192(long value)
        {
            return new fp192((ulong) value, 0, 0);
        }

        public static explicit operator long(fp192 value)
        {
            return (long) value.m_hi;
        }

        public static explicit operator fp192(float value)
        {
            return (double) value;
        }

        public static explicit operator float(fp192 value)
        {
            return (float) (double) value;
        }

        public static implicit operator fp192(double value)
        {
            if (value > MaxRaw)
                return MaxValue;
            if (value < MinRaw)
                return MinValue;
            var sign = value < 0;
            if (sign) value = -value;
            var hi = (ulong) value;
            var ff = (value - (long) value) * P64Of2;
            var lo = (ulong) ff;
            var oo = (ulong) ((ff - lo) * P64Of2);
            var fp = new fp192(hi, lo, oo);
            return sign ? -fp : fp;
        }

        public static explicit operator double(fp192 value)
        {
            var sign = value.m_hi >> 63 == 1;
            if (sign) value = -value;
            var r = value.m_hi + value.m_lo / P64Of2 + value.m_oo / P128Of2;
            return sign ? -r : r;
        }

        public static implicit operator fp192(fp value)
        {
            return new fp192((ulong) (int) (value.m_value >> 32), (ulong) (value.m_value & 0x00000000FFFFFFFF) << 32,
                0);
        }

        public static explicit operator fp(fp192 value)
        {
            return new fp(((long) (int) value.m_hi << 32) + (long) (value.m_lo >> 32));
        }

        public static implicit operator fp192(fp128 value)
        {
            return new fp192(value.m_hi, value.m_lo, 0);
        }

        public static explicit operator fp128(fp192 value)
        {
            return new fp128(value.m_hi, value.m_lo);
        }

        public static bool operator ==(fp192 left, fp192 right)
        {
            return left.m_hi == right.m_hi && left.m_lo == right.m_lo && left.m_oo == right.m_oo;
        }

        public static bool operator !=(fp192 left, fp192 right)
        {
            return !(left == right);
        }

        public static bool operator >(fp192 left, fp192 right)
        {
            return !(left <= right);
        }

        public static bool operator <(fp192 left, fp192 right)
        {
            return !(left >= right);
        }

        public static bool operator >=(fp192 left, fp192 right)
        {
            return (long) left.m_hi > (long) right.m_hi ||
                   left.m_hi == right.m_hi && left.m_lo >= right.m_lo;
        }

        public static bool operator <=(fp192 left, fp192 right)
        {
            return (long) left.m_hi < (long) right.m_hi ||
                   left.m_hi == right.m_hi && left.m_lo <= right.m_lo;
        }

        public static fp192 operator <<(fp192 value, int shift)
        {
            var hi = value.m_hi;
            var lo = value.m_lo;
            var oo = value.m_oo;
            shift &= 191;
            if (shift >= 128)
            {
                hi = oo << shift - 128;
                lo = 0;
                oo = 0;
            }
            else if (shift >= 64)
            {
                hi = lo << shift - 64;
                hi |= oo >> 128 - shift;
                lo = oo << shift - 64;
                oo = 0;
            }
            else if (shift != 0)
            {
                hi <<= shift;
                hi |= lo >> 64 - shift;
                lo <<= shift;
                lo |= oo >> 64 - shift;
                oo <<= shift;
            }

            return new fp192(hi, lo, oo);
        }

        public static fp192 operator >>(fp192 value, int shift)
        {
            var hi = value.m_hi;
            var lo = value.m_lo;
            var oo = value.m_oo;
            shift &= 191;
            if (shift >= 128)
            {
                oo = (ulong) ((long) hi >> shift - 128);
                lo = 0;
                hi = (ulong) ((long) hi >> 63);
            }
            else if (shift >= 64)
            {
                oo = lo >> shift - 64;
                oo |= (ulong) ((long) hi << 128 - shift);
                lo = (ulong) ((long) hi >> shift - 64);
                hi = (ulong) ((long) hi >> 63);
            }
            else if (shift != 0)
            {
                oo >>= shift;
                oo |= lo << 64 - shift;
                lo >>= shift;
                lo |= (ulong) ((long) hi << 64 - shift);
                hi = (ulong) ((long) hi >> shift);
            }

            return new fp192(hi, lo, oo);
        }

        public static fp192 operator |(fp192 left, fp192 right)
        {
            return new fp192(left.m_hi | right.m_hi, left.m_lo | right.m_lo, left.m_oo | right.m_oo);
        }

        public static fp192 operator &(fp192 left, fp192 right)
        {
            return new fp192(left.m_hi & right.m_hi, left.m_lo & right.m_lo, left.m_oo & right.m_oo);
        }

        public static fp192 operator ^(fp192 left, fp192 right)
        {
            return new fp192(left.m_hi ^ right.m_hi, left.m_lo ^ right.m_lo, left.m_oo ^ right.m_oo);
        }

        public static fp192 operator ~(fp192 value)
        {
            return new fp192(~value.m_hi, ~value.m_lo, ~value.m_oo);
        }

        public static fp192 operator +(fp192 left, fp192 right)
        {
            ulong hi = left.m_hi, lo = left.m_lo, oo = left.m_oo;
            oo += right.m_oo;
            var c = oo < right.m_oo ? 1u : 0u;
            lo += c;
            hi += lo < c ? 1u : 0u;
            lo += right.m_lo;
            hi += lo < right.m_lo ? 1u : 0u;
            hi += right.m_hi;
            return new fp192(hi, lo, oo);
        }

        public static fp192 operator -(fp192 left, fp192 right)
        {
            ulong hi = left.m_hi, lo = left.m_lo, oo = left.m_oo;
            var r = oo - right.m_oo;
            var b = oo < r ? 1u : 0u;
            oo = r;
            r = lo - b;
            b = lo < r ? 1u : 0u;
            lo = r;
            hi -= b;
            r = lo - right.m_lo;
            hi -= lo < r ? 1u : 0u;
            lo = r;
            hi -= right.m_hi;
            return new fp192(hi, lo, oo);
        }

        public static fp192 operator -(fp192 value)
        {
            var hi = value.m_hi;
            var lo = value.m_lo;
            var oo = value.m_oo;
            if (oo != 0)
            {
                oo = ~oo + 1;
                lo = ~lo;
                hi = ~hi;
            }
            else if (lo != 0)
            {
                lo = ~lo + 1;
                hi = ~hi;
            }
            else
            {
                hi = ~hi + 1;
            }

            return new fp192(hi, lo, oo);
        }

        public static fp192 operator *(fp192 left, fp192 right)
        {
            var sign1 = left.m_hi >> 63 == 1;
            if (sign1) left = -left;
            var sign2 = right.m_hi >> 63 == 1;
            if (sign2) right = -right;
            var r = fputil.Multi192U(left, right);
            return sign1 != sign2 ? -r : r;
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is fp192))
                throw new ArgumentException("Value is not an instance of fp192.");
            return CompareTo((fp192) value);
        }

        public override string ToString()
        {
            return $"{(double) this}";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.IsNullOrEmpty(format)
                ? ToString(formatProvider)
                : string.Format(formatProvider, format, (double) this);
        }

        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return this != 0;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return (byte) this;
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar((float) this, provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime((float) this, provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal((float) this, provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return (double) this;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return (short) this;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return (int) this;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return (long) this;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return (sbyte) this;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return (float) this;
        }

        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, "{0}", (double) this);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this, conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return (ushort) this;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return (uint) this;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return (ulong) this;
        }

        public int CompareTo(fp192 other)
        {
            if (this == other)
                return 0;
            return this > other ? 1 : -1;
        }

        public bool Equals(fp192 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is fp192 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (m_hi.GetHashCode() * 397) ^ m_lo.GetHashCode();
        }
    }
}