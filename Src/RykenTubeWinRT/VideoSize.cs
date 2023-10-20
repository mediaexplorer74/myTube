// Decompiled with JetBrains decompiler
// Type: RykenTube.VideoSize
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Linq;

namespace RykenTube
{
  public struct VideoSize
  {
    public int Width;
    public int Height;

    public static bool TryParse(string value, out VideoSize size)
    {
      size = new VideoSize();
      char ch;
      if (value.Contains<char>('x'))
      {
        ch = 'x';
      }
      else
      {
        if (!value.Contains<char>('*'))
          return false;
        ch = '*';
      }
      string[] strArray = value.Split(new char[1]{ ch }, StringSplitOptions.RemoveEmptyEntries);
      return strArray.Length == 2 && int.TryParse(strArray[0], out size.Width) && int.TryParse(strArray[1], out size.Height);
    }

    public override string ToString() => string.Format("{0}x{1}", (object) this.Width, (object) this.Height);
  }
}
