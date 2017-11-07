using System;

namespace Framework.Maths
{
    public partial struct Matrix4
    {
        public static bool operator ==(Matrix4 lhs, Matrix4 rhs)
        {
            return Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Matrix4 lhs, Matrix4 rhs)
        {
            return !Object.Equals(lhs, rhs);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }
            Matrix4 rhs = (Matrix4)obj;
            if (_m[0, 0] != rhs._m[0, 0] || _m[0, 1] != rhs._m[0, 1] || 
                _m[0, 2] != rhs._m[0, 2] || _m[0, 3] != rhs._m[0, 3])
            {
                return false;
            }
            if (_m[1, 0] != rhs._m[1, 0] || _m[1, 1] != rhs._m[1, 1] || 
                _m[1, 2] != rhs._m[1, 2] || _m[1, 3] != rhs._m[1, 3])
            {
                return false;
            }
            if (_m[2, 0] != rhs._m[2, 0] || _m[2, 1] != rhs._m[2, 1] || 
                _m[2, 2] != rhs._m[2, 2] || _m[2, 3] != rhs._m[0, 3])
            {
                return false;
            }
            if (_m[3, 0] != rhs._m[3, 0] || _m[3, 1] != rhs._m[3, 1] || 
                _m[3, 2] != rhs._m[3, 2] || _m[3, 3] != rhs._m[3, 3])
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Matrix4 operator +(Matrix4 lhs, Matrix4 rhs)
        {
            Matrix4 result = new Matrix4(lhs);
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    result._m[i, j] += rhs._m[i, j];
                }
            }
            return result;
        }
        public static Matrix4 operator -(Matrix4 lhs, Matrix4 rhs)
        {
            Matrix4 result = new Matrix4(lhs);
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    result._m[i, j] -= rhs._m[i, j];
                }
            }
            return result;
        }
        public static Matrix4 operator *(Matrix4 lhs, Matrix4 rhs)
        {
            return new Matrix4(
                lhs._m[0, 0] * rhs._m[0, 0] + lhs._m[0, 1] * rhs._m[1, 0] + lhs._m[0, 2] * rhs._m[2, 0] + lhs._m[0, 3] * rhs._m[3, 0],
                lhs._m[0, 0] * rhs._m[0, 1] + lhs._m[0, 1] * rhs._m[1, 1] + lhs._m[0, 2] * rhs._m[2, 1] + lhs._m[0, 3] * rhs._m[3, 1],
                lhs._m[0, 0] * rhs._m[0, 2] + lhs._m[0, 1] * rhs._m[1, 2] + lhs._m[0, 2] * rhs._m[2, 2] + lhs._m[0, 3] * rhs._m[3, 2],
                lhs._m[0, 0] * rhs._m[0, 3] + lhs._m[0, 1] * rhs._m[1, 3] + lhs._m[0, 2] * rhs._m[2, 3] + lhs._m[0, 3] * rhs._m[3, 3],
                lhs._m[1, 0] * rhs._m[0, 0] + lhs._m[1, 1] * rhs._m[1, 0] + lhs._m[1, 2] * rhs._m[2, 0] + lhs._m[1, 3] * rhs._m[3, 0],
                lhs._m[1, 0] * rhs._m[0, 1] + lhs._m[1, 1] * rhs._m[1, 1] + lhs._m[1, 2] * rhs._m[2, 1] + lhs._m[1, 3] * rhs._m[3, 1],
                lhs._m[1, 0] * rhs._m[0, 2] + lhs._m[1, 1] * rhs._m[1, 2] + lhs._m[1, 2] * rhs._m[2, 2] + lhs._m[1, 3] * rhs._m[3, 2],
                lhs._m[1, 0] * rhs._m[0, 3] + lhs._m[1, 1] * rhs._m[1, 3] + lhs._m[1, 2] * rhs._m[2, 3] + lhs._m[1, 3] * rhs._m[3, 3],
                lhs._m[2, 0] * rhs._m[0, 0] + lhs._m[2, 1] * rhs._m[1, 0] + lhs._m[2, 2] * rhs._m[2, 0] + lhs._m[2, 3] * rhs._m[3, 0],
                lhs._m[2, 0] * rhs._m[0, 1] + lhs._m[2, 1] * rhs._m[1, 1] + lhs._m[2, 2] * rhs._m[2, 1] + lhs._m[2, 3] * rhs._m[3, 1],
                lhs._m[2, 0] * rhs._m[0, 2] + lhs._m[2, 1] * rhs._m[1, 2] + lhs._m[2, 2] * rhs._m[2, 2] + lhs._m[2, 3] * rhs._m[3, 2],
                lhs._m[2, 0] * rhs._m[0, 3] + lhs._m[2, 1] * rhs._m[1, 3] + lhs._m[2, 2] * rhs._m[2, 3] + lhs._m[2, 3] * rhs._m[3, 3],
                lhs._m[3, 0] * rhs._m[0, 0] + lhs._m[3, 1] * rhs._m[1, 0] + lhs._m[3, 2] * rhs._m[2, 0] + lhs._m[3, 3] * rhs._m[3, 0],
                lhs._m[3, 0] * rhs._m[0, 1] + lhs._m[3, 1] * rhs._m[1, 1] + lhs._m[3, 2] * rhs._m[2, 1] + lhs._m[3, 3] * rhs._m[3, 1],
                lhs._m[3, 0] * rhs._m[0, 2] + lhs._m[3, 1] * rhs._m[1, 2] + lhs._m[3, 2] * rhs._m[2, 2] + lhs._m[3, 3] * rhs._m[3, 2],
                lhs._m[3, 0] * rhs._m[0, 3] + lhs._m[3, 1] * rhs._m[1, 3] + lhs._m[3, 2] * rhs._m[2, 3] + lhs._m[3, 3] * rhs._m[3, 3]);
        }
        public static Vector4 operator *(Matrix4 lhs, Vector4 vec4)
        {
            return new Vector4(
                 vec4.x * lhs._m[0, 0] + vec4.y * lhs._m[1, 0] + vec4.z * lhs._m[2, 0] + lhs._m[3, 0],
                 vec4.x * lhs._m[0, 1] + vec4.y * lhs._m[1, 1] + vec4.z * lhs._m[2, 1] + lhs._m[3, 1],
                 vec4.x * lhs._m[0, 2] + vec4.y * lhs._m[1, 2] + vec4.z * lhs._m[2, 2] + lhs._m[3, 2],
                 vec4.x * lhs._m[0, 3] + vec4.y * lhs._m[1, 3] + vec4.z * lhs._m[2, 3] + lhs._m[3, 3]);
        }
        public static Vector3 operator *(Matrix4 lhs, Vector3 vec)
	    {
		    Vector4 vec4 = new Vector4(vec);
            vec4 = lhs * vec4;
            vec4 /= MathUtil.IsZero(vec4.w) ? 1f : vec4.w;
		    return vec4.XYZ();
        }
        public static Matrix4 operator *(Matrix4 lhs, float scale)
        {
            Matrix4 ret = Matrix4.identity;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ret._m[i, j] = lhs._m[i, j] * scale;
                }
            }
            return ret;
        }
        public static Matrix4 operator /(Matrix4 lhs, Matrix4 rhs)
        {
            Matrix4 result = new Matrix4(lhs);
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    result._m[i, j] /= MathUtil.NotBeZero(rhs._m[i, j]);
                }
            }
            return result;
        }
    }
}
