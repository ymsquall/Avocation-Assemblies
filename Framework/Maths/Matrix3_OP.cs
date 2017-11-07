using System;

namespace Framework.Maths
{
    public partial struct Matrix3
    {
        public static bool operator ==(Matrix3 lhs, Matrix3 rhs)
        {
            return Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Matrix3 lhs, Matrix3 rhs)
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
            Matrix3 rhs = (Matrix3)obj;
            if (_m[0, 0] != rhs._m[0, 0] || _m[0, 1] != rhs._m[0, 1] || _m[0, 2] != rhs._m[0, 2] || _m[0, 3] != rhs._m[0, 3])
            {
                return false;
            }
            if (_m[1, 0] != rhs._m[1, 0] || _m[1, 1] != rhs._m[1, 1] || _m[1, 2] != rhs._m[1, 2] || _m[1, 3] != rhs._m[1, 3])
            {
                return false;
            }
            if (_m[2, 0] != rhs._m[2, 0] || _m[2, 1] != rhs._m[2, 1] || _m[2, 2] != rhs._m[2, 2] || _m[2, 3] != rhs._m[0, 3])
            {
                return false;
            }
            if (_m[3, 0] != rhs._m[3, 0] || _m[3, 1] != rhs._m[3, 1] || _m[3, 2] != rhs._m[3, 2] || _m[3, 3] != rhs._m[3, 3])
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Matrix3 operator +(Matrix3 lhs, Matrix3 rhs)
        {
            Matrix3 result = new Matrix3(lhs);
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    result._m[i, j] += rhs._m[i, j];
                }
            }
            return result;
        }
        public static Matrix3 operator -(Matrix3 lhs, Matrix3 rhs)
        {
            Matrix3 result = new Matrix3(lhs);
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    result._m[i, j] -= rhs._m[i, j];
                }
            }
            return result;
        }
        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs._m[0, 0] * rhs._m[0, 0] + lhs._m[0, 1] * rhs._m[1, 0] + lhs._m[0, 2] * rhs._m[2, 0],
                lhs._m[0, 0] * rhs._m[0, 1] + lhs._m[0, 1] * rhs._m[1, 1] + lhs._m[0, 2] * rhs._m[2, 1],
                lhs._m[0, 0] * rhs._m[0, 2] + lhs._m[0, 1] * rhs._m[1, 2] + lhs._m[0, 2] * rhs._m[2, 2],
                lhs._m[1, 0] * rhs._m[0, 0] + lhs._m[1, 1] * rhs._m[1, 0] + lhs._m[1, 2] * rhs._m[2, 0],
                lhs._m[1, 0] * rhs._m[0, 1] + lhs._m[1, 1] * rhs._m[1, 1] + lhs._m[1, 2] * rhs._m[2, 1],
                lhs._m[1, 0] * rhs._m[0, 2] + lhs._m[1, 1] * rhs._m[1, 2] + lhs._m[1, 2] * rhs._m[2, 2],
                lhs._m[2, 0] * rhs._m[0, 0] + lhs._m[2, 1] * rhs._m[1, 0] + lhs._m[2, 2] * rhs._m[2, 0],
                lhs._m[2, 0] * rhs._m[0, 1] + lhs._m[2, 1] * rhs._m[1, 1] + lhs._m[2, 2] * rhs._m[2, 1],
                lhs._m[2, 0] * rhs._m[0, 2] + lhs._m[2, 1] * rhs._m[1, 2] + lhs._m[2, 2] * rhs._m[2, 2]);
        }
        public static Vector3 operator *(Matrix3 lhs, Vector3 vec)
        {
            return new Vector3(
               lhs._m[0, 0] * vec.x + lhs._m[0, 1] * vec.y + lhs._m[0, 2] * vec.z,
               lhs._m[1, 0] * vec.x + lhs._m[1, 1] * vec.y + lhs._m[1, 2] * vec.z,
               lhs._m[2, 0] * vec.x + lhs._m[2, 1] * vec.y + lhs._m[2, 2] * vec.z);
        }
        public static Matrix3 operator *(Matrix3 lhs, float scale)
        {
            Matrix3 ret = Matrix3.identity;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    ret._m[i, j] = lhs._m[i, j] * scale;
                }
            }
            return ret;
        }
        public static Matrix3 operator /(Matrix3 lhs, Matrix3 rhs)
        {
            Matrix3 result = new Matrix3(lhs);
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    result._m[i, j] /= MathUtil.NotBeZero(rhs._m[i, j]);
                }
            }
            return result;
        }
    }
}
