using System;

namespace Views.Form.Views
{
    public interface IDeviceContext : IDisposable
    {
        #region Methods
        void ContentChange(int w, int h);
        bool Rasterization(byte[] backBuffer, byte[] frontBuffer);
        #endregion Methods

        #region Attributes
        int PixelByteSize { get; }
        int BufferMaxSize { get; }
        ICanvas Canvas { set; get; }
        #endregion Attributes
    }
}
