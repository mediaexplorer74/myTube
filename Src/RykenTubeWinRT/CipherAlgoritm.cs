// Decompiled with JetBrains decompiler
// Type: RykenTube.CipherAlgoritm
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Collections.Generic;

namespace RykenTube
{
  public class CipherAlgoritm
  {
    private List<CipherCode> codes;

    public CipherAlgoritm() => this.codes = new List<CipherCode>();

    public void Add(CipherCode code) => this.codes.Add(code);

    public override string ToString() => this.ToString("; ");

    public string ToString(string separator)
    {
      string str1 = "";
      foreach (CipherCode code in this.codes)
      {
        string str2 = code.ToString();
        if (str2 != null)
          str1 = separator == null ? str1 + str2 : str1 + str2 + separator;
      }
      return str1;
    }
  }
}
