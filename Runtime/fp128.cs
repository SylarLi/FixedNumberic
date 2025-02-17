using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Fixed.Numeric
{
    /// <summary>
    /// Fixed point(S1Q63.64)
    /// 范围: [-9223372036854775809, 9223372036854775808)
    /// 精度: 1e-20
    /// </summary>
    public struct fp128 : IComparable, IFormattable, IConvertible, IComparable<fp128>, IEquatable<fp128>
    {
        public static readonly fp128 MaxValue = new fp128(long.MaxValue, ulong.MaxValue);
        public static readonly fp128 MinValue = new fp128(0x8000000000000000, 0);
        public static readonly fp128 Epsilon = new fp128(0, 1);
        public static readonly fp128 Zero = new fp128(0, 0);
        public static readonly fp128 One = new fp128(1, 0);
        public static readonly fp128 Half = new fp128(0, 0x8000000000000000);
        public static readonly fp128 Two = new fp128(2, 0);

        private const int FracBits = 64;
        private const double MaxRaw = 9223372036854775808.0;
        private const double MinRaw = -9223372036854775809.0;
        private const double P64Of2 = 18446744073709551616.0;

        internal readonly ulong m_hi;
        internal readonly ulong m_lo;

        public fp128(ulong hi, ulong lo)
        {
            m_hi = hi;
            m_lo = lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(byte value)
        {
            return new fp128(value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator byte(fp128 value)
        {
            return (byte)value.m_hi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(sbyte value)
        {
            return new fp128((ulong)value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator sbyte(fp128 value)
        {
            return (sbyte)value.m_hi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(ushort value)
        {
            return new fp128(value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ushort(fp128 value)
        {
            return (ushort)value.m_hi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(short value)
        {
            return new fp128((ulong)value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator short(fp128 value)
        {
            return (short)value.m_hi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(uint value)
        {
            return new fp128(value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(fp128 value)
        {
            return (uint)value.m_hi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(int value)
        {
            return new fp128((ulong)value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(fp128 value)
        {
            return (int)value.m_hi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(ulong value)
        {
            return new fp128(value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(fp128 value)
        {
            return value.m_hi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(long value)
        {
            return new fp128((ulong)value, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(fp128 value)
        {
            return (long)value.m_hi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(float value)
        {
            return (double)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(fp128 value)
        {
            return (float)(double)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(double value)
        {
            if (value > MaxRaw)
                return MaxValue;
            if (value < MinRaw)
                return MinValue;
            var sign = value < 0;
            if (sign) value = -value;
            var hi = (ulong)value;
            var lo = (ulong)((value - (long)value) * P64Of2);
            var fp = new fp128(hi, lo);
            return sign ? -fp : fp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(fp128 value)
        {
            var sign = value.m_hi >> 63 == 1;
            if (sign) value = -value;
            var r = value.m_hi + value.m_lo / P64Of2;
            return sign ? -r : r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp128(fp value)
        {
            return new fp128((ulong)(int)(value.m_value >> 32), (ulong)(value.m_value & 0x00000000FFFFFFFF) << 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp(fp128 value)
        {
            return new fp(((long)(int)value.m_hi << 32) + (long)(value.m_lo >> 32));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fp128 left, fp128 right)
        {
            return left.m_hi == right.m_hi && left.m_lo == right.m_lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fp128 left, fp128 right)
        {
            return left.m_hi != right.m_hi || left.m_lo != right.m_lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(fp128 left, fp128 right)
        {
            return !(left <= right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(fp128 left, fp128 right)
        {
            return !(left >= right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(fp128 left, fp128 right)
        {
            return (long)left.m_hi > (long)right.m_hi ||
                   left.m_hi == right.m_hi && left.m_lo >= right.m_lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(fp128 left, fp128 right)
        {
            return (long)left.m_hi < (long)right.m_hi ||
                   left.m_hi == right.m_hi && left.m_lo <= right.m_lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator <<(fp128 value, int shift)
        {
            var hi = value.m_hi;
            var lo = value.m_lo;
            shift &= 127;
            if (shift >= 64)
            {
                hi = lo << shift - 64;
                lo = 0;
            }
            else if (shift != 0)
            {
                hi = hi << shift | (lo >> (64 - shift));
                lo <<= shift;
            }

            return new fp128(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator >>(fp128 value, int shift)
        {
            var hi = value.m_hi;
            var lo = value.m_lo;
            shift &= 127;
            if (shift >= 64)
            {
                lo = (ulong)((long)hi >> shift - 64);
                hi = (ulong)((long)hi >> 63);
            }
            else if (shift != 0)
            {
                lo = (lo >> shift) | (ulong)((long)hi << 64 - shift);
                hi = (ulong)((long)hi >> shift);
            }

            return new fp128(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator |(fp128 left, fp128 right)
        {
            return new fp128(left.m_hi | right.m_hi, left.m_lo | right.m_lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator &(fp128 left, fp128 right)
        {
            return new fp128(left.m_hi & right.m_hi, left.m_lo & right.m_lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator ^(fp128 left, fp128 right)
        {
            return new fp128(left.m_hi ^ right.m_hi, left.m_lo ^ right.m_lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator ~(fp128 value)
        {
            return new fp128(~value.m_hi, ~value.m_lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator +(fp128 left, fp128 right)
        {
            var r = left.m_lo + right.m_lo;
            var carry = r < left.m_lo ? 1u : 0u;
            return new fp128(left.m_hi + right.m_hi + carry, r);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator -(fp128 left, fp128 right)
        {
            var r = left.m_lo - right.m_lo;
            var borrow = r > left.m_lo ? 1u : 0u;
            return new fp128(left.m_hi - right.m_hi - borrow, r);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator -(fp128 value)
        {
            var hi = value.m_hi;
            var lo = value.m_lo;
            lo = ~lo + 1;
            hi = ~hi + (lo == 0 ? 1u : 0u);
            return new fp128(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp128 operator *(fp128 left, fp128 right)
        {
            var sign1 = left.m_hi >> 63 == 1;
            if (sign1) left = -left;
            var sign2 = right.m_hi >> 63 == 1;
            if (sign2) right = -right;
            var ret = fputil.Multi128U(left, right);
            if (sign1 != sign2) ret = -ret;
            return ret;
        }

        public static unsafe fp128 operator /(fp128 left, fp128 right)
        {
            if (right == 0)
                throw new DivideByZeroException();
            var sign1 = left.m_hi >> 63 == 1;
            if (sign1) left = -left;
            var sign2 = right.m_hi >> 63 == 1;
            if (sign2) right = -right;
            var temp = (byte*)UnsafeUtility.Malloc(48, UnsafeUtility.AlignOf<byte>(), Allocator.Temp);
            BitConverter.TryWriteBytes(new Span<byte>(temp, 8), left.m_lo);
            BitConverter.TryWriteBytes(new Span<byte>(temp + 8, 8), left.m_hi);
            BitConverter.TryWriteBytes(new Span<byte>(temp + 16, 8), right.m_lo);
            BitConverter.TryWriteBytes(new Span<byte>(temp + 24, 8), right.m_hi);
            BitConverter.TryWriteBytes(new Span<byte>(temp + 32, 8), 0xFFFFFFFFFFFFFFFF);
            BitConverter.TryWriteBytes(new Span<byte>(temp + 40, 8), 0xFFFFFFFFFFFFFFFF);
            var remainder = new BigInteger(new ReadOnlySpan<byte>(temp, 16));
            var divider = new BigInteger(new ReadOnlySpan<byte>(temp + 16, 16));
            var totalMask = new BigInteger(new ReadOnlySpan<byte>(temp + 32, 16));
            var quotient = BigInteger.Zero;
            var bitPos = FracBits + 1;
            var offset = fputil.Min(fputil.Ctz(divider), bitPos);
            divider >>= offset;
            bitPos -= offset;
            while (remainder != 0 && bitPos >= 0)
            {
                var shift = fputil.Clz(remainder);
                shift = fputil.Min(shift, bitPos);
                remainder <<= shift;
                bitPos -= shift;
                var div = BigInteger.DivRem(remainder, divider, out remainder);
                quotient += div << bitPos;
                if ((div & ~(totalMask >> bitPos)) != 0)
                    return sign1 == sign2 ? MaxValue : MinValue;
                remainder <<= 1;
                --bitPos;
            }

            quotient = quotient + 1 >> 1;
            UnsafeUtility.MemClear(temp, 16);
            quotient.TryWriteBytes(new Span<byte>(temp, 16), out _);
            var ret = new fp128(BitConverter.ToUInt64(new ReadOnlySpan<byte>(temp + 8, 8)),
                BitConverter.ToUInt64(new ReadOnlySpan<byte>(temp, 8)));
            if (sign1 != sign2) ret = -ret;
            UnsafeUtility.Free(temp, Allocator.Temp);
            return ret;
        }

        public static unsafe fp128 operator %(fp128 left, fp128 right)
        {
            if (right.m_hi == 0 && right.m_lo == 0)
                throw new InvalidOperationException();
            var sign1 = left.m_hi >> 63 == 1;
            if (sign1) left = -left;
            var sign2 = right.m_hi >> 63 == 1;
            if (sign2) right = -right;
            var temp = (byte*)UnsafeUtility.Malloc(32, UnsafeUtility.AlignOf<byte>(), Allocator.Temp);
            BitConverter.TryWriteBytes(new Span<byte>(temp, 8), left.m_lo);
            BitConverter.TryWriteBytes(new Span<byte>(temp + 8, 8), left.m_hi);
            BitConverter.TryWriteBytes(new Span<byte>(temp + 16, 8), right.m_lo);
            BitConverter.TryWriteBytes(new Span<byte>(temp + 24, 8), right.m_hi);
            var remainder = new BigInteger(new ReadOnlySpan<byte>(temp, 16)) % new BigInteger(new ReadOnlySpan<byte>(temp + 16, 16));
            UnsafeUtility.MemClear(temp, 16);
            remainder.TryWriteBytes(new Span<byte>(temp, 16), out _);
            var ret = new fp128(BitConverter.ToUInt64(new ReadOnlySpan<byte>(temp + 8, 8)),
                BitConverter.ToUInt64(new ReadOnlySpan<byte>(temp, 8)));
            UnsafeUtility.Free(temp, Allocator.Temp);
            if (sign1) ret = -ret;
            return ret;
        }

        public int CompareTo(object value)
        {
            if (value is not fp128)
                throw new ArgumentException("Value is not an instance of fp128.");
            return CompareTo((fp128)value);
        }

        public override string ToString()
        {
            return $"{(double)this}";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.IsNullOrEmpty(format)
                ? ToString(formatProvider)
                : string.Format(formatProvider, format, (double)this);
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
            return (byte)this;
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar((float)this, provider);
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime((float)this, provider);
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal((float)this, provider);
        }

        public double ToDouble(IFormatProvider provider)
        {
            return (double)this;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return (short)this;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return (int)this;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return (long)this;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return (sbyte)this;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return (float)this;
        }

        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, "{0}", (double)this);
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this, conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return (ushort)this;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return (uint)this;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return (ulong)this;
        }

        public int CompareTo(fp128 other)
        {
            if (this == other)
                return 0;
            return this > other ? 1 : -1;
        }

        public bool Equals(fp128 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is fp128 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (m_hi.GetHashCode() * 397) ^ m_lo.GetHashCode();
        }
    }
}