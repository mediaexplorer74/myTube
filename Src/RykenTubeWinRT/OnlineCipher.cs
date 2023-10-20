// Decompiled with JetBrains decompiler
// Type: RykenTube.OnlineCipher
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace RykenTube
{
  public static class OnlineCipher
  {
    public static event OnlineCipherEventHandler Completed;

    public static void DownloadCiphers(Uri path)
    {
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(path);
      req.BeginGetResponse((AsyncCallback) (a =>
      {
        Stream responseStream;
        try
        {
          responseStream = req.EndGetResponse(a).GetResponseStream();
        }
        catch
        {
          if (OnlineCipher.Completed == null)
            return;
          OnlineCipher.Completed(new OnlineCipherEventArgs()
          {
            Online = true,
            Result = (string) null
          });
          return;
        }
        StreamReader streamReader = new StreamReader(responseStream);
        List<Cipher> cipherList = new List<Cipher>();
        string str1 = "";
        string str2;
        while ((str2 = streamReader.ReadLine()) != null)
          str1 += str2;
        streamReader.Dispose();
        if (OnlineCipher.Completed == null)
          return;
        OnlineCipher.Completed(new OnlineCipherEventArgs()
        {
          Online = true,
          Result = str1
        });
      }), (object) 0);
    }

    public static void ReadCiphers(Stream s)
    {
      StreamReader streamReader = new StreamReader(s);
      List<Cipher> cipherList = new List<Cipher>();
      string str1 = "";
      string str2;
      while ((str2 = streamReader.ReadLine()) != null)
        str1 += str2;
      streamReader.Dispose();
      if (OnlineCipher.Completed == null)
        return;
      OnlineCipher.Completed(new OnlineCipherEventArgs()
      {
        Online = false,
        Result = str1
      });
    }

    public static List<Cipher> ReadCiphersNow(Stream s)
    {
      StreamReader streamReader = new StreamReader(s);
      List<Cipher> cipherList = new List<Cipher>();
      string str1 = "";
      string str2;
      while ((str2 = streamReader.ReadLine()) != null)
      {
        str1 = str1 + Environment.NewLine + str2;
        string[] strArray = str2.Split(',');
        int result;
        if (strArray.Length > 1 && int.TryParse(strArray[0], out result))
          cipherList.Add(new Cipher()
          {
            Length = result,
            String = strArray[1]
          });
      }
      streamReader.Dispose();
      return cipherList;
    }
  }
}
