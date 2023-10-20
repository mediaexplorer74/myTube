// Decompiled with JetBrains decompiler
// Type: myTube.TransferManager
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using UriTester;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace myTube
{
  public class TransferManager
  {
    public const string FileName = "TransferManager.xml";
    public const string VideoFolderName = "myTube";
    public const string AudioFolderName = "myTube Audio";
    public const string VideoExtension = ".mp4";
    public const string AudioExtension = ".dasha";
    public const string AdaptiveVideoExtension = ".dashv";
    public const string AdaptiveAudioExtension = ".dasha";
    public const string InfoFolderName = "Video Info Folder";
    public const string VideoGroupName = "Video";
    public const string AudioGroupName = "Audio";
    public int HowManyEntriesPerPage = 30;
    private VideoInfoLoader loader = new VideoInfoLoader();
    private XElement xml;
    private StorageFolder infoFolder;
    private bool loadedTransfers;
    private TransferInfo[] transfers;
    private IReadOnlyList<DownloadOperation> videoDownloads;
    private IReadOnlyList<DownloadOperation> audioDownloads;
    public static YouTubeQuality[] SupportedVideoQualities = new YouTubeQuality[11]
    {
      YouTubeQuality.LQ,
      YouTubeQuality.HQ,
      YouTubeQuality.SD,
      YouTubeQuality.HD,
      YouTubeQuality.HD60,
      YouTubeQuality.HD1080,
      YouTubeQuality.HD1080p60,
      YouTubeQuality.HD1440,
      YouTubeQuality.HD1440p60,
      YouTubeQuality.HD2160,
      YouTubeQuality.HD2160p60
    };
    private object transfersLock = new object();
    private object folderLock = new object();

    public static TransferManager Current { get; internal set; }

    public event EventHandler<TransferManagerActionEventArgs> OnAction;

    private async Task<IReadOnlyList<DownloadOperation>> UpdateDownloads(TransferType type)
    {
      if (type == TransferType.Video && this.videoDownloads != null)
        return this.videoDownloads;
      if (type == TransferType.Audio && this.audioDownloads != null)
        return this.audioDownloads;
      try
      {
        IReadOnlyList<DownloadOperation> transferGroupAsync = await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(BackgroundTransferGroup.CreateGroup(type == TransferType.Video ? "Video" : "Audio"));
        return type != TransferType.Audio ? (this.videoDownloads = transferGroupAsync) : (this.audioDownloads = transferGroupAsync);
      }
      catch (Exception ex)
      {
        throw new Exception("Unable to get list of downloads for type " + (object) type, ex);
      }
    }

    public TransferInfo[] Transfers
    {
      get
      {
        try
        {
          lock (this.transfersLock)
          {
            if (!this.loadedTransfers)
            {
              XElement[] array = this.xml.GetElement(nameof (Transfers)).Elements((XName) "TransferInfo").ToArray<XElement>();
              this.transfers = new TransferInfo[array.Length];
              for (int index = 0; index < this.transfers.Length; ++index)
                this.transfers[index] = new TransferInfo(array[index]);
              this.loadedTransfers = true;
            }
          }
          return this.transfers;
        }
        catch
        {
          this.loadedTransfers = false;
          return new TransferInfo[0];
        }
      }
    }

    public TransferManager()
    {
      this.xml = new XElement((XName) nameof (TransferManager));
      TransferManager.Current = this;
    }

    public TransferManager(XElement xml)
    {
      this.xml = xml;
      TransferManager.Current = this;
      this.ResumeAndCleanDownloads();
    }

    private async Task CreateXMLFolder()
    {
      if (this.infoFolder != null)
        return;
      try
      {
        this.infoFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Video Info Folder", (CreationCollisionOption) 3);
      }
      catch
      {
      }
    }

    public async Task ResumeAndCleanDownloads()
    {
      Helper.Write((object) nameof (TransferManager), (object) "Cleaning up old downloads");
      try
      {
        foreach (DownloadOperation downloadOperation in (IEnumerable<DownloadOperation>) await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(BackgroundTransferGroup.CreateGroup("Video")))
        {
          Helper.Write((object) nameof (TransferManager), (object) ("Download found: " + (object) downloadOperation.Progress.Status));
          if (downloadOperation.Progress.Status == 2)
          {
            try
            {
              downloadOperation.Resume();
            }
            catch
            {
            }
          }
          else if (downloadOperation.Progress.Status != 7)
          {
            BackgroundTransferStatus status = downloadOperation.Progress.Status;
          }
        }
      }
      catch
      {
      }
      bool deletedTransfer = false;
      TransferInfo[] transferInfoArray = this.Transfers;
      int index;
      TransferInfo transfer;
      for (index = 0; index < transferInfoArray.Length; ++index)
      {
        transfer = transferInfoArray[index];
        try
        {
          bool flag = transfer.VideoFilePath != (Uri) null && !string.IsNullOrWhiteSpace(transfer.VideoFilePath.OriginalString);
          if (flag)
            flag = await transfer.GetVideoStorageFile() == null;
          if (flag)
          {
            int num = await this.DeleteTransfer(transfer, TransferType.Video) ? 1 : 0;
            deletedTransfer = true;
          }
        }
        catch
        {
        }
        transfer = (TransferInfo) null;
      }
      transferInfoArray = (TransferInfo[]) null;
      try
      {
        foreach (DownloadOperation downloadOperation in (IEnumerable<DownloadOperation>) await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(BackgroundTransferGroup.CreateGroup("Audio")))
        {
          if (downloadOperation.Progress.Status == 2)
          {
            try
            {
              downloadOperation.Resume();
            }
            catch
            {
            }
          }
        }
      }
      catch
      {
      }
      transferInfoArray = this.Transfers;
      for (index = 0; index < transferInfoArray.Length; ++index)
      {
        transfer = transferInfoArray[index];
        try
        {
          bool flag = transfer.AudioFilePath != (Uri) null && !string.IsNullOrWhiteSpace(transfer.AudioFilePath.OriginalString);
          if (flag)
            flag = await transfer.GetAudioStorageFile() == null;
          if (flag)
          {
            int num1 = await this.DeleteTransfer(transfer, TransferType.Audio) ? 1 : 0;
            if (transfer.IsAdaptive)
            {
              int num2 = await this.DeleteTransfer(transfer, TransferType.Video) ? 1 : 0;
            }
            deletedTransfer = true;
          }
        }
        catch
        {
        }
        transfer = (TransferInfo) null;
      }
      transferInfoArray = (TransferInfo[]) null;
      try
      {
        if (!deletedTransfer)
          return;
        this.Save();
      }
      catch
      {
      }
    }

    public async Task<YouTubeEntry> SaveEntryIfNotExisting(string videoID)
    {
      if (this.HasTransferInfo(videoID))
      {
        YouTubeEntryClient client = new YouTubeEntryClient();
        try
        {
          if (await this.GetVideoString(videoID) == null)
          {
            YouTubeEntry entry = await client.GetInfo(videoID);
            if (entry != null)
              await this.SaveInfo(entry);
            return entry;
          }
        }
        catch
        {
        }
        client = (YouTubeEntryClient) null;
      }
      return (YouTubeEntry) null;
    }

    private async Task SaveInfo(YouTubeEntry entry)
    {
      await this.CreateXMLFolder();
      if (this.infoFolder == null)
        return;
      HttpClient imageClient = new HttpClient();
      Task<byte[]> imageStreamTask = (Task<byte[]>) null;
      if (!await this.infoFolder.FileExists(entry.ID + ".jpg"))
      {
        try
        {
          imageStreamTask = imageClient.GetByteArrayAsync(entry.GetThumb(ThumbnailQuality.MaxRes));
        }
        catch
        {
        }
      }
      try
      {
        await FileIO.WriteTextAsync((IStorageFile) await this.infoFolder.CreateFileAsync(entry.ID + ".xml", (CreationCollisionOption) 1), entry.OriginalString);
      }
      catch
      {
      }
      if (imageStreamTask != null)
      {
        try
        {
          byte[] imageStream = await imageStreamTask;
          await FileIO.WriteBytesAsync((IStorageFile) await this.infoFolder.CreateFileAsync(entry.ID + ".jpg", (CreationCollisionOption) 1), imageStream);
          imageStream = (byte[]) null;
        }
        catch
        {
        }
      }
      imageClient = (HttpClient) null;
      imageStreamTask = (Task<byte[]>) null;
    }

    private async Task DeleteInfo(string ID)
    {
      await this.CreateXMLFolder();
      if (this.infoFolder == null)
        return;
      try
      {
        await (await this.infoFolder.GetFileAsync(ID + ".jpg")).DeleteAsync();
      }
      catch
      {
      }
      try
      {
        await (await this.infoFolder.GetFileAsync(ID + ".xml")).DeleteAsync();
      }
      catch
      {
      }
    }

    public async Task<int> FindOldVideos()
    {
      StorageFolder localFolder = ApplicationData.Current.LocalFolder;
      int count = 0;
      try
      {
        StorageFolder transfers = await (await localFolder.GetFolderAsync("shared")).GetFolderAsync("transfers");
        IReadOnlyList<StorageFile> files = await transfers.GetFilesAsync();
        if (this.infoFolder == null)
          await this.CreateXMLFolder();
        YouTubeEntry entry;
        foreach (StorageFile file in (IEnumerable<StorageFile>) files)
        {
          if (file.Name.Contains(".xml"))
          {
            try
            {
              entry = new YouTubeEntry(await FileIO.ReadTextAsync((IStorageFile) file));
              if (!this.HasTransferInfo(entry.ID))
              {
                if (await transfers.FileExists(entry.ID + ".mp4"))
                {
                  await file.MoveAsync((IStorageFolder) this.infoFolder);
                  if (await transfers.FileExists(entry.ID + ".jpg"))
                    await (await transfers.GetFileAsync(entry.ID + ".jpg")).MoveAsync((IStorageFolder) this.infoFolder);
                  StorageFile fileAsync = await transfers.GetFileAsync(entry.ID + ".mp4");
                  this.AddTransferInfo(new TransferInfo(entry)
                  {
                    VideoFilePath = new Uri(fileAsync.Path)
                  });
                  ++count;
                }
              }
              entry = (YouTubeEntry) null;
            }
            catch
            {
            }
          }
        }
        files = await this.infoFolder.GetFilesAsync();
        foreach (StorageFile storageFile in (IEnumerable<StorageFile>) files)
        {
          if (storageFile.Name.Contains(".xml"))
          {
            try
            {
              entry = new YouTubeEntry(await FileIO.ReadTextAsync((IStorageFile) storageFile));
              if (!this.HasTransferInfo(entry.ID))
              {
                if (await transfers.FileExists(entry.ID + ".mp4"))
                {
                  StorageFile fileAsync = await transfers.GetFileAsync(entry.ID + ".mp4");
                  this.AddTransferInfo(new TransferInfo(entry)
                  {
                    VideoFilePath = new Uri(fileAsync.Path)
                  });
                  ++count;
                }
              }
              entry = (YouTubeEntry) null;
            }
            catch
            {
            }
          }
        }
        transfers = (StorageFolder) null;
        files = (IReadOnlyList<StorageFile>) null;
      }
      catch
      {
      }
      return count;
    }

    public async Task<DownloadOperation> StartTransfer(
      YouTubeEntry entry,
      YouTubeInfo info,
      YouTubeQuality qual,
      Progress<DownloadOperation> progress)
    {
      TransferManager sender = this;
      bool adaptive = false;
      if (info.QualityNeedsAudio(qual))
      {
        adaptive = true;
        if (await sender.GetTransferState(entry, TransferType.Audio) == TransferManager.State.None)
          throw new InvalidOperationException("The audio for this transfer must be downloaded before the video can be downloaded");
      }
      BackgroundDownloader downloader = new BackgroundDownloader();
      downloader.SetRequestHeader("User-Agent", YouTube.UserAgent);
      downloader.SuccessToastNotification = (ToastHelper.CreateToast(entry.GetThumb(ThumbnailQuality.SD), (qual == YouTubeQuality.Audio ? "Audio" : "Video") + " download finished", entry.Title, new TileArgs("myTube.VideoPage", entry.ID, 0)));
      downloader.Method = ("GET");
      downloader.TransferGroup = (BackgroundTransferGroup.CreateGroup(qual == YouTubeQuality.Audio ? "Audio" : "Video"));
      StorageFolder videosLibrary = KnownFolders.VideosLibrary;
      StorageFolder downloadsFolder = (StorageFolder) null;
      try
      {
        downloadsFolder = await videosLibrary.CreateFolderAsync("myTube", (CreationCollisionOption) 3);
      }
      catch
      {
        try
        {
          downloadsFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("myTube", (CreationCollisionOption) 3);
        }
        catch
        {
        }
      }
      if (adaptive || qual == YouTubeQuality.Audio)
      {
        try
        {
          downloadsFolder = await downloadsFolder.CreateFolderAsync("DASH", (CreationCollisionOption) 3);
        }
        catch
        {
        }
      }
      string str1 = entry.Title;
      foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
        str1 = str1.Replace(invalidFileNameChar, ' ');
      string str2 = str1;
      StorageFile fileAsync = await downloadsFolder.CreateFileAsync(qual != YouTubeQuality.Audio ? (!adaptive ? str2 + ".mp4" : str2 + ".dashv") : str2 + ".dasha", (CreationCollisionOption) 0);
      DownloadOperation download = downloader.CreateDownload((qual == YouTubeQuality.Audio ? info.GetLink(YouTubeQuality.HD4320, MediaLinkType.Audio) : info.GetLink(qual, MediaLinkType.AllVideo)).ToUri(UriKind.Absolute), (IStorageFile) fileAsync);
      download.put_CostPolicy((BackgroundTransferCostPolicy) 2);
      download.put_Priority((BackgroundTransferPriority) 0);
      TransferInfo transfer = sender.GetTransferInfo(entry);
      if (transfer == null)
      {
        transfer = new TransferInfo();
        sender.AddTransferInfo(transfer);
      }
      transfer.Title = entry.Title;
      transfer.ID = entry.ID;
      transfer.AuthorDisplayName = entry.AuthorDisplayName;
      if (adaptive)
        transfer.IsAdaptive = true;
      if (qual == YouTubeQuality.Audio)
      {
        transfer.AudioFilePath = new Uri(fileAsync.Path);
      }
      else
      {
        transfer.VideoQuality = new YouTubeQuality?(qual);
        transfer.VideoFilePath = new Uri(fileAsync.Path);
      }
      if (progress != null)
        download.StartAsync().AsTask<DownloadOperation, DownloadOperation>((IProgress<DownloadOperation>) progress);
      else
        download.StartAsync();
      if (qual == YouTubeQuality.Audio)
        transfer.AudioGUID = download.Guid.ToString();
      else
        transfer.VideoGUID = download.Guid.ToString();
      sender.SaveInfo(entry);
      sender.audioDownloads = sender.videoDownloads = (IReadOnlyList<DownloadOperation>) null;
      if (sender.OnAction != null)
        sender.OnAction((object) sender, new TransferManagerActionEventArgs()
        {
          Action = TransferManagerAction.Added,
          VideoID = entry.ID,
          Type = qual == YouTubeQuality.Audio ? TransferType.Audio : TransferType.Video
        });
      sender.Save();
      return download;
    }

    public async Task<bool> DeleteTransfer(YouTubeEntry entry, TransferType transferType)
    {
      TransferInfo transfer1 = (TransferInfo) null;
      foreach (TransferInfo transfer2 in this.Transfers)
      {
        if (transfer2.ID == entry.ID)
        {
          transfer1 = transfer2;
          break;
        }
      }
      return transfer1 != null && await this.DeleteTransfer(transfer1, transferType);
    }

    public async Task<bool> DeleteTransfer(TransferInfo transfer, TransferType transferType)
    {
      TransferManager transferManager = this;
      bool remove = false;
      bool flag1 = transferType == TransferType.Audio;
      if (flag1)
        flag1 = await transfer.GetVideoStorageFile() == null;
      bool flag2 = flag1;
      if (!flag2)
      {
        bool flag3 = transferType == TransferType.Video;
        if (flag3)
          flag3 = await transfer.GetAudioStorageFile() == null;
        flag2 = flag3;
      }
      if (flag2)
      {
        remove = true;
        await transferManager.DeleteInfo(transfer.ID);
        Helper.Write((object) transferManager, (object) "Deleted info for video");
      }
      else
        Helper.Write((object) transferManager, (object) "Info for video not deleted as one of its files still exists");
      DownloadOperation download = (DownloadOperation) null;
      bool returnVal = true;
      try
      {
        IReadOnlyList<DownloadOperation> downloadOperationList = await transferManager.UpdateDownloads(transferType);
        string str = transferType == TransferType.Audio ? transfer.AudioGUID : transfer.VideoGUID;
        if (!string.IsNullOrWhiteSpace(str))
        {
          foreach (DownloadOperation downloadOperation in (IEnumerable<DownloadOperation>) downloadOperationList)
          {
            if (downloadOperation.Guid.ToString() == str)
            {
              download = downloadOperation;
              break;
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        ((IAsyncInfo) download?.AttachAsync()).Cancel();
      }
      catch
      {
      }
      string originalString = (transferType == TransferType.Audio ? transfer.AudioFilePath : transfer.VideoFilePath).OriginalString;
      try
      {
        if (!string.IsNullOrWhiteSpace(originalString))
          await (await StorageFile.GetFileFromPathAsync(originalString)).DeleteAsync();
      }
      catch
      {
        returnVal = false;
      }
      switch (transferType)
      {
        case TransferType.Audio:
          transfer.AudioGUID = (string) null;
          transfer.AudioFilePath = (Uri) null;
          break;
        case TransferType.Video:
          transfer.IsAdaptive = false;
          transfer.VideoGUID = (string) null;
          transfer.VideoFilePath = (Uri) null;
          break;
      }
      if (remove)
      {
        try
        {
          transfer.XML.Remove();
        }
        catch
        {
        }
        transferManager.loadedTransfers = false;
      }
      else
      {
        transferManager.loadedTransfers = false;
        switch (transferType)
        {
          case TransferType.Audio:
            transfer.AudioFilePath = (Uri) null;
            break;
          case TransferType.Video:
            transfer.VideoFilePath = (Uri) null;
            break;
        }
      }
      transferManager.videoDownloads = transferManager.audioDownloads = (IReadOnlyList<DownloadOperation>) null;
      transferManager.Save();
      if (returnVal && transferManager.OnAction != null)
        transferManager.OnAction((object) transferManager, new TransferManagerActionEventArgs()
        {
          Action = TransferManagerAction.Removed,
          VideoID = transfer.ID,
          Type = transferType
        });
      return returnVal;
    }

    public void AddTransferInfo(TransferInfo transfer)
    {
      this.loadedTransfers = false;
      this.xml.GetElement("Transfers").AddFirst((object) transfer.XML);
    }

    public bool HasTransferInfo(YouTubeEntry entry) => this.HasTransferInfo(entry.ID);

    public bool HasTransferInfo(string videoID)
    {
      if (videoID != null)
      {
        foreach (TransferInfo transfer in this.Transfers)
        {
          if (transfer.ID == videoID)
            return true;
        }
      }
      return false;
    }

    public TransferInfo GetTransferInfo(DownloadOperation download)
    {
      foreach (TransferInfo transfer in this.Transfers)
      {
        if (download.Guid.ToString() == transfer.VideoGUID || download.Guid.ToString() == transfer.AudioGUID)
          return transfer;
      }
      return (TransferInfo) null;
    }

    public TransferInfo GetTransferInfo(YouTubeEntry entry)
    {
      if (entry != null)
      {
        foreach (TransferInfo transfer in this.Transfers)
        {
          if (transfer.ID == entry.ID)
            return transfer;
        }
      }
      return (TransferInfo) null;
    }

    public async Task<YouTubeEntry[]> GetEntries(int page, TransferType type)
    {
      TransferInfo[] transfers = ((IEnumerable<TransferInfo>) this.Transfers).Where<TransferInfo>((Func<TransferInfo, bool>) (t =>
      {
        if (type == TransferType.Video && t.VideoFilePath != (Uri) null && !string.IsNullOrWhiteSpace(t.VideoFilePath.OriginalString))
          return true;
        return type == TransferType.Audio && t.AudioFilePath != (Uri) null && !string.IsNullOrWhiteSpace(t.AudioFilePath.OriginalString);
      })).ToArray<TransferInfo>();
      int manyEntriesPerPage = this.HowManyEntriesPerPage;
      int startIndex = manyEntriesPerPage * page;
      int length = transfers.Length - startIndex;
      if (length < 0)
        length = 0;
      if (length > manyEntriesPerPage)
        length = manyEntriesPerPage;
      YouTubeEntry[] entries = new YouTubeEntry[length];
      for (int i = 0; i < entries.Length; ++i)
      {
        TransferInfo trans = transfers[startIndex + i];
        string videoString = await this.GetVideoString(trans.ID);
        if (videoString != null)
        {
          YouTubeEntry[] youTubeEntryArray = entries;
          int index = i;
          YouTubeEntry youTubeEntry = new YouTubeEntry(videoString);
          youTubeEntry.Title = trans.Title;
          youTubeEntry.AuthorDisplayName = trans.AuthorDisplayName;
          youTubeEntry.ID = trans.ID;
          youTubeEntryArray[index] = youTubeEntry;
        }
        else
        {
          YouTubeEntry[] youTubeEntryArray = entries;
          int index = i;
          YouTubeEntry youTubeEntry = new YouTubeEntry();
          youTubeEntry.Title = trans.Title;
          youTubeEntry.AuthorDisplayName = trans.AuthorDisplayName;
          youTubeEntry.ID = trans.ID;
          youTubeEntryArray[index] = youTubeEntry;
        }
        trans = (TransferInfo) null;
      }
      return entries;
    }

    public async Task<string> GetVideoString(string ID)
    {
      await this.CreateXMLFolder();
      if (this.infoFolder == null)
        return (string) null;
      try
      {
        return await FileIO.ReadTextAsync((IStorageFile) await this.infoFolder.GetFileAsync(ID + ".xml"));
      }
      catch
      {
        return (string) null;
      }
    }

    public Task<Uri> GetThumbUri(string ID) => Task.Run<Uri>((Func<Task<Uri>>) (async () =>
    {
      await this.CreateXMLFolder();
      if (this.infoFolder == null)
        return (Uri) null;
      try
      {
        return new Uri(Helper.ToLocalUriPath((await this.infoFolder.GetFileAsync(ID + ".jpg")).Path), UriKind.Absolute);
      }
      catch
      {
        return (Uri) null;
      }
    }));

    public Task<IRandomAccessStream> GetThumb(string ID) => Task.Run<IRandomAccessStream>((Func<Task<IRandomAccessStream>>) (async () =>
    {
      await this.CreateXMLFolder();
      if (this.infoFolder == null)
        return (IRandomAccessStream) null;
      try
      {
        return await (await this.infoFolder.GetFileAsync(ID + ".jpg")).OpenAsync((FileAccessMode) 0);
      }
      catch
      {
        return (IRandomAccessStream) null;
      }
    }));

    public Task<DownloadOperation> GetDownload(TransferInfo transfer, TransferType type) => Task.Run<DownloadOperation>((Func<Task<DownloadOperation>>) (async () =>
    {
      if (transfer != null)
      {
        IReadOnlyList<DownloadOperation> transferGroupAsync = await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(BackgroundTransferGroup.CreateGroup(type == TransferType.Video ? "Video" : "Audio"));
        string str = type == TransferType.Video ? transfer.VideoGUID : transfer.AudioGUID;
        foreach (DownloadOperation download in (IEnumerable<DownloadOperation>) transferGroupAsync)
        {
          if (download.Guid.ToString() == str)
            return download;
        }
      }
      return (DownloadOperation) null;
    }));

    public async Task<TransferManager.State> GetTransferState(YouTubeEntry entry) => await this.GetTransferState(entry.ID);

    public async Task<TransferManager.State> GetTransferState(string ID)
    {
      Task<TransferManager.State> transferState = this.GetTransferState(ID, TransferType.Audio);
      Task<TransferManager.State> videoStateTask = this.GetTransferState(ID, TransferType.Video);
      TransferManager.State audioState = await transferState;
      TransferManager.State state = await videoStateTask;
      return audioState == TransferManager.State.Downloading || state == TransferManager.State.Downloading ? TransferManager.State.Downloading : (audioState != TransferManager.State.Complete || state != TransferManager.State.Complete ? (audioState != TransferManager.State.None || state != TransferManager.State.None ? TransferManager.State.Complete : TransferManager.State.None) : TransferManager.State.Complete);
    }

    public async Task<TransferManager.State> GetTransferState(YouTubeEntry entry, TransferType type) => await this.GetTransferState(entry.ID, type);

    public Task<TransferManager.State> GetTransferState(string ID, TransferType type) => Task.Run<TransferManager.State>((Func<Task<TransferManager.State>>) (async () =>
    {
      string Tag = nameof (GetTransferState);
      Helper.Write((object) Tag, (object) string.Format("Getting TransferState for {0} ({1})", (object) ID, (object) type));
      TransferInfo[] transfers = this.Transfers;
      TransferInfo transfer = (TransferInfo) null;
      DownloadOperation download = (DownloadOperation) null;
      if (transfers == null)
        Helper.Write((object) Tag, (object) "The transfers are null");
      foreach (TransferInfo transferInfo in transfers)
      {
        if (transferInfo.ID == ID)
        {
          transfer = transferInfo;
          break;
        }
      }
      if (transfer != null)
      {
        Helper.Write((object) Tag, (object) ("Transfer info for " + ID + " exists"));
        try
        {
          IReadOnlyList<DownloadOperation> downloadOperationList = await this.UpdateDownloads(type);
          string str = type == TransferType.Video ? transfer.VideoGUID : transfer.AudioGUID;
          foreach (DownloadOperation downloadOperation in (IEnumerable<DownloadOperation>) downloadOperationList)
          {
            if (downloadOperation.Guid.ToString() == str)
            {
              download = downloadOperation;
              Helper.Write((object) Tag, (object) ("Download for " + ID + " exists"));
              break;
            }
          }
        }
        catch
        {
        }
        string str1 = (string) null;
        try
        {
          str1 = type == TransferType.Video ? transfer.VideoFilePath.OriginalString : transfer.AudioFilePath.OriginalString;
        }
        catch
        {
          Helper.Write((object) Tag, (object) ("There is no file path for " + ID + " (" + (object) type + ")"));
        }
        bool found;
        if (download != null)
        {
          if (download.Progress.Status != 5 && (download.Progress.TotalBytesToReceive <= 0UL || (long) download.Progress.TotalBytesToReceive != (long) download.Progress.BytesReceived))
            return download.Progress.Status == 6 || download.Progress.Status == 7 ? TransferManager.State.None : TransferManager.State.Downloading;
          if (!string.IsNullOrWhiteSpace(str1))
          {
            found = true;
            try
            {
              StorageFile fileFromPathAsync = await StorageFile.GetFileFromPathAsync(str1);
            }
            catch (FileNotFoundException ex)
            {
              found = false;
            }
            return !found ? TransferManager.State.None : TransferManager.State.Complete;
          }
        }
        else if (!string.IsNullOrWhiteSpace(str1))
        {
          found = true;
          try
          {
            StorageFile fileFromPathAsync = await StorageFile.GetFileFromPathAsync(str1);
          }
          catch (FileNotFoundException ex)
          {
            found = false;
          }
          return !found ? TransferManager.State.None : TransferManager.State.Complete;
        }
      }
      return TransferManager.State.None;
    }));

    public async void Save(string fileName = "TransferManager.xml")
    {
      try
      {
        StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, (CreationCollisionOption) 1);
        await FileIO.WriteTextAsync((IStorageFile) file, this.xml.ToString());
        StorageFolder videosLibrary = KnownFolders.VideosLibrary;
        if (videosLibrary != null)
        {
          StorageFolder myTubeFolder = await videosLibrary.CreateFolderAsync("myTube", (CreationCollisionOption) 3);
          if (myTubeFolder != null)
          {
            StorageFile storageFile1 = await file.CopyAsync((IStorageFolder) myTubeFolder, fileName + ".backup", (NameCollisionOption) 1);
            StorageFile storageFile2 = await file.CopyAsync((IStorageFolder) myTubeFolder, fileName ?? "", (NameCollisionOption) 1);
          }
          myTubeFolder = (StorageFolder) null;
        }
        file = (StorageFile) null;
      }
      catch
      {
      }
    }

    public static async Task<TransferManager> Load(string fileName = "TransferManager.xml", bool returnNull = false)
    {
      int num;
      try
      {
        StorageFolder videosLibrary = KnownFolders.VideosLibrary;
        StorageFolder storageFolder = (StorageFolder) null;
        if (videosLibrary != null)
          storageFolder = await videosLibrary.CreateFolderAsync("myTube", (CreationCollisionOption) 3);
        return await TransferManager.load(fileName, storageFolder, ApplicationData.Current.LocalFolder);
      }
      catch
      {
        num = 1;
      }
      if (num != 1)
      {
        TransferManager transferManager;
        return transferManager;
      }
      Helper.Write((object) nameof (TransferManager), (object) "Error loading from file, trying backup");
      try
      {
        StorageFolder videosLibrary = KnownFolders.VideosLibrary;
        if (videosLibrary != null)
        {
          if (await videosLibrary.CreateFolderAsync("myTube", (CreationCollisionOption) 3) != null)
          {
            TransferManager transferManager = await TransferManager.load(fileName + ".backup");
          }
        }
      }
      catch
      {
      }
      Helper.Write((object) nameof (TransferManager), (object) "Error loading from file");
      return !returnNull ? new TransferManager() : (TransferManager) null;
    }

    private static async Task<TransferManager> load(string fileName, params StorageFolder[] folders)
    {
      StorageFolder[] storageFolderArray = folders;
      for (int index = 0; index < storageFolderArray.Length; ++index)
      {
        StorageFolder folder = storageFolderArray[index];
        TransferManager transferManager;
        try
        {
          transferManager = await TransferManager.load(fileName, folder);
        }
        catch
        {
          continue;
        }
        return transferManager;
      }
      storageFolderArray = (StorageFolder[]) null;
      throw new FileNotFoundException();
    }

    private static async Task<TransferManager> load(string fileName, StorageFolder folder)
    {
      Helper.Write((object) nameof (TransferManager), (object) "Loading");
      StorageFile fileAsync = await folder.GetFileAsync(fileName);
      Helper.Write((object) nameof (TransferManager), (object) ("Got file " + fileName));
      string text = await FileIO.ReadTextAsync((IStorageFile) fileAsync);
      Helper.Write((object) nameof (TransferManager), (object) "Reading file");
      TransferManager transferManager = new TransferManager(XElement.Parse(text));
      Helper.Write((object) nameof (TransferManager), (object) "Successfully loaded");
      return transferManager;
    }

    public enum State
    {
      None,
      Downloading,
      Complete,
    }
  }
}
