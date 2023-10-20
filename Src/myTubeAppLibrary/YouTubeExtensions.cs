// Decompiled with JetBrains decompiler
// Type: myTube.YouTubeExtensions
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace myTube
{
  public static class YouTubeExtensions
  {
    public static async Task<Dictionary<YouTubeQuality, long>> GetFileSizes(
      this YouTubeInfo info,
      params YouTubeQuality[] quality)
    {
      HttpClient httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
      Dictionary<YouTubeQuality, Task<HttpResponseMessage>> dictionary = new Dictionary<YouTubeQuality, Task<HttpResponseMessage>>();
      foreach (YouTubeQuality youTubeQuality in quality)
      {
        if (info.HasQuality(youTubeQuality))
        {
          HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, info.GetLink(youTubeQuality).ToUri(UriKind.Absolute));
          Task<HttpResponseMessage> task = httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
          dictionary.Add(youTubeQuality, task);
        }
      }
      Dictionary<YouTubeQuality, long> sizes = new Dictionary<YouTubeQuality, long>();
      int pos = 0;
      foreach (KeyValuePair<YouTubeQuality, Task<HttpResponseMessage>> keyValuePair in dictionary)
      {
        KeyValuePair<YouTubeQuality, Task<HttpResponseMessage>> kvp = keyValuePair;
        ++pos;
        try
        {
          HttpResponseMessage httpResponseMessage = await kvp.Value;
          long? contentLength = httpResponseMessage.Content.Headers.ContentLength;
          if (contentLength.HasValue)
          {
            contentLength = httpResponseMessage.Content.Headers.ContentLength;
            sizes.Add(kvp.Key, contentLength.Value);
          }
          httpResponseMessage.RequestMessage.Dispose();
          httpResponseMessage.Content.Dispose();
          httpResponseMessage.Dispose();
        }
        catch
        {
        }
        kvp = new KeyValuePair<YouTubeQuality, Task<HttpResponseMessage>>();
      }
      GC.Collect();
      return sizes;
    }
  }
}
