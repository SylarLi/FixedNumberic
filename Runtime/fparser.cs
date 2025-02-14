using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fixed.Numeric
{
    public static class fparser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 to_fpvec2(Vector2Int v)
        {
            return new fpvec2(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec2 to_fpvec2(Vector2 v)
        {
            return new fpvec2(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 to_fpvec3(Vector3Int v)
        {
            return new fpvec3(v.x, v.y, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec3 to_fpvec3(Vector3 v)
        {
            return new fpvec3(v.x, v.y, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpvec4 to_fpvec4(Vector4 v)
        {
            return new fpvec4(v.x, v.y, v.z, v.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpquat to_fpquat(Quaternion q)
        {
            return new fpquat(q.x, q.y, q.z, q.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fpmatrix4x4 to_fpmatrix4x4(Matrix4x4 m)
        {
            return new fpmatrix4x4(m.m00, m.m01, m.m02, m.m03, m.m10, m.m11, m.m12, m.m13, m.m20, m.m21, m.m22, m.m23,
                m.m30, m.m31, m.m32, m.m33);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int to_vec2int(fpvec2 v)
        {
            return new Vector2Int((int) v.x, (int) v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int to_vec3int(fpvec3 v)
        {
            return new Vector3Int((int) v.x, (int) v.y, (int) v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 to_vec2(fpvec2 v)
        {
            return new Vector2((float) v.x, (float) v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 to_vec3(fpvec3 v)
        {
            return new Vector3((float) v.x, (float) v.y, (float) v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 to_vec4(fpvec4 v)
        {
            return new Vector4((float) v.x, (float) v.y, (float) v.z, (float) v.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion to_quat(fpquat quat)
        {
            return new Quaternion((float) quat.x, (float) quat.y, (float) quat.z, (float) quat.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 to_matrix4x4(fpmatrix4x4 m)
        {
            return new Matrix4x4(to_vec4(m.GetColumn(0)), to_vec4(m.GetColumn(1)), to_vec4(m.GetColumn(2)),
                to_vec4(m.GetColumn(3)));
        }
    }
}