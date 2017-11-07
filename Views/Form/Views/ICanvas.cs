using System;
using System.Drawing.Imaging;

namespace Views.Form.Views
{
    public interface ICanvas : IDisposable
    {
        #region Methods
        void Draw(byte[] backBuffer, byte[] frontBuffer);
        #endregion Methods

        #region Attributes
        PixelFormat PixFormat { set; get; }
        #endregion Attributes
    }
}
