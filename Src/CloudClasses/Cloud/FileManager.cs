// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.FileManager
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using System;
using System.IO;

namespace myTube.Cloud
{
  public static class FileManager
  {
    public static Func<string, Stream> LoadFile;
    public static Action<string, Stream> SaveFile;
    public static Func<string, bool> Exists;
    public static Action<int> Sleep;
    public static Func<string, string, bool> Copy;
  }
}
