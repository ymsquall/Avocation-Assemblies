using System;
#if WPF
using SysPoint = System.Windows.Point;
#else
using SysPoint = Framework.Maths.Point2;
#endif

namespace Framework.Maths
{
    public partial struct Vector2
    {
        public static implicit operator Vector2(float val)
        {
            return new Vector2(val, val);
        }
#if WPF
        public static implicit operator Vector2(SysPoint p)
        {
            return new Vector2(p.X, p.Y);
        }
        public static implicit operator SysPoint(Vector2 p)
        {
            return new SysPoint(p.x, p.y);
        }
#endif
        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Vector2 lhs, Vector2 rhs)
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
            Vector2 rhs = (Vector2)obj;
            if (x != rhs.x)
            {
                return false;
            }
            if (y != rhs.y)
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            Vector2 result = new Vector2(lhs);
            result.x += rhs.x;
            result.y += rhs.y;
            return result;
        }
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            Vector2 result = new Vector2(lhs);
            result.x -= rhs.x;
            result.y -= rhs.y;
            return result;
        }
        public static Vector2 operator *(Vector2 lhs, Vector2 rhs)
        {
            Vector2 result = new Vector2(lhs);
            result.x *= rhs.x;
            result.y *= rhs.y;
            return result;
        }
        public static Vector2 operator *(Vector2 lhs, float val)
        {
            Vector2 result = new Vector2(lhs);
            result.x *= val;
            result.y *= val;
            return result;
        }
        public static Vector2 operator *(Vector2 lhs, double val)
        {
            Vector2 result = new Vector2(lhs);
            result.x *= (float)val;
            result.y *= (float)val;
            return result;
        }
        public static Vector2 operator /(Vector2 lhs, Vector2 rhs)
        {
            Vector2 result = new Vector2(lhs);
            result.x /= MathUtil.NotBeZero(rhs.x);
            result.y /= MathUtil.NotBeZero(rhs.y);
            return result;
        }
        public static Vector2 operator /(Vector2 lhs, float val)
        {
            if (MathUtil.IsZero(val))
            {
                return lhs;
            }
            Vector2 result = new Vector2(lhs);
            result.x /= val;
            result.y /= val;
            return result;
        }
        public static Vector2 operator /(float val, Vector2 rhs)
        {
            return new Vector2(
                val / MathUtil.NotBeZero(rhs.x),
                val / MathUtil.NotBeZero(rhs.y));
        }
#if WPF
        public static Vector2 operator -(Vector2 lhs, SysPoint rhs)
        {
            Vector2 result = new Vector2(lhs);
            result.x -= (float)rhs.X;
            result.y -= (float)rhs.Y;
            return result;
        }
#endif
    }
}
