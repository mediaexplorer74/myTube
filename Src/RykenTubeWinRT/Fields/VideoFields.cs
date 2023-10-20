// Decompiled with JetBrains decompiler
// Type: RykenTube.Fields.VideoFields
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

namespace RykenTube.Fields
{
  public static class VideoFields
  {
    public const string StandardVideoFields = "kind, nextPageToken, items(kind, id, snippet(publishedAt, channelId, title, description, thumbnails, channelTitle, liveBroadcastContent, categoryId), statistics,contentDetails(duration, licensedContent, projection))";
    public const string PlaylistItemFields = "kind, nextPageToken, items(kind, id, snippet(title, resourceId(videoId), playlistId), contentDetails(videoId), status)";
    public const string PlaylistVideoFields = "kind, items(kind, id, snippet(title, publishedAt, channelId, thumbnails, description, channelTitle, liveBroadcastContent, categoryId), statistics,contentDetails(duration, licensedContent, projection))";
  }
}
