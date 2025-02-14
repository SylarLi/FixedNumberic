using System;
using NUnit.Framework;

namespace Fixed.Numeric.Editor.Test
{
    public class fptest
    {
        [Test]
        public void 测试fp等于操作符()
        {
            var n1 = new fp(456821);
            var n2 = new fp(456821);
            var n3 = new fp(-123456);
            Assert.IsTrue(n1 == n2);
            Assert.IsTrue(n2 == n1);
            Assert.IsFalse(n1 == n3);
            Assert.IsFalse(n3 == n1);
            Assert.IsFalse(n2 == n3);
            Assert.IsFalse(n3 == n2);
        }

        [Test]
        public void 测试fp不等于操作符()
        {
            var n1 = new fp(-7892214);
            var n2 = new fp(-7892214);
            var n3 = new fp(3245514);
            Assert.IsFalse(n1 != n2);
            Assert.IsFalse(n2 != n1);
            Assert.IsTrue(n1 != n3);
            Assert.IsTrue(n3 != n1);
            Assert.IsTrue(n2 != n3);
            Assert.IsTrue(n3 != n2);
        }

        [Test]
        public void 测试fp大于操作符()
        {
            var n1 = new fp(18);
            var n2 = new fp(17);
            var n3 = new fp(-19);
            Assert.IsTrue(n1 > n2);
            Assert.IsFalse(n2 > n1);
            Assert.IsTrue(n1 > n3);
            Assert.IsFalse(n3 > n1);
        }

        [Test]
        public void 测试fp大于等于操作符()
        {
            var n1 = new fp(789542);
            var n2 = new fp(789542);
            var n3 = new fp(789541);
            var n4 = new fp(-561);
            Assert.IsTrue(n1 >= n2);
            Assert.IsTrue(n2 >= n1);
            Assert.IsTrue(n1 >= n3);
            Assert.IsFalse(n3 >= n1);
            Assert.IsTrue(n1 >= n4);
            Assert.IsFalse(n4 >= n1);
        }

        [Test]
        public void 测试fp小于操作符()
        {
            var n1 = new fp(38466762);
            var n2 = new fp(1231434);
            var n3 = new fp(-38466762);
            Assert.IsFalse(n1 < n2);
            Assert.IsFalse(n1 < n3);
            Assert.IsTrue(n2 < n1);
            Assert.IsTrue(n3 < n1);
        }

        [Test]
        public void 测试fp小于等于操作符()
        {
            var n1 = new fp(-35876);
            var n2 = new fp(-35876);
            var n3 = new fp(-35875);
            var n4 = new fp(12452545);
            Assert.IsTrue(n1 <= n2);
            Assert.IsTrue(n2 <= n1);
            Assert.IsTrue(n1 <= n3);
            Assert.IsFalse(n3 <= n1);
            Assert.IsTrue(n1 <= n4);
            Assert.IsFalse(n4 <= n1);
        }

        [Test]
        public void 测试fp_Equals()
        {
            var n1 = new fp(456821);
            var n2 = new fp(456821);
            var nn = new fp(113211);
            Assert.IsTrue(n1.Equals(n2));
            Assert.IsTrue(n1.Equals((object) n2));
            Assert.IsFalse(n1.Equals(nn));
            Assert.IsFalse(n1.Equals((object) nn));
        }

        [Test]
        public void 测试fp_CompareTo()
        {
            var n1 = new fp(12315357);
            var n2 = new fp(12315357);
            var n3 = new fp(54768);
            Assert.AreEqual(n1.CompareTo(n2), 0);
            Assert.AreEqual(n1.CompareTo(n3), 1);
            Assert.AreEqual(n3.CompareTo(n1), -1);
        }

        [Test]
        public void 测试fp_byte转换()
        {
            for (var i = (int) byte.MinValue; i <= byte.MaxValue; i++)
            {
                var b = (byte) i;
                var f = (fp) b;
                Assert.AreEqual((byte) f, b);
            }
        }

        [Test]
        public void 测试fp_sbyte转换()
        {
            for (var i = (int) sbyte.MinValue; i <= sbyte.MaxValue; i++)
            {
                var b = (sbyte) i;
                var f = (fp) b;
                Assert.AreEqual((sbyte) f, b);
            }
        }

        [Test]
        public void 测试fp_ushort转换()
        {
            for (var i = (int) ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                var b = (ushort) i;
                var f = (fp) b;
                Assert.AreEqual((ushort) f, b);
            }
        }

        [Test]
        public void 测试fp_short转换()
        {
            for (var i = (int) short.MinValue; i <= short.MaxValue; i++)
            {
                var b = (short) i;
                var f = (fp) b;
                Assert.AreEqual((short) f, b);
            }
        }

        [Test]
        public void 测试fp_uint转换()
        {
            for (var i = (ulong) uint.MinValue; i <= uint.MaxValue; i += 65535)
            {
                var b = (uint) i;
                var f = (fp) b;
                Assert.AreEqual((uint) f, b);
                i += 1;
            }
        }

        [Test]
        public void 测试fp_int转换()
        {
            for (var i = (long) int.MinValue; i <= int.MaxValue; i += 65535)
            {
                var b = (int) i;
                var f = (fp) b;
                Assert.AreEqual((int) f, b);
                i += 1;
            }
        }

        [Test]
        public void 测试fp_ulong转换()
        {
            var max = (ulong) fp.MaxValue;
            for (var i = ulong.MinValue; i <= max; i += 65535)
            {
                var f = (fp) i;
                Assert.AreEqual((ulong) f, i);
                i += 1;
            }
        }

        [Test]
        public void 测试fp_long转换()
        {
            var max = (long) fp.MaxValue;
            for (var i = (long) fp.MinValue; i <= max; i += 65535)
            {
                var f = (fp) i;
                Assert.AreEqual((long) f, i);
                i += 1;
            }
        }

        [Test]
        public void 测试fp_float转换()
        {
            Assert.AreEqual((float) fp.Epsilon, (float) (fp) (float) fp.Epsilon);
            Assert.AreEqual((float) fp.MinValue, (float) (fp) (float) fp.MinValue);
            Assert.AreEqual((float) fp.Zero, (float) (fp) (float) fp.Zero);
            Assert.AreEqual((float) fp.One, (float) (fp) (float) fp.One);
            var n = 0L;
            for (var s = 0; s <= 62; s++)
            {
                n <<= 1;
                n |= 1;
                for (var i = 0; i < 62 - s; i++)
                {
                    var f = new fp(n << i);
                    Assert.AreEqual((float) f, (float) (fp) (float) f);
                }
            }

            // 因浮点数精度问题，当fp >= new fp(0x7FFFFFC000000000)测试会失败
            // Assert.AreEqual((float) fp.MaxValue, (float) (fp) (float) fp.MaxValue);
        }

        [Test]
        public void 测试fp_double转换()
        {
            Assert.AreEqual((double) fp.Epsilon, (double) (fp) (double) fp.Epsilon);
            Assert.AreEqual((double) fp.MinValue, (double) (fp) (double) fp.MinValue);
            // 因浮点数精度问题，fp.MaxValue测试会失败
            // Assert.AreEqual((double) fp.MaxValue, (double) (fp) (double) fp.MaxValue);
            Assert.AreEqual((double) fp.Zero, (double) (fp) (double) fp.Zero);
            Assert.AreEqual((double) fp.One, (double) (fp) (double) fp.One);
            var n = 0L;
            for (var s = 0; s <= 62; s++)
            {
                n <<= 1;
                n |= 1;
                for (var i = 0; i < 62 - s; i++)
                {
                    var f = new fp(n << i);
                    Assert.AreEqual((double) f, (double) (fp) (double) f);
                }
            }

            // 因浮点数精度问题，当fp >= new fp(0x7FFFFFFFFFFFFE00)测试会失败
            // Assert.AreEqual((float) fp.MaxValue, (float) (fp) (float) fp.MaxValue);
        }

        [Test]
        public void 测试fp_左移操作符()
        {
            var n = 123354L;
            var f = new fp(n);
            Assert.AreEqual(f << 4, new fp(n << 4));
            Assert.AreEqual(f << 128, new fp(n << (128 & 31)));
        }

        [Test]
        public void 测试fp_右移操作符()
        {
            var n = 1158123354L;
            var f = new fp(n);
            Assert.AreEqual(f >> 4, new fp(n >> 4));
            Assert.AreEqual(f >> 128, new fp(n >> (128 & 31)));
        }

        [Test]
        public void 测试fp_取反操作符()
        {
            fp f1 = 1.23554;
            fp f2 = -1.23554;
            Assert.AreEqual(f1, -f2);
            Assert.AreEqual(-f1, f2);
            f1 = 0;
            f2 = 0;
            Assert.AreEqual(f1, -f2);
            Assert.AreEqual(-f1, f2);
        }

        [Test]
        public void 测试fp_加法()
        {
            TestPlus(346.45442d, 1.2551d);
            TestPlus(346.45442d, -1.2551d);
            TestPlus(-346.45442d, 1.2551d);
            TestPlus(-346.45442d, -1.2551d);
            TestPlus(0.0d, 0.0d);
            TestPlus(0.123132123123132d, 0.687961234543214d);

            TestPlus(346.45442f, 1.2551f);
            TestPlus(346.45442f, -1.2551f);
            TestPlus(-346.45442f, 1.2551f);
            TestPlus(-346.45442f, -1.2551f);
            TestPlus(0.0f, 0.0f);
        }

        private static void TestPlus(float f1, float f2)
        {
            // 单精度浮点数有效位数7位，误差取动态范围
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (float) (fp1 + fp2);
            var r2 = (float) fp1 + (float) fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        private static void TestPlus(double f1, double f2)
        {
            // 双精度浮点数有效位数15~16位，误差取动态范围
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (double) (fp1 + fp2);
            var r2 = (double) fp1 + (double) fp2;
            var s1 = r1.ToString("G15");
            var s2 = r2.ToString("G15");
            var n1 = double.Parse(s1);
            var n2 = double.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(15 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        [Test]
        public void 测试fp_减法()
        {
            TestMinus(346.45442d, 1.2551d);
            TestMinus(346.45442d, -1.2551d);
            TestMinus(-346.45442d, 1.2551d);
            TestMinus(-346.45442d, -1.2551d);
            TestMinus(0.0d, 0.0d);
            TestMinus(0.123132123123132d, 0.687961234543214d);

            TestMinus(346.45442f, 1.2551f);
            TestMinus(346.45442f, -1.2551f);
            TestMinus(-346.45442f, 1.2551f);
            TestMinus(-346.45442f, -1.2551f);
            TestMinus(0.0f, 0.0f);
        }

        private static void TestMinus(float f1, float f2)
        {
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (float) (fp1 - fp2);
            var r2 = (float) fp1 - (float) fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        private static void TestMinus(double f1, double f2)
        {
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (double) (fp1 - fp2);
            var r2 = (double) fp1 - (double) fp2;
            var s1 = r1.ToString("G15");
            var s2 = r2.ToString("G15");
            var n1 = double.Parse(s1);
            var n2 = double.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(15 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        [Test]
        public void 测试fp_乘法()
        {
            TestMultiply(-1.1d, -1d);
            TestMultiply(4567.0d, 1234.0d);
            TestMultiply(4567.0d, -1234.0d);
            TestMultiply(-4567.0d, 1234.0d);
            TestMultiply(-12347.52d, -11234.31d);
            TestMultiply(-1.32453242d, 0.865663214d);
            TestMultiply(-90148.7500946145d, 1245.865663214d);

            TestMultiply(4567.0f, 1234.0f);
            TestMultiply(4567.0f, -1234.0f);
            TestMultiply(-12347.546f, -11234.245f);
            TestMultiply(-1.32453242f, 0.865663214f);
            TestMultiply(-1.32453242f, 0f);
        }

        private static void TestMultiply(float f1, float f2)
        {
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (float) (fp1 * fp2);
            var r2 = (float) fp1 * (float) fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        private static void TestMultiply(double f1, double f2)
        {
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (double) (fp1 * fp2);
            var r2 = (double) fp1 * (double) fp2;
            var s1 = r1.ToString("G15");
            var s2 = r2.ToString("G15");
            var n1 = double.Parse(s1);
            var n2 = double.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(15 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        [Test]
        public void 测试fp_除法()
        {
            TestDiv(112313232d, 0.1d);
            TestDiv(4567.0d, 1234.0d);
            TestDiv(4567.0d, -1234.0d);
            TestDiv(-12347.52d, -11234.31d);
            TestDiv(1.865663214d, -112313232.32453d);
            TestDiv(-1.32453242d, 0.865663214d);

            TestDiv(4567.0f, 1234.0f);
            TestDiv(4567.0f, -1234.0f);
            TestDiv(-12347.546f, -11234.245f);
            TestDiv(-112313232.32453f, 1.865663214f);
            TestDiv(-1.32453242f, 0.865663214f);

            Assert.Catch<DivideByZeroException>(() =>
            {
                _ = new fp(123) / fp.Zero;
            }, "DivideByZeroException");
        }

        private static void TestDiv(float f1, float f2)
        {
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (float) (fp1 / fp2);
            var r2 = (float) fp1 / (float) fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        private static void TestDiv(double f1, double f2)
        {
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (double) (fp1 / fp2);
            var r2 = (double) fp1 / (double) fp2;
            var s1 = r1.ToString("G15");
            var s2 = r2.ToString("G15");
            var n1 = double.Parse(s1);
            var n2 = double.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(15 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        [Test]
        public void 测试fp_求余()
        {
            TestMod(112313232d, 0.1d);
            TestMod(4567.0d, 1234.0d);
            TestMod(4567.0d, -1234.0d);
            TestMod(-12347.52d, -11234.31d);
            TestMod(1.865663214d, -112313232.32453d);
            TestMod(-1.32453242d, 0.865663214d);

            TestMod(4567.0f, 1234.0f);
            TestMod(4567.0f, -1234.0f);
            TestMod(-12347.546f, -11234.245f);
            TestMod(-112313232.32453f, 1.865663214f);
            TestMod(-1.32453242f, 0.865663214f);
        }

        private static void TestMod(float f1, float f2)
        {
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (float) (fp1 % fp2);
            var r2 = (float) fp1 % (float) fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }

        private static void TestMod(double f1, double f2)
        {
            fp fp1 = f1;
            fp fp2 = f2;
            var r1 = (double) (fp1 % fp2);
            var r2 = (double) fp1 % (double) fp2;
            var s1 = r1.ToString("G15");
            var s2 = r2.ToString("G15");
            var n1 = double.Parse(s1);
            var n2 = double.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int) Math.Log10(n2a) + 1;
            var w = Math.Min(15 - int_w - 1, 9);
            Assert.LessOrEqual((fp) Math.Abs(n1 - n2), (fp) Math.Pow(10.0d, -w));
        }
    }
}