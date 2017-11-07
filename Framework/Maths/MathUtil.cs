using System;

namespace Framework.Maths
{
    public static class MathUtil
    {
        public static T Clamp<T>(T value, T max, T min)
            where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }
        public static float Repeat(float value, float max)
        {
            return value % max;
        }
        public static int Repeat(int value, int max)
        {
            return value % max;
        }
        public static float PingPong(float value, float max)
        {
            return value % max;
        }
        public static int PingPong(int value, int max)
        {
            return value % max;
        }
        // 求最大公约数
        public static float MaxGongYueShu(int n1, int n2)
        {
            int temp = Math.Max(n1, n2);
            n2 = Math.Min(n1, n2);//n2中存放两个数中最小的
            n1 = temp;//n1中存放两个数中最大的
            while (n2 != 0)
            {
                n1 = n1 > n2 ? n1 : n2;//使n1中的数大于n2中的数
                int m = n1 % n2;
                n1 = n2;
                n2 = m;
            }
            return n1;
        }
        // 获得宽高比
        public static float GetAspectXY(ref float x, ref float y)
        {
            float gys = MaxGongYueShu((int)x, (int)y);
            x = x / gys;
            y = y / gys;
            return gys;
        }
        // 等比限制最大值
        public static void RatioLimitAt(float max, float ratio, ref float x, ref float y)
        {
            float clipX = x, clipY = y;
            if (clipX > clipY)
            {
                if (clipX > max)
                    clipX = max;
                clipY = clipX / ratio;
            }
            else if (clipX < clipY)
            {
                if (clipY > 1920)
                    clipY = 1920;
                clipX = clipY * ratio;
            }
            else
            {
                if (clipX > 1920)
                    clipX = 1920;
                clipY = clipX;
            }
        }

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
            float r = left.r + (right.r - left.r) * gradient * a;
            float g = left.g + (right.g - left.g) * gradient * a;
            float b = left.b + (right.b - left.b) * gradient * a;
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
        public static void IntersectVertexByDist(IVertex left, IVertex right, Vector3 leftVec, Vector3 rightVec, float d, Action<Vector3, Vector3, Vector2, Color> ret)
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
            ret?.Invoke(pos, norm, uv, color);
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

        public static bool RandomBool()
        {
            return GlobalRandomInst.Next() % 2 == 0;
        }
        public static int Random(int min, int max)
        {
            return GlobalRandomInst.Next() % max + min;
        }
        public static double Random(double min, double max)
        {
            return GlobalRandomInst.NextDouble() * (max - min) + min;
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

        public static readonly Random GlobalRandomInst = new Random();
        #endregion Fields
    }
}
