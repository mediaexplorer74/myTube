// Decompiled with JetBrains decompiler
// Type: RykenTube.CommentClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace RykenTube
{
  public class CommentClient : YouTubeClient<Comment>
  {
    public YouTubeOrder Order = YouTubeOrder.Published;
    public bool RelevantToMe = true;
    private string id;
    private bool repliesClient;

    public string ParentId => this.id;

    private CommentClient(int num)
      : base(num)
    {
      this.UseRandomQuery = false;
    }

    protected override EntryClient<Comment> GetRefreshClientOverride() => (EntryClient<Comment>) null;

    public CommentClient(string parentId, int num)
      : base(num)
    {
      this.DefaultCacheGroupName = "comments";
      this.ClearParts(Part.Snippet, Part.Replies);
      this.id = parentId;
      this.version = 3;
      this.UseAccessToken = true;
      if (this.version == 2)
        this.BaseAddress.BaseAddress = (YouTube.HTTPS ? "https" : "http") + "://gdata.youtube.com/feeds/api/videos/" + parentId + "/comments";
      else if (this.version == 3)
      {
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/commentThreads";
        this.BaseAddress["videoId"] = parentId;
        this.BaseAddress["textFormat"] = "plainText";
      }
      this.UseRandomQuery = false;
    }

    protected override string GetCacheName() => this.id;

    public static CommentClient GetRepliesClient(string topLevelCommentId, int num)
    {
      CommentClient repliesClient = new CommentClient(num);
      repliesClient.repliesClient = true;
      repliesClient.version = 3;
      repliesClient.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/comments";
      repliesClient.ClearParts(new Part[1]);
      repliesClient.BaseAddress["parentId"] = topLevelCommentId;
      repliesClient.BaseAddress["textFormat"] = "plainText";
      return repliesClient;
    }

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      this.BaseAddress.RemoveValue("start-index");
      if (this.version == 2)
      {
        if (this.Order == YouTubeOrder.Published)
          this.BaseAddress.SetValue("orderby", (object) "published");
        else
          this.BaseAddress.RemoveValue("orderby");
      }
      else
      {
        if (this.version != 3 || this.repliesClient)
          return;
        switch (this.Order)
        {
          case YouTubeOrder.Relevance:
          case YouTubeOrder.Rating:
            this.BaseAddress.SetValue("order", (object) "relevance");
            break;
          default:
            this.BaseAddress.RemoveValue("order");
            break;
        }
      }
    }

    public static Task<Comment> Post(string videoId, string content)
    {
      if (!YouTube.IsSignedIn)
        throw new InvalidOperationException("Must be signed in to post a comment");
      if (YouTube.DefaultVersion != 2)
        return Task.Run<Comment>((Func<Task<Comment>>) (async () =>
        {
          URLConstructor urlConstructor = new URLConstructor("https://www.googleapis.com/youtube/v3/commentThreads");
          urlConstructor["part"] = "snippet";
          urlConstructor["shareOnGooglePlus"] = "false";
          urlConstructor["key"] = YouTube.APIKey;
          JObject jobject1 = new JObject();
          JObject jobject2 = new JObject();
          jobject1.Add("snippet", (JToken) jobject2);
          JObject jobject3 = new JObject();
          jobject2.Add("topLevelComment", (JToken) jobject3);
          jobject2.Add(nameof (videoId), (JToken) videoId);
          JObject jobject4 = new JObject();
          jobject3.Add("snippet", (JToken) jobject4);
          jobject4.Add("textOriginal", (JToken) content);
          string str1 = jobject1.ToString();
          HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
          HttpStringContent httpStringContent = new HttpStringContent(str1, (UnicodeEncoding) 0, "application/json");
          httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
          ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
          Comment comment;
          try
          {
            HttpResponseMessage response = await httpClient.PostAsync(urlConstructor.ToUri(UriKind.Absolute), (IHttpContent) httpStringContent);
            if (response.IsSuccessStatusCode)
            {
              comment = new Comment(JToken.Parse(await response.Content.ReadAsStringAsync()));
            }
            else
            {
              string str2 = await response.Content.ReadAsStringAsync();
              throw new Exception("Response failed with status code " + (object) response.StatusCode + " and error:\n" + str2);
            }
          }
          catch
          {
            throw;
          }
          return comment;
        }));
      TaskCompletionSource<Comment> tcs = new TaskCompletionSource<Comment>();
      XDocument x = new XDocument();
      XNamespace xnamespace = (XNamespace) "http://www.w3.org/2005/Atom";
      XElement content1 = new XElement(xnamespace + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) ((XNamespace) "http://gdata.youtube.com/schemas/2007").NamespaceName));
      content1.Add((object) new XElement(xnamespace + nameof (content))
      {
        Value = content
      });
      x.Add((object) content1);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/videos/" + videoId + "/comments");
      req.Method = "POST";
      req.ContentType = "application/atom+xml";
      req.Headers["Authorization"] = "Bearer " + YouTube.AccessToken;
      req.Headers["GData-Version"] = "2";
      req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
      req.BeginGetRequestStream((AsyncCallback) (a =>
      {
        using (StreamWriter streamWriter = new StreamWriter(req.EndGetRequestStream(a)))
        {
          streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
          streamWriter.WriteLine(x.ToString());
        }
        req.BeginGetResponse((AsyncCallback) (a2 =>
        {
          try
          {
            tcs.SetResult((Comment) null);
          }
          catch (WebException ex)
          {
            tcs.SetException((Exception) ex);
            using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
              streamReader.ReadToEnd();
          }
        }), (object) 0);
      }), (object) 0);
      return tcs.Task;
    }

    public static async Task Delete(string commentId)
    {
      // ISSUE: unable to decompile the method.
    }

    public static async Task<Comment> Reply(string content, string parentId)
    {
      if (YouTube.AccessToken == null)
        throw new InvalidOperationException("Must be signed into YouTube");
      string uriString = "https://www.googleapis.com/youtube/v3/comments?part=snippet&key=" + YouTube.APIKey;
      HttpStringContent httpStringContent = new HttpStringContent(new JObject()
      {
        ["snippet"] = ((JToken) new JObject()
        {
          [nameof (parentId)] = (JToken) parentId,
          ["textOriginal"] = (JToken) content
        }),
        ["kind"] = ((JToken) "youtube#comment")
      }.ToString(), (UnicodeEncoding) 0, "application/json");
      HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
      httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
      ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
      Comment comment;
      try
      {
        HttpResponseMessage result = await httpClient.PostAsync(new Uri(uriString), (IHttpContent) httpStringContent);
        if (result.IsSuccessStatusCode)
        {
          comment = new Comment(JToken.Parse(await result.Content.ReadAsStringAsync()))
          {
            CommentThreadId = parentId
          };
        }
        else
        {
          if (result.Content != null)
          {
            string str = await result.Content.ReadAsStringAsync();
          }
          throw new Exception("Failed to post reply with error code " + (object) result.StatusCode);
        }
      }
      catch
      {
        throw;
      }
      return comment;
    }

    protected override Task<Comment[]> InternalRefresh(
      Comment[] items,
      EntryClient<Comment> refreshClient)
    {
      throw new NotImplementedException();
    }

    protected override async Task<Comment[]> ParseV2(XDocument xdoc)
    {
      foreach (XElement element in xdoc.Element(YouTube.Atom("feed")).Elements(YouTube.Atom("link")))
      {
        if (element.Attribute((XName) "rel") != null && element.Attribute((XName) "rel").Value == "next" && element.Attribute((XName) "href") != null)
        {
          URLConstructor urlConstructor = new URLConstructor(element.Attribute((XName) "href").Value);
          if (urlConstructor.ContainsKey("start-token"))
            urlConstructor.GetValue("start-token");
        }
      }
      List<XElement> list = xdoc.Descendants(YouTube.Atom("entry")).ToList<XElement>();
      Comment[] v2 = new Comment[list.Count];
      for (int index = 0; index < list.Count; ++index)
        v2[index] = new Comment(list[index]);
      return v2;
    }

    protected override async Task<Comment[]> ParseV3(JObject jo)
    {
      JArray jarray = (JArray) jo["items"];
      Comment[] v3 = new Comment[jarray.Count];
      for (int index = 0; index < v3.Length; ++index)
        v3[index] = new Comment(jarray[index]);
      return v3;
    }

    protected override Task<Comment[]> ParseOther(string s) => throw new NotImplementedException();

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();
  }
}
