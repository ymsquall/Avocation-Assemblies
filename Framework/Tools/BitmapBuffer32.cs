using Framework.Maths;

namespace Framework.Tools
{
    public class BitmapBuffer32
    {
        public const byte BytesPerPixel = 4;
        private int mStride = 0;
        private short mWidth = 0, mHeight = 0;
        private byte[] mBuffer;
        private int mPixelCount = 0;

        public BitmapBuffer32(int w, int h)
        {
            Rebuild((short)w, (short)h);
        }
        public BitmapBuffer32(short w, short h)
        {
            Rebuild(w, h);
        }

        public int Width { get { return mWidth; } }
        public int Height { get { return mHeight; } }
        public int Stride { get { return mStride; } }
        public byte[] Buffer { get { return mBuffer; } }

        private Color tempColor = Color.Black;
        public Color this[int index]
        {
            get
            {
                if (index < 0 || index >= mPixelCount)
                {
                    tempColor = Color.Black;
                }
                else
                {
                    index *= BytesPerPixel;
#if WPF
                    tempColor = Color.FromArgb(mBuffer[index + 3], mBuffer[index + 2], mBuffer[index + 1], mBuffer[index]);
#else
                    tempColor = Color.FromArgb(buffer[index], buffer[index + 1], buffer[index + 2], buffer[index + 3]);
#endif
                }
                return tempColor;
            }
            set
            {
                if (index < 0 || index >= mPixelCount)
                {
                    return;
                }
                index *= BytesPerPixel;
#if WPF
                mBuffer[index + 0] = value.ByteB;
                mBuffer[index + 1] = value.ByteG;
                mBuffer[index + 2] = value.ByteR;
                mBuffer[index + 3] = value.ByteA;
#else
                mBuffer[index + 0] = value.ByteA;
                mBuffer[index + 1] = value.ByteR;
                mBuffer[index + 2] = value.ByteG;
                mBuffer[index + 3] = value.ByteB;
#endif
            }
        }
        public Color this[short row, short col]
        {
            get
            {
                return this[row * mWidth + col];
            }
            set
            {
                this[row * mWidth + col] = value;
            }
        }

        public void Rebuild(short w, short h)
        {
            mWidth = w; mHeight = h;
            mStride = w * BytesPerPixel;
            mPixelCount = w * h;
            mBuffer = new byte[mPixelCount * BytesPerPixel];
        }
    }
}
