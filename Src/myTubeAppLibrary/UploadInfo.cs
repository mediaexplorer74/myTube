// Decompiled with JetBrains decompiler
// Type: myTube.UploadInfo
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace myTube
{
  public class UploadInfo
  {
    public string UploadGUID { get; set; }

    public Uri UploadUri { get; set; }

    public string Title { get; set; }

    public string ContentFilePath { get; set; }

    public async Task<StorageFile> GetFile()
    {
      try
      {
        return this.ContentFilePath == null ? (StorageFile) null : await StorageFile.GetFileFromPathAsync(this.ContentFilePath);
      }
      catch
      {
        return (StorageFile) null;
      }
    }
  }
}
