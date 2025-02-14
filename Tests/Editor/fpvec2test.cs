using UnityEngine;
using NUnit.Framework;

namespace Fixed.Numeric.Editor.Test
{
    public class fpvec2Tests
    {
        [Test]
        public void Zero_IsCorrect()
        {
            Assert.AreEqual(0f, (float)fpvec2.zero.x, 0.0001f);
            Assert.AreEqual(0f, (float)fpvec2.zero.y, 0.0001f);
        }

        [Test]
        public void One_IsCorrect()
        {
            Assert.AreEqual(1f, (float)fpvec2.one.x, 0.0001f);
            Assert.AreEqual(1f, (float)fpvec2.one.y, 0.0001f);
        }

        [Test]
        public void Constructor_Works()
        {
            var testCases = new[] {
                new Vector2(1.23456f, -7.89012f),
                new Vector2(-3.14159f, 2.71828f),
                new Vector2(0.577216f, 1.414213f),
                new Vector2(123.456f, -789.012f)
        };

            foreach (var v in testCases)
            {
                var fpv = new fpvec2((fp)v.x, (fp)v.y);
                Assert.AreEqual(v.x, (float)fpv.x, 0.0001f);
                Assert.AreEqual(v.y, (float)fpv.y, 0.0001f);
            }
        }

        [Test]
        public void Indexer_Works()
        {
            var fpv = new fpvec2((fp)1.23456f, (fp)(-7.89012f));
            Assert.AreEqual((fp)1.23456f, fpv[0]);
            Assert.AreEqual((fp)(-7.89012f), fpv[1]);
        }

        [Test]
        public void SqrMagnitude_IsCorrect()
        {
            var testCases = new[] {
                new Vector2(3.14159f, 2.71828f),
                new Vector2(-5.55555f, 6.66666f),
                new Vector2(0.123456f, -0.789012f)
            };

            foreach (var v in testCases)
            {
                var fpv = new fpvec2((fp)v.x, (fp)v.y);
                Assert.AreEqual(v.sqrMagnitude, (float)fpv.sqrMagnitude, 0.001f);
            }
        }

        [Test]
        public void Magnitude_IsCorrect()
        {
            var testCases = new[] {
                new Vector2(3.14159f, 2.71828f),
                new Vector2(-5.55555f, 6.66666f),
                new Vector2(0.123456f, -0.789012f)
            };

            foreach (var v in testCases)
            {
                var fpv = new fpvec2((fp)v.x, (fp)v.y);
                Assert.AreEqual(v.magnitude, (float)fpv.magnitude, 0.001f);
            }
        }

        [Test]
        public void Normalized_IsCorrect()
        {
            var testCases = new[] {
                new Vector2(3.14159f, 2.71828f),
                new Vector2(-5.55555f, 6.66666f),
                new Vector2(0.123456f, -0.789012f)
            };

            foreach (var v in testCases)
            {
                var fpv = new fpvec2((fp)v.x, (fp)v.y);
                var normalized = fpv.normalized;
                Assert.AreEqual(v.normalized.x, (float)normalized.x, 0.001f);
                Assert.AreEqual(v.normalized.y, (float)normalized.y, 0.001f);
            }
        }

        [Test]
        public void Normalize_Works()
        {
            var v = new Vector2(3.14159f, 2.71828f);
            var fpv = new fpvec2((fp)v.x, (fp)v.y);

            v.Normalize();
            fpv.Normalize();
            Assert.AreEqual(v.x, (float)fpv.x, 0.001f);
            Assert.AreEqual(v.y, (float)fpv.y, 0.001f);
        }

        [Test]
        public void Set_Works()
        {
            var fpv = new fpvec2();
            fpv.Set((fp)3.14159f, (fp)2.71828f);
            Assert.AreEqual(3.14159f, (float)fpv.x, 0.0001f);
            Assert.AreEqual(2.71828f, (float)fpv.y, 0.0001f);
        }

        [Test]
        public void Scale_Works()
        {
            var v = new Vector2(3.14159f, 2.71828f);
            var scale = new Vector2(1.414213f, 0.577216f);
            var fpv = new fpvec2((fp)v.x, (fp)v.y);
            var fpScale = new fpvec2((fp)scale.x, (fp)scale.y);

            Vector2 vScaled = Vector2.Scale(v, scale);
            fpvec2 fpvScaled = fpvec2.Scale(fpv, fpScale);
            Assert.AreEqual(vScaled.x, (float)fpvScaled.x, 0.001f);
            Assert.AreEqual(vScaled.y, (float)fpvScaled.y, 0.001f);
        }

        [Test]
        public void Dot_Works()
        {
            var v1 = new Vector2(3.14159f, 2.71828f);
            var v2 = new Vector2(1.414213f, 0.577216f);
            var fpv1 = new fpvec2((fp)v1.x, (fp)v1.y);
            var fpv2 = new fpvec2((fp)v2.x, (fp)v2.y);

            Assert.AreEqual(Vector2.Dot(v1, v2), (float)fpvec2.Dot(fpv1, fpv2), 0.001f);
        }

        [Test]
        public void Distance_Works()
        {
            var v1 = new Vector2(3.14159f, 2.71828f);
            var v2 = new Vector2(1.414213f, 0.577216f);
            var fpv1 = new fpvec2((fp)v1.x, (fp)v1.y);
            var fpv2 = new fpvec2((fp)v2.x, (fp)v2.y);

            Assert.AreEqual(Vector2.Distance(v1, v2), (float)fpvec2.Distance(fpv1, fpv2), 0.001f);
        }

        [Test]
        public void Lerp_Works()
        {
            var v1 = new Vector2(3.14159f, 2.71828f);
            var v2 = new Vector2(1.414213f, 0.577216f);
            var fpv1 = new fpvec2((fp)v1.x, (fp)v1.y);
            var fpv2 = new fpvec2((fp)v2.x, (fp)v2.y);

            float t = 0.5f;
            Vector2 vLerped = Vector2.Lerp(v1, v2, t);
            fpvec2 fpvLerped = fpvec2.Lerp(fpv1, fpv2, (fp)t);
            Assert.AreEqual(vLerped.x, (float)fpvLerped.x, 0.001f);
            Assert.AreEqual(vLerped.y, (float)fpvLerped.y, 0.001f);
        }
    }
}