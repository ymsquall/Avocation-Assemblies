using System;

namespace Framework.Maths
{
    public partial struct Vertex2D
    {
        #region Constructor
        public Vertex2D(int _x, int _y, float z)
        {
            x = _x;
            y = _y;
            color = Color.White;
            diffuse = Color.Zero;
            specular = Color.Zero;
            depth = z;
            uv0 = Vector2.zero;
        }
        public Vertex2D(Point2 pt, float z)
        {
            x = pt.X;
            y = pt.Y;
            color = Color.White;
            diffuse = Color.Zero;
            specular = Color.Zero;
            depth = z;
            uv0 = Vector2.zero;
        }
        public Vertex2D(int _x, int _y, float z, Color c)
        {
            x = _x;
            y = _y;
            color = c;
            diffuse = Color.Zero;
            specular = Color.Zero;
            depth = z;
            uv0 = Vector2.zero;
        }
        public Vertex2D(Point2 pt, float z, Color c)
        {
            x = pt.X;
            y = pt.Y;
            color = c;
            diffuse = Color.Zero;
            specular = Color.Zero;
            depth = z;
            uv0 = Vector2.zero;
        }
        public Vertex2D(Vertex2D oth)
        {
            x = oth.x;
            y = oth.y;
            color = oth.color;
            diffuse = oth.diffuse;
            specular = oth.specular;
            depth = oth.depth;
            uv0 = oth.uv0;
        }
        #endregion Constructor

        #region Methods
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", x, x, depth);
        }
        public void FillRenderData(Vector2 uv, Color vertexColor, bool correction/*透视修正*/)
        {
            if(correction)
            {
                // 透视修正纹理映射 u/z v/z 1/z;
                depth = 1f / (MathUtil.NotBeZero(depth));
                uv0 = uv * depth;
                color = vertexColor;
                diffuse *= depth;
                specular *= depth;
            }
            else
            {
                uv0 = uv;
                color = vertexColor;
            }
        }
        #endregion Methods

        #region Attributes
        public float Length2D
        {
            get { return (float)Math.Sqrt(SqrLength2D); }
        }
        public int SqrLength2D
        {
            get { return x * x + y * y; }
        }
        public float Length3D
        {
            get { return (float)Math.Sqrt(Length3DSqrt); }
        }
        public float Length3DSqrt
        {
            get { return x * x + y * y + depth * depth; }
        }
        #endregion Attributes

        #region Fields
        public int x, y;
        public Color color;
        public Color diffuse;
        public Color specular;
        public float depth;
        public Vector2 uv0;
        #endregion Fields
    }
}
