using System;
using System.ComponentModel;

namespace Framework.Maths
{
    public partial struct Vector3
    {
        #region Constructor
        public Vector3(Vector3 oth)
        {
            x = oth.x;
            y = oth.y;
            z = oth.z;
        }
        public Vector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        #endregion Constructor

        #region Methods
        public override string ToString()
        {
            return string.Format("[{0:f2},{1:f2},{2:f2}]", x, y, z);
        }
        // 单位化;
        public float Normalize()
        {
            float length = Length;
            if (length > MathUtil.Epsilon)
            {
                float invLength = 1f / length;
                x *= invLength;
                y *= invLength;
                z *= invLength;
            }
            return length;
        }
        public static Vector3 Normalize(Vector3 vec)
        {
            Vector3 ret = new Vector3(vec);
            ret.Normalize();
            return ret;
        }
        // 点积;
        public float Dot(Vector3 oth)
        {
            return x * oth.x + y * oth.y + z * oth.z;
        }
        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.Dot(rhs);
        }
        // 差积;
        public Vector3 Cross(Vector3 oth)
        {
            return new Vector3(y * oth.z - z * oth.y, z * oth.x - x * oth.z, x * oth.y - y * oth.x);
        }
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return lhs.Cross(rhs);
        }
        // 中心点;
        public Vector3 CenterPoint(Vector3 oth)
        {
            return new Vector3((x + oth.x) * 0.5f, (y + oth.y) * 0.5f, (z + oth.z) * 0.5f);
        }
        // 按标准轴计算法线向量;
	    public Vector3 Perpendicular()
	    {
            float squareZero = (float)(MathUtil.Epsilon * MathUtil.Epsilon);
            Vector3 perp = Cross(Vector3.right);
            if (perp.SqrLength < squareZero)
		    {
                perp = Cross(Vector3.up);
		    }
		    perp.Normalize();
		    return perp;
	    }
	    // 近似相等判断;
	    public bool ApproximateEqual(Vector3 rhs, float precision)
	    {
            return (Math.Abs(rhs.x - x) <= precision && Math.Abs(rhs.y - y) <= precision && Math.Abs(rhs.z - z) <= precision);
	    }
        // 坐标变换;
        public Vector3 TransformCoord(Matrix4 mat4x4)
        {
            Vector4 vec4 = new Vector4(
                 x * mat4x4[0, 0] + y * mat4x4[1, 0] + z * mat4x4[2, 0] + mat4x4[3, 0],
                 x * mat4x4[0, 1] + y * mat4x4[1, 1] + z * mat4x4[2, 1] + mat4x4[3, 1],
                 x * mat4x4[0, 2] + y * mat4x4[1, 2] + z * mat4x4[2, 2] + mat4x4[3, 2],
                 x * mat4x4[0, 3] + y * mat4x4[1, 3] + z * mat4x4[2, 3] + mat4x4[3, 3]);
            vec4 /= MathUtil.NotBeZero(vec4.w);
            return vec4.XYZ();
        }
        // 方向变换;
        public Vector3 TransformNormal(Matrix4 mat4x4)
        {
            return new Vector3(
                x * mat4x4[0, 0] + y * mat4x4[1, 0] + z * mat4x4[2, 0],
                x * mat4x4[0, 1] + y * mat4x4[1, 1] + z * mat4x4[2, 1],
                x * mat4x4[0, 2] + y * mat4x4[1, 2] + z * mat4x4[2, 2]);
        }
        #endregion Methods

        #region Attributes
        public float X
        {
            set { x = value; }
            get { return x; }
        }
        public float Y
        {
            set { y = value; }
            get { return y; }
        }
        public float Z
        {
            set { z = value; }
            get { return z; }
        }
        [Browsable(false)]
        public Vector3 Center { get { return new Vector3(x * 0.5f, y * 0.5f, z * 0.5f); } }
        // 长度;
        [Browsable(false)]
        public float Length
        {
            get { return (float)Math.Sqrt(SqrLength); }
        }
        [Browsable(false)]
        public float SqrLength
        {
            get { return x * x + y * y + z * z; }
        }
        #endregion Attributes

        #region Fields
        public readonly static Vector3 zero = new Vector3(0f, 0f, 0f);
        public readonly static Vector3 one = new Vector3(1f, 1f, 1f);
        public readonly static Vector3 up = new Vector3(0f, 1f, 0f);
        public readonly static Vector3 down = new Vector3(0f, -1f, 0f);
        public readonly static Vector3 left = new Vector3(-1f, 0f, 0f);
        public readonly static Vector3 right = new Vector3(1f, 0f, 0f);
        public readonly static Vector3 front = new Vector3(0f, 0f, 1f);
        public readonly static Vector3 back = new Vector3(0f, 0f, -1f);
        public float x, y, z;
        #endregion Fields
    }
}
