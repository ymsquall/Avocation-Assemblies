using System;

namespace Framework.Maths
{
    public partial struct Vector3
    {
        public static implicit operator Vector3(float val)
        {
            return new Vector3(val, val, val);
        }
        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            // 注意这里调用Equals来判断是否相等，而不是在自己的函数中判断;
            // 这是因为如果在自己的函数中判断;
            // 比如有rhs=null的情况。如果是这种情况。我们要判断if(rhs==null){…}。;
            // 其中rhs==null也是调用一个等号运算符，这里面有一个递归的过程，造成了死循环。;
            return Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !Object.Equals(lhs, rhs);
        }
        public override bool Equals(object obj)
        {
            //判断与之比较的类型是否为null。这样不会造成递归的情况   
            if (obj == null)
            {
                return false;
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }
            Vector3 rhs = (Vector3)obj;
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
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            Vector3 result = new Vector3(lhs);
            result.x += rhs.x;
            result.y += rhs.y;
            result.z += rhs.z;
            return result;
        }
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            Vector3 result = new Vector3(lhs);
            result.x -= rhs.x;
            result.y -= rhs.y;
            result.z -= rhs.z;
            return result;
        }
        public static Vector3 operator *(Vector3 lhs, Vector3 rhs)
        {
            Vector3 result = new Vector3(lhs);
            result.x *= rhs.x;
            result.y *= rhs.y;
            result.z *= rhs.z;
            return result;
        }
        public static Vector3 operator *(Vector3 lhs, float val)
        {
            Vector3 result = new Vector3(lhs);
            result.x *= val;
            result.y *= val;
            result.z *= val;
            return result;
        }
        public static Vector3 operator /(Vector3 lhs, Vector3 rhs)
        {
            Vector3 result = new Vector3(lhs);
            result.x /= MathUtil.NotBeZero(rhs.x);
            result.y /= MathUtil.NotBeZero(rhs.y);
            result.z /= MathUtil.NotBeZero(rhs.z);
            return result;
        }
        public static Vector3 operator /(Vector3 lhs, float val)
        {
            if (MathUtil.IsZero(val))
            {
                return lhs;
            }
            Vector3 result = new Vector3(lhs);
            result.x /= val;
            result.y /= val;
            result.z /= val;
            return result;
        }
        public static Vector3 operator /(float val, Vector3 rhs)
        {
            return new Vector3(
                val / MathUtil.NotBeZero(rhs.x),
                val / MathUtil.NotBeZero(rhs.y),
                val / MathUtil.NotBeZero(rhs.z));
        }
    }
}
