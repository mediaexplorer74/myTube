// Decompiled with JetBrains decompiler
// Type: RykenTube.Cipher
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Collections.Generic;
using System.Linq;

namespace RykenTube
{
  public class Cipher
  {
    public int Length;
    private string str = "";

    public string String
    {
      get => this.str;
      set => this.str = value.Replace(" ", "");
    }

    public static string NewDecipher(string a, string decipherString)
    {
      string[] strArray1 = a.Split("");
      foreach (string str in decipherString.Replace(" ", "").Replace("\n", "").Split(";"))
      {
        char[] chArray = new char[1]{ ':' };
        string[] strArray2 = str.Split(chArray);
        if (strArray2[0] == "slice")
        {
          int result;
          if (strArray2.Length != 0 && int.TryParse(strArray2[1], out result))
            strArray1 = ((IEnumerable<string>) strArray1).Skip<string>(result).ToArray<string>();
        }
        else if (strArray2[0] == "reverse")
        {
          strArray1 = ((IEnumerable<string>) strArray1).Reverse<string>().ToArray<string>();
        }
        else
        {
          int result;
          if (strArray2[0] == "d" && strArray2.Length != 0 && int.TryParse(strArray2[1], out result))
            strArray1 = Cipher.d(strArray1, result);
        }
      }
      a = "";
      for (int index = 0; index < strArray1.Length; ++index)
        a += strArray1[index];
      return a;
    }

    public static string FlatDecipher(string a)
    {
      string[] strArray = Cipher.d(((IEnumerable<string>) Cipher.d(((IEnumerable<string>) Cipher.d(((IEnumerable<string>) ((IEnumerable<string>) a.Split("")).Skip<string>(1).ToArray<string>()).Reverse<string>().ToArray<string>(), 41)).Reverse<string>().ToArray<string>(), 41)).Skip<string>(1).ToArray<string>(), 15);
      a = "";
      for (int index = 0; index < strArray.Length; ++index)
        a += strArray[index];
      return a;
    }

    private static string[] d(string[] a, int b)
    {
      string str = a[0];
      a[0] = a[b % a.Length];
      a[b] = str;
      return a;
    }
  }
}
