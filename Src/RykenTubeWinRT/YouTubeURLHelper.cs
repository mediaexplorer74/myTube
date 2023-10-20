// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeURLHelper
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RykenTube
{
  public static class YouTubeURLHelper
  {
    public static YouTubeURLInfo GetUrlType(Uri uri) => YouTubeURLHelper.GetUrlType(uri.OriginalString);

    public static YouTubeURLInfo GetUrlType(string url)
    {
      url = url.Replace("&amp;", "&");
      url = url.Replace("\\u0026", "&");
      YouTubeURLInfo urlType = new YouTubeURLInfo()
      {
        Type = YouTubeURLType.None
      };
      URLConstructor urlConstructor = new URLConstructor(url);
      urlType.OriginalURL = url;
      if (urlConstructor.BaseAddress.Contains("youtube.com/"))
      {
        if (urlConstructor.BaseAddress.Contains("youtube.com/watch"))
        {
          if (!string.IsNullOrWhiteSpace(urlConstructor["v"]))
          {
            urlType.Type = YouTubeURLType.Video;
            if (urlConstructor.ContainsKey("v"))
              urlType.ID = urlConstructor["v"];
            if (urlConstructor.ContainsKey("list"))
              urlType.AddSecondaryInfo("list", urlConstructor["list"]);
          }
        }
        else if (urlConstructor.BaseAddress.Contains("youtube.com/user/") || urlConstructor.BaseAddress.Contains("youtube.com/channel/"))
        {
          string[] source = urlConstructor.BaseAddress.Split('/');
          if (!string.IsNullOrWhiteSpace(((IEnumerable<string>) source).Last<string>()))
          {
            urlType.Type = YouTubeURLType.Channel;
            urlType.ID = ((IEnumerable<string>) source).Last<string>();
          }
        }
        else if (urlConstructor.BaseAddress.Contains("youtube.com/playlist"))
        {
          if (urlConstructor.ContainsKey("list"))
          {
            urlType.Type = YouTubeURLType.Playlist;
            urlType.ID = urlConstructor["list"];
          }
        }
        else
        {
          string[] source = urlConstructor.BaseAddress.Split('/');
          if (!string.IsNullOrWhiteSpace(((IEnumerable<string>) source).Last<string>()))
          {
            urlType.Type = YouTubeURLType.Channel;
            urlType.ID = ((IEnumerable<string>) source).Last<string>();
          }
        }
      }
      else if (urlConstructor.BaseAddress.Contains("youtu.be/"))
      {
        urlType.Type = YouTubeURLType.Video;
        string[] source = urlConstructor.BaseAddress.Split('/');
        if (source.Length > 1)
          urlType.ID = ((IEnumerable<string>) source).Last<string>();
      }
      else if (urlConstructor.ContainsKey("v"))
      {
        if (!string.IsNullOrWhiteSpace(urlConstructor["v"]))
        {
          urlType.Type = YouTubeURLType.Video;
          urlType.ID = urlConstructor["v"];
          if (urlConstructor.ContainsKey("list"))
            urlType.AddSecondaryInfo("list", urlConstructor["list"]);
        }
      }
      else if (urlConstructor.ContainsKey("list"))
      {
        urlType.Type = YouTubeURLType.Playlist;
        urlType.ID = urlConstructor["list"];
      }
      else
        urlType.Type = YouTubeURLType.None;
      return urlType;
    }

    public static async Task<string[]> FindFromURL(string url)
    {
      URLConstructor constructor = new URLConstructor(url);
      List<string> ids = new List<string>();
      if (constructor.BaseAddress.Contains("youtube.com/watch") && constructor.ContainsKey("v"))
        return new string[1]{ constructor["v"] };
      try
      {
        string stringAsync = await new HttpClient().GetStringAsync(url);
        if (stringAsync != null)
        {
          int startIndex1 = 0;
          int num1 = -1;
          int startIndex2;
          while ((startIndex2 = stringAsync.IndexOf("src=\"", startIndex1)) != -1)
          {
            string inQuotes = YouTubeURLHelper.findInQuotes(stringAsync, startIndex2);
            startIndex1 = startIndex2 + 5 + inQuotes.Length;
            string str = (string) null;
            URLConstructor urlConstructor = new URLConstructor(inQuotes);
            if (urlConstructor.BaseAddress.Contains("youtube.com/embed/"))
              str = ((IEnumerable<string>) urlConstructor.BaseAddress.Split('/')).Last<string>();
            if (str != null && !ids.Contains(str))
              ids.Add(str);
          }
          num1 = -1;
          int startIndex3 = 0;
          int startIndex4;
          while ((startIndex4 = stringAsync.IndexOf("href=\"", startIndex3)) != -1)
          {
            string inQuotes = YouTubeURLHelper.findInQuotes(stringAsync, startIndex4);
            startIndex3 = startIndex4 + 6 + inQuotes.Length;
            string str = (string) null;
            URLConstructor urlConstructor = new URLConstructor(inQuotes);
            if (urlConstructor.BaseAddress.Contains("watch") && urlConstructor.ContainsKey("v"))
              str = urlConstructor["v"];
            if (str != null && !ids.Contains(str))
              ids.Add(str);
          }
          if (constructor.BaseAddress.Contains("theverge.com"))
          {
            num1 = -1;
            int startIndex5 = 0;
            int startIndex6;
            while ((startIndex6 = stringAsync.IndexOf(".addVideo", startIndex5)) != -1)
            {
              int num2 = stringAsync.IndexOf("]);", startIndex6);
              startIndex5 = startIndex6 + 10;
              if (num2 != -1)
              {
                string s = stringAsync.Substring(startIndex6, num2 - startIndex6);
                if (s.ToLower().Contains("youtube"))
                {
                  int startIndex7 = s.IndexOf("provider_video_id");
                  if (startIndex7 != -1)
                  {
                    int startIndex8 = s.ToLower().IndexOf(":", startIndex7);
                    if (startIndex8 != 1)
                    {
                      string inQuotes = YouTubeURLHelper.findInQuotes(s, startIndex8);
                      if (!string.IsNullOrWhiteSpace(inQuotes))
                        ids.Add(inQuotes);
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch
      {
      }
      return ids.ToArray();
    }

    public static YouTubeURLInfo[] GetIDsFromWords(string content)
    {
      List<YouTubeURLInfo> youTubeUrlInfoList = new List<YouTubeURLInfo>();
      bool flag = false;
      int startIndex = 0;
      content += " ";
      for (int index = 0; index < content.Length; ++index)
      {
        if (content[index] != ' ')
        {
          if (!flag)
          {
            startIndex = index;
            flag = true;
          }
        }
        else if (flag)
        {
          flag = false;
          YouTubeURLInfo urlType = YouTubeURLHelper.GetUrlType(content.Substring(startIndex, index - startIndex));
          if (urlType.Type != YouTubeURLType.None)
            youTubeUrlInfoList.Add(urlType);
        }
      }
      return youTubeUrlInfoList.ToArray();
    }

    public static YouTubeURLInfo[] GetIDsFromHtml(string content) => YouTubeURLHelper.GetIDsFromHtml(content, "href=\"", ":\"");

    public static YouTubeURLInfo[] GetIDsFromQuotes(
      string content,
      YouTubeURLType type,
      params string[] searhStrings)
    {
      List<YouTubeURLInfo> youTubeUrlInfoList = new List<YouTubeURLInfo>();
      foreach (string searhString in searhStrings)
      {
        foreach (string allInQuote in YouTubeURLHelper.findAllInQuotes(content, searhString))
          youTubeUrlInfoList.Add(new YouTubeURLInfo()
          {
            ID = allInQuote,
            OriginalURL = allInQuote,
            Type = type
          });
      }
      return youTubeUrlInfoList.ToArray();
    }

    private static List<string> findAllInQuotes(string content, string hrefString)
    {
      List<string> allInQuotes = new List<string>();
      int startIndex = 0;
      bool flag = false;
      int length = hrefString.Length;
      int index1 = 0;
      for (int index2 = 0; index2 < content.Length; ++index2)
      {
        char ch = content[index2];
        if (!flag)
        {
          if ((int) ch == (int) hrefString[index1])
          {
            ++index1;
            if (index1 >= length)
            {
              flag = true;
              index1 = 0;
              startIndex = index2 + 1;
            }
          }
          else
            index1 = 0;
        }
        else if (ch == '"' && index2 > startIndex)
        {
          flag = false;
          string str = content.Substring(startIndex, index2 - startIndex);
          allInQuotes.Add(str);
        }
      }
      return allInQuotes;
    }

    private static List<YouTubeURLInfo> getIDsFromHtml(string content, string hrefString)
    {
      List<YouTubeURLInfo> idsFromHtml = new List<YouTubeURLInfo>();
      int startIndex = 0;
      bool flag = false;
      int length = hrefString.Length;
      int index1 = 0;
      for (int index2 = 0; index2 < content.Length; ++index2)
      {
        char ch = content[index2];
        if (!flag)
        {
          if ((int) ch == (int) hrefString[index1])
          {
            ++index1;
            if (index1 >= length)
            {
              flag = true;
              index1 = 0;
              startIndex = index2 + 1;
            }
          }
          else
            index1 = 0;
        }
        else if (ch == '"' && index2 > startIndex)
        {
          flag = false;
          YouTubeURLInfo urlType = YouTubeURLHelper.GetUrlType(content.Substring(startIndex, index2 - startIndex));
          if (urlType.Type != YouTubeURLType.None)
            idsFromHtml.Add(urlType);
        }
      }
      return idsFromHtml;
    }

    public static YouTubeURLInfo[] GetIDsFromHtml(string content, params string[] searchStrings)
    {
      List<YouTubeURLInfo> youTubeUrlInfoList = new List<YouTubeURLInfo>();
      foreach (string searchString in searchStrings)
      {
        foreach (YouTubeURLInfo youTubeUrlInfo in YouTubeURLHelper.getIDsFromHtml(content, searchString))
          youTubeUrlInfoList.Add(youTubeUrlInfo);
      }
      return youTubeUrlInfoList.ToArray();
    }

    private static string findInQuotes(string s, int startIndex)
    {
      string inQuotes = "";
      bool flag = false;
      for (int index = startIndex; index < s.Length; ++index)
      {
        if (flag)
        {
          if (s[index] != '"')
            inQuotes += s[index].ToString();
          else
            break;
        }
        else if (s[index] == '"')
          flag = true;
      }
      return inQuotes;
    }
  }
}
