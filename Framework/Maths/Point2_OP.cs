using System;
#if WPF
using SysPoint = System.Windows.Point;
#else
using SysPoint = Framework.Maths.Point2;
#endif

namespace Framework.Maths
{
    public partial struct Point2
    {
        public static implicit operator Point2(int val)
        {
            return new Point2(val, val);
        }
#if WPF
        public static implicit operator Point2(SysPoint p)
        {
            return new Point2(p.X, p.Y);
        }
        public static implicit operator SysPoint(Point2 p)
        {
            return new SysPoint(p.X, p.Y);
        }
#endif
        public static bool operator ==(Point2 lhs, Point2 rhs)
        {
            return Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Point2 lhs, Point2 rhs)
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
            Point2 rhs = (Point2)obj;
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
        public static Point2 operator +(Point2 lhs, Point2 rhs)
        {
            Point2 result = new Point2(lhs);
            result.x += rhs.x;
            result.y += rhs.y;
            return result;
        }
        public static Point2 operator -(Point2 lhs, Point2 rhs)
        {
            Point2 result = new Point2(lhs);
            result.x -= rhs.x;
            result.y -= rhs.y;
            return result;
        }
        public static Point2 operator *(Point2 lhs, Point2 rhs)
        {
            Point2 result = new Point2(lhs);
            result.x *= rhs.x;
            result.y *= rhs.y;
            return result;
        }
        public static Point2 operator *(Point2 lhs, int val)
        {
            Point2 result = new Point2(lhs);
            result.x *= val;
            result.y *= val;
            return result;
        }
        public static Point2 operator /(Point2 lhs, Point2 rhs)
        {
            Point2 result = new Point2(lhs);
            result.x /= rhs.x == 0 ? 1 : rhs.x;
            result.y /= rhs.y == 0 ? 1 : rhs.y;
            return result;
        }
        public static Point2 operator /(Point2 lhs, int val)
        {
            if (val == 0)
            {
                return lhs;
            }
            Point2 result = new Point2(lhs);
            result.x /= val;
            result.y /= val;
            return result;
        }
        public static Point2 operator /(float val, Point2 rhs)
        {
            return new Point2(
                val / rhs.x == 0 ? 1 : rhs.x,
                val / rhs.y == 0 ? 1 : rhs.y);
        }
#if WPF
        public static Point2 operator -(Point2 lhs, SysPoint rhs)
        {
            Point2 result = new Point2(lhs);
            result.x -= (int)rhs.X;
            result.y -= (int)rhs.Y;
            return result;
        }
#endif
    }
}
