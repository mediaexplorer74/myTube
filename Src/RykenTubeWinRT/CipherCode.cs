// Decompiled with JetBrains decompiler
// Type: RykenTube.CipherCode
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

namespace RykenTube
{
  public struct CipherCode
  {
    public object Data;
    public CipherType Type;

    public CipherCode(CipherType type, object data)
    {
      this.Type = type;
      this.Data = data;
    }

    public CipherCode(CipherType type)
    {
      this.Type = type;
      this.Data = (object) null;
    }

    public override string ToString()
    {
      string str1 = (string) null;
      string str2 = (string) null;
      switch (this.Type)
      {
        case CipherType.Reverse:
          str1 = "reverse";
          break;
        case CipherType.Slice:
          str1 = "slice";
          break;
        case CipherType.Splice:
          str1 = "splice";
          break;
        case CipherType.D:
          str1 = "d";
          break;
      }
      if (this.Data != null)
        str2 = this.Data.ToString();
      if (str1 == null)
        return (string) null;
      string str3 = str1;
      if (str2 != null)
        str3 = str3 + ":" + str2;
      return str3;
    }
  }
}
