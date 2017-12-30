using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OGLTest.Utilities
{
  class ImageDataLoader
  {
    public int Width { get; }
    public int Height { get; }
    public int Stride { get; }

    public int[,] Data { get; }

    public ImageDataLoader(string file)
    {
      using (Bitmap bitmap = new Bitmap(file))
      {
        Width = bitmap.Width;
        Height = bitmap.Height;
        Data = new int[Width, Height];

        var bitmapData = bitmap.LockBits(
          new Rectangle(0, 0, Width, Height),
          ImageLockMode.ReadOnly,
          PixelFormat.Format32bppArgb);

        FillData(bitmapData);

        bitmap.UnlockBits(bitmapData);
      }
    }

    private void FillData(BitmapData bitmapData)
    {
      IntPtr bitmapDataBytes = bitmapData.Scan0;
      for (int i = 0; i < Height; i++)
      {
        for (int j = 0; j < Width; j++)
        {
          var pixelColor = Marshal.ReadInt32(bitmapDataBytes);
          Data[i, j] = pixelColor;
          bitmapDataBytes = IntPtr.Add(bitmapDataBytes, 4);
        }
      }
    }

  }
}