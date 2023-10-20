// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.WriteableBitmapExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class WriteableBitmapExtensions
  {
    public static async Task WaitForLoadedAsync(this WriteableBitmap wb, int timeoutInMs = 0)
    {
      int totalWait = 0;
      while (((BitmapSource) wb).PixelWidth <= 1 && ((BitmapSource) wb).PixelHeight <= 1)
      {
        await Task.Delay(10);
        totalWait += 10;
        if (timeoutInMs > 0 && totalWait > timeoutInMs)
          break;
      }
    }
  }
}
