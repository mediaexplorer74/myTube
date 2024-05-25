// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ClipToBoundsHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class ClipToBoundsHandler
  {
    private FrameworkElement _fe;

    public void Attach(FrameworkElement fe)
    {
      this._fe = fe;
      this.UpdateClipGeometry();
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(fe.add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(fe.remove_SizeChanged), new SizeChangedEventHandler(this.OnSizeChanged));
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
      if (this._fe == null)
        return;
      this.UpdateClipGeometry();
    }

    private void UpdateClipGeometry()
    {
      FrameworkElement fe = this._fe;
      RectangleGeometry rectangleGeometry1 = new RectangleGeometry();
      rectangleGeometry1.put_Rect((Rect) new Rect(0.0, 0.0, this._fe.ActualWidth, this._fe.ActualHeight));
      RectangleGeometry rectangleGeometry2 = rectangleGeometry1;
      ((UIElement) fe).put_Clip(rectangleGeometry2);
    }

    public void Detach()
    {
      if (this._fe == null)
        return;
      WindowsRuntimeMarshal.RemoveEventHandler<SizeChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._fe.remove_SizeChanged), new SizeChangedEventHandler(this.OnSizeChanged));
      this._fe = (FrameworkElement) null;
    }
  }
}
