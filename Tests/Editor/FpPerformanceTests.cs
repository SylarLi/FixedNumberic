using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Fixed.Numeric.Editor.Test
{
    public class FpPerformanceTests
    {
        private const int Iterations = 1000000;
        private fp[] fpArray;
        private float[] floatArray;
        private readonly System.Random rng = new System.Random(42);

        [OneTimeSetUp]
        public void Setup()
        {
            fpArray = new fp[Iterations];
            floatArray = new float[Iterations];

            for (int i = 0; i < Iterations; i++)
            {
                float val = (float)(rng.NextDouble() * 200 - 100);
                fpArray[i] = (fp)val;
                floatArray[i] = val;
            }
        }

        [UnityTest]
        public IEnumerator CompareAdditionPerformance()
        {
            var stopwatch = new Stopwatch();

            // 测试float加法
            stopwatch.Start();
            float floatSum = TestFloatAddition();
            stopwatch.Stop();
            long floatTime = stopwatch.ElapsedMilliseconds;

            // 测试fp加法
            stopwatch.Restart();
            fp fpSum = TestFpAddition();
            stopwatch.Stop();
            long fpTime = stopwatch.ElapsedMilliseconds;

            // 输出结果
            UnityEngine.Debug.Log($"Float Addition: {floatTime}ms, Result: {floatSum}");
            UnityEngine.Debug.Log($"FP Addition: {fpTime}ms, Result: {fpSum}");
            UnityEngine.Debug.Log($"FP is {fpTime / (float)floatTime}x slower");

            yield return null;
        }

        [UnityTest]
        public IEnumerator CompareMultiplicationPerformance()
        {
            var stopwatch = new Stopwatch();
 
            // 测试float乘法
            stopwatch.Start();
            float floatProduct = TestFloatMultiplication();
            stopwatch.Stop();
            long floatTime = stopwatch.ElapsedMilliseconds;

            // 测试fp乘法
            stopwatch.Restart();
            fp fpProduct = TestFpMultiplication();
            stopwatch.Stop();
            long fpTime = stopwatch.ElapsedMilliseconds;

            // 输出结果
            UnityEngine.Debug.Log($"Float Multiplication: {floatTime}ms, Result: {floatProduct}");
            UnityEngine.Debug.Log($"FP Multiplication: {fpTime}ms, Result: {fpProduct}");
            UnityEngine.Debug.Log($"FP is {fpTime / (float)floatTime}x slower");

            yield return null;
        }

        [UnityTest]
        public IEnumerator CompareSubtractionPerformance()
        {
            var stopwatch = new Stopwatch();

            // 测试float减法
            stopwatch.Start();
            float floatResult = TestFloatSubtraction();
            stopwatch.Stop();
            long floatTime = stopwatch.ElapsedMilliseconds;

            // 测试fp减法
            stopwatch.Restart();
            fp fpResult = TestFpSubtraction();
            stopwatch.Stop();
            long fpTime = stopwatch.ElapsedMilliseconds;

            UnityEngine.Debug.Log($"Float Subtraction: {floatTime}ms, Result: {floatResult}");
            UnityEngine.Debug.Log($"FP Subtraction: {fpTime}ms, Result: {fpResult}");
            UnityEngine.Debug.Log($"FP is {fpTime / (float)floatTime}x slower");

            yield return null;
        }

        [UnityTest]
        public IEnumerator CompareDivisionPerformance()
        {
            var stopwatch = new Stopwatch();

            // 测试float除法
            stopwatch.Start();
            float floatResult = TestFloatDivision();
            stopwatch.Stop();
            long floatTime = stopwatch.ElapsedMilliseconds;

            // 测试fp除法
            stopwatch.Restart();
            fp fpResult = TestFpDivision();
            stopwatch.Stop();
            long fpTime = stopwatch.ElapsedMilliseconds;

            UnityEngine.Debug.Log($"Float Division: {floatTime}ms, Result: {floatResult}");
            UnityEngine.Debug.Log($"FP Division: {fpTime}ms, Result: {fpResult}");
            UnityEngine.Debug.Log($"FP is {fpTime / (float)floatTime}x slower");

            yield return null;
        }

        [UnityTest]
        public IEnumerator CompareModuloPerformance()
        {
            var stopwatch = new Stopwatch();

            // 测试float求余（需转换为double避免精度问题）
            stopwatch.Start();
            float floatResult = TestFloatModulo();
            stopwatch.Stop();
            long floatTime = stopwatch.ElapsedMilliseconds;

            // 测试fp求余
            stopwatch.Restart();
            fp fpResult = TestFpModulo();
            stopwatch.Stop();
            long fpTime = stopwatch.ElapsedMilliseconds;

            UnityEngine.Debug.Log($"Float Modulo: {floatTime}ms, Result: {floatResult}");
            UnityEngine.Debug.Log($"FP Modulo: {fpTime}ms, Result: {fpResult}");
            UnityEngine.Debug.Log($"FP is {fpTime / (float)floatTime}x slower");

            yield return null;
        }

        private float TestFloatSubtraction()
        {
            float result = 0;
            for (int i = 0; i < Iterations; i++)
            {
                result -= floatArray[i];
            }
            return result;
        }

        private fp TestFpSubtraction()
        {
            fp result = fp.Zero;
            for (int i = 0; i < Iterations; i++)
            {
                result -= fpArray[i];
            }
            return result;
        }

        private float TestFloatDivision()
        {
            float result = float.MaxValue;
            foreach (var num in floatArray)
            {
                result /= Mathf.Abs(num) + 0.0001f; // 避免除零
            }
            return result;
        }

        private fp TestFpDivision()
        {
            fp result = fp.MaxValue;
            foreach (var num in fpArray)
            {
                result /= fpmath.Abs(num) + fp.Epsilon;
            }
            return result;
        }

        private float TestFloatModulo()
        {
            float result = 0;
            for (int i = 1; i < Iterations; i++)
            {
                float divisor = Mathf.Abs(floatArray[i]) + 0.0001f; // 保证除数不为零
                result += floatArray[i - 1] % divisor;
            }
            return result;
        }

        private fp TestFpModulo()
        {
            fp result = fp.Zero;
            for (int i = 1; i < Iterations; i++)
            {
                fp divisor = fpmath.Abs(fpArray[i]) + fp.Epsilon;
                result += fpArray[i - 1] % divisor;
            }
            return result;
        }


        private float TestFloatAddition()
        {
            float sum = 0;
            foreach (var num in floatArray)
            {
                sum += num;
            }
            return sum;
        }

        private fp TestFpAddition()
        {
            fp sum = fp.Zero;
            foreach (var num in fpArray)
            {
                sum += num;
            }
            return sum;
        }

        private float TestFloatMultiplication()
        {
            float product = 1;
            foreach (var num in floatArray)
            {
                product *= num != 0 ? num : 1;
            }
            return product;
        }

        private fp TestFpMultiplication()
        {
            fp product = fp.One;
            foreach (var num in fpArray)
            {
                product *= num != fp.Zero ? num : fp.One;
            }
            return product;
        }
    }
}
