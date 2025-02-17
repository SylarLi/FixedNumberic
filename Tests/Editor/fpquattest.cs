using NUnit.Framework;
using UnityEngine;

namespace Fixed.Numeric.Editor.Test
{
    public class fpquattest
    {
        // 测试构造函数
        [Test]
        public void TestConstructor()
        {
            // 使用复杂浮点数进行测试
            fp x = 0.123456789f;
            fp y = 0.987654321f;
            fp z = 0.543219876f;
            fp w = 0.678901234f;

            fpquat quat = new fpquat(x, y, z, w);

            Assert.AreEqual(x, quat.x);
            Assert.AreEqual(y, quat.y);
            Assert.AreEqual(z, quat.z);
            Assert.AreEqual(w, quat.w);
        }

        // 测试乘法
        [Test]
        public void TestMultiply()
        {
            // 使用复杂浮点数进行测试
            fpquat quat1 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat2 = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);

            fpquat result = quat1 * quat2;

            // 这里需要根据fpquat的乘法实现来验证结果
            // 假设fpquat有一个正确的乘法实现
            // 可以使用Unity的Quaternion来验证结果
            Quaternion unityQuat1 = new Quaternion(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            Quaternion unityQuat2 = new Quaternion(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);
            Quaternion unityResult = unityQuat1 * unityQuat2;

            Assert.AreEqual(unityResult.x, (double)result.x, 0.0001f);
            Assert.AreEqual(unityResult.y, (double)result.y, 0.0001f);
            Assert.AreEqual(unityResult.z, (double)result.z, 0.0001f);
            Assert.AreEqual(unityResult.w, (double)result.w, 0.0001f);
        }

        // 测试逆
        [Test]
        public void TestInverse()
        {
            // 使用复杂浮点数进行测试
            fpquat quat = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat inverse = fpquat.Inverse(quat);
            fpquat result = quat * inverse;

            Quaternion quat1 = new Quaternion(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            Quaternion inverse1 = Quaternion.Inverse(quat1);
            Quaternion result1 = quat1 * inverse1;

            Assert.AreEqual(result1.x, (float)result.x, 0.0001f);
            Assert.AreEqual(result1.y, (float)result.y, 0.0001f);
            Assert.AreEqual(result1.z, (float)result.z, 0.0001f);
            Assert.AreEqual(result1.w, (float)result.w, 0.0001f);
        }

        // 测试归一化
        [Test]
        public void TestNormalize()
        {
            // 使用复杂浮点数进行测试
            fpquat quat = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat normalized = quat.normalized;

            Quaternion quat1 = new Quaternion(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            Quaternion normalized1 = quat1.normalized;

            // 验证归一化后的四元数模长接近 1
            var length = fpmath.Sqrt(normalized.x * normalized.x + normalized.y * normalized.y + normalized.z * normalized.z + normalized.w * normalized.w);
            Assert.AreEqual(1f, (float)length, 0.0001f);

            // 验证归一化后的四元数与Unity的归一化结果接近
            Assert.AreEqual(normalized1.x, (float)normalized.x, 0.0001f);
            Assert.AreEqual(normalized1.y, (float)normalized.y, 0.0001f);
            Assert.AreEqual(normalized1.z, (float)normalized.z, 0.0001f);
            Assert.AreEqual(normalized1.w, (float)normalized.w, 0.0001f);
        }

        // 测试点积
        [Test]
        public void TestDot()
        {
            // 使用复杂浮点数进行测试
            fpquat quat1 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat2 = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);

            var dotResult = fpquat.Dot(quat1, quat2);

            // 使用 UnityEngine.Quaternion 验证点积结果
            Quaternion unityQuat1 = new Quaternion((float)quat1.x, (float)quat1.y, (float)quat1.z, (float)quat1.w);
            Quaternion unityQuat2 = new Quaternion((float)quat2.x, (float)quat2.y, (float)quat2.z, (float)quat2.w);
            float unityDotResult = Quaternion.Dot(unityQuat1, unityQuat2);

            Assert.AreEqual(unityDotResult, (float)dotResult, 0.0001f);
        }

        // 测试角度
        [Test]
        public void TestAngle()
        {
            // 使用复杂浮点数进行测试
            fpquat quat1 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat2 = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);

            var angle = fpquat.Angle(quat1, quat2);

            // 使用 UnityEngine.Quaternion 验证角度结果
            Quaternion unityQuat1 = new Quaternion((float)quat1.x, (float)quat1.y, (float)quat1.z, (float)quat1.w);
            Quaternion unityQuat2 = new Quaternion((float)quat2.x, (float)quat2.y, (float)quat2.z, (float)quat2.w);
            float unityAngle = Quaternion.Angle(unityQuat1, unityQuat2);

            Assert.AreEqual(unityAngle, (float)angle, 0.0001f);
        }

        // 测试从欧拉角创建四元数
        [Test]
        public void TestEuler()
        {
            // 使用复杂浮点数作为欧拉角
            float x = 12.3456789f;
            float y = 23.456789f;
            float z = 34.56789f;

            fpquat quat = fpquat.Euler(x, y, z);
            Quaternion unityQuat = Quaternion.Euler(x, y, z);

            Assert.AreEqual(unityQuat.x, (float)quat.x, 0.0001f);
            Assert.AreEqual(unityQuat.y, (float)quat.y, 0.0001f);
            Assert.AreEqual(unityQuat.z, (float)quat.z, 0.0001f);
            Assert.AreEqual(unityQuat.w, (float)quat.w, 0.0001f);
        }

        // 测试四元数插值（Lerp）
        [Test]
        public void TestLerp()
        {
            fpquat quat1 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat2 = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);
            float t = 0.3456789f;

            fpquat result = fpquat.Lerp(quat1, quat2, t);
            Quaternion unityQuat1 = new Quaternion((float)quat1.x, (float)quat1.y, (float)quat1.z, (float)quat1.w);
            Quaternion unityQuat2 = new Quaternion((float)quat2.x, (float)quat2.y, (float)quat2.z, (float)quat2.w);
            Quaternion unityResult = Quaternion.Lerp(unityQuat1, unityQuat2, t);

            Assert.AreEqual((float)unityResult.x, (float)result.x, 0.0001f);
            Assert.AreEqual((float)unityResult.y, (float)result.y, 0.0001f);
            Assert.AreEqual((float)unityResult.z, (float)result.z, 0.0001f);
            Assert.AreEqual((float)unityResult.w, (float)result.w, 0.0001f);
        }

        // 测试四元数到欧拉角的转换
        [Test]
        public void TestToEulerAngles()
        {
            // 使用复杂浮点数创建四元数
            fpquat quat = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);

            fpvec3 euler = quat.eulerAngles;
            Quaternion unityQuat = new Quaternion((float)quat.x, (float)quat.y, (float)quat.z, (float)quat.w);
            Vector3 unityEuler = unityQuat.eulerAngles;

            Assert.AreEqual((float)unityEuler.x, (float)euler.x, 0.0001f);
            Assert.AreEqual((float)unityEuler.y, (float)euler.y, 0.0001f);
            Assert.AreEqual((float)unityEuler.z, (float)euler.z, 0.0001f);
        }

        // 测试四元数的相等性比较
        [Test]
        public void TestEquality()
        {
            fpquat quat1 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat2 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat3 = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);

            Assert.IsTrue(quat1 == quat2);
            Assert.IsFalse(quat1 == quat3);
        }

        // 测试四元数的不相等性比较
        [Test]
        public void TestInequality()
        {
            fpquat quat1 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat2 = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);

            Assert.IsTrue(quat1 != quat2);
        }

        // 测试四元数的旋转向量
        [Test]
        public void TestRotateVector()
        {
            fpquat quat = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpvec3 vec = new fpvec3(0.23456789f, 0.3456789f, 0.456789f);
            fpvec3 rotatedVec = quat * vec;

            Quaternion unityQuat = new Quaternion((float)quat.x, (float)quat.y, (float)quat.z, (float)quat.w);
            Vector3 unityVec = new Vector3((float)vec.x, (float)vec.y, (float)vec.z);
            Vector3 unityRotatedVec = unityQuat * unityVec;

            Assert.AreEqual((float)unityRotatedVec.x, (float)rotatedVec.x, 0.0001f);
            Assert.AreEqual((float)unityRotatedVec.y, (float)rotatedVec.y, 0.0001f);
            Assert.AreEqual((float)unityRotatedVec.z, (float)rotatedVec.z, 0.0001f);
        }

        // 测试四元数的 SetLookRotation 方法
        [Test]
        public void TestSetLookRotation()
        {
            fpquat quat = new fpquat();
            fpvec3 forward = new fpvec3(0.23456789f, 0.3456789f, 0.456789f);
            fpvec3 up = new fpvec3(0.567890123f, 0.678901234f, 0.78901234f);
            
            quat.SetLookRotation(forward, up);

            Quaternion unityQuat = new Quaternion();
            Vector3 unityForward = new Vector3((float)forward.x, (float)forward.y, (float)forward.z);
            Vector3 unityUp = new Vector3((float)up.x, (float)up.y, (float)up.z);
            unityQuat.SetLookRotation(unityForward, unityUp);

            Assert.AreEqual((float)unityQuat.x, (float)quat.x, 0.0001f);
            Assert.AreEqual((float)unityQuat.y, (float)quat.y, 0.0001f);
            Assert.AreEqual((float)unityQuat.z, (float)quat.z, 0.0001f);
            Assert.AreEqual((float)unityQuat.w, (float)quat.w, 0.0001f);
        }

        // 测试四元数的 SetFromToRotation 方法
        [Test]
        public void TestSetFromToRotation()
        {
            fpquat quat = new fpquat();
            fpvec3 from = new fpvec3(0.23456789f, 0.3456789f, 0.456789f);
            fpvec3 to = new fpvec3(0.567890123f, 0.678901234f, 0.78901234f);

            quat.SetFromToRotation(from, to);

            Quaternion unityQuat = new Quaternion();
            Vector3 unityFrom = new Vector3((float)from.x, (float)from.y, (float)from.z);
            Vector3 unityTo = new Vector3((float)to.x, (float)to.y, (float)to.z);
            unityQuat.SetFromToRotation(unityFrom, unityTo);

            Assert.AreEqual((float)unityQuat.x, (float)quat.x, 0.0001f);
            Assert.AreEqual((float)unityQuat.y, (float)quat.y, 0.0001f);
            Assert.AreEqual((float)unityQuat.z, (float)quat.z, 0.0001f);
            Assert.AreEqual((float)unityQuat.w, (float)quat.w, 0.0001f);
        }

        // 测试四元数的 Set 方法
        [Test]
        public void TestSet()
        {
            fpquat quat = new fpquat();
            float x = 0.123456789f;
            float y = 0.987654321f;
            float z = 0.543219876f;
            float w = 0.678901234f;

            quat.Set(x, y, z, w);

            Assert.AreEqual(x, (float)quat.x, 0.0001f);
            Assert.AreEqual(y, (float)quat.y, 0.0001f);
            Assert.AreEqual(z, (float)quat.z, 0.0001f);
            Assert.AreEqual(w, (float)quat.w, 0.0001f);
        }

        // 测试四元数的 Identity 静态属性
        [Test]
        public void TestIdentity()
        {
            fpquat identity = fpquat.identity;
            Quaternion unityIdentity = Quaternion.identity;

            Assert.AreEqual((float)unityIdentity.x, (float)identity.x, 0.0001f);
            Assert.AreEqual((float)unityIdentity.y, (float)identity.y, 0.0001f);
            Assert.AreEqual((float)unityIdentity.z, (float)identity.z, 0.0001f);
            Assert.AreEqual((float)unityIdentity.w, (float)identity.w, 0.0001f);
        }

        // 测试四元数的 AngleAxis 静态方法
        [Test]
        public void TestAngleAxis()
        {
            float angle = 45.12f;
            fpvec3 axis = new fpvec3(5.14, 1, -23.2);

            fpquat quat = fpquat.AngleAxis(angle, axis);
            Quaternion unityQuat = Quaternion.AngleAxis(angle, new Vector3((float)axis.x, (float)axis.y, (float)axis.z));

            Assert.AreEqual((float)unityQuat.x, (float)quat.x, 0.0001f);
            Assert.AreEqual((float)unityQuat.y, (float)quat.y, 0.0001f);
            Assert.AreEqual((float)unityQuat.z, (float)quat.z, 0.0001f);
            Assert.AreEqual((float)unityQuat.w, (float)quat.w, 0.0001f);
        }

        // 测试四元数的 Euler 静态方法（虽然之前有实例相关测试，但这里补充静态方法测试）
        [Test]
        public void TestStaticEuler()
        {
            float x = 12.3456789f;
            float y = 23.456789f;
            float z = 34.56789f;

            fpquat quat = fpquat.Euler(x, y, z);
            Quaternion unityQuat = Quaternion.Euler(x, y, z);

            Assert.AreEqual((float)unityQuat.x, (float)quat.x, 0.0001f);
            Assert.AreEqual((float)unityQuat.y, (float)quat.y, 0.0001f);
            Assert.AreEqual((float)unityQuat.z, (float)quat.z, 0.0001f);
            Assert.AreEqual((float)unityQuat.w, (float)quat.w, 0.0001f);
        }

        // ------------------------ 以下函数由于计算复杂导致误差放大 --------------------------- //

        // 测试四元数的 SlerpUnclamped 静态方法（如果之前未完整覆盖）
        [Test]
        public void TestSlerpUnclamped()
        {
            float t = 1.23456789f;
            fpquat quat1 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat2 = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);
            fpquat result = fpquat.SlerpUnclamped(quat1, quat2, t);
            var unityQuat1 = new Quaternion((float)quat1.x, (float)quat1.y, (float)quat1.z, (float)quat1.w);
            var unityQuat2 = new Quaternion((float)quat2.x, (float)quat2.y, (float)quat2.z, (float)quat2.w);
            var unityResult = Quaternion.SlerpUnclamped(unityQuat1, unityQuat2, t);

            Assert.AreEqual((float)unityResult.x, (float)result.x, 0.01f);
            Assert.AreEqual((float)unityResult.y, (float)result.y, 0.01f);
            Assert.AreEqual((float)unityResult.z, (float)result.z, 0.01f);
            Assert.AreEqual((float)unityResult.w, (float)result.w, 0.01f);
        }

        // 测试四元数的 Slerp 静态方法（如果之前未完整覆盖）
        [Test]
        public void TestSlerp()
        {
            float t = 0.23456789f;
            fpquat quat1 = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat quat2 = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);
            fpquat result = fpquat.Slerp(quat1, quat2, t);
            var unityQuat1 = new Quaternion((float)quat1.x, (float)quat1.y, (float)quat1.z, (float)quat1.w);
            var unityQuat2 = new Quaternion((float)quat2.x, (float)quat2.y, (float)quat2.z, (float)quat2.w);
            var unityResult = Quaternion.Slerp(unityQuat1, unityQuat2, t);

            Assert.AreEqual((float)unityResult.x, (float)result.x, 0.01f);
            Assert.AreEqual((float)unityResult.y, (float)result.y, 0.01f);
            Assert.AreEqual((float)unityResult.z, (float)result.z, 0.01f);
            Assert.AreEqual((float)unityResult.w, (float)result.w, 0.01f);
        }

        // 测试四元数的 RotateTowards 静态方法
        [Test]
        public void TestRotateTowards()
        {
            fpquat fromQuat = new fpquat(0.123456789f, 0.987654321f, 0.543219876f, 0.678901234f);
            fpquat toQuat = new fpquat(0.987654321f, 0.123456789f, 0.678901234f, 0.543219876f);
            float maxDegreesDelta = 12.345f;
            fpquat result = fpquat.RotateTowards(fromQuat, toQuat, maxDegreesDelta);

            Quaternion unityFromQuat = new Quaternion((float)fromQuat.x, (float)fromQuat.y, (float)fromQuat.z, (float)fromQuat.w);
            Quaternion unityToQuat = new Quaternion((float)toQuat.x, (float)toQuat.y, (float)toQuat.z, (float)toQuat.w);
            Quaternion unityResult = Quaternion.RotateTowards(unityFromQuat, unityToQuat, maxDegreesDelta);

            Assert.AreEqual((float)unityResult.x, (float)result.x, 0.01f);
            Assert.AreEqual((float)unityResult.y, (float)result.y, 0.01f);
            Assert.AreEqual((float)unityResult.z, (float)result.z, 0.01f);
            Assert.AreEqual((float)unityResult.w, (float)result.w, 0.01f);
        }
    }
}