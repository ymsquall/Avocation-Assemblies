#if !UNITY
    using Bitmap = System.Drawing.Bitmap;
    using Rectangle = System.Drawing.Rectangle;
    using PixelFormat = System.Drawing.Imaging.PixelFormat;
    using ColorPalette = System.Drawing.Imaging.ColorPalette;
    using BitmapData = System.Drawing.Imaging.BitmapData;
    using ImageLockMode = System.Drawing.Imaging.ImageLockMode;
#else
    
#endif

namespace Framework.Maths.ColorSpace
{
    unsafe class ColorChanell
    {
        public enum Channel:int
        {
            One=0,
            Two=1,
            Three=2
        }
        
        public unsafe static Bitmap GetChannel(Bitmap SrcImg,Channel channel)
        {
            int X, Y,Width,Height,StrideS,StrideD;
            byte* PointerS, PointerD;
            Bitmap DestImg = new Bitmap(SrcImg.Width, SrcImg.Height, PixelFormat.Format8bppIndexed);
            Width = SrcImg.Width; Height = SrcImg.Height;
            ColorPalette Pal = DestImg.Palette;
            for (Y = 0; Y < Pal.Entries.Length; Y++)   Pal.Entries[Y] = Color.FromArgb(255, Y, Y, Y);
            DestImg.Palette = Pal;
            BitmapData SrcData = SrcImg.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData DestData = DestImg.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            StrideS = SrcData.Stride; StrideD = DestData.Stride;
            for (Y = 0; Y < Height; Y++)
            {
                PointerS = (byte*)SrcData.Scan0 + Y * StrideS;
                PointerD = (byte*)DestData.Scan0 + Y * StrideD;
                for (X = 0; X < Width; X++)
                {
                    *PointerD = *(PointerS+(int)channel);
                    PointerS += 3;
                    PointerD++;
                }
            }
            SrcImg.UnlockBits(SrcData);
            DestImg.UnlockBits(DestData);
            return DestImg;
        }
        
        public unsafe static Bitmap GetLabImage(Bitmap SrcImg)
        {
            Bitmap DestImg = new Bitmap(SrcImg.Width, SrcImg.Height, PixelFormat.Format24bppRgb);
            BitmapData SrcData = SrcImg.LockBits(new Rectangle(0, 0, SrcImg.Width, SrcImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData DestData = DestImg.LockBits(new Rectangle(0, 0, DestImg.Width, DestImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            for (int Y = 0; Y < SrcImg.Height; Y++)
                RGBLAB.ToLAB((byte*)(SrcData.Scan0 + Y * SrcData.Stride), (byte*)(DestData.Scan0 + Y * DestData.Stride), SrcData.Width);
            SrcImg.UnlockBits(SrcData);
            DestImg.UnlockBits(DestData);
            return DestImg;
        }
        
        public unsafe static Bitmap GetXYZImage(Bitmap SrcImg)
        {
            Bitmap DestImg = new Bitmap(SrcImg.Width, SrcImg.Height, PixelFormat.Format24bppRgb);
            BitmapData SrcData = SrcImg.LockBits(new Rectangle(0, 0, SrcImg.Width, SrcImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData DestData = DestImg.LockBits(new Rectangle(0, 0, DestImg.Width, DestImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            for (int Y = 0; Y < SrcImg.Height; Y++)
            {
                RGBXYZ.ToXYZ((byte*)(SrcData.Scan0 + Y * SrcData.Stride), (byte*)(DestData.Scan0 + Y * DestData.Stride), SrcData.Width);
            }
            SrcImg.UnlockBits(SrcData);
            DestImg.UnlockBits(DestData);
            return DestImg;
        }

        public unsafe static Bitmap GetYCbCrImage(Bitmap SrcImg)
        {
            Bitmap DestImg = new Bitmap(SrcImg.Width, SrcImg.Height, PixelFormat.Format24bppRgb);
            BitmapData SrcData = SrcImg.LockBits(new Rectangle(0, 0, SrcImg.Width, SrcImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData DestData = DestImg.LockBits(new Rectangle(0, 0, DestImg.Width, DestImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            for (int Y = 0; Y < SrcImg.Height; Y++)
                RGBYCbCr.ToYCbCr((byte*)(SrcData.Scan0 + Y * SrcData.Stride), (byte*)(DestData.Scan0 + Y * DestData.Stride), SrcData.Width);
            SrcImg.UnlockBits(SrcData);
            DestImg.UnlockBits(DestData);
            return DestImg;
        }

        public unsafe static Bitmap GetYDbDrImage(Bitmap SrcImg)
        {
            Bitmap DestImg = new Bitmap(SrcImg.Width, SrcImg.Height, PixelFormat.Format24bppRgb);
            BitmapData SrcData = SrcImg.LockBits(new Rectangle(0, 0, SrcImg.Width, SrcImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData DestData = DestImg.LockBits(new Rectangle(0, 0, DestImg.Width, DestImg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            for (int Y = 0; Y < SrcImg.Height; Y++)
                RGBYDbDr.ToYDbDr((byte*)(SrcData.Scan0 + Y * SrcData.Stride), (byte*)(DestData.Scan0 + Y * DestData.Stride), SrcData.Width);
            SrcImg.UnlockBits(SrcData);
            DestImg.UnlockBits(DestData);
            return DestImg;
        }

    }
}
