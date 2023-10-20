// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapFloodFillExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapFloodFillExtensions
  {
    public static void FloodFill(
      this WriteableBitmap target,
      int x,
      int y,
      int outlineColor,
      int fillColor)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      List<WriteableBitmapFloodFillExtensions.Pnt> pntList = new List<WriteableBitmapFloodFillExtensions.Pnt>();
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
      {
        X = x,
        Y = y
      });
      while (pntList.Count > 0)
      {
        WriteableBitmapFloodFillExtensions.Pnt pnt = pntList[pntList.Count - 1];
        pntList.RemoveAt(pntList.Count - 1);
        if (pnt.X != -1 && pnt.X != pixelWidth && pnt.Y != -1 && pnt.Y != pixelHeight && pixels[pixelWidth * pnt.Y + pnt.X] != outlineColor && pixels[pixelWidth * pnt.Y + pnt.X] != fillColor)
        {
          pixels[pixelWidth * pnt.Y + pnt.X] = fillColor;
          pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
          {
            X = pnt.X,
            Y = pnt.Y - 1
          });
          pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
          {
            X = pnt.X + 1,
            Y = pnt.Y
          });
          pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
          {
            X = pnt.X,
            Y = pnt.Y + 1
          });
          pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
          {
            X = pnt.X - 1,
            Y = pnt.Y
          });
        }
      }
      target.Invalidate();
    }

    public static void FloodFillReplace(
      this WriteableBitmap target,
      int x,
      int y,
      int oldColor,
      int fillColor)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      List<WriteableBitmapFloodFillExtensions.Pnt> pntList = new List<WriteableBitmapFloodFillExtensions.Pnt>();
      pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
      {
        X = x,
        Y = y
      });
      while (pntList.Count > 0)
      {
        WriteableBitmapFloodFillExtensions.Pnt pnt = pntList[pntList.Count - 1];
        pntList.RemoveAt(pntList.Count - 1);
        if (pnt.X != -1 && pnt.X != pixelWidth && pnt.Y != -1 && pnt.Y != pixelHeight && pixels[pixelWidth * pnt.Y + pnt.X] == oldColor)
        {
          pixels[pixelWidth * pnt.Y + pnt.X] = fillColor;
          pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
          {
            X = pnt.X,
            Y = pnt.Y - 1
          });
          pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
          {
            X = pnt.X + 1,
            Y = pnt.Y
          });
          pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
          {
            X = pnt.X,
            Y = pnt.Y + 1
          });
          pntList.Add(new WriteableBitmapFloodFillExtensions.Pnt()
          {
            X = pnt.X - 1,
            Y = pnt.Y
          });
        }
      }
      target.Invalidate();
    }

    public static void FloodFillScanline(
      this WriteableBitmap target,
      int x,
      int y,
      int outlineColor,
      int fillColor)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      Stack<WriteableBitmapFloodFillExtensions.Pnt> pntStack = new Stack<WriteableBitmapFloodFillExtensions.Pnt>();
      pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
      {
        X = x,
        Y = y
      });
label_15:
      while (pntStack.Count > 0)
      {
        WriteableBitmapFloodFillExtensions.Pnt pnt = pntStack.Pop();
        x = pnt.X;
        y = pnt.Y;
        int num1 = y;
        while (num1 >= 0 && pixels[x + pixelWidth * num1] != fillColor && pixels[x + pixelWidth * num1] != outlineColor)
          --num1;
        int num2 = num1 + 1;
        bool flag1 = false;
        bool flag2 = false;
        while (true)
        {
          if (num2 < pixelHeight && pixels[x + pixelWidth * num2] != fillColor && pixels[x + pixelWidth * num2] != outlineColor)
          {
            pixels[x + pixelWidth * num2] = fillColor;
            if (!flag1 && x > 0 && pixels[x - 1 + pixelWidth * num2] != fillColor && pixels[x - 1 + pixelWidth * num2] != outlineColor)
            {
              pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
              {
                X = x - 1,
                Y = num2
              });
              flag1 = true;
            }
            else if (flag1 && x > 0 && (pixels[x - 1 + pixelWidth * num2] == fillColor || pixels[x - 1 + pixelWidth * num2] == outlineColor))
              flag1 = false;
            if (!flag2 && x < pixelWidth - 1 && pixels[x + 1 + pixelWidth * num2] != fillColor && pixels[x + 1 + pixelWidth * num2] != outlineColor)
            {
              pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
              {
                X = x + 1,
                Y = num2
              });
              flag2 = true;
            }
            else if (flag2 && x < pixelWidth - 1 && (pixels[x + 1 + pixelWidth * num2] == fillColor || pixels[x + 1 + pixelWidth * num2] == outlineColor))
              flag2 = false;
            ++num2;
          }
          else
            goto label_15;
        }
      }
    }

    public static void FloodFillScanlineReplace(
      this WriteableBitmap target,
      int x,
      int y,
      int oldColor,
      int fillColor)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      if (pixels[x + pixelWidth * y] == fillColor)
        return;
      Stack<WriteableBitmapFloodFillExtensions.Pnt> pntStack = new Stack<WriteableBitmapFloodFillExtensions.Pnt>();
      pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
      {
        X = x,
        Y = y
      });
label_17:
      while (pntStack.Count > 0)
      {
        WriteableBitmapFloodFillExtensions.Pnt pnt = pntStack.Pop();
        x = pnt.X;
        y = pnt.Y;
        int num1 = y;
        while (num1 >= 0 && pixels[x + pixelWidth * num1] == oldColor)
          --num1;
        int num2 = num1 + 1;
        bool flag1 = false;
        bool flag2 = false;
        while (true)
        {
          if (num2 < pixelHeight && pixels[x + pixelWidth * num2] == oldColor)
          {
            pixels[x + pixelWidth * num2] = fillColor;
            if (!flag1 && x > 0 && pixels[x - 1 + pixelWidth * num2] == oldColor)
            {
              pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
              {
                X = x - 1,
                Y = num2
              });
              flag1 = true;
            }
            else if (flag1 && x > 0 && pixels[x - 1 + pixelWidth * num2] != oldColor)
              flag1 = false;
            if (!flag2 && x < pixelWidth - 1 && pixels[x + 1 + pixelWidth * num2] == oldColor)
            {
              pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
              {
                X = x + 1,
                Y = num2
              });
              flag2 = true;
            }
            else if (flag2 && x < pixelWidth - 1 && pixels[x + 1 + pixelWidth * num2] != oldColor)
              flag2 = false;
            ++num2;
          }
          else
            goto label_17;
        }
      }
    }

    public static void FloodFillScanlineReplace(
      this WriteableBitmap target,
      int x,
      int y,
      int oldColor,
      int fillColor,
      byte maxDiff)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      if (pixels[x + pixelWidth * y] == fillColor)
        return;
      Stack<WriteableBitmapFloodFillExtensions.Pnt> pntStack = new Stack<WriteableBitmapFloodFillExtensions.Pnt>();
      pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
      {
        X = x,
        Y = y
      });
label_17:
      while (pntStack.Count > 0)
      {
        WriteableBitmapFloodFillExtensions.Pnt pnt = pntStack.Pop();
        x = pnt.X;
        y = pnt.Y;
        int num1 = y;
        while (num1 >= 0 && (int) pixels[x + pixelWidth * num1].MaxDiff(oldColor) < (int) maxDiff)
          --num1;
        int num2 = num1 + 1;
        bool flag1 = false;
        bool flag2 = false;
        while (true)
        {
          if (num2 < pixelHeight && (int) pixels[x + pixelWidth * num2].MaxDiff(oldColor) < (int) maxDiff)
          {
            pixels[x + pixelWidth * num2] = fillColor;
            if (!flag1 && x > 0 && (int) pixels[x - 1 + pixelWidth * num2].MaxDiff(oldColor) < (int) maxDiff)
            {
              pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
              {
                X = x - 1,
                Y = num2
              });
              flag1 = true;
            }
            else if (flag1 && x > 0 && (int) pixels[x - 1 + pixelWidth * num2].MaxDiff(oldColor) >= (int) maxDiff)
              flag1 = false;
            if (!flag2 && x < pixelWidth - 1 && (int) pixels[x + 1 + pixelWidth * num2].MaxDiff(oldColor) < (int) maxDiff)
            {
              pntStack.Push(new WriteableBitmapFloodFillExtensions.Pnt()
              {
                X = x + 1,
                Y = num2
              });
              flag2 = true;
            }
            else if (flag2 && x < pixelWidth - 1 && (int) pixels[x + 1 + pixelWidth * num2].MaxDiff(oldColor) >= (int) maxDiff)
              flag2 = false;
            ++num2;
          }
          else
            goto label_17;
        }
      }
    }

    private struct Pnt
    {
      public int X;
      public int Y;
    }
  }
}
