namespace Framework.Maths.ColorSpace
{
    public unsafe partial struct CIELAB
    {
        #region Constructor
        public CIELAB(Color color)
        {
            l = a = b = 0;
            alpha = color.a;
            CovnertFrom(color);
        }
        #endregion Constructor

        #region Methods
        private void CovnertFrom(Color color)
        {
            int from = color.ToRgb();
            int to = 0;
            RGBLAB.ToLAB((byte*)&from, (byte*)&to);
            FromLab(to);
        }
        private Color CovnertTo()
        {
            int from = ToLab();
            int to = 0;
            RGBLAB.ToRGB((byte*)&from, (byte*)&to);
            Color ret = Color.FromRgb(to);
            ret.a = alpha;
            return ret;
        }
        public static CIELAB FromColor(Color color)
        {
            CIELAB ret = new CIELAB();
            ret.CovnertFrom(color);
            ret.alpha = color.a;
            return ret;
        }
        public static Color ToColor(CIELAB fab)
        {
            Color ret = fab.CovnertTo();
            ret.a = fab.alpha;
            return ret;
        }
        public void FromLab(int lab)
        {
            l = 0xFF & lab;
            a = 0xFF00 & lab;
            a >>= 8;
            b = 0xFF0000 & lab;
            b >>= 16;
        }
        public static int ToLab(double l, double a, double b)
        {
            return (int)(((uint)b << 16) | (ushort)(((ushort)a << 8) | (int)l));
        }
        public static int ToLab(int l, int a, int b)
        {
            return (int)(((uint)b << 16) | (ushort)(((ushort)a << 8) | l));
        }
        public int ToLab()
        {
            return (int)(((uint)b << 16) | (ushort)(((ushort)a << 8) | l));
        }
        public static Color Interpolate(Color _from, Color _to, float gradient)
        {
            var left = FromColor(_from);
            var right = FromColor(_to);

            if (gradient < 0) { gradient = 0; }
            else if (gradient > 1) { gradient = 1; }
            float alpha = _from.a + (_to.a - _from.a) * gradient;
            float l = left.l + (right.l - left.l) * gradient;
            float a = left.a + (right.a - left.a) * gradient;
            float b = left.b + (right.b - left.b) * gradient;
            int from = ToLab(l, a, b);
            int to = 0;
            RGBLAB.ToRGB((byte*)&from, (byte*)&to);
            Color ret = Color.FromRgb(to);
            ret.a = alpha;
            return ret;
        }
        #endregion Methods

        #region Attributes
        public int L { set { l = value; } get { return l; } }
        public int A { set { a = value; } get { return a; } }
        public int B { set { b = value; } get { return b; } }
        #endregion Attributes

        #region Fields
        public int l, a, b;
        public float alpha;
        #endregion Fields
    }
}
