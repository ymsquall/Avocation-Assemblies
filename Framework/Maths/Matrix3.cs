using System;

namespace Framework.Maths
{
    public partial struct Matrix3
    {
        #region Constructor
        public Matrix3(float m00, float m01, float m02,
                       float m10, float m11, float m12,
                       float m20, float m21, float m22)
        {
            _m = new float[3, 3];
            _m[0, 0] = m00; _m[0, 1] = m01; _m[0, 2] = m02;
            _m[1, 0] = m10; _m[1, 1] = m11; _m[1, 2] = m12;
            _m[2, 0] = m20; _m[2, 1] = m21; _m[2, 2] = m22;
        }
        public Matrix3(Matrix3 oth)
        {
            _m = new float[3, 3];
            _m[0, 0] = oth._m[0, 0]; _m[0, 1] = oth._m[0, 1]; _m[0, 2] = oth._m[0, 2];
            _m[1, 0] = oth._m[1, 0]; _m[1, 1] = oth._m[1, 1]; _m[1, 2] = oth._m[1, 2];
            _m[2, 0] = oth._m[2, 0]; _m[2, 1] = oth._m[2, 1]; _m[2, 2] = oth._m[2, 2];
        }
        #endregion Constructor

        #region Methods
        public void Identity()
        {
            _m[0, 0] = 1f; _m[0, 1] = 0f; _m[0, 2] = 0f;
            _m[1, 0] = 0f; _m[1, 1] = 1f; _m[1, 2] = 0f;
            _m[2, 0] = 0f; _m[2, 1] = 0f; _m[2, 2] = 1f;
        }
        public static Matrix3 MakeScale(Vector3 scale)
        {
            return MakeScale(scale.x, scale.y, scale.z);
        }
        public static Matrix3 MakeScale(float x, float y, float z)
        {
            return new Matrix3(x, 0, 0, 0, y, 0, 0, 0, z);
        }
        public static Matrix3 MakeRotation(float pitch, float yaw, float roll)
        {
            Matrix3 xAxis = new Matrix3((float)Math.Cos(pitch), (float)Math.Sin(pitch), 0.0f,
                                   -(float)Math.Sin(pitch), (float)Math.Cos(pitch), 0.0f,
                                    0.0f, 0.0f, 0.0f);
            Matrix3 yAxis = new Matrix3((float)Math.Cos(yaw), 0.0f, (float)Math.Sin(yaw),
                                   0.0f, 1.0f, 0.0f,
                                   -(float)Math.Sin(yaw), 0.0f, (float)Math.Cos(yaw));
            Matrix3 zAxis = new Matrix3((float)Math.Cos(roll), 0.0f, (float)Math.Sin(roll),
                                    0.0f, 1.0f, 0.0f,
                                   -(float)Math.Sin(roll), 0.0f, (float)Math.Cos(roll));
            return new Matrix3(xAxis * yAxis * zAxis);
        }
        #endregion Methods

        #region Attributes
        public float this[int i, int j]
        {
            get { if (i >= 3 || j >= 3) { return 0; } return _m[i, j]; }
            set { if (i >= 3 || j >= 3) { return; } _m[i, j] = value; }
        }
        public Vector3 Scale
        {
            set
            {
                _m[0, 0] = value.x;
                _m[1, 1] = value.y;
                _m[2, 2] = value.z;
            }
            get { return new Vector3(_m[0, 0], _m[1, 1], _m[2, 2]); }
        }
        #endregion Attributes

        #region Fields
        public readonly static Matrix3 identity = 
            new Matrix3(1f, 0f, 0f,
                        0f, 1f, 0f,
                        0f, 0f, 1f);
        private float[,] _m;
        #endregion Fields
    }
}
