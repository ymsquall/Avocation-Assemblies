using System;

namespace Framework.Maths
{
    public partial struct Matrix4
    {
        #region Constructor
        public Matrix4(float m00, float m01, float m02, float m03,
                       float m10, float m11, float m12, float m13,
                       float m20, float m21, float m22, float m23,
                       float m30, float m31, float m32, float m33)
        {
            _m = new float[4, 4];
            _m[0, 0] = m00; _m[0, 1] = m01; _m[0, 2] = m02; _m[0, 3] = m03;
            _m[1, 0] = m10; _m[1, 1] = m11; _m[1, 2] = m12; _m[1, 3] = m13;
            _m[2, 0] = m20; _m[2, 1] = m21; _m[2, 2] = m22; _m[2, 3] = m23;
            _m[3, 0] = m30; _m[3, 1] = m31; _m[3, 2] = m32; _m[3, 3] = m33;
        }
        public Matrix4(Matrix4 oth)
        {
            _m = new float[4, 4];
            _m[0, 0] = oth._m[0, 0]; _m[0, 1] = oth._m[0, 1]; _m[0, 2] = oth._m[0, 2]; _m[0, 3] = oth._m[0, 3];
            _m[1, 0] = oth._m[1, 0]; _m[1, 1] = oth._m[1, 1]; _m[1, 2] = oth._m[1, 2]; _m[1, 3] = oth._m[1, 3];
            _m[2, 0] = oth._m[2, 0]; _m[2, 1] = oth._m[2, 1]; _m[2, 2] = oth._m[2, 2]; _m[2, 3] = oth._m[2, 3];
            _m[3, 0] = oth._m[3, 0]; _m[3, 1] = oth._m[3, 1]; _m[3, 2] = oth._m[3, 2]; _m[3, 3] = oth._m[3, 3];
        }
        #endregion Constructor

        #region Methods
        // 单位矩阵;
        public void Identity()
        {
            _m[0, 0] = 1f; _m[0, 1] = 0f; _m[0, 2] = 0f; _m[0, 3] = 0f;
            _m[1, 0] = 0f; _m[1, 1] = 1f; _m[1, 2] = 0f; _m[1, 3] = 0f;
            _m[2, 0] = 0f; _m[2, 1] = 0f; _m[2, 2] = 1f; _m[2, 3] = 0f;
            _m[3, 0] = 0f; _m[3, 1] = 0f; _m[3, 2] = 0f; _m[3, 3] = 1f;
        }
        // 行列式;
        public float Determinant()
        {
            float b00 = _m[0, 0] * _m[1, 1] - _m[0, 1] * _m[1, 0];
            float b01 = _m[0, 0] * _m[1, 2] - _m[0, 2] * _m[1, 0];
            float b02 = _m[0, 0] * _m[1, 3] - _m[0, 3] * _m[1, 0];
            float b03 = _m[0, 1] * _m[1, 2] - _m[0, 2] * _m[1, 1];
            float b04 = _m[0, 1] * _m[1, 3] - _m[0, 3] * _m[1, 1];
            float b05 = _m[0, 2] * _m[1, 3] - _m[0, 3] * _m[1, 2];
            float b06 = _m[2, 0] * _m[3, 1] - _m[2, 1] * _m[3, 0];
            float b07 = _m[2, 0] * _m[3, 2] - _m[2, 2] * _m[3, 0];
            float b08 = _m[2, 0] * _m[3, 3] - _m[2, 3] * _m[3, 0];
            float b09 = _m[2, 1] * _m[3, 2] - _m[2, 2] * _m[3, 1];
            float b10 = _m[2, 1] * _m[3, 3] - _m[2, 3] * _m[3, 1];
            float b11 = _m[2, 2] * _m[3, 3] - _m[2, 3] * _m[3, 2];
            return b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;
        }
        // 矩阵的转置;
        public Matrix4 Transpose()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = i; j < 4; j++)
                {
                    float temp = _m[i, j];
                    _m[i, j] = _m[j, i];
                    _m[j, i] = temp;
                }
            }
            return this;
        }
        // 逆矩阵;
        public static Matrix4 Inverse(Matrix4 mat)
        {
            float det =
                mat[0, 0] * (mat[1, 1] * mat[2, 2] - mat[1, 2] * mat[2, 1]) -
                mat[0, 1] * (mat[1, 0] * mat[2, 2] - mat[1, 2] * mat[2, 0]) +
                mat[0, 2] * (mat[1, 0] * mat[2, 1] - mat[1, 1] * mat[2, 0]);
            if (Math.Abs(det) < MathUtil.Epsilon)
            {
                return mat;
            }
            float det_inv = 1f / det;
            Matrix4 ret = Matrix4.identity;
            ret[0, 0] = det_inv * (mat[1, 1] * mat[2, 2] - mat[1, 2] * mat[2, 1]);
            ret[0, 1] = -det_inv * (mat[0, 1] * mat[2, 2] - mat[0, 2] * mat[2, 1]);
            ret[0, 2] = det_inv * (mat[0, 1] * mat[1, 2] - mat[0, 2] * mat[1, 1]);
            ret[0, 3] = 1f;

            ret[1, 0] = -det_inv * (mat[1, 0] * mat[2, 2] - mat[1, 2] * mat[2, 0]);
            ret[1, 1] = det_inv * (mat[0, 0] * mat[2, 2] - mat[0, 2] * mat[2, 0]);
            ret[1, 2] = -det_inv * (mat[0, 0] * mat[1, 2] - mat[0, 2] * mat[1, 0]);
            ret[1, 3] = 1f;

            ret[2, 0] = det_inv * (mat[1, 0] * mat[2, 1] - mat[1, 1] * mat[2, 0]);
            ret[2, 1] = -det_inv * (mat[0, 0] * mat[2, 1] - mat[0, 1] * mat[2, 0]);
            ret[2, 2] = det_inv * (mat[0, 0] * mat[1, 1] - mat[0, 1] * mat[1, 0]);
            ret[2, 3] = 1f;

            ret[3, 0] = -(mat[3, 0] * ret[0, 0] + mat[3, 1] * ret[1, 0] + mat[3, 2] * ret[2, 0]);
            ret[3, 1] = -(mat[3, 0] * ret[0, 1] + mat[3, 1] * ret[1, 1] + mat[3, 2] * ret[2, 1]);
            ret[3, 2] = -(mat[3, 0] * ret[0, 2] + mat[3, 1] * ret[1, 2] + mat[3, 2] * ret[2, 2]);
            ret[3, 3] = 1f;
            return ret;
        }
        public static Matrix4 MakeTrans(Vector3 trans)
        {
            return MakeTrans(trans.x, trans.y, trans.z);
        }
        public static Matrix4 MakeTrans(float x, float y, float z)
        {
            return new Matrix4(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    x, y, z, 1);
        }
        public static Matrix4 MakeScale(Vector3 scale)
        {
            return MakeScale(scale.x, scale.y, scale.z);
        }
        public static Matrix4 MakeScale(float x, float y, float z)
        {
            return new Matrix4(
                    x, 0, 0, 0,
                    0, y, 0, 0,
                    0, 0, z, 0,
                    0, 0, 0, 1);
        }
        // 欧拉角旋转矩阵;
        public static Matrix4 MakeRotationX(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix4(
                    1, 0, 0, 0,
                    0, cos, sin, 0,
                    0, -sin, cos, 0,
                    0, 0, 0, 1);
        }
        public static Matrix4 MakeRotationY(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix4(
                    cos, 0, -sin, 0,
                    0, 1, 0, 0,
                    sin, 0, cos, 0,
                    0, 0, 0, 1);
        }
        public static Matrix4 MakeRotationZ(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            return new Matrix4(
                    cos, sin, 0, 0,
                    -sin, cos, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1);
        }
        public static Matrix4 MakeRotationPYR(float pitch, float yaw, float roll)
        {
            Matrix4 xAxis = MakeRotationX(pitch);
            Matrix4 yAxis = MakeRotationY(yaw);
            Matrix4 zAxis = MakeRotationZ(roll);
            return new Matrix4(xAxis * yAxis * zAxis);
        }
        public static Matrix4 MakeRotationPYR(Vector3 rotation)
        {
            return MakeRotationPYR(rotation.x, rotation.y, rotation.z);
        }
        public static Matrix4 Rotation(Vector3 axis, float theta)
        {
            // 构造一个四元数旋转=》矩阵;
            float qx = axis.x * (float)Math.Sin(theta * 0.5f);
            float qy = axis.y * (float)Math.Sin(theta * 0.5f);
            float qz = axis.z * (float)Math.Sin(theta * 0.5f);
            float qw = (float)Math.Cos(theta * 0.5f);
            // 单位化;
            float mag2 = qw * qw + qx * qx + qy * qy + qz * qz;
            if (!MathUtil.IsZero(mag2) && (Math.Abs(mag2 - 1.0f) > MathUtil.Epsilon))
            {
                float mag = (float)Math.Sqrt(mag2);
                qw /= mag;
                qx /= mag;
                qy /= mag;
                qz /= mag;
            }
            // 转成矩阵;
            float x2 = qx * qx;
            float y2 = qy * qy;
            float z2 = qz * qz;
            float w2 = qw * qw;
            float xy = qx * qy;
            float xz = qx * qz;
            float yz = qy * qz;
            float wx = qw * qx;
            float wy = qw * qy;
            float wz = qw * qz;
            return new Matrix4(w2 + x2 - y2 - z2, 2.0f * (xy - wz), 2.0f * (xz + wy), 0.0f,
                2.0f * (xy + wz), w2 - x2 + y2 - z2, 2.0f * (yz - wx), 0.0f,
                2.0f * (xz - wy), 2.0f * (yz + wx), w2 - x2 - y2 + z2, 0.0f,
                0.0f, 0.0f, 0.0f, w2 + x2 + y2 + z2);
        }
        #endregion Methods

        #region Attributes
        public float this[int i, int j]
        {
            get
            { 
                if (i >= 4 || j >= 4)
                {
                    return 0;
                }
                if (i < 0 || j < 0)
                {
                    return 0;
                } 
                return _m[i, j]; 
            }
            set
            { 
                if (i >= 4 || j >= 4)
                { 
                    return;
                }
                if (i < 0 || j < 0)
                {
                    return;
                } 
                _m[i, j] = value;
            }
        }
        public Vector3 Translation
        {
            get
            {
                return new Vector3(_m[3, 0], _m[3, 1], _m[3, 2]);
            }
        }
        #endregion Attributes

        #region Fields
        public readonly static Matrix4 identity =
            new Matrix4(1f, 0f, 0f, 0f,
                        0f, 1f, 0f, 0f,
                        0f, 0f, 1f, 0f,
                        0f, 0f, 0f, 1f);
        private float[,] _m;
        #endregion Fields
    }
}
