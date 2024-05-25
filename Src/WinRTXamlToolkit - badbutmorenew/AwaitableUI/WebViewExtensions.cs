// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.WebViewExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class WebViewExtensions
  {
    public static async Task NavigateAsync(this WebView webView, Uri source)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WebViewExtensions.\u003C\u003Ec__DisplayClass1 cDisplayClass1 = new WebViewExtensions.\u003C\u003Ec__DisplayClass1()
      {
        webView = webView,
        tcs = new TaskCompletionSource<object>(),
        nceh = (TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>) null
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      cDisplayClass1.nceh = new TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>((object) cDisplayClass1, __methodptr(\u003CNavigateAsync\u003Eb__0));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>>((Func<TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>, EventRegistrationToken>) new Func<TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>, EventRegistrationToken>(cDisplayClass1.webView.add_NavigationCompleted), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(cDisplayClass1.webView.remove_NavigationCompleted), cDisplayClass1.nceh);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass1.webView.Navigate((Uri) source);
      // ISSUE: reference to a compiler-generated field
      object task = await cDisplayClass1.tcs.Task;
    }
  }
}
