using System;
using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// Fixed point(S1Q31.32)
    /// 范围: [-2147483648, 2147483648)
    /// 精度: 1e-9
    /// </summary>
    public struct fp : IComparable, IFormattable, IConvertible, IComparable<fp>, IEquatable<fp>
    {
        public static readonly fp MaxValue = new fp(MaxRaw);
        public static readonly fp MinValue = new fp(MinRaw);
        public static readonly fp Epsilon = new fp(1);
        public static readonly fp Zero = new fp(0);
        public static readonly fp One = new fp(0x100000000);
        public static readonly fp Half = new fp(0x80000000);
        public static readonly fp Two = new fp(0x200000000);

        private const long MaxRaw = long.MaxValue;
        private const long MinRaw = long.MinValue;
        private const int TotalBits = 64;
        private const int FracBits = 32;
        private const long Fraction = 1L << FracBits;
        private const long FracMask = 0x00000000FFFFFFFF;
        private const ulong TotalMask = 0xFFFFFFFFFFFFFFFF;

        internal readonly long m_value;

        public fp(long value)
        {
            m_value = value;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(byte value)
        {
            return new fp((long)value << FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator byte(fp value)
        {
            return (byte)(value.m_value >> FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(sbyte value)
        {
            return new fp((long)value << FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator sbyte(fp value)
        {
            return (sbyte)(value.m_value >> FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(ushort value)
        {
            return new fp((long)value << FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ushort(fp value)
        {
            return (ushort)(value.m_value >> FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(short value)
        {
            return new fp((long)value << FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator short(fp value)
        {
            return (short)(value.m_value >> FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(uint value)
        {
            return new fp((long)value << FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(fp value)
        {
            return (uint)(value.m_value >> FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(int value)
        {
            return new fp((long)value << FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(fp value)
        {
            return (int)(value.m_value >> FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(ulong value)
        {
            return new fp((long)value << FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(fp value)
        {
            return (ulong)(value.m_value >> FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(long value)
        {
            return new fp(value << FracBits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(fp value)
        {
            return value.m_value >> FracBits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(float value)
        {
            return new fp((long)((double)value * Fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(fp value)
        {
            return (float)((double)value.m_value / Fraction);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(double value)
        {
            return new fp((long)(value * Fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(fp value)
        {
            return (double)value.m_value / Fraction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fp left, fp right)
        {
            return left.m_value == right.m_value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fp left, fp right)
        {
            return left.m_value != right.m_value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(fp left, fp right)
        {
            return left.m_value > right.m_value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(fp left, fp right)
        {
            return left.m_value < right.m_value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(fp left, fp right)
        {
            return left.m_value >= right.m_value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(fp left, fp right)
        {
            return left.m_value <= right.m_value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator +(fp left, fp right)
        {
            return new fp(left.m_value + right.m_value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator -(fp left, fp right)
        {
            return new fp(left.m_value - right.m_value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator -(fp value)
        {
            return new fp(-value.m_value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator *(fp left, fp right)
        {
            var l = left.m_value;
            var li = l >> FracBits;
            var lf = (ulong)(l & FracMask);
            var r = right.m_value;
            var ri = r >> FracBits;
            var rf = (ulong)(r & FracMask);
            return new fp((li * ri << FracBits) + li * (long)rf + ri * (long)lf + (long)(lf * rf >> FracBits));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator /(fp left, fp right)
        {
            var xl = left.m_value;
            var yl = right.m_value;
            if (yl == 0)
                throw new DivideByZeroException();
            var remainder = (ulong)fputil.Abs(xl);
            var divider = (ulong)fputil.Abs(yl);
            var quotient = 0UL;
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
                var div = remainder / divider;
                remainder %= divider;
                quotient += div << bitPos;
                if ((div & ~(TotalMask >> bitPos)) != 0)
                    return ((xl ^ yl) & MinRaw) == 0 ? MaxValue : MinValue;
                remainder <<= 1;
                --bitPos;
            }

            quotient = quotient + 1 >> 1;
            var result = (long)quotient;
            if (((xl ^ yl) & MinRaw) != 0)
                result = -result;
            return new fp(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator %(fp left, fp right)
        {
            if (right.m_value == 0)
                throw new InvalidOperationException();
            return new fp(left.m_value % right.m_value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator <<(fp value, int shift)
        {
            return new fp(value.m_value << (shift & TotalBits - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator >>(fp value, int shift)
        {
            return new fp(value.m_value >> (shift & TotalBits - 1));
        }

        public int CompareTo(object value)
        {
            if (value is not fp a)
                throw new ArgumentException("Value is not an instance of fp.");
            return CompareTo(a);
        }

        public override string ToString()
        {
            return $"{(double)this:F10}";
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
            return m_value != 0;
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
            return string.Format(provider, "{0:F10}", (double)this);
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

        public int CompareTo(fp other)
        {
            if (this == other)
                return 0;
            return this > other ? 1 : -1;
        }

        public bool Equals(fp other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is fp other && Equals(other);
        }

        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }
    }
}