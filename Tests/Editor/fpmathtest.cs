using UnityEngine;
using NUnit.Framework;
using System;

namespace Fixed.Numeric.Editor.Test
{
    public class fpmathtest
    {
        [Test]
        public void FloatingPointComparison()
        {
            float epsilon = 0.0001f;
            var random = new System.Random(0);

            for (int i = 0; i < 1000; i++)
            {
                float value = (float)(random.NextDouble() * Math.Pow(10, random.Next(-5, 5)));
                if (Math.Abs(value) < 0.01) continue;
                float power = UnityEngine.Random.Range(-2f, 2f);
                float a = (float)(random.NextDouble() * 100 - 50);
                float b = (float)(random.NextDouble() * 100 - 50);
                float t = (float)random.NextDouble();

                Assert.AreEqual((float)Math.Sin(value), (float)fpmath.Sin(value), epsilon);
                Assert.AreEqual((float)Math.Cos(value), (float)fpmath.Cos(value), epsilon);
                Assert.AreEqual((float)Math.Tan(value), (float)fpmath.Tan(value), epsilon);
                Assert.AreEqual((float)Math.Atan(value), (float)fpmath.Atan(value), epsilon);
                Assert.AreEqual((float)Math.Atan2(a, b), (float)fpmath.Atan2(a, b), epsilon);
                if (value >= -1 && value <= 1)
                {
                    Assert.AreEqual((float)Math.Asin(value), (float)fpmath.Asin(value), epsilon);
                    Assert.AreEqual((float)Math.Acos(value), (float)fpmath.Acos(value), epsilon);
                }
                var temp = (float)Math.Exp(value);
                if (!float.IsInfinity(temp) && temp <= (float)fp.MaxValue)
                {
                    Assert.AreEqual(temp, (float)fpmath.Exp(value), epsilon);
                }
                Assert.AreEqual((float)Math.Log(value), (float)fpmath.Log(value), epsilon);
                Assert.AreEqual((float)Math.Log10(value), (float)fpmath.Log10(value), epsilon);

                if (value >= 0)
                {
                    Assert.AreEqual((float)Math.Sqrt(value), (float)fpmath.Sqrt(value), epsilon);
                }

                Assert.AreEqual((float)Math.Pow(value, power), (float)fpmath.Pow(value, power), epsilon, $"Value: {value}, Power: {power}");

                Assert.AreEqual(Mathf.Abs(value), (float)fpmath.Abs(value), epsilon);

                Assert.AreEqual(Mathf.Min(a, b), (float)fpmath.Min(a, b), epsilon);
                Assert.AreEqual(Mathf.Max(a, b), (float)fpmath.Max(a, b), epsilon);

                Assert.AreEqual(Mathf.Clamp(value, a, b), (float)fpmath.Clamp(value, a, b), epsilon);

                Assert.AreEqual(Mathf.Lerp(a, b, t), (float)fpmath.Lerp(a, b, t), epsilon);
            }
        }
    }
}