using System;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fixed.Numeric.Editor.Test
{
    public class fp128test
    {
        [Test]
        public void 测试fp128_除法()
        {
            TestDiv(2, 1);
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
                _ = new fp128(123, 0) / fp128.Zero;
            }, "DivideByZeroException");
        }

        private static void TestDiv(float f1, float f2)
        {
            fp128 fp1 = f1;
            fp128 fp2 = f2;
            var r1 = (float)(fp1 / fp2);
            var r2 = (float)fp1 / (float)fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int)Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp128)Math.Abs(n1 - n2), (fp128)Math.Pow(10.0d, -w));
        }

        private static void TestDiv(double f1, double f2)
        {
            fp128 fp1 = f1;
            fp128 fp2 = f2;
            var r1 = (double)(fp1 / fp2);
            var r2 = (double)fp1 / (double)fp2;
            var s1 = r1.ToString("G15");
            var s2 = r2.ToString("G15");
            var n1 = double.Parse(s1);
            var n2 = double.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int)Math.Log10(n2a) + 1;
            var w = Math.Min(15 - int_w - 1, 9);
            Assert.LessOrEqual((fp128)Math.Abs(n1 - n2), (fp128)Math.Pow(10.0d, -w));
        }

        [Test]
        public void 测试fp128求余()
        {
            var i = 0;
            while (i++ < 1000)
            {
                TestMod(Random.Range((float)fp128.MinValue, (float)fp128.MaxValue),
                    Random.Range((float)fp128.MinValue, (float)fp128.MaxValue));
            }

            TestMod(2, 1);
        }

        private static void TestMod(float f1, float f2)
        {
            fp128 fp1 = f1;
            fp128 fp2 = f2;
            var r1 = (float)(fp1 % fp2);
            var r2 = (float)fp1 % (float)fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int)Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp128)Math.Abs(n1 - n2), (fp128)Math.Pow(10.0d, -w));
        }

        [Test]
        public void 测试fp128_加法()
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
            TestPlus(-346.45442f, -1.8551f);
            TestPlus(0.0f, 0.0f);
        }

        private static void TestPlus(float f1, float f2)
        {
            // 单精度浮点数有效位数7位，误差取动态范围
            fp128 fp1 = f1;
            fp128 fp2 = f2;
            var r1 = (float)(fp1 + fp2);
            var r2 = (float)fp1 + (float)fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int)Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp128)Math.Abs(n1 - n2), (fp128)Math.Pow(10.0d, -w));
        }

        private static void TestPlus(double f1, double f2)
        {
            // 双精度浮点数有效位数15~16位，误差取动态范围
            fp128 fp1 = f1;
            fp128 fp2 = f2;
            var r1 = (double)(fp1 + fp2);
            var r2 = (double)fp1 + (double)fp2;
            var s1 = r1.ToString("G15");
            var s2 = r2.ToString("G15");
            var n1 = double.Parse(s1);
            var n2 = double.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int)Math.Log10(n2a) + 1;
            var w = Math.Min(15 - int_w - 1, 9);
            Assert.LessOrEqual((fp128)Math.Abs(n1 - n2), (fp128)Math.Pow(10.0d, -w));
        }

        [Test]
        public void 测试fp128_乘法()
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
            fp128 fp1 = f1;
            fp128 fp2 = f2;
            var r1 = (float)(fp1 * fp2);
            var r2 = (float)fp1 * (float)fp2;
            var s1 = r1.ToString("G7");
            var s2 = r2.ToString("G7");
            var n1 = float.Parse(s1);
            var n2 = float.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int)Math.Log10(n2a) + 1;
            var w = Math.Min(7 - int_w - 1, 9);
            Assert.LessOrEqual((fp128)Math.Abs(n1 - n2), (fp128)Math.Pow(10.0d, -w));
        }

        private static void TestMultiply(double f1, double f2)
        {
            fp128 fp1 = f1;
            fp128 fp2 = f2;
            var r1 = (double)(fp1 * fp2);
            var r2 = (double)fp1 * (double)fp2;
            var s1 = r1.ToString("G15");
            var s2 = r2.ToString("G15");
            var n1 = double.Parse(s1);
            var n2 = double.Parse(s2);
            var n2a = Math.Abs(n2);
            var int_w = n2a < 1 ? 0 : (int)Math.Log10(n2a) + 1;
            var w = Math.Min(15 - int_w - 1, 9);
            Assert.LessOrEqual((fp128)Math.Abs(n1 - n2), (fp128)Math.Pow(10.0d, -w));
        }
    }
}