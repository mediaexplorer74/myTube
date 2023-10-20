// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Net.WebHelper
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Net.Http;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace WinRTXamlToolkit.Net
{
  public static class WebHelper
  {
    public static bool IsConnectedToInternet()
    {
      ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
      return connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == 3;
    }

    public static async Task<string> DownloadStringAsync(string url)
    {
      HttpClientHandler handler = new HttpClientHandler()
      {
        UseDefaultCredentials = true,
        AllowAutoRedirect = true
      };
      HttpResponseMessage response = await new HttpClient((HttpMessageHandler) handler)
      {
        MaxResponseContentBufferSize = 196608L
      }.GetAsync(url);
      response.EnsureSuccessStatusCode();
      string responseBody = await response.Content.ReadAsStringAsync();
      return responseBody;
    }
  }
}
