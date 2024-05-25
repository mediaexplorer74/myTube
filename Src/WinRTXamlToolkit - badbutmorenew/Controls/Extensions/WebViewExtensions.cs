// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.WebViewExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class WebViewExtensions
  {
    public static async Task<string> GetTitle(this WebView webView) => await webView.InvokeScriptAsync("eval", (IEnumerable<string>) new string[1]
    {
      "document.title"
    });

    public static async Task<string> GetAddress(this WebView webView)
    {
      string address = await webView.InvokeScriptAsync("eval", (IEnumerable<string>) new string[1]
      {
        "document.location.href"
      });
      return address != null ? address : ((object) webView.Source).ToString();
    }

    public static async Task<string> GetHead(this WebView webView)
    {
      try
      {
        string head = await webView.InvokeScriptAsync("eval", (IEnumerable<string>) new string[1]
        {
          "document.getElementsByTagName('head')[0].innerHTML"
        });
        return head;
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    private static string GetTagAttributeBySpecificAttribute(
      string htmlFragment,
      string tagName,
      string testAttributeName,
      string testAttributeValue,
      string attributeToGet)
    {
      Match match1 = new Regex(string.Format("\\<{0}[^\\>]+{1}=\\\"{2}\\\"[^\\>]+{3}=\\\"(?<retGroup>[^\\>]+?)\\\"", (object) tagName, (object) testAttributeName, (object) testAttributeValue, (object) attributeToGet), RegexOptions.Multiline).Match(htmlFragment);
      if (match1.Success)
        return match1.Groups["retGroup"].Value;
      Match match2 = new Regex(string.Format("\\<{0}[^\\>]+{3}=\\\"(?<retGroup>[^\\>]+?)\\\"[^\\>]+{1}=\\\"{2}\\\"", (object) tagName, (object) testAttributeName, (object) testAttributeValue, (object) attributeToGet), RegexOptions.Multiline).Match(htmlFragment);
      return match2.Success ? match2.Groups["retGroup"].Value : (string) null;
    }

    public static async Task<Uri> GetFavIconLink(this WebView webView)
    {
      string head = await webView.GetHead();
      if (head == null)
        return (Uri) null;
      head = head.ToLower();
      string favIconString = WebViewExtensions.GetTagAttributeBySpecificAttribute(head, "link", "rel", "shortcut icon", "href");
      string address = await webView.GetAddress();
      Uri uri = new Uri(address);
      if (favIconString != null)
      {
        if (!favIconString.ToLower().StartsWith("http://") && !favIconString.ToLower().StartsWith("https://"))
          favIconString = string.Format("{0}://{1}/{2}", (object) uri.Scheme, (object) uri.Host, (object) favIconString.TrimStart('/'));
        return new Uri(favIconString);
      }
      return new Uri(string.Format("{0}://{1}/favicon.ico", (object) uri.Scheme, (object) uri.Host));
    }
  }
}
