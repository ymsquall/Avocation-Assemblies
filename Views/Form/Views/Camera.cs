using System;
using System.ComponentModel;
using Framework.Maths;
using Views.Form.Views.Convert;

namespace Views.Form.Views
{
    public class Camera : ICamera, IDisposable
    {
        #region Defines
        public enum ClearType
        {
            DontClear,
            SolidColor,
            DepthOnly,
        }
        //public enum FrustumFace
        //{
        //    Near,
        //    Far, 
        //    Left, 
        //    Right,
        //    Top,
        //    Bottom,
        //}
        #endregion Defines

        #region Events
        public delegate void BGColorHandler(Color oldColor, Color newColor);
        public BGColorHandler OnBGColorChanged;
        #endregion Events

        #region Constructor
        private Camera() { }
        #endregion Constructor

        #region Methods
        public static Camera BuildCamera(float near, float far, float fov, float aspect)
        {
            Camera ret = new Camera();
            ret._neerDist = near;
            ret._farDist = far;
            ret._aspect = aspect;
            ret._fov = (float)Math.Tan(MathUtil.Angle2Radian(fov * 0.5f));
            return ret;
        }
        public void SetLookAt(Vector3 lookat)
        {
            _lookDir = lookat - _position;
            _distance = _lookDir.Normalize();
        }
        public void ViewSizeChanged(int width, int height)
        {
            if (width == 0 || height == 0)
            {
                return;
            }
            _aspect = (float)width / (float)height;
        }
        public void Dispose()
        {

        }
        // 以观察者的位置，观察位置以及up向量构造该观察者的视图矩阵;
        public Matrix4 LookAtLH()
        {
            Vector3 xAxis = Vector3.Normalize(Vector3.Cross(Vector3.up, _lookDir));
            Vector3 yAxis = Vector3.Normalize(Vector3.Cross(_lookDir, xAxis));
            float dXPos = -Vector3.Dot(xAxis, _position);
            float dYPos = -Vector3.Dot(yAxis, _position);
            float dZPos = -Vector3.Dot(_lookDir, _position);
            /*
             * xAxis.x      yAxis.x     zAxis.x     0
             * xAxis.y      yAxis.y     zAxis.y     0
             * xAxis.z      yAxis.z     zAxis.z     0
             * dXPos        dYPos       dZPos       1
             */
            return new Matrix4(
                xAxis.x, yAxis.x, _lookDir.x, 0,
                xAxis.y, yAxis.y, _lookDir.y, 0,
                xAxis.z, yAxis.z, _lookDir.z, 0,
                dXPos, dYPos, dZPos, 1);
        }

        public Matrix4 PerspectiveFovLH()//float fov, float aspect, float near, float far)
        {
            /*
             * w        0       0                       0
             * 0        h       0                       0
             * 0        0       far/(far-near)          1
             * 0        0       -near*far/(far-near)    0
             */
            float tanFov = (float)Math.Tan(_fov * 0.5f);
            float h = 1f / (MathUtil.NotBeZero(tanFov));
            float w = h / (MathUtil.NotBeZero(_aspect));
            float fn = _farDist - _neerDist;
            return new Matrix4(
                w, 0, 0, 0,
                0, h, 0, 0,
                0, 0, _farDist / (MathUtil.NotBeZero(fn)), 1,
                0, 0, (-_neerDist * _farDist) / (MathUtil.NotBeZero(fn)), 0);
        }
        public Matrix4 PerspectiveFovRH()//float fov, float aspect, float near, float far)
        {
            /*
             * w        0       0                       0
             * 0        h       0                       0
             * 0        0       far/(near-far)         -1
             * 0        0       near*far/(near-far)     0
             */
            float tanFov = (float)Math.Tan(_fov * 0.5f);
            float h = 1f / (MathUtil.NotBeZero(tanFov));
            float w = h / (MathUtil.NotBeZero(_aspect));
            float nf = _neerDist - _farDist;
            return new Matrix4(
                w, 0, 0, 0,
                0, h, 0, 0,
                0, 0, _farDist / (MathUtil.NotBeZero(nf)), -1,
                0, 0, (_neerDist * _farDist) / (MathUtil.NotBeZero(nf)), 0);
        }
        public void UpdateMatrixs()
        {
            viewMatrix = LookAtLH();
            projMatrix = PerspectiveFovLH();
        }
        public void RestPosAndDir()
        {
            _position = DefaultPosition;
            _lookDir = DefaultLookDir;
        }
        public void StartMove(Vector3 dir)
        {
            _moveDir += dir;
        }
        public void EndMove(Vector3 dir)
        {
            _moveDir -= dir;
        }
        public void Move(Vector3 dir, float delta)
        {
            Vector3 zAxis = Vector3.Normalize(_lookDir);
            Vector3 xAxis = Vector3.Normalize(Vector3.Cross(Vector3.up, _lookDir));
            Vector3 yAxis = Vector3.Normalize(Vector3.Cross(zAxis, xAxis));
            Vector3 worldDir = zAxis * dir.z + xAxis * dir.x + yAxis * dir.y;
            _position = _position + worldDir * delta;
        }
        public void YawPitchRoll(float yaw, float pitch, float roll, bool lockTarget)
        {
            if (lockTarget)
            {
                Vector3 lookat = _position + _lookDir * _distance;
                Matrix4 rotaMat = Matrix4.MakeRotationPYR(0, yaw, roll);
                Vector3 backLookDir = new Vector3(-_lookDir.x, -_lookDir.y, -_lookDir.z);
                backLookDir = rotaMat * backLookDir;
                backLookDir.y -= pitch;
                backLookDir.Normalize();
                _lookDir = new Vector3(-backLookDir.x, -backLookDir.y, -backLookDir.z);
                _position = lookat - _lookDir * _distance;
            }
            else
            {
                _lookDir.x -= yaw;
                _lookDir.y -= pitch;
                _lookDir.z -= roll;
                _lookDir.Normalize();
            }
        }
        public void Update(float deltaTime)
        {
            // 操作更新;
            Vector3 moveDir = new Vector3(_moveDir);
            float moveLen = moveDir.Normalize();
            if(moveLen > 0)
            {
                // 移动;
                float deltaDist = deltaTime * 4f;
                Move(moveDir, deltaDist);
            }
        }
        public bool BackfaceBeCulled(Vector3 p1WorldPos, Vector3 p2WorldPos, Vector3 p3WorldPos)
        {
            // 背面剔除;
            if (!_backFaceCull)
            {
                return false;
            }
            Plane plane = Plane.PlaneFromPoints(p1WorldPos, p2WorldPos, p3WorldPos);
            Vector3 center = (p1WorldPos + p2WorldPos + p3WorldPos) / 3f;
            Vector3 camNorm = Vector3.Normalize(center - _position);
            float dotVal = Vector3.Dot(plane.Normal, camNorm);
            if (dotVal > 0)
            {
                // 法线和视线同向，不绘制;
                return true;
            }
            return false;
        }
        public int VisibleTest(Vector3 viewPos0, Vector3 viewPos1, Vector3 viewPos2,
            ref bool p0Inside, ref bool p1Inside, ref bool p2Inside)
        {
            int outsideNum = 0;
            p0Inside = false; p1Inside = false; p2Inside = false;
            // 对近裁剪平面进行三角形分割处理，与其它裁剪平面相交的三角形交给光栅化时的屏幕空间裁剪;
            // p1;
            if (viewPos0.z > _farDist)
            {
                // 超出远裁剪面;
                outsideNum++;
            }
            else if (viewPos0.z < _neerDist)
            {
                // 超出近裁剪面;
                outsideNum++;
            }
            else
            {
                // 在裁剪面内;
                p0Inside = true;
            }
            // p2;
            if (viewPos1.z > _farDist)
            {
                // 超出远裁剪面;
                outsideNum++;
            }
            else if (viewPos1.z < _neerDist)
            {
                // 超出近裁剪面;
                outsideNum++;
            }
            else
            {
                // 在裁剪面内;
                p1Inside = true;
            }
            // p3;
            if (viewPos2.z > _farDist)
            {
                // 超出远裁剪面;
                outsideNum++;
            }
            else if (viewPos2.z < _neerDist)
            {
                // 超出近裁剪面;
                outsideNum++;
            }
            else
            {
                // 在裁剪面内;
                p2Inside = true;
            }
            return outsideNum;
        }
        #endregion Methods

        #region Attributes
        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Color BGColor
        {
            set
            {
                if (_backgroundColor != value)
                {
                    if (null != OnBGColorChanged)
                    {
                        OnBGColorChanged(_backgroundColor, value);
                    }
                }
                _backgroundColor = value;
            }
            get { return _backgroundColor; }
        }
        public ClearType ClearMode { set; get; }

        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 Pos
        {
            set { _position = value; }
            get { return _position; }
        }
        [TypeConverterAttribute(typeof(MathConverter)),
            EditorAttribute(typeof(MathUITypeEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public Vector3 LookDir
        {
            set { _lookDir = value; }
            get { return _lookDir; }
        }
        public float Fov 
        { 
            set { _fov = value; } 
            get { return _fov; }
        }
        public float Near
        { 
            set { _neerDist = value; }
            get { return _neerDist; }
        }
        public float Far
        { 
            set { _farDist = value; } 
            get { return _farDist; }
        }
        public bool BackFaceCull
        { 
            set { _backFaceCull = value; } 
            get { return _backFaceCull; } 
        }
        #endregion Attributes

        #region Fields
        public readonly static Camera Default = new Camera();
        public readonly static Vector3 DefaultPosition = new Vector3(0f, 0f, -10f);
        public readonly static Vector3 DefaultLookDir = Vector3.front;
        private Color _backgroundColor = Color.Black;
        private Vector3 _position = DefaultPosition;
        private Vector3 _lookDir = DefaultLookDir;
        private float _fov = 0.78f;          // 视野范围;
        private float _aspect = 1.5f;        // 宽高比;
        private float _neerDist = 0.01f;     // 近裁剪平面距离;
        private float _farDist = 100f;       // 远裁剪平面距离;
        private bool _backFaceCull = true;
        private Vector3 _moveDir = Vector3.zero;
        private float _distance = 10f;
        public Matrix4 viewMatrix = Matrix4.identity;
        public Matrix4 projMatrix = Matrix4.identity;
        #endregion Fields
    }
}
