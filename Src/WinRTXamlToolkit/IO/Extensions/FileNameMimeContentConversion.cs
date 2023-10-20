// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.IO.Extensions.FileNameMimeContentConversion
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.IO;

namespace WinRTXamlToolkit.IO.Extensions
{
  public static class FileNameMimeContentConversion
  {
    public static string GetMimeContentTypeFromFileName(this string fileName)
    {
      switch (Path.GetExtension(fileName))
      {
        case ".wav":
          return "audio/wav";
        case ".au":
          return "audio/basic";
        case ".snd":
          return "audio/basic";
        case ".mid":
          return "audio/mid";
        case ".rmi":
          return "audio/mid";
        case ".mp3":
          return "audio/mpeg";
        case ".aif":
          return "audio/x-aiff";
        case ".aifc":
          return "audio/x-aiff";
        case ".aiff":
          return "audio/x-aiff";
        case ".m3u":
          return "audio/x-mpegurl";
        case ".ra":
          return "audio/x-pn-realaudio";
        case ".ram":
          return "audio/x-pn-realaudio";
        default:
          return (string) null;
      }
    }
  }
}
