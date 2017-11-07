using System;

namespace Framework.Maths
{
    public partial struct Plane
    {
        public static implicit operator Plane(Vector4 val)
        {
            return new Plane(val.x, val.y, val.z, val.w);
        }
        public static bool operator ==(Plane lhs, Plane rhs)
        {
            return Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Plane lhs, Plane rhs)
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
            Plane rhs = (Plane)obj;
            if (a != rhs.a || b != rhs.b || c != rhs.c || d != rhs.d)
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Plane operator +(Plane lhs, Plane rhs)
        {
            Plane result = new Plane(lhs);
            result.a += rhs.a;
            result.b += rhs.b;
            result.c += rhs.c;
            result.d += rhs.d;
            return result;
        }
        public static Plane operator -(Plane lhs, Plane rhs)
        {
            Plane result = new Plane(lhs);
            result.a -= rhs.a;
            result.b -= rhs.b;
            result.c -= rhs.c;
            result.d -= rhs.d;
            return result;
        }
    }
}
