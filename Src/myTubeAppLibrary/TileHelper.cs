// Decompiled with JetBrains decompiler
// Type: myTube.TileHelper
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using UriTester;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
  public static class TileHelper
  {
    public static PlatformType Platform = PlatformType.Unknown;
    private static StorageFolder mainFolder = ApplicationData.Current.LocalFolder;
    private const string TileFolderName = "Tiles";
    private static Grid tileGrid = (Grid) null;

    private static async Task<StorageFolder> GetTileFolder() => TileHelper.mainFolder != null ? await TileHelper.mainFolder.CreateFolderAsync("Tiles", (CreationCollisionOption) 3) : (StorageFolder) null;

    public static string CreateTileID(TypeConstructor type) => TileHelper.CreateTileID(TileHelper.CreateTileUrl(type));

    public static string CreateTileID(URLConstructor url) => TileHelper.CreateTileID(url.ToString());

    public static string CreateTileID(YouTubeEntry entry) => WebUtility.UrlEncode(entry.ID);

    public static string CreateTileID(string url)
    {
      string tileId = WebUtility.UrlEncode(url);
      if (tileId.Length > 48)
        tileId = tileId.Substring(0, 48);
      return tileId;
    }

    public static URLConstructor CreateTileUrl(TypeConstructor type) => new URLConstructor("Typed")
    {
      ["Con"] = type.ToString()
    };

    public static URLConstructor CreateTileUrlFromID(string id) => new URLConstructor(WebUtility.UrlDecode(id));

    public static string IDToFileName(string id) => id.Replace("?", "(qu)}").Replace("%3F", "(qu)").Replace("=", "(eq)").Replace("%3D", "(eq)");

    public static string IDFromFileName(string name) => name.Replace("(qu)", "?").Replace("(eq)", "=");

    private static URLConstructor TileProperties(string title, bool updateInBackground) => new URLConstructor("Properties")
    {
      ["Title"] = title,
      ["BackgroundUpdate"] = updateInBackground.ToString()
    };

    public static void ResetMainTile() => TileUpdateManager.CreateTileUpdaterForApplication().Clear();

    public static void SetMainTileImages(
      string smallImage,
      string mediumImage,
      string wideImage,
      XmlDocument xmlForSmallTile = null)
    {
      TileUpdater updaterForApplication = TileUpdateManager.CreateTileUpdaterForApplication();
      updaterForApplication.Clear();
      updaterForApplication.Update(new TileNotification(TileHelper.GetPrimaryImageTileXml(smallImage, mediumImage, wideImage, "none", xmlForSmallTile)));
    }

    public static void SetMainTileImages(
      string smallImage,
      string mediumImage,
      string wideImage,
      string largeImage,
      XmlDocument xmlForSmallTile = null)
    {
      TileUpdater updaterForApplication = TileUpdateManager.CreateTileUpdaterForApplication();
      updaterForApplication.Clear();
      updaterForApplication.Update(new TileNotification(TileHelper.GetPrimaryImageTileXml(smallImage, mediumImage, wideImage, largeImage, "none", xmlForSmallTile)));
    }

    public static async Task<SecondaryTile> CreateTile(YouTubeEntry entry, TileArgs args)
    {
      string tileId = TileHelper.CreateTileID(entry);
      string str1 = args.ToString();
      string title = entry.Title;
      string str2 = str1;
      Uri uri = new Uri("ms-appx:///Assets/Logo.scale-100.png", UriKind.Absolute);
      SecondaryTile tile = new SecondaryTile(tileId, title, str2, uri, (TileSize) 0);
      tile.VisualElements.put_Wide310x150Logo(new Uri("ms-appx:///Assets/TileLogo.scale-180.png", UriKind.Absolute));
      tile.VisualElements.put_ShowNameOnSquare150x150Logo(false);
      tile.VisualElements.put_ForegroundText((ForegroundText) 1);
      return tile;
    }

    public static async Task<SecondaryTile> CreateTile(
      TypeConstructor type,
      string title,
      TileArgs tileArgs)
    {
      TileHelper.CreateTileUrl(type);
      string str1 = TileHelper.CreateTileID(type);
      if (str1.Length > 64)
        str1 = str1.Substring(0, 64);
      tileArgs.Constructor = type;
      string str2 = tileArgs.ToString();
      SecondaryTile tile;
      try
      {
        SecondaryTile secondaryTile = new SecondaryTile(str1, title, str2, new Uri("ms-appx:///Assets/Logo.scale-100.png", UriKind.Absolute), (TileSize) 4);
        secondaryTile.VisualElements.put_Wide310x150Logo(new Uri("ms-appx:///Assets/LogoWide.scale-200.png", UriKind.Absolute));
        secondaryTile.VisualElements.put_Square310x310Logo(new Uri("ms-appx:///Assets/Logo.scale-100.png", UriKind.Absolute));
        secondaryTile.VisualElements.put_ShowNameOnSquare150x150Logo(false);
        secondaryTile.VisualElements.put_ForegroundText((ForegroundText) 1);
        tile = secondaryTile;
      }
      catch (Exception ex)
      {
        throw new Exception("Unable to pin tile with id: \n" + str1, ex);
      }
      return tile;
    }

    public static async Task CleanUpFolders()
    {
      IReadOnlyList<StorageFolder> folders = await (await TileHelper.GetTileFolder()).GetFoldersAsync();
      IReadOnlyList<SecondaryTile> allAsync = await SecondaryTile.FindAllAsync();
      foreach (StorageFolder storageFolder in (IEnumerable<StorageFolder>) folders)
      {
        string tileId = TileHelper.CreateTileID(TileHelper.IDFromFileName(storageFolder.Name));
        if (!SecondaryTile.Exists(tileId) && !SecondaryTile.Exists(tileId + "."))
          await storageFolder.DeleteAsync();
      }
    }

    public static async Task DeleteAndCleanUpImages(this SecondaryTile tile)
    {
      string id = tile.TileId;
      try
      {
        int num = await tile.RequestDeleteAsync() ? 1 : 0;
      }
      catch
      {
      }
      try
      {
        await (await TileHelper.GetFolderForTile(id)).DeleteAsync((StorageDeleteOption) 1);
      }
      catch
      {
      }
    }

    private static string ToLocalUriPath(string path) => path.Replace(ApplicationData.Current.LocalFolder.Path, "ms-appdata:///local").Replace("\\", "/");

    private static async Task<StorageFolder> GetFolderForTile(YouTubeEntry entry) => await TileHelper.GetFolderForTile(TileHelper.CreateTileID(entry));

    private static async Task<StorageFolder> GetFolderForTile(TypeConstructor type) => await TileHelper.GetFolderForTile(TileHelper.CreateTileID(type));

    public static async Task<StorageFolder> GetFolderForTile(string id)
    {
      StorageFolder tileFolder = await TileHelper.GetTileFolder();
      string fileName = TileHelper.IDToFileName(id);
      try
      {
        return await tileFolder.CreateFolderAsync(fileName, (CreationCollisionOption) 3);
      }
      catch
      {
      }
      while (fileName.EndsWith("."))
        fileName = fileName.Substring(0, fileName.Length - 1);
      return await tileFolder.CreateFolderAsync(fileName, (CreationCollisionOption) 3);
    }

    public static async Task<StorageFile> GetTileImageFile(YouTubeEntry entry, TileSize size)
    {
      StorageFolder folderForTile = await TileHelper.GetFolderForTile(entry);
      try
      {
        return await folderForTile.GetFileAsync(size.ToString() + ".png");
      }
      catch
      {
        return (StorageFile) null;
      }
    }

    public static async Task<StorageFile> GetTileImageFile(
      TypeConstructor type,
      int index,
      TileSize size)
    {
      IReadOnlyList<StorageFile> filesAsync = await (await TileHelper.GetFolderForTile(type)).GetFilesAsync();
      try
      {
        foreach (StorageFile tileImageFile in (IEnumerable<StorageFile>) filesAsync)
        {
          if (tileImageFile.Name.Split("-")[0] == index.ToString())
            return tileImageFile;
        }
      }
      catch
      {
        return (StorageFile) null;
      }
      return (StorageFile) null;
    }

    public static async Task<StorageFile> GetTileImageFile(
      TypeConstructor type,
      YouTubeEntry entry,
      TileSize size)
    {
      IReadOnlyList<StorageFile> filesAsync = await (await TileHelper.GetFolderForTile(type)).GetFilesAsync();
      try
      {
        foreach (StorageFile tileImageFile in (IEnumerable<StorageFile>) filesAsync)
        {
          if (tileImageFile.Name.Contains(entry.ID) && tileImageFile.Name.Contains(size.ToString()))
            return tileImageFile;
        }
      }
      catch
      {
        return (StorageFile) null;
      }
      return (StorageFile) null;
    }

    public static async Task<StorageFile> GetTileImageFile(
      string id,
      YouTubeEntry entry,
      TileSize size)
    {
      IReadOnlyList<StorageFile> filesAsync = await (await TileHelper.GetFolderForTile(id)).GetFilesAsync();
      try
      {
        foreach (StorageFile tileImageFile in (IEnumerable<StorageFile>) filesAsync)
        {
          if (tileImageFile.Name.Contains(entry.ID) && tileImageFile.Name.Contains(size.ToString()))
            return tileImageFile;
        }
      }
      catch
      {
        return (StorageFile) null;
      }
      return (StorageFile) null;
    }

    public static async Task<StorageFile> CreateTileImageFile(
      YouTubeEntry entry,
      RenderTargetBitmap bitmap,
      TileSize size)
    {
      StorageFile file = await (await TileHelper.GetFolderForTile(entry)).CreateFileAsync(size.ToString() + ".png", (CreationCollisionOption) 1);
      IBuffer pixelBuffer = await bitmap.GetPixelsAsync();
      using (IRandomAccessStream stream = await file.OpenAsync((FileAccessMode) 1))
      {
        BitmapEncoder async = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
        async.SetPixelData((BitmapPixelFormat) 87, (BitmapAlphaMode) 1, (uint) bitmap.PixelWidth, (uint) bitmap.PixelHeight, 96.0, 96.0, pixelBuffer.ToArray());
        await async.FlushAsync();
      }
      return file;
    }

    public static async Task<StorageFile> CreateTileImageFile(
      TypeConstructor type,
      YouTubeEntry entry,
      RenderTargetBitmap bitmap,
      TileSize size)
    {
      return await TileHelper.CreateTileImageFile(TileHelper.CreateTileID(type), entry, bitmap, size);
    }

    public static async Task<StorageFile> CreateTileImageFile(
      string id,
      YouTubeEntry entry,
      RenderTargetBitmap bitmap,
      TileSize size)
    {
      StorageFile file = await (await TileHelper.GetFolderForTile(id)).CreateFileAsync(entry.ID + "-" + (object) size + ".png", (CreationCollisionOption) 1);
      byte[] pixelBuffer = (await bitmap.GetPixelsAsync()).ToArray();
      GC.Collect();
      using (IRandomAccessStream stream = await file.OpenAsync((FileAccessMode) 1))
      {
        BitmapEncoder async = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
        async.SetPixelData((BitmapPixelFormat) 87, (BitmapAlphaMode) 1, (uint) bitmap.PixelWidth, (uint) bitmap.PixelHeight, 96.0, 96.0, pixelBuffer);
        await async.FlushAsync();
      }
      pixelBuffer = (byte[]) null;
      GC.Collect();
      return file;
    }

    public static XmlDocument GetPrimaryImageTileXml(
      string smallPath,
      string medPath,
      string widePath,
      string branding,
      XmlDocument smallXml = null)
    {
      medPath = TileHelper.ToLocalUriPath(medPath);
      widePath = TileHelper.ToLocalUriPath(widePath);
      smallPath = TileHelper.ToLocalUriPath(smallPath);
      XmlDocument templateContent1 = TileUpdateManager.GetTemplateContent((TileTemplateType) 0);
      templateContent1.SetImage(0, medPath);
      templateContent1.SetBranding(branding);
      XmlDocument templateContent2 = TileUpdateManager.GetTemplateContent((TileTemplateType) 10);
      templateContent2.SetImage(0, widePath);
      templateContent2.SetBranding(branding);
      IXmlNode ixmlNode1 = templateContent2.ImportNode(templateContent1.GetElementsByTagName("binding").Item(0U), true);
      ((IReadOnlyList<IXmlNode>) templateContent2.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode1);
      if (smallXml != null)
      {
        smallXml.SetImage(0, smallPath);
        IXmlNode ixmlNode2 = templateContent2.ImportNode(smallXml.GetElementsByTagName("binding").Item(0U), true);
        ((IReadOnlyList<IXmlNode>) templateContent2.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode2);
      }
      return templateContent2;
    }

    public static XmlDocument GetPrimaryImageTileXml(
      string smallPath,
      string medPath,
      string widePath,
      string largePath,
      string branding,
      XmlDocument smallXml = null)
    {
      medPath = TileHelper.ToLocalUriPath(medPath);
      widePath = TileHelper.ToLocalUriPath(widePath);
      smallPath = TileHelper.ToLocalUriPath(smallPath);
      largePath = TileHelper.ToLocalUriPath(smallPath);
      XmlDocument templateContent1 = TileUpdateManager.GetTemplateContent((TileTemplateType) 0);
      templateContent1.SetImage(0, medPath);
      templateContent1.SetBranding(branding);
      XmlDocument templateContent2 = TileUpdateManager.GetTemplateContent((TileTemplateType) 10);
      templateContent2.SetImage(0, widePath);
      templateContent2.SetBranding(branding);
      XmlDocument templateContent3 = TileUpdateManager.GetTemplateContent((TileTemplateType) 48);
      templateContent3.SetImage(0, largePath);
      templateContent3.SetBranding(branding);
      IXmlNode ixmlNode1 = templateContent3.ImportNode(templateContent1.GetElementsByTagName("binding").Item(0U), true);
      ((IReadOnlyList<IXmlNode>) templateContent3.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode1);
      IXmlNode ixmlNode2 = templateContent3.ImportNode(templateContent2.GetElementsByTagName("binding").Item(0U), true);
      ((IReadOnlyList<IXmlNode>) templateContent3.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode2);
      if (smallXml != null)
      {
        smallXml.SetImage(0, smallPath);
        IXmlNode ixmlNode3 = templateContent3.ImportNode(smallXml.GetElementsByTagName("binding").Item(0U), true);
        ((IReadOnlyList<IXmlNode>) templateContent3.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode3);
      }
      return templateContent3;
    }

    public static Task<TileNotification> GetAdaptiveTileNotification(YouTubeEntry entry) => TileHelper.GetAdaptiveTileNotification(entry, Branding.Name);

    public static async Task<TileNotification> GetAdaptiveTileNotification(
      YouTubeEntry entry,
      Branding branding)
    {
      XElement xml = new XElement((XName) "tile");
      XElement visual = new XElement((XName) "visual").SetBranding(branding);
      xml.Add((object) visual);
      UserInfo info = await new UserInfoClient().GetInfo(entry.Author);
      XElement el1 = visual.AddTile(TileTemplate.TileSmall).SetBranding(Branding.None);
      el1.AddText(entry.Title).SetWrap(true).SetTextStyle(TextStyle.Caption);
      el1.AddImage(entry.GetThumb(ThumbnailQuality.High).OriginalString).SetPlacement(Placement.Peek);
      XElement el2 = visual.AddTile(TileTemplate.TileMedium).SetBranding(branding);
      el2.SetOverlay(50);
      el2.AddImage(entry.GetThumb(ThumbnailQuality.Med).OriginalString).SetPlacement(Placement.Background);
      el2.AddGroup().AddSubgroup().SetWeight(5);
      el2.AddText(entry.AuthorDisplayName).SetAlign(Align.Left).SetTextStyle(TextStyle.Caption);
      el2.AddText(entry.Title).SetWrap(true).SetAlign(Align.Left).SetTextStyle(TextStyle.Base);
      XElement el3 = visual.AddTile(TileTemplate.TileWide).SetTextStacking(TextStacking.Center);
      el3.SetOverlay(50);
      el3.AddImage(entry.GetThumb(ThumbnailQuality.High).OriginalString).SetPlacement(Placement.Background);
      XElement el4 = el3.AddGroup();
      XElement el5 = el4.AddSubgroup().SetWeight(1);
      XElement el6 = el4.AddSubgroup().SetWeight(2).SetTextStacking(TextStacking.Center);
      el5.AddImage(info.ThumbUri.OriginalString).SetCrop(Crop.Circle);
      string title = entry.Title;
      el6.AddText(title).SetWrap(true).SetTextStyle(TextStyle.Base).SetAlign(Align.Left);
      XElement el7 = visual.AddTile(TileTemplate.TileLarge).SetOverlay(50);
      el7.AddImage(entry.GetThumb(ThumbnailQuality.Med).OriginalString).SetPlacement(Placement.Background);
      XElement el8 = el7.AddGroup();
      el8.AddSubgroup().SetWeight(1);
      XElement el9 = el8.AddSubgroup().SetWeight(2).SetTextStacking(TextStacking.Center);
      el8.AddSubgroup().SetWeight(1);
      el9.AddImage(info.ThumbUri.OriginalString).SetCrop(Crop.Circle).SetRemoveMargin(true);
      el7.AddText(entry.AuthorDisplayName).SetTextStyle(TextStyle.Caption).SetAlign(Align.Center);
      el7.AddText(entry.Title).SetTextStyle(TextStyle.Base).SetAlign(Align.Center).SetWrap(true);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml.ToString());
      return new TileNotification(xmlDocument);
    }

    public static async Task<XmlDocument> GetSecondaryImageTileXml(
      StorageFile mediumImage,
      StorageFile wideImage,
      string branding)
    {
      XmlDocument templateContent1 = TileUpdateManager.GetTemplateContent((TileTemplateType) 0);
      string localUriPath = TileHelper.ToLocalUriPath(mediumImage.Path);
      templateContent1.SetImage(0, localUriPath);
      templateContent1.SetBranding(branding);
      XmlDocument templateContent2 = TileUpdateManager.GetTemplateContent((TileTemplateType) 10);
      templateContent2.SetImage(0, TileHelper.ToLocalUriPath(wideImage.Path));
      templateContent2.SetBranding(branding);
      IXmlNode ixmlNode = templateContent2.ImportNode(templateContent1.GetElementsByTagName("binding").Item(0U), true);
      ((IReadOnlyList<IXmlNode>) templateContent2.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode);
      return templateContent2;
    }

    public static async Task<XmlDocument> GetSecondaryImageTileXml(
      string mediumImage,
      string wideImage,
      string branding)
    {
      XmlDocument templateContent1 = TileUpdateManager.GetTemplateContent((TileTemplateType) 0);
      string localUriPath = TileHelper.ToLocalUriPath(mediumImage);
      templateContent1.SetImage(0, localUriPath);
      templateContent1.SetBranding(branding);
      XmlDocument templateContent2 = TileUpdateManager.GetTemplateContent((TileTemplateType) 10);
      templateContent2.SetImage(0, TileHelper.ToLocalUriPath(wideImage));
      templateContent2.SetBranding(branding);
      IXmlNode ixmlNode = templateContent2.ImportNode(templateContent1.GetElementsByTagName("binding").Item(0U), true);
      ((IReadOnlyList<IXmlNode>) templateContent2.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode);
      return templateContent2;
    }

    private static async Task<Grid> GetVideoTileGrid() => TileHelper.tileGrid == null ? XamlReader.Load(await FileIO.ReadTextAsync((IStorageFile) await (await Package.Current.InstalledLocation.GetFolderAsync("TileXML")).GetFileAsync("VideoListTile.xml"))) as Grid : TileHelper.tileGrid;

    public static async Task UpdateSecondaryTile(YouTubeEntry entry)
    {
      string tileId = TileHelper.CreateTileID(entry);
      if (!SecondaryTile.Exists(tileId))
        return;
      TileUpdater updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
      if (TileHelper.Platform == PlatformType.WindowsUWP)
      {
        updater.Update(await TileHelper.GetAdaptiveTileNotification(entry, Branding.None));
      }
      else
      {
        StorageFile mediumImage = await TileHelper.GetTileImageFile(entry, (TileSize) 3);
        XmlDocument secondaryImageTileXml = await TileHelper.GetSecondaryImageTileXml(mediumImage, await TileHelper.GetTileImageFile(entry, (TileSize) 4), "none");
        mediumImage = (StorageFile) null;
        updater.Update(new TileNotification(secondaryImageTileXml));
      }
      updater = (TileUpdater) null;
    }

    public static TileNotification GetPlainTileNotification(YouTubeEntry entry)
    {
      XmlDocument templateContent1 = TileUpdateManager.GetTemplateContent((TileTemplateType) 7);
      templateContent1.SetImage(0, entry.GetThumb(ThumbnailQuality.SD).ToString());
      templateContent1.SetText(0, entry.AuthorDisplayName);
      templateContent1.SetText(1, entry.Title);
      templateContent1.SetBranding("logo");
      XmlDocument templateContent2 = TileUpdateManager.GetTemplateContent((TileTemplateType) 13);
      templateContent2.SetImage(0, entry.GetThumb(ThumbnailQuality.SD).ToString());
      templateContent2.SetText(0, entry.AuthorDisplayName);
      templateContent2.SetText(1, entry.Title);
      templateContent2.SetBranding("logo");
      XmlDocument templateContent3 = TileUpdateManager.GetTemplateContent((TileTemplateType) 50);
      templateContent3.SetImage(0, entry.GetThumb(ThumbnailQuality.SD).ToString());
      templateContent3.SetText(0, entry.AuthorDisplayName);
      templateContent3.SetText(1, entry.Title);
      templateContent3.SetBranding("logo");
      IXmlNode ixmlNode1 = templateContent1.ImportNode(templateContent2.GetElementsByTagName("binding").Item(0U), true);
      ((IReadOnlyList<IXmlNode>) templateContent1.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode1);
      IXmlNode ixmlNode2 = templateContent1.ImportNode(templateContent3.GetElementsByTagName("binding").Item(0U), true);
      ((IReadOnlyList<IXmlNode>) templateContent1.GetElementsByTagName("visual"))[0].AppendChild(ixmlNode2);
      return new TileNotification(templateContent1);
    }

    public static async Task<List<TileNotification>> UpdateSecondaryTile(
      TypeConstructor type,
      Func<FrameworkElement, RenderTargetBitmap, Task<RenderTargetBitmap>> renderTask,
      bool updateInternally,
      string tileID = null,
      Action writeMemory = null,
      bool forceRerenderingOfTiles = false,
      ThumbnailQuality thumbQual = ThumbnailQuality.High,
      string accessToken = null)
    {
      string id = TileHelper.CreateTileID(type);
      if (tileID != null)
        id = tileID;
      List<TileNotification> notifications = new List<TileNotification>();
      if (!updateInternally || SecondaryTile.Exists(id))
      {
        YouTubeClient<YouTubeEntry> youTubeClient1 = type.Construct() as YouTubeClient<YouTubeEntry>;
        youTubeClient1.UseRandomQuery = true;
        if (youTubeClient1 != null)
        {
          YouTubeClient<YouTubeEntry> youTubeClient2 = youTubeClient1;
          YouTubeEntryClient youTubeEntryClient = new YouTubeEntryClient(3);
          youTubeEntryClient.APIKey = "AIzaSyCRuvaqjnVtmh6FOnfIDQ8XyDVWzi_6UIA";
          youTubeEntryClient.UseAccessToken = false;
          youTubeClient2.RefreshClientOverride = (EntryClient<YouTubeEntry>) youTubeEntryClient;
          if (youTubeClient1 is SignedInUserClient && (youTubeClient1 as SignedInUserClient).Type == UserFeed.Subscriptions)
            youTubeClient1 = (YouTubeClient<YouTubeEntry>) new SubscriptionsPageClient();
          if (accessToken != null)
            youTubeClient1.AccessToken = accessToken;
          YouTubeEntry[] videos = (YouTubeEntry[]) null;
          try
          {
            videos = await youTubeClient1.GetFeed(0);
          }
          catch
          {
            return notifications;
          }
          if (videos != null)
          {
            bool deletedVideos = false;
            GC.Collect();
            TileUpdater updater = (TileUpdater) null;
            if (videos.Length > 5)
            {
              YouTubeEntry[] youTubeEntryArray1 = new YouTubeEntry[5];
              for (int index = 0; index < 5; ++index)
                youTubeEntryArray1[index] = videos[index];
              videos = youTubeEntryArray1;
            }
            if (TileHelper.Platform != PlatformType.WindowsUWP)
              videos = ((IEnumerable<YouTubeEntry>) videos).Reverse<YouTubeEntry>().ToArray<YouTubeEntry>();
            int count = 0;
            Grid tileElement = (Grid) null;
            ImageBrush image = (ImageBrush) null;
            List<TextBlock> textBlocks = (List<TextBlock>) null;
            RenderTargetBitmap renderTargetBitmap1 = (RenderTargetBitmap) null;
            BitmapImage bit = (BitmapImage) null;
            YouTubeEntry[] youTubeEntryArray = videos;
            for (int index = 0; index < youTubeEntryArray.Length; ++index)
            {
              YouTubeEntry v = youTubeEntryArray[index];
              ++count;
              List<TileNotification> tileNotificationList;
              TileNotification tileNotification1;
              if (TileHelper.Platform == PlatformType.WindowsPhone && renderTask != null)
              {
                if (tileElement == null)
                {
                  bit = new BitmapImage();
                  tileElement = await TileHelper.GetVideoTileGrid();
                  ((FrameworkElement) tileElement).put_Height(336.0);
                  ((FrameworkElement) tileElement).put_Width(336.0);
                  ImageBrush imageBrush1 = new ImageBrush();
                  ((Brush) imageBrush1).put_Opacity(0.5);
                  ((TileBrush) imageBrush1).put_Stretch((Stretch) 3);
                  image = imageBrush1;
                  ImageBrush imageBrush2 = image;
                  ScaleTransform scaleTransform = new ScaleTransform();
                  scaleTransform.put_CenterX(0.5);
                  scaleTransform.put_CenterY(0.5);
                  scaleTransform.put_ScaleX(1.35);
                  scaleTransform.put_ScaleY(1.35);
                  ((Brush) imageBrush2).put_RelativeTransform((Transform) scaleTransform);
                  ((Panel) tileElement).put_Background((Brush) image);
                  textBlocks = Helper.FindChildren<TextBlock>((DependencyObject) tileElement, 100);
                  foreach (TextBlock textBlock in textBlocks)
                    textBlock.put_FontFamily(new FontFamily("Segoe WP"));
                  GC.Collect();
                }
                if (image.ImageSource != null && image.ImageSource is BitmapImage)
                {
                  (image.ImageSource as BitmapImage).put_UriSource((Uri) null);
                  image.put_ImageSource((ImageSource) null);
                  GC.Collect();
                }
                Helper.Write((object) nameof (TileHelper), (object) "Creating tile");
                if (writeMemory != null)
                  writeMemory();
                StorageFile mediumFile = await TileHelper.GetTileImageFile(id, v, (TileSize) 3);
                bool rendered = false;
                IRandomAccessStream stream = (IRandomAccessStream) null;
                bool flag1 = forceRerenderingOfTiles || mediumFile == null;
                if (!flag1)
                  flag1 = (await mediumFile.GetBasicPropertiesAsync()).Size < 1024UL;
                BitmapImage bitmapImage;
                if (flag1)
                {
                  if (image.ImageSource == null)
                  {
                    try
                    {
                      bitmapImage = bit;
                      ((BitmapSource) bitmapImage).SetSource(stream = await Helper.GetImageStream(v.GetThumb(thumbQual)));
                      bitmapImage = (BitmapImage) null;
                    }
                    catch
                    {
                    }
                    image.put_ImageSource((ImageSource) bit);
                    if (stream != null)
                    {
                      ((IDisposable) stream).Dispose();
                      stream = (IRandomAccessStream) null;
                    }
                  }
                  textBlocks[0].put_Text(v.AuthorDisplayName.ToLower());
                  textBlocks[1].put_Text(v.Title.ToUpper());
                  ((FrameworkElement) tileElement).put_Width(336.0);
                  ((UIElement) tileElement).UpdateLayout();
                  RenderTargetBitmap renderTargetBitmap2 = new RenderTargetBitmap();
                  GC.Collect();
                  mediumFile = await TileHelper.CreateTileImageFile(id, v, await renderTask((FrameworkElement) tileElement, renderTargetBitmap2), (TileSize) 3);
                  rendered = true;
                }
                string medPath = mediumFile.Path;
                mediumFile = (StorageFile) null;
                StorageFile largeFile = await TileHelper.GetTileImageFile(id, v, (TileSize) 4);
                GC.Collect();
                bool flag2 = forceRerenderingOfTiles || largeFile == null;
                if (!flag2)
                  flag2 = (await largeFile.GetBasicPropertiesAsync()).Size < 1024UL;
                if (flag2)
                {
                  if (image.ImageSource == null)
                  {
                    try
                    {
                      bitmapImage = bit;
                      ((BitmapSource) bitmapImage).SetSource(stream = await Helper.GetImageStream(v.GetThumb(thumbQual)));
                      bitmapImage = (BitmapImage) null;
                    }
                    catch
                    {
                    }
                    image.put_ImageSource((ImageSource) bit);
                    if (stream != null)
                    {
                      ((IDisposable) stream).Dispose();
                      stream = (IRandomAccessStream) null;
                    }
                  }
                  if (!rendered)
                  {
                    textBlocks[0].put_Text(v.AuthorDisplayName.ToLower());
                    textBlocks[1].put_Text(v.Title.ToUpper());
                  }
                  ((FrameworkElement) tileElement).put_Width(691.0);
                  ((UIElement) tileElement).UpdateLayout();
                  RenderTargetBitmap renderTargetBitmap3 = new RenderTargetBitmap();
                  GC.Collect();
                  largeFile = await TileHelper.CreateTileImageFile(id, v, await renderTask((FrameworkElement) tileElement, renderTargetBitmap3), (TileSize) 4);
                  rendered = true;
                }
                if (stream != null)
                {
                  ((IDisposable) stream).Dispose();
                  stream = (IRandomAccessStream) null;
                }
                string path = largeFile.Path;
                largeFile = (StorageFile) null;
                renderTargetBitmap1 = (RenderTargetBitmap) null;
                tileNotificationList = notifications;
                XmlDocument secondaryImageTileXml = await TileHelper.GetSecondaryImageTileXml(medPath, path, "name");
                tileNotificationList.Add(tileNotification1 = new TileNotification(secondaryImageTileXml));
                tileNotificationList = (List<TileNotification>) null;
                string str;
                medPath = str = (string) null;
                Helper.Write((object) nameof (TileHelper), (object) "Saved tile images");
                if (writeMemory != null)
                  writeMemory();
                mediumFile = (StorageFile) null;
                stream = (IRandomAccessStream) null;
                medPath = (string) null;
                largeFile = (StorageFile) null;
              }
              else if (TileHelper.Platform == PlatformType.WindowsUWP)
              {
                tileNotificationList = notifications;
                TileNotification tileNotification2 = await TileHelper.GetAdaptiveTileNotification(v);
                tileNotificationList.Add(tileNotification1 = tileNotification2);
                tileNotificationList = (List<TileNotification>) null;
              }
              else
                notifications.Add(tileNotification1 = TileHelper.GetPlainTileNotification(v));
              GC.Collect();
              if (updateInternally)
              {
                if (updater == null)
                {
                  updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(id);
                  updater.Clear();
                  updater.EnableNotificationQueue(true);
                }
                updater.Update(tileNotification1);
                Helper.Write((object) nameof (TileHelper), (object) "Added notification to notification queue");
              }
              if (!deletedVideos)
              {
                deletedVideos = true;
                try
                {
                  IReadOnlyList<StorageFile> filesAsync = await (await TileHelper.GetFolderForTile(id)).GetFilesAsync();
                  GC.Collect();
                  foreach (StorageFile storageFile in (IEnumerable<StorageFile>) filesAsync)
                  {
                    bool flag = false;
                    foreach (YouTubeEntry youTubeEntry in videos)
                    {
                      if (storageFile.Name.Contains(youTubeEntry.ID))
                      {
                        flag = true;
                        break;
                      }
                    }
                    if (!flag)
                    {
                      try
                      {
                        await storageFile.DeleteAsync();
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
              }
              GC.Collect();
              v = (YouTubeEntry) null;
            }
            youTubeEntryArray = (YouTubeEntry[]) null;
            updater = (TileUpdater) null;
            tileElement = (Grid) null;
            image = (ImageBrush) null;
            textBlocks = (List<TextBlock>) null;
            bit = (BitmapImage) null;
          }
          videos = (YouTubeEntry[]) null;
        }
      }
      return notifications;
    }

    public static XElement AddText(this XElement el, string text)
    {
      XElement content = new XElement((XName) nameof (text));
      content.Value = text;
      el.Add((object) content);
      return content;
    }

    public static XElement AddImage(this XElement el, string src)
    {
      XElement xelement = new XElement((XName) "image");
      xelement.GetAttribute(nameof (src)).Value = src;
      el.Add((object) xelement);
      return xelement;
    }

    public static XElement AddTile(this XElement el, TileTemplate template)
    {
      XElement xelement = new XElement((XName) "binding");
      xelement.GetAttribute(nameof (template)).Value = template.ToString();
      el.Add((object) xelement);
      return xelement;
    }

    public static XElement AddGroup(this XElement el)
    {
      XElement content = new XElement((XName) "group");
      el.Add((object) content);
      return content;
    }

    public static XElement AddSubgroup(this XElement el)
    {
      XElement content = new XElement((XName) "subgroup");
      el.Add((object) content);
      return content;
    }

    private static string placementToString(Placement placement)
    {
      switch (placement)
      {
        case Placement.Peek:
          return "peek";
        case Placement.Background:
          return "background";
        case Placement.Hero:
          return "hero";
        case Placement.AppLogoOverride:
          return "appLogoOverride";
        default:
          return placement.ToString().ToLower();
      }
    }

    public static XElement SetPlacement(this XElement el, Placement placement)
    {
      el.GetAttribute(nameof (placement)).Value = TileHelper.placementToString(placement);
      return el;
    }

    public static XElement SetTextStyle(this XElement el, TextStyle style)
    {
      el.GetAttribute("hint-style").Value = TileHelper.TextStyleToString(style);
      return el;
    }

    public static XElement SetBranding(this XElement el, Branding branding)
    {
      el.GetAttribute(nameof (branding)).Value = branding.ToString().ToLower();
      return el;
    }

    public static XElement SetTextStacking(this XElement el, TextStacking stacking)
    {
      el.GetAttribute("hint-textStacking").Value = stacking.ToString().ToLower();
      return el;
    }

    public static XElement SetWeight(this XElement el, int weight)
    {
      el.GetAttribute("hint-weight").Value = weight.ToString();
      return el;
    }

    public static XElement SetOverlay(this XElement el, int overlay)
    {
      el.GetAttribute("hint-overlay").Value = overlay.ToString();
      return el;
    }

    public static XElement SetCrop(this XElement el, Crop crop)
    {
      el.GetAttribute("hint-crop").Value = crop.ToString().ToLower();
      return el;
    }

    public static XElement SetAlign(this XElement el, Align align)
    {
      el.GetAttribute("hint-align").Value = align.ToString().ToLower();
      return el;
    }

    public static XElement SetWrap(this XElement el, bool wrap)
    {
      el.GetAttribute("hint-wrap").Value = TileHelper.boolToString(wrap);
      return el;
    }

    public static XElement SetRemoveMargin(this XElement el, bool removeMargin)
    {
      el.GetAttribute("hint-removeMargin").Value = TileHelper.boolToString(removeMargin);
      return el;
    }

    private static string boolToString(bool b) => b.ToString().ToLower();

    private static string TextStyleToString(TextStyle style) => style.ToString().ToLower().Replace("subtle", "Subtle").Replace("numeral", "Numeral").Replace("Number", "Number");

    public static void SetImage(this XmlDocument doc, int index, string src) => ((XmlElement) ((IReadOnlyList<IXmlNode>) doc.GetElementsByTagName("image"))[index]).SetAttribute(nameof (src), src);

    public static void SetText(this XmlDocument doc, int index, string text) => ((XmlElement) ((IReadOnlyList<IXmlNode>) doc.GetElementsByTagName(nameof (text)))[index]).AppendChild((IXmlNode) doc.CreateTextNode(text));

    public static void SetBranding(this XmlDocument doc, string text) => ((XmlElement) ((IReadOnlyList<IXmlNode>) doc.GetElementsByTagName("binding"))[0]).SetAttribute("branding", text);
  }
}
