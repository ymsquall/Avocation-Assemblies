using System;

namespace Framework.Maths
{
    public partial struct Vector2
    {
        #region Constructor
        public Vector2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }
        public Vector2(double _x, double _y)
        {
            x = (float)_x;
            y = (float)_y;
        }
        public Vector2(Vector2 oth)
        {
            x = oth.x;
            y = oth.y;
        }
        #endregion Constructor

        #region Methods
        public float Normalize()
        {
            float length = Length;
            if (length > MathUtil.Epsilon)
            {
                float invLength = 1f / length;
                x *= invLength;
                y *= invLength;
            }
            return length;
        }
        public static Vector2 Normalize(Vector2 vec)
        {
            Vector2 ret = new Vector2(vec);
            ret.Normalize();
            return ret;
        }
        public float Dot(Vector2 oth)
        {
            return x * oth.x + y * oth.y;
        }
        public Vector2 CenterPoint(Vector2 oth)
        {
            return new Vector2((x + oth.x) * 0.5f, (y + oth.y) * 0.5f);
        }
        #endregion Methods

        #region Attributes
        public float Length
        {
            get { return (float)Math.Sqrt(SqrLength); }
        }
        public float SqrLength
        {
            get { return x * x + y * y; }
        }
        #endregion Attributes

        #region Fields
        public readonly static Vector2 zero = new Vector2(0f, 0f);
        public readonly static Vector2 one = new Vector2(1f, 1f);
        public float x, y;
        #endregion Fields
    }
}
