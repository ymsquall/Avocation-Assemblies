using System;

namespace Framework.Maths
{
    public partial struct Vector4
    {
        public static implicit operator Vector4(Vector3 val)
        {
            return new Vector4(val.x, val.y, val.z, 1);
        }
        public static implicit operator Vector4(float val)
        {
            return new Vector4(val, val, val, 1);
        }
        public static bool operator ==(Vector4 lhs, Vector4 rhs)
        { 
            return Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Vector4 lhs, Vector4 rhs)
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
            Vector4 rhs = (Vector4)obj;
            if (x != rhs.x)
            {
                return false;
            }
            if (y != rhs.y)
            {
                return false;
            }
            if (z != rhs.z)
            {
                return false;
            }
            if (w != rhs.w)
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Vector4 operator +(Vector4 lhs, Vector4 rhs)
        {
            Vector4 result = new Vector4(lhs);
            result.x += rhs.x;
            result.y += rhs.y;
            result.z += rhs.z;
            result.w += rhs.w;
            return result;
        }
        public static Vector4 operator +(Vector4 lhs, float val)
        {
            Vector4 result = new Vector4(lhs);
            result.x += val;
            result.y += val;
            result.z += val;
            result.w += val;
            return result;
        }
        public static Vector4 operator -(Vector4 lhs, Vector4 rhs)
        {
            Vector4 result = new Vector4(lhs);
            result.x -= rhs.x;
            result.y -= rhs.y;
            result.z -= rhs.z;
            result.w -= rhs.w;
            return result;
        }
        public static Vector4 operator -(Vector4 lhs, float val)
        {
            Vector4 result = new Vector4(lhs);
            result.x -= val;
            result.y -= val;
            result.z -= val;
            result.w -= val;
            return result;
        }
        public static Vector4 operator -(float val, Vector4 rhs)
        {
            return new Vector4(val - rhs.x, val - rhs.y, val - rhs.z, val - rhs.w);
        }
        public static Vector4 operator *(Vector4 lhs, Vector4 rhs)
        {
            Vector4 result = new Vector4(lhs);
            result.x *= rhs.x;
            result.y *= rhs.y;
            result.z *= rhs.z;
            result.w *= rhs.w;
            return result;
        }
        public static Vector4 operator *(Vector4 lhs, float val)
        {
            Vector4 result = new Vector4(lhs);
            result.x *= val;
            result.y *= val;
            result.z *= val;
            result.w *= val;
            return result;
        }
        public static Vector4 operator /(Vector4 lhs, Vector4 rhs)
        {
            Vector4 result = new Vector4(lhs);
            result.x /= MathUtil.NotBeZero(rhs.x);
            result.y /= MathUtil.NotBeZero(rhs.y);
            result.z /= MathUtil.NotBeZero(rhs.z);
            result.w /= MathUtil.NotBeZero(rhs.w);
            return result;
        }
        public static Vector4 operator /(Vector4 lhs, float val)
        {
            if (MathUtil.IsZero(val))
            {
                return lhs;
            }
            Vector4 result = new Vector4(lhs);
            result.x /= val;
            result.y /= val;
            result.z /= val;
            result.w /= val;
            return result;
        }
        public static Vector4 operator /(float val, Vector4 rhs)
        {
            return new Vector4(
                val / MathUtil.NotBeZero(rhs.x),
                val / MathUtil.NotBeZero(rhs.y),
                val / MathUtil.NotBeZero(rhs.z),
                val / MathUtil.NotBeZero(rhs.w));
        }
    }
}
