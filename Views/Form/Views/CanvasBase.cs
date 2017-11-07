using System;
using System.Drawing.Imaging;

namespace Views.Form.Views
{
    public abstract class CanvasBase : ICanvas
    {
        #region Constructor
        private CanvasBase() { }
        public CanvasBase(int w, int h, PixelFormat pf)
        {
            if (w <= 0 || h <= 0)
            {
                throw new Exception(string.Format("画布尺寸非法[{0}, {1}], 无法创建宽度或高度<0的画布", w, h));
            }
            if (w > MaxSurfaceWidth || h > MaxSurfaceHeight)
            {
                throw new Exception(string.Format("画布尺寸非法[{0}, {1}], 无法创建宽度>{2}或高度>{3}的画布", w, h, MaxSurfaceWidth, MaxSurfaceHeight));
            }
            PixFormat = pf;
        }
        #endregion Constructor

        #region Methods
        public abstract void Draw(byte[] backBuffer, byte[] frontBuffer);
        public virtual void Dispose() { }
        #endregion Methods

        #region Attributes
        public PixelFormat PixFormat { set; get; }
        #endregion Attributes

        #region Fields
        public const int MaxSurfaceWidth = 4096;
        public const int MaxSurfaceHeight = 4096;
        #endregion Fields
    }
}
