// Decompiled with JetBrains decompiler
// Type: RykenTube.HttpFilters.DashFilter
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace RykenTube.HttpFilters
{
  public class DashFilter : IHttpFilter, IDisposable
  {
    private HttpBaseProtocolFilter baseFilter;
    private bool recordManifest;

    public DashManifest Manifest { get; private set; }

    public DashFilter() => this.baseFilter = new HttpBaseProtocolFilter();

    public void Dispose()
    {
    }

    public void StartRecordingManifests() => this.recordManifest = true;

    public void StopRecordingManifests() => this.recordManifest = false;

    public IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> SendRequestAsync(
      HttpRequestMessage request)
    {
      if (!request.RequestUri.OriginalString.Contains("manifest.googlevideo"))
        return this.baseFilter.SendRequestAsync(request);
      try
      {
        return AsyncInfo.Run<HttpResponseMessage, HttpProgress>((Func<CancellationToken, IProgress<HttpProgress>, Task<HttpResponseMessage>>) (async (cancellationToken, progress) =>
        {
          HttpResponseMessage httpResponseMessage;
          try
          {
            HttpResponseMessage response = await this.baseFilter.SendRequestAsync(request);
            response.Content.Headers.ContentType.MediaType = "application/dash+xml";
            DashManifest dashManifest =
                new DashManifest(XElement.Parse(await response.Content.ReadAsStringAsync()));
            dashManifest.GetProfiles();
            dashManifest.RemoveAdaptationSet("video/webm");
            dashManifest.RemoveAdaptationSet("audio/webm");
            if (this.recordManifest)
              this.Manifest = dashManifest;
            dashManifest.GetString();
            HttpStreamContent httpStreamContent = new HttpStreamContent(await dashManifest.GetInputStream());
            httpStreamContent.Headers.ContentType = response.Content.Headers.ContentType;
            response.Content = (IHttpContent) httpStreamContent;
            httpResponseMessage = response;
          }
          catch
          {
            throw;
          }
          return httpResponseMessage;
        }));
      }
      catch
      {
        return this.baseFilter.SendRequestAsync(request);
      }
    }
  }
}
