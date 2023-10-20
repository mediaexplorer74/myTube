// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeHelpers
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RykenTube
{
  public static class YouTubeHelpers
  {
    public static string GetBodyStringFromNavigateJson(JToken json)
    {
      string fromNavigateJson = (string) null;
      if (json[(object) "body"] != null)
      {
        JToken jtoken = json[(object) "body"][(object) "content"];
        if (jtoken != null)
        {
          if (jtoken.Type == JTokenType.String)
            fromNavigateJson = (string) jtoken;
          else if (jtoken.Type == JTokenType.Object && jtoken is JObject jobject)
          {
            foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
            {
              if (keyValuePair.Value.Type == JTokenType.String)
              {
                fromNavigateJson = (string) keyValuePair.Value;
                break;
              }
            }
          }
        }
      }
      else if (json[(object) "content_html"] != null)
        fromNavigateJson = (string) json[(object) "content_html"];
      return fromNavigateJson;
    }

    public static string GetBodyStringFromNavigateJson(JArray json)
    {
      foreach (JToken child in json.Children())
      {
        string fromNavigateJson = YouTubeHelpers.GetBodyStringFromNavigateJson(child);
        if (fromNavigateJson != null)
          return fromNavigateJson;
      }
      return (string) null;
    }

    public static string GetSessionTokenFromHTML(
      string page,
      string searchString = "data-token=",
      string startFrom = null)
    {
      int num = page.IndexOf(startFrom ?? searchString);
      if (num == -1)
        num = 0;
      int startIndex = 0;
      int index1 = 0;
      bool flag1 = false;
      string sessionTokenFromHtml = (string) null;
      bool flag2 = false;
      for (int index2 = num; index2 < page.Length; ++index2)
      {
        char ch = page[index2];
        if (!flag1)
        {
          if (ch == '"' && (flag2 || (int) ch != (int) searchString[index1]))
          {
            flag1 = true;
            if (flag2)
              startIndex = index2 + 1;
          }
          if ((int) ch == (int) searchString[index1])
          {
            ++index1;
            if (index1 >= searchString.Length && !flag2)
            {
              flag2 = true;
              index1 = 0;
              startIndex = index2 + 2;
            }
          }
          else
            index1 = 0;
        }
        else
        {
          index1 = 0;
          if (ch == '"')
          {
            flag1 = false;
            if (index2 > startIndex & flag2)
            {
              sessionTokenFromHtml = page.Substring(startIndex, index2 - startIndex);
              break;
            }
          }
        }
      }
      return sessionTokenFromHtml;
    }

    public static string GetStringFromQuotesHTML(string page, string searchString)
    {
      int num = page.IndexOf(searchString);
      if (num == -1)
        num = 0;
      int startIndex = 0;
      int index1 = 0;
      bool flag1 = false;
      string stringFromQuotesHtml = (string) null;
      bool flag2 = false;
      for (int index2 = num; index2 < page.Length; ++index2)
      {
        char ch = page[index2];
        if (!flag1)
        {
          if (ch == '"' && (flag2 || (int) ch != (int) searchString[index1]))
          {
            flag1 = true;
            if (flag2)
              startIndex = index2 + 1;
          }
          if ((int) ch == (int) searchString[index1])
          {
            ++index1;
            if (index1 >= searchString.Length && !flag2)
            {
              flag2 = true;
              index1 = 0;
              startIndex = index2 + 2;
            }
          }
          else
            index1 = 0;
        }
        else
        {
          index1 = 0;
          if (ch == '"')
          {
            flag1 = false;
            if (index2 > startIndex & flag2)
            {
              stringFromQuotesHtml = page.Substring(startIndex, index2 - startIndex);
              break;
            }
          }
        }
      }
      return stringFromQuotesHtml;
    }

    public static string GetSetPlaylistVideoId(string page, string videoId)
    {
      string str1 = "data-set-video-id=\"";
      string str2 = "data-video-id=\"";
      string setPlaylistVideoId = (string) null;
      int num1 = 0;
      for (int index1 = 0; index1 < page.Length; ++index1)
      {
        int num2 = (int) page[index1];
        if (num2 == 60)
          num1 = index1;
        if (num2 == 62)
        {
          setPlaylistVideoId = (string) null;
          bool flag = false;
          for (int index2 = num1; index2 < index1; ++index2)
          {
            if (page[index2] == '"')
            {
              if (page.Substring(index2 - (str1.Length - 1), str1.Length) == str1)
              {
                setPlaylistVideoId = page.Substring(index2 + 1, page.IndexOf('"', index2 + 1) - index2 - 1);
                if (flag)
                  return setPlaylistVideoId;
              }
              if (page.Substring(index2 - (str2.Length - 1), str2.Length) == str2 && page.Substring(index2 + 1, page.IndexOf('"', index2 + 1) - index2 - 1) == videoId)
              {
                flag = true;
                if (setPlaylistVideoId != null)
                  return setPlaylistVideoId;
              }
            }
          }
        }
      }
      return setPlaylistVideoId;
    }

    public static string GenerateMixPlaylistIDForVideo(YouTubeEntry entry) => "RD" + entry.ID;

    public static YouTubeClient<YouTubeEntry> GenerateMixPlaylistClientForVideo(
      YouTubeEntry entry,
      int howMany)
    {
      return (YouTubeClient<YouTubeEntry>) new PlaylistClient(YouTubeHelpers.GenerateMixPlaylistIDForVideo(entry), howMany);
    }
  }
}
