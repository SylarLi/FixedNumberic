using UnityEngine;
using NUnit.Framework;
using System;

namespace Fixed.Numeric.Editor.Test
{
    public class fpvec3Tests
    {
        private const float Epsilon = 0.0001f;

        [Test]
        public void Zero_IsCorrect()
        {
            Assert.AreEqual(Vector3.zero.x, (float)fpvec3.zero.x, Epsilon);
            Assert.AreEqual(Vector3.zero.y, (float)fpvec3.zero.y, Epsilon);
            Assert.AreEqual(Vector3.zero.z, (float)fpvec3.zero.z, Epsilon);
        }

        [Test]
        public void One_IsCorrect()
        {
            Assert.AreEqual(Vector3.one.x, (float)fpvec3.one.x, Epsilon);
            Assert.AreEqual(Vector3.one.y, (float)fpvec3.one.y, Epsilon);
            Assert.AreEqual(Vector3.one.z, (float)fpvec3.one.z, Epsilon);
        }

        [Test]
        public void Constructor_Works()
        {
            var floatVec = new Vector3(1.5235f, 2.5789f, 3.4567f);
            var fixedVec = new fpvec3((fp)1.5235f, (fp)2.5789f, (fp)3.4567f);
            Assert.AreEqual(floatVec.x, (float)fixedVec.x, Epsilon);
            Assert.AreEqual(floatVec.y, (float)fixedVec.y, Epsilon);
            Assert.AreEqual(floatVec.z, (float)fixedVec.z, Epsilon);
        }

        [Test]
        public void Indexer_Works()
        {
            var floatVec = new Vector3(1.5235f, 2.5789f, 3.4567f);
            var fixedVec = new fpvec3((fp)1.5235f, (fp)2.5789f, (fp)3.4567f);
            Assert.AreEqual(floatVec[0], (float)fixedVec[0], Epsilon);
            Assert.AreEqual(floatVec[1], (float)fixedVec[1], Epsilon);
            Assert.AreEqual(floatVec[2], (float)fixedVec[2], Epsilon);
            floatVec[0] = 3.5678f;
            floatVec[1] = 4.5432f;
            floatVec[2] = 5.6789f;
            fixedVec[0] = (fp)3.5678f;
            fixedVec[1] = (fp)4.5432f;
            fixedVec[2] = (fp)5.6789f;
            Assert.AreEqual(floatVec[0], (float)fixedVec[0], Epsilon);
            Assert.AreEqual(floatVec[1], (float)fixedVec[1], Epsilon);
            Assert.AreEqual(floatVec[2], (float)fixedVec[2], Epsilon);

            Assert.Throws<IndexOutOfRangeException>(() => { var _ = fixedVec[3]; });
            Assert.Throws<IndexOutOfRangeException>(() => { fixedVec[3] = (fp)1.0f; });
        }

        [Test]
        public void Set_Works()
        {
            var floatVec = new Vector3();
            var fixedVec = new fpvec3();
            floatVec.Set(1, 2, 3);
            fixedVec.Set((fp)1, (fp)2, (fp)3);
            Assert.AreEqual(floatVec.x, (float)fixedVec.x, Epsilon);
            Assert.AreEqual(floatVec.y, (float)fixedVec.y, Epsilon);
            Assert.AreEqual(floatVec.z, (float)fixedVec.z, Epsilon);
        }

        [Test]
        public void Scale_Works()
        {
            var floatVec = new Vector3(2, 3, 4);
            var floatScale = new Vector3(4, 5, 6);
            var fixedVec = new fpvec3((fp)2, (fp)3, (fp)4);
            var fixedScale = new fpvec3((fp)4, (fp)5, (fp)6);
            floatVec.Scale(floatScale);
            fixedVec.Scale(fixedScale);
            Assert.AreEqual(floatVec.x, (float)fixedVec.x, Epsilon);
            Assert.AreEqual(floatVec.y, (float)fixedVec.y, Epsilon);
            Assert.AreEqual(floatVec.z, (float)fixedVec.z, Epsilon);
        }

        [Test]
        public void MagnitudeTest()
        {
            var floatVec = new Vector3(3.1415f, 4.2678f, 5.3246f);
            var fixedVec = new fpvec3((fp)3.1415f, (fp)4.2678f, (fp)5.3246f);
            Assert.AreEqual(floatVec.magnitude, (float)fixedVec.magnitude, Epsilon);
        }

        [Test]
        public void NormalizeTest()
        {
            var floatVec = new Vector3(3.1415f, 4.2678f, 5.3246f);
            var fixedVec = new fpvec3((fp)3.1415f, (fp)4.2678f, (fp)5.3246f);

            var floatNormalized = floatVec.normalized;
            var fixedNormalized = fixedVec.normalized;

            Assert.AreEqual(floatNormalized.x, (float)fixedNormalized.x, Epsilon);
            Assert.AreEqual(floatNormalized.y, (float)fixedNormalized.y, Epsilon);
            Assert.AreEqual(floatNormalized.z, (float)fixedNormalized.z, Epsilon);
        }

        [Test]
        public void DotProductTest()
        {
            var floatA = new Vector3(1.2345f, 2.3456f, 3.4567f);
            var floatB = new Vector3(4.5678f, 5.6789f, 6.7890f);
            var fixedA = new fpvec3((fp)1.2345f, (fp)2.3456f, (fp)3.4567f);
            var fixedB = new fpvec3((fp)4.5678f, (fp)5.6789f, (fp)6.7890f);

            var floatDot = Vector3.Dot(floatA, floatB);
            var fixedDot = fpvec3.Dot(fixedA, fixedB);
            Assert.AreEqual(floatDot, (float)fixedDot, Epsilon);
        }

        [Test]
        public void CrossProductTest()
        {
            var floatA = new Vector3(1.2345f, 2.3456f, 3.4567f);
            var floatB = new Vector3(4.5678f, 5.6789f, 6.7890f);
            var fixedA = new fpvec3((fp)1.2345f, (fp)2.3456f, (fp)3.4567f);
            var fixedB = new fpvec3((fp)4.5678f, (fp)5.6789f, (fp)6.7890f);

            var floatCross = Vector3.Cross(floatA, floatB);
            var fixedCross = fpvec3.Cross(fixedA, fixedB);

            Assert.AreEqual(floatCross.x, (float)fixedCross.x, Epsilon);
            Assert.AreEqual(floatCross.y, (float)fixedCross.y, Epsilon);
            Assert.AreEqual(floatCross.z, (float)fixedCross.z, Epsilon);
        }

        [Test]
        public void AdditionTest()
        {
            var floatA = new Vector3(1.2345f, 2.3456f, 3.4567f);
            var floatB = new Vector3(4.5678f, 5.6789f, 6.7890f);
            var fixedA = new fpvec3((fp)1.2345f, (fp)2.3456f, (fp)3.4567f);
            var fixedB = new fpvec3((fp)4.5678f, (fp)5.6789f, (fp)6.7890f);

            var floatResult = floatA + floatB;
            var fixedResult = fixedA + fixedB;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
        }

        [Test]
        public void SubtractionTest()
        {
            var floatA = new Vector3(4.5678f, 5.6789f, 6.7890f);
            var floatB = new Vector3(1.2345f, 2.3456f, 3.4567f);
            var fixedA = new fpvec3((fp)4.5678f, (fp)5.6789f, (fp)6.7890f);
            var fixedB = new fpvec3((fp)1.2345f, (fp)2.3456f, (fp)3.4567f);

            var floatResult = floatA - floatB;
            var fixedResult = fixedA - fixedB;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
        }

        [Test]
        public void MultiplicationTest()
        {
            var floatVec = new Vector3(1.2345f, 2.3456f, 3.4567f);
            var fixedVec = new fpvec3((fp)1.2345f, (fp)2.3456f, (fp)3.4567f);
            var scalar = 2.5f;

            var floatResult = floatVec * scalar;
            var fixedResult = fixedVec * (fp)scalar;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
        }

        [Test]
        public void DivisionTest()
        {
            var floatVec = new Vector3(3.08625f, 5.864f, 8.64175f);
            var fixedVec = new fpvec3((fp)3.08625f, (fp)5.864f, (fp)8.64175f);
            var scalar = 2.5f;

            var floatResult = floatVec / scalar;
            var fixedResult = fixedVec / (fp)scalar;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
        }

        [Test]
        public void NegationTest()
        {
            var floatVec = new Vector3(1.2345f, 2.3456f, 3.4567f);
            var fixedVec = new fpvec3((fp)1.2345f, (fp)2.3456f, (fp)3.4567f);

            var floatResult = -floatVec;
            var fixedResult = -fixedVec;

            Assert.AreEqual(floatResult.x, (float)fixedResult.x, Epsilon);
            Assert.AreEqual(floatResult.y, (float)fixedResult.y, Epsilon);
            Assert.AreEqual(floatResult.z, (float)fixedResult.z, Epsilon);
        }

        [Test]
        public void EqualityTest()
        {
            var floatA = new Vector3(1.2345f, 2.3456f, 3.4567f);
            var floatB = new Vector3(1.2345f, 2.3456f, 3.4567f);
            var floatC = new Vector3(4.5678f, 5.6789f, 6.7890f);

            var fixedA = new fpvec3((fp)1.2345f, (fp)2.3456f, (fp)3.4567f);
            var fixedB = new fpvec3((fp)1.2345f, (fp)2.3456f, (fp)3.4567f);
            var fixedC = new fpvec3((fp)4.5678f, (fp)5.6789f, (fp)6.7890f);

            Assert.AreEqual(floatA == floatB, fixedA == fixedB);
            Assert.AreEqual(floatA == floatC, fixedA == fixedC);
            Assert.AreEqual(floatA != floatB, fixedA != fixedB);
            Assert.AreEqual(floatA != floatC, fixedA != fixedC);
        }
    }
}
