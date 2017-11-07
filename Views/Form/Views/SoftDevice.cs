using Views.Form.Shape;
using System;
using System.Collections.Generic;
using Framework.Maths;
using Views.Form.Views.Lights;

namespace Views.Form.Views
{
    public enum RenderType
    {
        WireFrame,
        Solid,
    }
    public class SoftDevice
    {
        #region Methods
        public void InitDevice()
        {
            if (_inited)
            {
                return;
            }
            _context = new DefaultDeviceContext();
            _inited = true;
        }
        public void DestoryDevice()
        {
            if(!_inited)
            {
                return;
            }
            ReleaseDevice();
            if (null != _context)
            {
                _context.Dispose();
                _context = null;
            }
            _inited = false;
        }
        public bool ResetDevice(int width, int height)
        {
            if (!_inited)
            {
                return false;
            }
            int pixelCount = width * height;
            int len = pixelCount * _context.PixelByteSize;
            if (len > _context.BufferMaxSize)
            {
                return false;
            }
            _width = width; _height = height;
            _backBuffer = new byte[len];
            _frontBuffer = new byte[len];
            _depthBuffer = new float[pixelCount];
            _context.ContentChange(_width, _height);
            _backgroundColorChanged = true;
            return true;
        }
        public void RestoreDevice(int width, int height)
        {
            if (!_inited || width == 0 || height == 0)
            {
                return;
            }
            ReleaseDevice();
            ResetDevice(width, height);
            _camera.ViewSizeChanged(width, height);
        }
        public void ReleaseDevice()
        {
            if (!_inited)
            {
                return;
            }
            if (null != _backBuffer)
            {
                _backBuffer = null;
            }
            if (null != _frontBuffer)
            {
                _frontBuffer = null;
            }
            if (null != _depthBuffer)
            {
                _depthBuffer = null;
            }
            _width = _height = 0;
        }
        private void _SortShapeList(List<IShape> shapes)
        {

        }
        private void _Present()
        {
            if (!_inited || null == _backBuffer || null == _frontBuffer)
            {
                return;
            }
            _context.Rasterization(_backBuffer, _frontBuffer);
        }
        private void _Clear()
        {
            if (!_inited)
            {
                return;
            }
            switch (_camera.ClearMode)
            {
                case Camera.ClearType.SolidColor:
                    _frontBuffer = new byte[_width * _height * 4];
                    break;
                case Camera.ClearType.DepthOnly:
                    break;
                case Camera.ClearType.DontClear:
                    return;
            }
            int depthLength = _depthBuffer.Length;
            float depthValue = 0;
            if (_usePerspectiveCorrection)
            {
                depthValue = float.MinValue;
            }
            else
            {
                depthValue = float.MaxValue;
            }
            for (int i = 0; i < depthLength; ++i)
            {
                _depthBuffer[i] = depthValue;
            }
        }
        public bool ZTest(int x, int y, float zBuff)
        {
            // 屏幕裁剪;
            if (!_inited)
            {
                return false;
            }
            if (x < 0 || y < 0 || x >= _width || y >= _height)
            {
                return false;
            }
            if (_renderMode == RenderType.WireFrame)
            {
                return true;
            }
            // ztest;
            int pixIdx = y * _width + x;
            if (_usePerspectiveCorrection)
            {
                if (_depthBuffer[pixIdx] > zBuff)
                {
                    // 深度剔除 ztest;
                    return false;
                }
            }
            else
            {
                if (_depthBuffer[pixIdx] < zBuff)
                {
                    return false;
                }
            }
            return true;
        }
        private void _SetPixel(int x, int y, float zBuff, Color c)
        {
            if (!_inited)
            {
                return;
            }
            if (x < 0 || y < 0 || x >= _width || y >= _height)
            {
                return;
            }
            int pixIdx = y * _width + x;
            _depthBuffer[pixIdx] = zBuff;
            int index = pixIdx * 4;
            _frontBuffer[index] = c.ByteA;
            _frontBuffer[index + 1] = c.ByteR;
            _frontBuffer[index + 2] = c.ByteG;
            _frontBuffer[index + 3] = c.ByteB;
        }
        // 渲染流程;
        private void _Render(List<IShape> shapes, List<IPosLight> lights)
        {
            if (!_inited || null == shapes || null == lights || _height == 0 || _width == 0)
            {
                return;
            }
            // 清屏;
            _Clear();
            // 按队列排序形状列表(未实现);
            _SortShapeList(shapes);
            // 更新view proj矩阵;
            _camera.UpdateMatrixs();
            // 收集顶点进行坐标变换，得到其在屏幕上的位置;
            int shapeCount = shapes.Count;
            for (int i = 0; i < shapeCount; ++i)
            {
                IShape shape = shapes[i];
                if (null == shape || !shape.Enabled)
                {
                    continue;
                }
                // 计算旋转位移和缩放;
                Matrix4 worldMat = shape.BuildWorldMatrix();
                // 合并变换;
                var transformMat = worldMat * _camera.viewMatrix * _camera.projMatrix;
                // 收集顶点准备进行绘制;
                shape.CollectVertex(transformMat, _width, _height, _renderMode, lights, _SetPixel);
            }
            // 将像素点光栅化到图片上;
            _Present();
        }
        public void RenderOneFrame(List<IShape> shapes, List<IPosLight> lights)
        {
            if (!_inited)
            {
                return;
            }
            // 背景颜色;
            if (_backgroundColorChanged)
            {
                Color clr = _camera.BGColor;
                byte[] buff = new byte[] { clr.ByteA, clr.ByteR, clr.ByteG, clr.ByteB };
                int pixelCount = _backBuffer.Length / _context.PixelByteSize;
                for (int i = 0; i < pixelCount; ++i)
                {
                    Array.Copy(buff, 0, _backBuffer, i * buff.Length, buff.Length);
                }
                _backgroundColorChanged = false;
            }
            // 进入渲染流程;
            _Render(shapes, lights);
        }
        private void _OnBGColorChanged(Color co, Color cn)
        {
            _backgroundColorChanged = true;
        }
        #endregion Methods

        #region Attributes
        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
        public IDeviceContext Context { get { return _context; } }
        public Camera Camera
        {
            set
            {
                if (null != _camera)
                {
                    _camera.OnBGColorChanged = null;
                    _camera.Dispose();
                }
                _camera = value;
                _camera.OnBGColorChanged = _OnBGColorChanged;
            }
            get { return _camera; }
        }
        public RenderType RenderMode
        { 
            set { _renderMode = value; } 
            get { return _renderMode; } 
        }
        public bool PerspectiveCorrection 
        { 
            set { _usePerspectiveCorrection = value; } 
            get { return _usePerspectiveCorrection; }
        }
        #endregion Attributes

        #region Fields
        public readonly static SoftDevice Default = new SoftDevice();
        private bool _inited = false;
        private IDeviceContext _context = null;
        private Camera _camera = null;
        private int _width = 0, _height = 0;
        private byte[] _backBuffer = null, _frontBuffer = null;
        private float[] _depthBuffer = null;    // 深度缓冲区;
        private RenderType _renderMode = RenderType.WireFrame;
        private bool _backgroundColorChanged = false;
        private bool _usePerspectiveCorrection = true;  // 使用透视修正纹理映射;
        #endregion Fields
    }
}
