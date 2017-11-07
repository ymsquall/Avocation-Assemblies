namespace Framework.Maths
{
    // 自定义颜色类;
    public partial struct Color
    {
        #region Constructor
        public Color(Color oth)
        {
            a = oth.a; r = oth.r; g = oth.g; b = oth.b;
        }
        #endregion Constructor

        #region Methods
        public static Color FromArgb(float _a, float _r, float _g, float _b)
        {
            if (_a > 1f) { _a = 1f; } else if (_a < 0f) { _a = 0f; }
            if (_r > 1f) { _r = 1f; } else if (_r < 0f) { _r = 0f; }
            if (_g > 1f) { _g = 1f; } else if (_g < 0f) { _g = 0f; }
            if (_b > 1f) { _b = 1f; } else if (_b < 0f) { _b = 0f; }
            return new Color() { a = _a, r = _r, g = _g, b = _b };
        }
        public static Color FromArgb(byte _a, byte _r, byte _g, byte _b)
        {
            Color result = new Color();
            result.ByteA = _a;
            result.ByteR = _r;
            result.ByteG = _g;
            result.ByteB = _b;
            return result;
        }
        // 颜色乘法;
        public static Color Multiply(Color lhs, float diffuse, bool applyAlpha = false)
        {
            Color result = new Color(lhs);
            if (applyAlpha)
            {
                result.a *= diffuse;
            }
            result.r *= diffuse;
            result.g *= diffuse;
            result.b *= diffuse;
            return result;
        }
        public static Color Multiply(Color lhs, Color rhs, bool applyAlpha = false)
        {
            Color result = new Color(lhs);
            if (applyAlpha)
            {
                result.a *= rhs.a;
            }
            else
            {
                result.a = result.a > rhs.a ? result.a : rhs.a;
            }
            result.r *= rhs.r;
            result.g *= rhs.g;
            result.b *= rhs.b;
            return result;
        }
        public static int ToRgb(Color color)
        {
            return color.ToRgb();
        }
        public int ToRgb()
        {
            return (int)(((uint)ByteR << 16) | (ushort)(((ushort)ByteG << 8) | ByteB));
        }
        public static void FromRgb(int rgb, ref Color color)
        {
            int b = 0xFF & rgb;
            int g = 0xFF00 & rgb;
            g >>= 8;
            int r = 0xFF0000 & rgb;
            r >>= 16;
            color.ByteR = (byte)r;
            color.ByteG = (byte)g;
            color.ByteB = (byte)b;
        }
        public static Color FromRgb(int rgb)
        {
            Color color = new Color();
            FromRgb(rgb, ref color);
            return color;
        }
        #endregion Methods

        #region Attributes
        public byte ByteA
        {
            set
            {
                if (value == 0)
                {
                    a = 0f;
                }
                else if (value == 255)
                {
                    a = 1f;
                }
                else
                {
                    a = (float)(value + 0.5f) * MathUtil.InvMaxByteF;
                    a = a > 1f ? 1f : a;
                    a = a < 0f ? 0f : a;
                }
            }
            get
            {
                if (a > 1) { return byte.MaxValue; }
                else if (a < 0) { return byte.MinValue; }
                return (byte)(a * MathUtil.MaxByteF);
            }
        }
        public byte ByteR
        {
            set
            {
                if (value == 0)
                {
                    r = 0f;
                }
                else if (value == 255)
                {
                    r = 1f;
                }
                else
                {
                    r = (float)(value + 0.5f) * MathUtil.InvMaxByteF;
                    r = r > 1f ? 1f : r;
                    r = r < 0f ? 0f : r;
                }
            }
            get
            {
                if (r > 1) { return byte.MaxValue; }
                else if (r < 0) { return byte.MinValue; }
                return (byte)(r * MathUtil.MaxByteF);
            }
        }
        public byte ByteG
        {
            set
            {
                if (value == 0)
                {
                    g = 0f;
                }
                else if (value == 255)
                {
                    g = 1f;
                }
                else
                {
                    g = (float)(value + 0.5f) * MathUtil.InvMaxByteF;
                    g = g > 1f ? 1f : g;
                    g = g < 0f ? 0f : g;
                }
            }
            get
            {
                if (g > 1) { return byte.MaxValue; }
                else if (g < 0) { return byte.MinValue; }
                return (byte)(g * MathUtil.MaxByteF);
            }
        }
        public byte ByteB
        {
            set
            {
                if (value == 0)
                {
                    b = 0f;
                }
                else if (value == 255)
                {
                    b = 1f;
                }
                else
                {
                    b = (float)(value + 0.5f) * MathUtil.InvMaxByteF;
                    b = b > 1f ? 1f : b;
                    b = b < 0f ? 0f : b;
                }
            }
            get
            {
                if (b > 1) { return byte.MaxValue; }
                else if (b < 0) { return byte.MinValue; }
                return (byte)(b * MathUtil.MaxByteF);
            }
        }
        #endregion Attributes

        #region Fields
        public readonly static Color Zero = Color.FromArgb(0f, 0f, 0f, 0f);
        public readonly static Color White = Color.FromArgb(1f, 1f, 1f, 1f);
        public readonly static Color Black = Color.FromArgb(1f, 0f, 0f, 0f);
        public readonly static Color Blue = Color.FromArgb(1f, 0f, 0f, 1f);
        public readonly static Color Green = Color.FromArgb(1f, 0f, 1f, 0f);
        public readonly static Color Red = Color.FromArgb(1f, 1f, 0f, 0f);
        public readonly static Color Gray = Color.FromArgb(1f, 0.31f, 0.31f, 0.31f);
        public readonly static Color Yellow = Color.FromArgb(1f, 1f, 1f, 0f);
        public float a, r, g, b;
        #endregion Fields
    }
}
