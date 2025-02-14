using UnityEngine;
using NUnit.Framework;
using System;

namespace Fixed.Numeric.Editor.Test
{
    public class fpvec4Tests
    {
        private const float Epsilon = 0.0001f;

        [Test]
        public void Zero_IsCorrect()
        {
            Assert.AreEqual(Vector4.zero.x, (float)fpvec4.zero.x, Epsilon);
            Assert.AreEqual(Vector4.zero.y, (float)fpvec4.zero.y, Epsilon);
            Assert.AreEqual(Vector4.zero.z, (float)fpvec4.zero.z, Epsilon);
            Assert.AreEqual(Vector4.zero.w, (float)fpvec4.zero.w, Epsilon);
        }

        [Test]
        public void One_IsCorrect()
        {
            Assert.AreEqual(Vector4.one.x, (float)fpvec4.one.x, Epsilon);
            Assert.AreEqual(Vector4.one.y, (float)fpvec4.one.y, Epsilon);
            Assert.AreEqual(Vector4.one.z, (float)fpvec4.one.z, Epsilon);
            Assert.AreEqual(Vector4.one.w, (float)fpvec4.one.w, Epsilon);
        }

        [Test]
        public void Constructor_Works()
        {
            var floatVec = new Vector4(1.5235f, 2.5789f, 3.4567f, 4.5678f);
            var fixedVec = new fpvec4((fp)1.5235f, (fp)2.5789f, (fp)3.4567f, (fp)4.5678f);
            Assert.AreEqual(floatVec.x, (float)fixedVec.x, Epsilon);
            Assert.AreEqual(floatVec.y, (float)fixedVec.y, Epsilon);
            Assert.AreEqual(floatVec.z, (float)fixedVec.z, Epsilon);
            Assert.AreEqual(floatVec.w, (float)fixedVec.w, Epsilon);
        }

        [Test]
        public void Indexer_Works()
        {
            var floatVec = new Vector4(1.5235f, 2.5789f, 3.4567f, 4.5678f);
            var fixedVec = new fpvec4((fp)1.5235f, (fp)2.5789f, (fp)3.4567f, (fp)4.5678f);
            Assert.AreEqual(floatVec[0], (float)fixedVec[0], Epsilon);
            Assert.AreEqual(floatVec[1], (float)fixedVec[1], Epsilon);
            Assert.AreEqual(floatVec[2], (float)fixedVec[2], Epsilon);
            Assert.AreEqual(floatVec[3], (float)fixedVec[3], Epsilon);
            floatVec[0] = 3.5678f;
            floatVec[1] = 4.5432f;
            floatVec[2] = 5.6789f;
            floatVec[3] = 6.7890f;
            fixedVec[0] = (fp)3.5678f;
            fixedVec[1] = (fp)4.5432f;
            fixedVec[2] = (fp)5.6789f;
            fixedVec[3] = (fp)6.7890f;
            Assert.AreEqual(floatVec[0], (float)fixedVec[0], Epsilon);
            Assert.AreEqual(floatVec[1], (float)fixedVec[1], Epsilon);
            Assert.AreEqual(floatVec[2], (float)fixedVec[2], Epsilon);
            Assert.AreEqual(floatVec[3], (float)fixedVec[3], Epsilon);

            Assert.Throws<IndexOutOfRangeException>(() => { var _ = fixedVec[4]; });
            Assert.Throws<IndexOutOfRangeException>(() => { fixedVec[4] = (fp)1.0f; });
        }

        [Test]
        public void Set_Works()
        {
            var floatVec = new Vector4();
            var fixedVec = new fpvec4();
            floatVec.Set(1, 2, 3, 4);
            fixedVec.Set((fp)1, (fp)2, (fp)3, (fp)4);
            Assert.AreEqual(floatVec.x, (float)fixedVec.x, Epsilon);
            Assert.AreEqual(floatVec.y, (float)fixedVec.y, Epsilon);
            Assert.AreEqual(floatVec.z, (float)fixedVec.z, Epsilon);
            Assert.AreEqual(floatVec.w, (float)fixedVec.w, Epsilon);
        }

        [Test]
        public void Scale_Works()
        {
            var floatVec = new Vector4(2, 3, 4, 5);
            var floatScale = new Vector4(4, 5, 6, 7);
            var fixedVec = new fpvec4((fp)2, (fp)3, (fp)4, (fp)5);
            var fixedScale = new fpvec4((fp)4, (fp)5, (fp)6, (fp)7);
            floatVec.Scale(floatScale);
            fixedVec.Scale(fixedScale);
            Assert.AreEqual(floatVec.x, (float)fixedVec.x, Epsilon);
            Assert.AreEqual(floatVec.y, (float)fixedVec.y, Epsilon);
            Assert.AreEqual(floatVec.z, (float)fixedVec.z, Epsilon);
            Assert.AreEqual(floatVec.w, (float)fixedVec.w, Epsilon);
        }

        [Test]
        public void MagnitudeTest()
        {
            var floatVec = new Vector4(3.1415f, 4.2678f, 5.3246f, 6.4357f);
            var fixedVec = new fpvec4((fp)3.1415f, (fp)4.2678f, (fp)5.3246f, (fp)6.4357f);
            Assert.AreEqual(floatVec.magnitude, (float)fixedVec.magnitude, Epsilon);
        }

        [Test]
        public void NormalizeTest()
        {
            var floatVec = new Vector4(3.1415f, 4.2678f, 5.3246f, 6.4357f);
            var fixedVec = new fpvec4((fp)3.1415f, (fp)4.2678f, (fp)5.3246f, (fp)6.4357f);

            var floatNormalized = floatVec.normalized;
            var fixedNormalized = fixedVec.normalized;

            Assert.AreEqual(floatNormalized.x, (float)fixedNormalized.x, Epsilon);
            Assert.AreEqual(floatNormalized.y, (float)fixedNormalized.y, Epsilon);
            Assert.AreEqual(floatNormalized.z, (float)fixedNormalized.z, Epsilon);
            Assert.AreEqual(floatNormalized.w, (float)fixedNormalized.w, Epsilon);
        }

        [Test]
        public void DotProductTest()
        {
            var floatA = new Vector4(1.2345f, 2.3456f, 3.4567f, 4.5678f);
            var floatB = new Vector4(4.5678f, 5.6789f, 6.7890f, 7.8901f);
            var fixedA = new fpvec4((fp)1.2345f, (fp)2.3456f, (fp)3.4567f, (fp)4.5678f);
            var fixedB = new fpvec4((fp)4.5678f, (fp)5.6789f, (fp)6.7890f, (fp)7.8901f);

            var floatDot = Vector4.Dot(floatA, floatB);
            var fixedDot = fpvec4.Dot(fixedA, fixedB);
            Assert.AreEqual(floatDot, (float)fixedDot, Epsilon);
        }
        [Test]
        public void AdditionTest()
        {
            var floatA = new Vector4(1.2f, 2.3f, 3.4f, 4.5f);
            var floatB = new Vector4(5.6f, 6.7f, 7.8f, 8.9f);
            var fixedA = new fpvec4((fp)1.2f, (fp)2.3f, (fp)3.4f, (fp)4.5f);
            var fixedB = new fpvec4((fp)5.6f, (fp)6.7f, (fp)7.8f, (fp)8.9f);

            var floatResult = floatA + floatB;
            var fixedResult = fixedA + fixedB;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void SubtractionTest()
        {
            var floatA = new Vector4(1.2f, 2.3f, 3.4f, 4.5f);
            var floatB = new Vector4(5.6f, 6.7f, 7.8f, 8.9f);
            var fixedA = new fpvec4((fp)1.2f, (fp)2.3f, (fp)3.4f, (fp)4.5f);
            var fixedB = new fpvec4((fp)5.6f, (fp)6.7f, (fp)7.8f, (fp)8.9f);

            var floatResult = floatA - floatB;
            var fixedResult = fixedA - fixedB;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void ScalarMultiplicationTest()
        {
            var floatVec = new Vector4(1.2f, 2.3f, 3.4f, 4.5f);
            var fixedVec = new fpvec4((fp)1.2f, (fp)2.3f, (fp)3.4f, (fp)4.5f);
            float scalar = 2.5f;
            fp fixedScalar = (fp)2.5f;

            var floatResult = floatVec * scalar;
            var fixedResult = fixedVec * fixedScalar;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void EqualityTest()
        {
            var floatA = new Vector4(1.2f, 2.3f, 3.4f, 4.5f);
            var floatB = new Vector4(1.2f, 2.3f, 3.4f, 4.5f);
            var fixedA = new fpvec4((fp)1.2f, (fp)2.3f, (fp)3.4f, (fp)4.5f);
            var fixedB = new fpvec4((fp)1.2f, (fp)2.3f, (fp)3.4f, (fp)4.5f);

            Assert.IsTrue(floatA == floatB);
            Assert.IsTrue(fixedA == fixedB);
        }

        [Test]
        public void InequalityTest()
        {
            var floatA = new Vector4(1.2f, 2.3f, 3.4f, 4.5f);
            var floatB = new Vector4(5.6f, 6.7f, 7.8f, 8.9f);
            var fixedA = new fpvec4((fp)1.2f, (fp)2.3f, (fp)3.4f, (fp)4.5f);
            var fixedB = new fpvec4((fp)5.6f, (fp)6.7f, (fp)7.8f, (fp)8.9f);

            Assert.IsTrue(floatA != floatB);
            Assert.IsTrue(fixedA != fixedB);
        }

        [Test]
        public void DivisionTest()
        {
            var floatA = new Vector4(4, 8, 12, 16);
            var floatB = 1.5545f;
            var fixedA = new fpvec4((fp)4, (fp)8, (fp)12, (fp)16);
            var fixedB = 1.5545f;

            var floatResult = floatA / floatB;
            var fixedResult = fixedA / fixedB;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void ScalarDivisionTest()
        {
            var floatVec = new Vector4(4, 8, 12, 16);
            var fixedVec = new fpvec4((fp)4, (fp)8, (fp)12, (fp)16);
            float scalar = 2;
            fp fixedScalar = (fp)2;

            var floatResult = floatVec / scalar;
            var fixedResult = fixedVec / fixedScalar;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void NegationTest()
        {
            var floatVec = new Vector4(1, 2, 3, 4);
            var fixedVec = new fpvec4((fp)1, (fp)2, (fp)3, (fp)4);

            var floatNegated = -floatVec;
            var fixedNegated = -fixedVec;

            Assert.AreEqual(floatNegated.x, (float)fixedNegated.x, Epsilon);
            Assert.AreEqual(floatNegated.y, (float)fixedNegated.y, Epsilon);
            Assert.AreEqual(floatNegated.z, (float)fixedNegated.z, Epsilon);
            Assert.AreEqual(floatNegated.w, (float)fixedNegated.w, Epsilon);
        }

        [Test]
        public void ProjectTest()
        {
            var floatVector = new Vector4(1.23456789f, 2.34567891f, 3.45678912f, 4.56789123f);
            var floatOnNormal = new Vector4(5.67891234f, 6.78912345f, 7.89123456f, 8.91234567f);

            var fixedVector = new fpvec4((fp)1.23456789f, (fp)2.34567891f, (fp)3.45678912f, (fp)4.56789123f);
            var fixedOnNormal = new fpvec4((fp)5.67891234f, (fp)6.78912345f, (fp)7.89123456f, (fp)8.91234567f);

            var floatProjected = Vector4.Project(floatVector, floatOnNormal);
            var fixedProjected = fpvec4.Project(fixedVector, fixedOnNormal);

            Assert.AreEqual(floatProjected.x, (float)fixedProjected.x, Epsilon);
            Assert.AreEqual(floatProjected.y, (float)fixedProjected.y, Epsilon);
            Assert.AreEqual(floatProjected.z, (float)fixedProjected.z, Epsilon);
            Assert.AreEqual(floatProjected.w, (float)fixedProjected.w, Epsilon);
        }

         [Test]
        public void LerpTest()
        {
            var floatFrom = new Vector4(1.123456f, 2.234567f, 3.345678f, 4.456789f);
            var floatTo = new Vector4(5.567890f, 6.678901f, 7.789012f, 8.890123f);
            float t = 0.34567f;

            var fixedFrom = new fpvec4((fp)1.123456f, (fp)2.234567f, (fp)3.345678f, (fp)4.456789f);
            var fixedTo = new fpvec4((fp)5.567890f, (fp)6.678901f, (fp)7.789012f, (fp)8.890123f);
            fp fixedT = (fp)t;

            var floatResult = Vector4.Lerp(floatFrom, floatTo, t);
            var fixedResult = fpvec4.Lerp(fixedFrom, fixedTo, fixedT);

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void MoveTowardsTest()
        {
            var floatCurrent = new Vector4(1.12345f, 2.23456f, 3.34567f, 4.45678f);
            var floatTarget = new Vector4(5.56789f, 6.67890f, 7.78901f, 8.89012f);
            float maxDistanceDelta = 0.56789f;

            var fixedCurrent = new fpvec4((fp)1.12345f, (fp)2.23456f, (fp)3.34567f, (fp)4.45678f);
            var fixedTarget = new fpvec4((fp)5.56789f, (fp)6.67890f, (fp)7.78901f, (fp)8.89012f);
            fp fixedMaxDistanceDelta = (fp)maxDistanceDelta;

            var floatResult = Vector4.MoveTowards(floatCurrent, floatTarget, maxDistanceDelta);
            var fixedResult = fpvec4.MoveTowards(fixedCurrent, fixedTarget, fixedMaxDistanceDelta);

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void MinTest()
        {
            var floatA = new Vector4(1.23456f, 2.34567f, 3.45678f, 4.56789f);
            var floatB = new Vector4(5.67890f, 1.12345f, 7.89012f, 3.34567f);

            var fixedA = new fpvec4((fp)1.23456f, (fp)2.34567f, (fp)3.45678f, (fp)4.56789f);
            var fixedB = new fpvec4((fp)5.67890f, (fp)1.12345f, (fp)7.89012f, (fp)3.34567f);

            var floatResult = Vector4.Min(floatA, floatB);
            var fixedResult = fpvec4.Min(fixedA, fixedB);

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void MaxTest()
        {
            var floatA = new Vector4(1.23456f, 2.34567f, 3.45678f, 4.56789f);
            var floatB = new Vector4(5.67890f, 1.12345f, 7.89012f, 3.34567f);

            var fixedA = new fpvec4((fp)1.23456f, (fp)2.34567f, (fp)3.45678f, (fp)4.56789f);
            var fixedB = new fpvec4((fp)5.67890f, (fp)1.12345f, (fp)7.89012f, (fp)3.34567f);

            var floatResult = Vector4.Max(floatA, floatB);
            var fixedResult = fpvec4.Max(fixedA, fixedB);

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void LerpUnclampedTest()
        {
            var floatFrom = new Vector4(1.123456f, 2.234567f, 3.345678f, 4.456789f);
            var floatTo = new Vector4(5.567890f, 6.678901f, 7.789012f, 8.890123f);
            float t = 1.34567f;

            var fixedFrom = new fpvec4((fp)1.123456f, (fp)2.234567f, (fp)3.345678f, (fp)4.456789f);
            var fixedTo = new fpvec4((fp)5.567890f, (fp)6.678901f, (fp)7.789012f, (fp)8.890123f);
            fp fixedT = (fp)t;

            var floatResult = Vector4.LerpUnclamped(floatFrom, floatTo, t);
            var fixedResult = fpvec4.LerpUnclamped(fixedFrom, fixedTo, fixedT);

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
            Assert.AreEqual(floatResult.w, (float)fixedResult.w, Epsilon);
        }

        [Test]
        public void EqualsTest()
        {
            var floatA = new Vector4(1.23456f, 2.34567f, 3.45678f, 4.56789f);
            var floatB = new Vector4(1.23456f, 2.34567f, 3.45678f, 4.56789f);
            var floatC = new Vector4(5.67890f, 6.67890f, 7.78901f, 8.89012f);

            var fixedA = new fpvec4((fp)1.23456f, (fp)2.34567f, (fp)3.45678f, (fp)4.56789f);
            var fixedB = new fpvec4((fp)1.23456f, (fp)2.34567f, (fp)3.45678f, (fp)4.56789f);
            var fixedC = new fpvec4((fp)5.67890f, (fp)6.67890f, (fp)7.78901f, (fp)8.89012f);

            Assert.IsTrue(floatA.Equals(floatB));
            Assert.IsTrue(fixedA.Equals(fixedB));
            Assert.IsFalse(floatA.Equals(floatC));
            Assert.IsFalse(fixedA.Equals(fixedC));
        }
    }
}
