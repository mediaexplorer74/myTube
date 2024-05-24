// Decompiled with JetBrains decompiler
// Type: myTube.UploadsManager
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using Newtonsoft.Json.Linq;
using RykenTube;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace myTube
{
  public class UploadsManager
  {
    private const string GroupName = "VideoUploads";
    private const string FileName = "UploadsManager.json";

    public List<UploadInfo> Uploads { get; internal set; }

    public UploadsManager() => this.Uploads = new List<UploadInfo>();

    public async Task Clean()
    {
      try
      {
        IReadOnlyList<UploadOperation> downloads = await BackgroundUploader.GetCurrentUploadsForTransferGroupAsync(BackgroundTransferGroup.CreateGroup("VideoUploads"));
        foreach (UploadOperation upload in (IEnumerable<UploadOperation>) downloads)
        {
          try
          {
            if (upload.Progress.Status == BackgroundTransferStatus.Error)
              await this.CancelUpload(upload);
          }
          catch
          {
          }
        }
        UploadInfo[] uploadInfoArray = this.Uploads.ToArray();
        for (int index = 0; index < uploadInfoArray.Length; ++index)
        {
          UploadInfo info = uploadInfoArray[index];
          bool flag = false;
          foreach (UploadOperation uploadOperation in (IEnumerable<UploadOperation>) downloads)
          {
            if (uploadOperation.Guid.ToString() == info.UploadGUID)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            await this.CancelUpload(info);
        }
        uploadInfoArray = (UploadInfo[]) null;
        downloads = (IReadOnlyList<UploadOperation>) null;
      }
      catch
      {
      }
    }

    public async Task CancelUpload(UploadOperation upload)
    {
      foreach (UploadInfo upload1 in this.Uploads)
      {
        if (upload1.UploadGUID == upload.Guid.ToString())
        {
          await this.CancelUpload(upload1, upload);
          return;
        }
      }
      try
      {
        ((IAsyncInfo) upload.AttachAsync()).Cancel();
      }
      catch
      {
      }
    }

    public async Task CancelUpload(UploadInfo info)
    {
      try
      {
        foreach (UploadOperation upload in (IEnumerable<UploadOperation>) await BackgroundUploader.GetCurrentUploadsForTransferGroupAsync(BackgroundTransferGroup.CreateGroup("VideoUploads")))
        {
          if (upload.Guid.ToString() == info.UploadGUID)
          {
            await this.CancelUpload(info, upload);
            return;
          }
        }
      }
      catch
      {
      }
      if (!this.Uploads.Contains(info))
        return;
      this.Uploads.Remove(info);
      await this.Save();
    }

    public async Task CancelUpload(UploadInfo info, UploadOperation upload)
    {
      try
      {
        ((IAsyncInfo) upload.AttachAsync()).Cancel();
      }
      catch
      {
      }
      try
      {
        if (this.Uploads.Contains(info))
          this.Uploads.Remove(info);
      }
      catch
      {
      }
      try
      {
        await this.Save();
      }
      catch
      {
      }
    }

    public async Task<UploadOperation> GetUploadOperation(UploadInfo info)
    {
      try
      {
        foreach (UploadOperation uploadOperation in
                    (IEnumerable<UploadOperation>) await BackgroundUploader.GetCurrentUploadsForTransferGroupAsync(BackgroundTransferGroup.CreateGroup("VideoUploads")))
        {
          if (uploadOperation.Guid.ToString() == info.UploadGUID)
            return uploadOperation;
        }
      }
      catch
      {
      }
      return (UploadOperation) null;
    }

    public async Task<UploadInfo> GetUploadInfo(UploadOperation upload)
    {
      foreach (UploadInfo upload1 in this.Uploads)
      {
        if (upload1.UploadGUID == upload.Guid.ToString())
          return upload1;
      }
      return (UploadInfo) null;
    }

    public async Task<UploadOperation> StartUpload(IStorageFile file, Uri uploadTo, string title)
    {
      BackgroundUploader backgroundUploader = new BackgroundUploader();
      backgroundUploader.SetRequestHeader("User-Agent", YouTube.UserAgent);
      backgroundUploader.Method = "PUT";
      backgroundUploader.TransferGroup = BackgroundTransferGroup.CreateGroup("VideoUploads");
      backgroundUploader.CostPolicy = (BackgroundTransferCostPolicy) 2;
      backgroundUploader.SuccessToastNotification = ToastHelper.CreateToast("Upload complete",
          title, new TileArgs("myTube.UploaderPage", overCanvasPage: 0));
      UploadOperation upload = backgroundUploader.CreateUpload(uploadTo, file);
      backgroundUploader.CostPolicy = (BackgroundTransferCostPolicy) 2;
      this.Uploads.Add(new UploadInfo()
      {
        UploadGUID = upload.Guid.ToString(),
        Title = title,
        ContentFilePath = ((IStorageItem) file).Path,
        UploadUri = uploadTo
      });
      upload.StartAsync();
      try
      {
        await this.Save();
      }
      catch
      {
      }
      return upload;
    }

    public async Task Save(string fileName = "UploadsManager.json")
    {
      UploadsManager o = this;
      try
      {
        string str = JObject.FromObject((object) o).ToString();
        await FileIO.WriteTextAsync((IStorageFile) await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, (CreationCollisionOption) 1), str);
        str = (string) null;
      }
      catch
      {
      }
    }

    public static async Task<UploadsManager> Load(string fileName = "UploadsManager.json")
    {
      try
      {
        return JObject.Parse(await FileIO.ReadTextAsync((IStorageFile) await ApplicationData.Current.LocalFolder.GetFileAsync(fileName))).ToObject<UploadsManager>();
      }
      catch
      {
        return new UploadsManager();
      }
    }
  }
}
