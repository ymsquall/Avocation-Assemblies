using System;
using System.Drawing.Imaging;

namespace Views.Form.Views
{
    public class BitmapCanvas : CanvasBase
    {
        #region Constructor
        public BitmapCanvas(int w, int h, PixelFormat pf) : base(w, h, pf)
        {
            _bitmap = new System.Drawing.Bitmap(w, h, PixFormat);
        }
        #endregion Constructor

        #region Methods
        public static System.Drawing.Bitmap GetBitmap(ICanvas canvas)
        {
            if (null == canvas)
            {
                return null;
            }
            BitmapCanvas bitmapCanvas = canvas as BitmapCanvas;
            if (null == bitmapCanvas)
            {
                return null;
            }
            return bitmapCanvas._bitmap;
        }
        public override void Draw(byte[] backBuffer, byte[] frontBuffer)
        {
            if (null == backBuffer || null == frontBuffer)
            {
                return;
            }
            if (null == _bitmap)
            {
                return;
            }
            BitmapData data = null;
            try
            {
                data = _bitmap.LockBits(new System.Drawing.Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadWrite, PixFormat);
                int stride = data.Stride;
                if (backBuffer.Length != (stride * _bitmap.Height))
                {
                    _bitmap.UnlockBits(data);
                    return;
                }
                unsafe
                {
                    int index = 0;
                    int width = data.Width;
                    int height = data.Height;
                    int backBuffLength = backBuffer.Length;
                    int frontBuffLength = frontBuffer.Length;
                    byte* ptr = (byte*)(data.Scan0);
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if (index >= backBuffLength || index >= frontBuffLength)
                            {
                                _bitmap.UnlockBits(data);
                                return;
                            }
                            byte fa = frontBuffer[index];
                            if (fa == 0)            // alpha test volume ?;
                            {
                                ptr[3] = backBuffer[index];         // a;
                                ptr[2] = backBuffer[index + 1];     // r;
                                ptr[1] = backBuffer[index + 2];     // g;
                                ptr[0] = backBuffer[index + 3];     // b;
                            }
                            else
                            {
                                ptr[3] = fa;        // a;
                                ptr[2] = frontBuffer[index + 1];    // r;
                                ptr[1] = frontBuffer[index + 2];    // g;
                                ptr[0] = frontBuffer[index + 3];    // b;
                            }
                            ptr += 4;
                            index += 4;
                        }
                        ptr += stride - width * 4;
                        index += stride - width * 4;
                    }
                }
                _bitmap.UnlockBits(data);
            }
            catch (Exception e)
            {
                if (null != data)
                {
                    _bitmap.UnlockBits(data);
                    data = null;
                }
                throw e;
            }
        }
        public override void Dispose()
        {
            if (null != _bitmap)
            {
                _bitmap.Dispose();
                _bitmap = null;
            }
        }
        #endregion Methods

        #region Fields
        private System.Drawing.Bitmap _bitmap;
        #endregion Fields
    }
}
