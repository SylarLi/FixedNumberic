using System;
using System.Runtime.CompilerServices;

namespace Fixed.Numeric
{
    /// <summary>
    /// Rectangle with fp64
    /// </summary>
    public struct fprect : IEquatable<fprect>
    {
        public fpvec2 min;

        public fpvec2 max;

        public fp xMin
        {
            get => min.x;
            set => min.x = value;
        }

        public fp yMin
        {
            get => min.y;
            set => min.y = value;
        }

        public fp xMax
        {
            get => max.x;
            set => max.x = value;
        }

        public fp yMax
        {
            get => max.y;
            set => max.y = value;
        }

        public fp width => max.x - min.x;

        public fp height => max.y - min.y;

        public fpvec2 center => (min + max) * fp.Half;

        public fpvec2 size => new fpvec2(width, height);

        public fp area => width * height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Overlaps(in fprect c)
        {
            return !(max.x < c.min.x || min.x > c.max.x || max.y < c.min.y || min.y > c.max.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(in fprect c)
        {
            return min.x <= c.min.x && min.y <= c.min.y && max.x >= c.max.x && max.y >= c.max.y;
        }

        public bool Equals(fprect other)
        {
            return min.Equals(other.min) && max.Equals(other.max);
        }

        public override bool Equals(object obj)
        {
            return obj is fprect other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (min.GetHashCode() * 397) ^ max.GetHashCode();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in fprect a, in fprect b)
        {
            return a.min == b.min && a.max == b.max;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in fprect a, in fprect b)
        {
            return !(a == b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fprect Union(in fprect a, in fprect b)
        {
            fprect rect;
            rect.min = fpvec2.Min(a.min, b.min);
            rect.max = fpvec2.Max(a.max, b.max);
            return rect;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe fprect Union(fpvec2* points, int length)
        {
            var rect = new fprect
            {
                min = new fpvec2(fp.MaxValue, fp.MaxValue),
                max = new fpvec2(fp.MinValue, fp.MinValue)
            };
            for (var j = length - 1; j >= 0; j--)
            {
                rect.min = fpvec2.Min(rect.min, points[j]);
                rect.max = fpvec2.Max(rect.max, points[j]);
            }

            return rect;
        }
    }
}