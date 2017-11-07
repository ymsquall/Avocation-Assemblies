using System;

namespace Framework.Maths
{
    public partial struct Color
    {
        public static implicit operator System.Windows.Media.Color(Color val)
        {
            byte a, r, g, b;
            // a;
            if (val.a > 1f) { a = byte.MaxValue; }
            else if (val.a < 0f) { a = byte.MinValue; }
            else { a = (byte)(val.a * MathUtil.MaxByteF); }
            // r;
            if (val.r > 1f) { r = byte.MaxValue; }
            else if (val.r < 0f) { r = byte.MinValue; }
            else { r = (byte)(val.r * MathUtil.MaxByteF); }
            // g;
            if (val.g > 1f) { g = byte.MaxValue; }
            else if (val.g < 0f) { g = byte.MinValue; }
            else { g = (byte)(val.g * MathUtil.MaxByteF); }
            // b;
            if (val.b > 1f) { b = byte.MaxValue; }
            else if (val.b < 0f) { b = byte.MinValue; }
            else { b = (byte)(val.b * MathUtil.MaxByteF); }
            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }
        public static implicit operator Color(System.Windows.Media.Color val)
        {
            return Color.FromArgb(val.A, val.R, val.G, val.B);
        }

        public static implicit operator System.Drawing.Color(Color val)
        {
            byte a, r, g, b;
            // a;
            if (val.a > 1f) { a = byte.MaxValue; }
            else if (val.a < 0f) { a = byte.MinValue; }
            else { a = (byte)(val.a * MathUtil.MaxByteF); }
            // r;
            if (val.r > 1f) { r = byte.MaxValue; }
            else if (val.r < 0f) { r = byte.MinValue; }
            else { r = (byte)(val.r * MathUtil.MaxByteF); }
            // g;
            if (val.g > 1f) { g = byte.MaxValue; }
            else if (val.g < 0f) { g = byte.MinValue; }
            else { g = (byte)(val.g * MathUtil.MaxByteF); }
            // b;
            if (val.b > 1f) { b = byte.MaxValue; }
            else if (val.b < 0f) { b = byte.MinValue; }
            else { b = (byte)(val.b * MathUtil.MaxByteF); }
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }
        public static implicit operator Color(System.Drawing.Color val)
        {
            return Color.FromArgb(val.A, val.R, val.G, val.B);
        }
        public static bool operator ==(Color lhs, Color rhs)
        {
            return Object.Equals(lhs, rhs);
        }
        public static bool operator ==(Color lhs, System.Windows.Media.Color rhs)
        {
            return Object.Equals(lhs, (Color)rhs);
        }
        public static bool operator ==(System.Windows.Media.Color lhs, Color rhs)
        {
            Color clr = (Color)lhs;
            return Object.Equals(clr, rhs);
        }
        public static bool operator ==(Color lhs, System.Drawing.Color rhs)
        {
            return Object.Equals(lhs, (Color)rhs);
        }
        public static bool operator ==(System.Drawing.Color lhs, Color rhs)
        {
            Color clr = (Color)lhs;
            return Object.Equals(clr, rhs);
        }
        public static bool operator !=(Color lhs, Color rhs)
        {
            return !Object.Equals(lhs, rhs);
        }
        public static bool operator !=(Color lhs, System.Windows.Media.Color rhs)
        {
            return !Object.Equals(lhs, (Color)rhs);
        }
        public static bool operator !=(System.Windows.Media.Color lhs, Color rhs)
        {
            return !Object.Equals((Color)lhs, rhs);
        }
        public static bool operator !=(Color lhs, System.Drawing.Color rhs)
        {
            return !Object.Equals(lhs, (Color)rhs);
        }
        public static bool operator !=(System.Drawing.Color lhs, Color rhs)
        {
            return !Object.Equals((Color)lhs, rhs);
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
            Color rhs = (Color)obj;
            if (a != rhs.a || r != rhs.r || g != rhs.g || b != rhs.b)
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Color operator +(Color lhs, Color rhs)
        {
            Color result = new Color(lhs);
            result.a += rhs.a;
            result.r += rhs.r;
            result.g += rhs.g;
            result.b += rhs.b;
            result.a = result.a > 1f ? 1f : result.a;
            result.a = result.a < 0f ? 0f : result.a;
            result.r = result.r > 1f ? 1f : result.r;
            result.r = result.r > 1f ? 1f : result.r;
            result.g = result.g > 1f ? 1f : result.g;
            result.g = result.g > 1f ? 1f : result.g;
            result.b = result.b > 1f ? 1f : result.b;
            result.b = result.b > 1f ? 1f : result.b;
            return result;
        }
        public static Color operator -(Color lhs, Color rhs)
        {
            Color result = new Color(lhs);
            result.a -= rhs.a;
            result.r -= rhs.r;
            result.g -= rhs.g;
            result.b -= rhs.b;
            result.a = result.a > 1f ? 1f : result.a;
            result.a = result.a < 0f ? 0f : result.a;
            result.r = result.r > 1f ? 1f : result.r;
            result.r = result.r > 1f ? 1f : result.r;
            result.g = result.g > 1f ? 1f : result.g;
            result.g = result.g > 1f ? 1f : result.g;
            result.b = result.b > 1f ? 1f : result.b;
            result.b = result.b > 1f ? 1f : result.b;
            return result;
        }
        public static Color operator *(Color lhs, float value)
        {
            Color result = new Color(lhs);
            result.a *= value;
            result.r *= value;
            result.g *= value;
            result.b *= value;
            result.a = result.a > 1f ? 1f : result.a;
            result.a = result.a < 0f ? 0f : result.a;
            result.r = result.r > 1f ? 1f : result.r;
            result.r = result.r > 1f ? 1f : result.r;
            result.g = result.g > 1f ? 1f : result.g;
            result.g = result.g > 1f ? 1f : result.g;
            result.b = result.b > 1f ? 1f : result.b;
            result.b = result.b > 1f ? 1f : result.b;
            return result;
        }
        // 暂不实现颜色间的 * / 法，其中alpha值的计算不确定;
    }
}
