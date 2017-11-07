using System;
using System.Drawing.Imaging;
using System.ComponentModel;
using Framework.Maths;

namespace Views.Form.Materials
{
    public class Texture
    {
        #region Constructor
        protected Texture() { }
        public Texture(string path)
        {
            BitmapData bd = null;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                bitmap = image as System.Drawing.Bitmap;
                if (null != bitmap)
                {
                    _width = bitmap.Width;
                    _height = bitmap.Height;
                    bd = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, _width, _height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    if (null != bd)
                    {
                        _buffer = new byte[bd.Width * bd.Height * ImagePixelSize];
                        unsafe
                        {
                            int index = 0;
                            int width = bd.Width;
                            int height = bd.Height;
                            int stride = bd.Stride;
                            byte* ptr = (byte*)(bd.Scan0);
                            for (int i = 0; i < height; i++)
                            {
                                for (int j = 0; j < width; j++)
                                {
                                    _buffer[index] = ptr[0];
                                    _buffer[index + 1] = ptr[1];
                                    _buffer[index + 2] = ptr[2];
                                    _buffer[index + 3] = ptr[3];
                                    ptr += 4;
                                    index += 4;
                                }
                                ptr += stride - width * ImagePixelSize;
                                index += stride - width * ImagePixelSize;
                            }
                        }
                    }
                    bitmap.UnlockBits(bd);
                }
            }
            catch (System.Exception e)
            {
                if (null != bd && null != bitmap)
                {
                    bitmap.UnlockBits(bd);
                }
                _width = _height = 0;
                _buffer = null;
                _name = "Exception";
                throw e;
            }
        }
        #endregion Constructor

        #region Methods
        public override string ToString()
        {
            return string.Format("{0}({1},{2})", _name, _width, _height);
        }
        public Color GetColor(float _u, float _v)
        {
            if (null == _buffer)
            {
                return Color.White;
            }
            int u = Math.Abs((int)(_u * (float)(_width)) % _width);
            int v = Math.Abs((int)(_v * (float)(_height)) % _height);

            int pos = (u + v * _width) * 4;
            if (((pos + 3) >= _buffer.Length) || pos < 0)
            {
                return Color.White;
            }
            byte b = _buffer[pos];
            byte g = _buffer[pos + 1];
            byte r = _buffer[pos + 2];
            byte a = _buffer[pos + 3];

            return Color.FromArgb(a, r, g, b);
        }
        public Color GetColor(Vector2 uv)
        {
            return GetColor(uv.x, uv.y);
        }
        #endregion Methods

        #region Attributes
        [Browsable(false)]
        public string Name { set { _name = value; } get { return _name; } }
        [ReadOnly(true)]
        public int Width { set { _width = value; } get { return _width; } }
        [ReadOnly(true)]
        public int Height { set { _height = value; } get { return _height; } }
        #endregion Attributes

        #region Fields
        private const int ImagePixelSize = 4;
        public readonly static Texture WhiteBlock = new Texture()
        {
            _buffer = new byte[]{ 
                255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
                255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
                255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
                255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 },
            _width = 4,
            _height = 4,
            _name = "tex_WhiteBlock"
        };
        private byte[] _buffer = null;
        private int _width = 0, _height = 0;
        private string _name = string.Empty;
        #endregion Fields
    }
}
