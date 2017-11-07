using System;

namespace Framework.Maths
{
    public partial struct Vertex2D
    {
        public static implicit operator Vertex2D(int val)
        {
            return new Vertex2D(val, val, 0);
        }
        public static bool operator ==(Vertex2D lhs, Vertex2D rhs)
        {
            return Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Vertex2D lhs, Vertex2D rhs)
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
            Vertex2D rhs = (Vertex2D)obj;
            if (x != rhs.x)
            {
                return false;
            }
            if (y != rhs.y)
            {
                return false;
            }
            if (depth != rhs.depth)
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Vertex2D operator +(Vertex2D lhs, Vertex2D rhs)
        {
            Vertex2D result = new Vertex2D(lhs);
            result.x += rhs.x;
            result.y += rhs.y;
            return result;
        }
        public static Vertex2D operator -(Vertex2D lhs, Vertex2D rhs)
        {
            Vertex2D result = new Vertex2D(lhs);
            result.x -= rhs.x;
            result.y -= rhs.y;
            return result;
        }
        public static Point2 operator -(Vertex2D lhs, Point2 rhs)
        {
            Point2 result = new Point2(lhs.x, lhs.y);
            return result - rhs;
        }
        public static Vertex2D operator *(Vertex2D lhs, Vertex2D rhs)
        {
            Vertex2D result = new Vertex2D(lhs);
            result.x *= rhs.x;
            result.y *= rhs.y;
            return result;
        }
        public static Vertex2D operator *(Vertex2D lhs, int val)
        {
            Vertex2D result = new Vertex2D(lhs);
            result.x *= val;
            result.y *= val;
            return result;
        }
        public static Vertex2D operator /(Vertex2D lhs, Vertex2D rhs)
        {
            Vertex2D result = new Vertex2D(lhs);
            result.x /= rhs.x == 0 ? 1 : rhs.x;
            result.y /= rhs.y == 0 ? 1 : rhs.y;
            return result;
        }
        public static Vertex2D operator /(Vertex2D lhs, int val)
        {
            if (val == 0)
            {
                return lhs;
            }
            Vertex2D result = new Vertex2D(lhs);
            result.x /= val;
            result.y /= val;
            return result;
        }
        public static Vertex2D operator /(float val, Vertex2D rhs)
        {
            return new Vertex2D(
                val / rhs.x == 0 ? 1 : rhs.x,
                val / rhs.y == 0 ? 1 : rhs.y,
                rhs.depth);
        }
    }
}
