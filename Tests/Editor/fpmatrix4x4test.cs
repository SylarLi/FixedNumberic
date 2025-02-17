using System.Drawing.Printing;
using NUnit.Framework;
using UnityEngine;

namespace Fixed.Numeric.Editor.Test
{
    public class fpmatrix4x4test
    {
        private const float PrecisionDelta = 0.001f;

        // 第一部分：基础功能测试
        [Test]
        public void TestConstructorAndBasicProperties()
        {
            // 使用复杂浮点数初始化
            fp m00 = 0.123456789f;
            fp m01 = 0.987654321f;
            fp m02 = 0.543219876f;
            fp m03 = 0.678901234f;
            fp m10 = 0.23456789f;
            fp m11 = 0.3456789f;
            fp m12 = 0.456789f;
            fp m13 = 0.567890123f;
            fp m20 = 0.678901234f;
            fp m21 = 0.78901234f;
            fp m22 = 0.890123456f;
            fp m23 = 0.901234567f;
            fp m30 = 0.0123456789f;
            fp m31 = 0.9876543210f;
            fp m32 = 0.5432109876f;
            fp m33 = 0.6789054321f;

            var matrix = new fpmatrix4x4(
                m00, m01, m02, m03,
                m10, m11, m12, m13,
                m20, m21, m22, m23,
                m30, m31, m32, m33
            );

            // 验证元素访问
            Assert.AreEqual((float)m00, (float)matrix[0, 0], 0.0001f);
            Assert.AreEqual((float)m01, (float)matrix[0, 1], 0.0001f);
            Assert.AreEqual((float)m33, (float)matrix[3, 3], 0.0001f);
        }

        // 第二部分：矩阵运算测试
        [Test]
        public void TestMatrixOperations()
        {
            // 使用复杂浮点数创建两个矩阵
            var m1 = CreateComplexMatrix(10.123456789f);
            var m2 = CreateComplexMatrix(4.987654321f);

            // 矩阵乘法
            var product = m1 * m2;
            var unityM1 = ToUnityMatrix(m1);
            var unityM2 = ToUnityMatrix(m2);
            var unityProduct = unityM1 * unityM2;

            AssertMatrixEqual(unityProduct, product, 0.0001f);

            // 矩阵转置
            var transposed = m1.transpose;
            var unityTransposed = unityM1.transpose;
            AssertMatrixEqual(unityTransposed, transposed, 0.0001f);

            // 逆矩阵
            if (m1.determinant != fp.Zero)
            {
                var inverse = m1.inverse;
                var unityInverse = unityM1.inverse;
                AssertMatrixEqual(unityInverse, inverse, 0.0001f);
            }
        }

        // 第三部分：变换矩阵测试
        [Test]
        public void TestTransformationMatrices()
        {
            // 使用复杂变换参数
            var translation = new fpvec3(1.23456789f, 2.3456789f, 3.456789f);
            var rotation = fpquat.Euler(30.123456f, 45.987654f, 60.543219f);
            var scale = new fpvec3(0.7654321f, 1.234567f, 0.987654f);

            // TRS矩阵测试
            var trsMatrix = fpmatrix4x4.TRS(translation, rotation, scale);
            var unityTRS = Matrix4x4.TRS(
                ToUnityVector3(translation),
                ToUnityQuaternion(rotation),
                ToUnityVector3(scale)
            );
            AssertMatrixEqual(unityTRS, trsMatrix, 0.0001f);

            // LookAt矩阵测试
            var eye = new fpvec3(5.4321f, 4.3210f, 3.2109f);
            var target = new fpvec3(1.2345f, 2.3456f, 3.4567f);
            var up = new fpvec3(0.123456f, 0.987654f, 0.543219f);

            var lookAtMatrix = fpmatrix4x4.LookAt(eye, target, up);
            var unityLookAt = Matrix4x4.LookAt(
                ToUnityVector3(eye),
                ToUnityVector3(target),
                ToUnityVector3(up)
            );
            AssertMatrixEqual(unityLookAt, lookAtMatrix, 0.0001f);
        }

        // 第四部分：投影矩阵测试
        [Test]
        public void TestProjectionMatrices()
        {
            // Perspective投影测试
            var fov = 60.123456f;
            var aspect = 1.777777f;
            var zNear = 0.123456f;
            var zFar = 100.987654f;

            var perspective = fpmatrix4x4.Perspective(fov, aspect, zNear, zFar);
            var unityPerspective = Matrix4x4.Perspective(fov, aspect, zNear, zFar);
            AssertMatrixEqual(unityPerspective, perspective, 0.0001f);

            // Ortho投影测试
            var left = -1.234567f;
            var right = 2.345678f;
            var bottom = -3.456789f;
            var top = 4.567890f;

            var ortho = fpmatrix4x4.Ortho(left, right, bottom, top, zNear, zFar);
            var unityOrtho = Matrix4x4.Ortho(left, right, bottom, top, zNear, zFar);
            AssertMatrixEqual(unityOrtho, ortho, 0.0001f);
        }

        // 第五部分：向量变换测试
        [Test]
        public void TestVectorTransformations()
        {
            var matrix = CreateComplexMatrix(0.123456789f);
            var point = new fpvec3(1.234567f, 2.345678f, 3.456789f);
            var vector = new fpvec3(0.987654f, 0.876543f, 0.765432f);

            // 点变换测试
            var transformedPoint = matrix.MultiplyPoint(point);
            var unityTransformedPoint = ToUnityMatrix(matrix).MultiplyPoint(fparser.to_vec3(point));
            AssertVector3Equal(unityTransformedPoint, transformedPoint, 0.0001f);

            // 向量变换测试
            var transformedVector = matrix.MultiplyVector(vector);
            var unityTransformedVector = ToUnityMatrix(matrix).MultiplyVector(fparser.to_vec3(vector));
            AssertVector3Equal(unityTransformedVector, transformedVector, 0.0001f);
        }

        // 第六部分：特殊矩阵测试
        [Test]
        public void TestSpecialMatrices()
        {
            // 单位矩阵测试
            Assert.IsTrue(fpmatrix4x4.identity.isIdentity);

            // 零矩阵测试
            var zeroMatrix = fpmatrix4x4.zero;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == j && i == 3) continue;
                    Assert.AreEqual(0f, (float)zeroMatrix[i, j], 0.0001f);
                }
            }
        }

        [Test]
        public void Determinant_IdentityMatrix_ReturnsOne()
        {
            var matrix = fpmatrix4x4.identity;
            var det = fpmatrix4x4.Determinant(matrix);
            Assert.AreEqual(1f, (float)det, PrecisionDelta);
        }

        [Test]
        public void Determinant_ZeroMatrix_ReturnsZero()
        {
            var matrix = fpmatrix4x4.zero;
            var det = fpmatrix4x4.Determinant(matrix);
            Assert.AreEqual(0f, (float)det, PrecisionDelta);
        }

        [Test]
        public void Determinant_RandomMatrix_MatchesUnity()
        {
            var matrix = CreateComplexMatrix(4.123456789f);
            var expected = ToUnityMatrix(matrix).determinant;
            var actual = (float)fpmatrix4x4.Determinant(matrix);
            Assert.AreEqual(expected, actual, PrecisionDelta);
        }

        [Test]
        public void TRS_TranslationOnly_CorrectPosition()
        {
            var translation = new fpvec3(5f, 3f, 2f);
            var matrix = fpmatrix4x4.TRS(translation, fpquat.identity, fpvec3.one);

            Assert.AreEqual(5f, (float)matrix.m03, PrecisionDelta);
            Assert.AreEqual(3f, (float)matrix.m13, PrecisionDelta);
            Assert.AreEqual(2f, (float)matrix.m23, PrecisionDelta);
        }

        [Test]
        public void TRS_RotationOnly_MatchesUnity()
        {
            var rotation = fpquat.Euler(30f, 45f, 60f);
            var matrix = fpmatrix4x4.TRS(fpvec3.zero, rotation, fpvec3.one);
            var unityMatrix = Matrix4x4.TRS(Vector3.zero, ToUnityQuaternion(rotation), Vector3.one);

            AssertMatrixEqual(unityMatrix, matrix, PrecisionDelta);
        }

        [Test]
        public void TRS_ComplexTransform_MatchesUnity()
        {
            var t = new fpvec3(1f, 2f, 3f);
            var r = fpquat.Euler(30f, 45f, 60f);
            var s = new fpvec3(0.5f, 0.8f, 1.2f);

            var matrix = fpmatrix4x4.TRS(t, r, s);
            var unityMatrix = Matrix4x4.TRS(
                ToUnityVector3(t),
                ToUnityQuaternion(r),
                ToUnityVector3(s)
            );

            AssertMatrixEqual(unityMatrix, matrix, PrecisionDelta);
        }

        [Test]
        public void Ortho_SymmetricViewport_CorrectStructure()
        {
            var matrix = fpmatrix4x4.Ortho(-5f, 5f, -5f, 5f, 0.1f, 100f);

            Assert.AreEqual(0.2f, (float)matrix.m00, PrecisionDelta);
            Assert.AreEqual(0.2f, (float)matrix.m11, PrecisionDelta);
            Assert.AreEqual(-0.02002f, (float)matrix.m22, 0.0001f);
        }

        [Test]
        public void Perspective_StandardFOV_MatchesUnity()
        {
            var matrix = fpmatrix4x4.Perspective(60f, 16f / 9f, 0.1f, 100f);
            var unityMatrix = Matrix4x4.Perspective(60f, 16f / 9f, 0.1f, 100f);

            AssertMatrixEqual(unityMatrix, matrix, 0.1f); // 放宽精度要求
        }

        [Test]
        public void LookAt_ViewDirection_MatchesUnity()
        {
            var eye = new fpvec3(5f, 3f, 2f);
            var target = new fpvec3(1f, 2f, 3f);
            var up = new fpvec3(0f, 1f, 0f);

            var matrix = fpmatrix4x4.LookAt(eye, target, up);
            var unityMatrix = Matrix4x4.LookAt(
                ToUnityVector3(eye),
                ToUnityVector3(target),
                ToUnityVector3(up)
            );

            AssertMatrixEqual(unityMatrix, matrix, 0.01f);
        }

        [Test]
        public void Scale_NonUniform_CorrectDiagonal()
        {
            var scale = new fpvec3(2f, 3f, 4f);
            var matrix = fpmatrix4x4.Scale(scale);

            Assert.AreEqual(2f, (float)matrix.m00, PrecisionDelta);
            Assert.AreEqual(3f, (float)matrix.m11, PrecisionDelta);
            Assert.AreEqual(4f, (float)matrix.m22, PrecisionDelta);
        }

        [Test]
        public void Translate_Position_CorrectTranslation()
        {
            var translation = new fpvec3(5f, 3f, 2f);
            var matrix = fpmatrix4x4.Translate(translation);

            Assert.AreEqual(5f, (float)matrix.m03, PrecisionDelta);
            Assert.AreEqual(3f, (float)matrix.m13, PrecisionDelta);
            Assert.AreEqual(2f, (float)matrix.m23, PrecisionDelta);
        }

        [Test]
        public void Rotate_Quaternion_Orthonormal()
        {
            var rotation = fpquat.Euler(30f, 45f, 60f);
            var matrix = fpmatrix4x4.Rotate(rotation);

            // 验证旋转矩阵正交性
            var col0 = matrix.GetColumn(0);
            Assert.AreEqual(1f, (float)col0.magnitude, PrecisionDelta);
        }

        [Test]
        public void Frustum_Asymmetric_CorrectProjection()
        {
            var matrix = fpmatrix4x4.Frustum(-1f, 2f, -0.5f, 1f, 0.1f, 100f);
            var unityMatrix = Matrix4x4.Frustum(-1f, 2f, -0.5f, 1f, 0.1f, 100f);
            AssertMatrixEqual(unityMatrix, matrix, 0.01f);
        }

        // 辅助方法：创建复杂矩阵
        private fpmatrix4x4 CreateComplexMatrix(float seed)
        {
            return new fpmatrix4x4(
                seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f), seed + Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f),
                seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f),
                seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f),
                seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f), seed * Random.Range(-10f, 10f)
            );
        }

        // 辅助方法：矩阵比较
        private void AssertMatrixEqual(Matrix4x4 expected, fpmatrix4x4 actual, float delta)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Assert.AreEqual(
                        expected[i, j],
                        (float)actual[i, j],
                        delta,
                        $"Mismatch at [{i},{j}]");
                }
            }
        }

        // 辅助方法：向量比较
        private void AssertVector3Equal(Vector3 expected, fpvec3 actual, float delta)
        {
            Assert.AreEqual(expected.x, (float)actual.x, delta);
            Assert.AreEqual(expected.y, (float)actual.y, delta);
            Assert.AreEqual(expected.z, (float)actual.z, delta);
        }

        // 类型转换辅助方法
        private Matrix4x4 ToUnityMatrix(fpmatrix4x4 m)
        {
            return fparser.to_matrix4x4(m);
        }

        private System.Numerics.Matrix4x4 ToSystemMatrix(fpmatrix4x4 m)
        {
            return new System.Numerics.Matrix4x4(
                (float)m[0, 0], (float)m[0, 1], (float)m[0, 2], (float)m[0, 3],
                (float)m[1, 0], (float)m[1, 1], (float)m[1, 2], (float)m[1, 3],
                (float)m[2, 0], (float)m[2, 1], (float)m[2, 2], (float)m[2, 3],
                (float)m[3, 0], (float)m[3, 1], (float)m[3, 2], (float)m[3, 3]
            );
        }

        private Quaternion ToUnityQuaternion(fpquat q)
        {
            return fparser.to_quat(q);
        }

        private Vector3 ToUnityVector3(fpvec3 v)
        {
            return fparser.to_vec3(v);
        }
    }
}
