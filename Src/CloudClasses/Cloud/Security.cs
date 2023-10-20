// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Security
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using System;
using System.Text;

namespace myTube.Cloud
{
  public static class Security
  {
    private static string acceptableChars = "01234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-.";

    public static string TransformString(string UntransformedToken)
    {
      if (string.IsNullOrWhiteSpace(UntransformedToken))
        return (string) null;
      Random random = new Random(12);
      byte[] bytes1 = Encoding.UTF8.GetBytes(UntransformedToken);
      byte[] bytes2 = new byte[bytes1.Length];
      bytes1.CopyTo((Array) bytes2, 0);
      byte num1 = bytes2[bytes2.Length - 1];
      for (int index1 = 0; index1 < bytes1.Length; ++index1)
      {
        byte num2 = bytes1[index1];
        byte num3 = num2;
        byte num4 = (byte) ((uint) num2 + (uint) num1);
        byte num5 = (byte) ((uint) num4 + 24U);
        byte num6 = (byte) ((uint) (byte) ((uint) num4 * (uint) num5) / 2U);
        byte num7 = (byte) ((uint) num6 + (uint) (byte) ((uint) num6 % (uint) sbyte.MaxValue));
        byte[] buffer1 = new byte[1];
        random.NextBytes(buffer1);
        byte[] buffer2 = new byte[1];
        for (int index2 = 0; index2 < (int) buffer1[0]; ++index2)
        {
          random.NextBytes(buffer2);
          num7 += buffer2[0];
        }
        byte index3 = (byte) ((uint) (byte) ((uint) (byte) ((uint) num7 - 7U) * 3U) % (uint) Security.acceptableChars.Length);
        bytes2[index1] = (byte) Security.acceptableChars[(int) index3];
        num1 = num3;
      }
      return Encoding.UTF8.GetString(bytes2, 0, bytes2.Length);
    }
  }
}
