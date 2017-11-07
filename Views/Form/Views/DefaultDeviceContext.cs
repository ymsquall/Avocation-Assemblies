using Framework.Logger;
using System;
using System.Drawing.Imaging;

namespace Views.Form.Views
{
    public class DefaultDeviceContext : IDeviceContext
    {
        #region Methods
        public void ContentChange(int w, int h)
        {
            try
            {
                if (null != _canvas)
                {
                    _canvas.Dispose();
                }
                _canvas = new BitmapCanvas(w, h, PixelFormat.Format32bppArgb);
            }
            catch(Exception e)
            {
                LogSys.Popup("Exception", e.Message);
            }
        }
        public virtual bool Rasterization(byte[] backBuffer, byte[] frontBuffer)
        {
            if (null == _canvas)
            {
                return false;
            }
            _canvas.Draw(backBuffer, frontBuffer);
            return true;
        }
        public void Dispose()
        {
            if (null != _canvas)
            {
                _canvas.Dispose();
                _canvas = null;
            }
        }
        #endregion Methods

        #region Attributes
        public int PixelByteSize { get { return 4; } }
        public int BufferMaxSize { get { return CanvasBase.MaxSurfaceWidth * CanvasBase.MaxSurfaceHeight * PixelByteSize; } }
        public ICanvas Canvas { set { _canvas = value; } get { return _canvas; } }
        #endregion Attributes

        #region Fields
        private ICanvas _canvas = null;
        #endregion Fields
    }
}
