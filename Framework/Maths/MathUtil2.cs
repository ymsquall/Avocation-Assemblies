using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftRenderEngine.Shape;

namespace SoftRenderEngine.Maths
{
    public static class MathUtil
    {
        #region Methods
        // 限制数值在[min-max]之内;
        public static float Clamp(float value, float min = 0, float max = 1)
        {
            if (value < min) { return min; }
            if (value > max) { return max; }
            return value;
        }
        public static int Clamp(int value, int min = 0, int max = 255)
        {
            // 尽量减少函数调用;
            if (value < min) { return min; }
            if (value > max) { return max; }
            return value;
        }
        // 插值方法，其中gradient为差值比例，取值范围[0-1];
        public static float Interpolate(float min, float max, float gradient)
        {
            if (gradient < 0) { gradient = 0; }
            else if (gradient > 1) { gradient = 1; }
            return min + (max - min) * gradient;
        }
        public static int Interpolate(int min, int max, float gradient)
        {
            if (gradient < 0) { gradient = 0; }
            else if (gradient > 1) { gradient = 1; }
            return min + (int)((float)(max - min) * gradient);
        }
        public static Color Interpolate(Color left, Color right, float gradient)
        {
            if (gradient < 0) { gradient = 0; }
            else if (gradient > 1) { gradient = 1; }
            float a = left.a + (right.a - left.a) * gradient;
            float r = left.r + (right.r - left.r) * gradient;
            float g = left.g + (right.g - left.g) * gradient;
            float b = left.b + (right.b - left.b) * gradient;
            return Color.FromArgb(a, r, g, b);
        }
        public static Vector2 Interpolate(Vector2 left, Vector2 right, float gradient)
        {
            Vector2 dir = right - left;
            float totalLengtn = dir.Normalize();
            return left + dir * Clamp(gradient * totalLengtn, 0, totalLengtn);
        }
        public static Vector3 Interpolate(Vector3 left, Vector3 right, float gradient)
        {
            Vector3 dir = right - left;
            float totalLengtn = dir.Normalize();
            return left + dir * Clamp(gradient * totalLengtn, 0, totalLengtn);
        }
        public static Vertex IntersectVertexByDist(Vertex left, Vertex right, Vector3 leftVec, Vector3 rightVec, float d)
        {
            // 计算left-right与距离d的交点;
            // 直线方程 x = x1 + (x2 - x1) * t, y = y1 + (y2 - y1) * t;
            Vector3 vec = rightVec - leftVec;
            float t = (d - leftVec.z) / NotBeZero(vec.z);
            float x = leftVec.x + vec.x * t;
            float y = leftVec.y + vec.y * t;
            float totalLen = vec.SqrLength;
            float len = (leftVec - new Vector3(x, y, d)).SqrLength;
            float gradient = len / NotBeZero(totalLen);
            Vector3 pos = Interpolate(left.Position, right.Position, gradient);
            Vector3 norm = Interpolate(left.Normal, right.Normal, gradient);
            Vector2 uv = Interpolate(left.UV0, right.UV0, gradient);
            Color color = Interpolate(left.Color, right.Color, gradient);
            return new Vertex(pos, norm, color, uv);
        }
        public static bool IsZero(float value, float precision = Epsilon)
        {
            return value <= precision && value >= -precision;
        }
        public static float NotBeZero(float value)
        {
            if (value <= Epsilon && value >= -Epsilon)
            {
                return 1f;
            }
            return value;
        }
        public static float NotBeZero(int value)
        {
            return value == 0 ? 1 : value;
        }
        public static float Angle2Radian(float angle)
        {
            return Deg2Rad * angle;
        }
        public static float Radian2Angle(float radian)
        {
            return Rad2Deg * radian;
        }
        #endregion Methods

        #region Fields
        // 尽量减少函数调用为原则编写;
        public const float MaxByteF = 255f;
        public const float InvMaxByteF = 0.0039215f;    // 1 / 255 使用乘法替换除法;
        public const float Epsilon = 1.4013e-045f;
        public const float PI = 3.14159f;
        public const float Deg2Rad = 0.0174533f;        // pi/180;
        public const float Rad2Deg = 57.2958f;          // 180/pi;
        #endregion Fields
    }
}
