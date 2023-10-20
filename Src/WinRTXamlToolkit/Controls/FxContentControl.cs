// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.FxContentControl
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Controls.Fx;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Controls
{
  public class FxContentControl : ContentControl
  {
    private readonly EventThrottler updateThrottler = new EventThrottler();
    private Image _backgroundFxImage;
    private Image _foregroundFxImage;
    private ContentPresenter _contentPresenter;
    private Grid _renderedGrid;
    private static readonly DependencyProperty _BackgroundFxProperty = DependencyProperty.Register(nameof (BackgroundFx), (Type) typeof (CpuShaderEffect), (Type) typeof (FxContentControl), new PropertyMetadata((object) null, new PropertyChangedCallback(FxContentControl.OnBackgroundFxChanged)));
    private static readonly DependencyProperty _ForegroundFxProperty = DependencyProperty.Register(nameof (ForegroundFx), (Type) typeof (CpuShaderEffect), (Type) typeof (FxContentControl), new PropertyMetadata((object) null, new PropertyChangedCallback(FxContentControl.OnForegroundFxChanged)));

    public static DependencyProperty BackgroundFxProperty => FxContentControl._BackgroundFxProperty;

    public CpuShaderEffect BackgroundFx
    {
      get => (CpuShaderEffect) ((DependencyObject) this).GetValue(FxContentControl.BackgroundFxProperty);
      set => ((DependencyObject) this).SetValue(FxContentControl.BackgroundFxProperty, (object) value);
    }

    private static void OnBackgroundFxChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FxContentControl fxContentControl = (FxContentControl) d;
      CpuShaderEffect oldValue = (CpuShaderEffect) e.OldValue;
      CpuShaderEffect backgroundFx = fxContentControl.BackgroundFx;
      fxContentControl.OnBackgroundFxChanged(oldValue, backgroundFx);
    }

    private async void OnBackgroundFxChanged(
      CpuShaderEffect oldBackgroundFx,
      CpuShaderEffect newBackgroundFx)
    {
      if (this._renderedGrid == null || ((FrameworkElement) this._renderedGrid).ActualHeight <= 0.0)
        return;
      await this.UpdateFxAsync();
    }

    public static DependencyProperty ForegroundFxProperty => FxContentControl._ForegroundFxProperty;

    public CpuShaderEffect ForegroundFx
    {
      get => (CpuShaderEffect) ((DependencyObject) this).GetValue(FxContentControl.ForegroundFxProperty);
      set => ((DependencyObject) this).SetValue(FxContentControl.ForegroundFxProperty, (object) value);
    }

    private static void OnForegroundFxChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FxContentControl fxContentControl = (FxContentControl) d;
      CpuShaderEffect oldValue = (CpuShaderEffect) e.OldValue;
      CpuShaderEffect foregroundFx = fxContentControl.ForegroundFx;
      fxContentControl.OnForegroundFxChanged(oldValue, foregroundFx);
    }

    private async void OnForegroundFxChanged(
      CpuShaderEffect oldForegroundFx,
      CpuShaderEffect newForegroundFx)
    {
      if (this._renderedGrid == null || ((FrameworkElement) this._renderedGrid).ActualHeight <= 0.0)
        return;
      await this.UpdateFxAsync();
    }

    public FxContentControl() => ((Control) this).put_DefaultStyleKey((object) typeof (FxContentControl));

    protected virtual async void OnApplyTemplate()
    {
      // ISSUE: reference to a compiler-generated method
      this.\u003C\u003En__FabricatedMethod8();
      this._backgroundFxImage = ((Control) this).GetTemplateChild("BackgroundFxImage") as Image;
      this._foregroundFxImage = ((Control) this).GetTemplateChild("ForegroundFxImage") as Image;
      this._contentPresenter = ((Control) this).GetTemplateChild("ContentPresenter") as ContentPresenter;
      this._renderedGrid = ((Control) this).GetTemplateChild("RenderedGrid") as Grid;
      if (this._renderedGrid != null)
      {
        Grid renderedGrid = this._renderedGrid;
        WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) renderedGrid).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) renderedGrid).remove_SizeChanged), new SizeChangedEventHandler(this.OnContentPresenterSizeChanged));
      }
      if (((FrameworkElement) this._renderedGrid).ActualHeight <= 0.0)
        return;
      await this.UpdateFxAsync();
    }

    private async void OnContentPresenterSizeChanged(
      object sender,
      SizeChangedEventArgs sizeChangedEventArgs)
    {
      await this.UpdateFxAsync();
    }

    public async Task UpdateFxAsync()
    {
      if (((FrameworkElement) this._renderedGrid).ActualHeight < 2.0 || ((FrameworkElement) this._renderedGrid).ActualWidth < 2.0 || this._backgroundFxImage == null || this._foregroundFxImage == null)
      {
        if (this._backgroundFxImage != null)
          this._backgroundFxImage.put_Source((ImageSource) null);
        if (this._foregroundFxImage == null)
          return;
        this._foregroundFxImage.put_Source((ImageSource) null);
      }
      else
      {
        RenderTargetBitmap rtb = new RenderTargetBitmap();
        await rtb.RenderAsync((UIElement) this._renderedGrid);
        if (rtb.PixelHeight == 0)
        {
          this._backgroundFxImage.put_Source((ImageSource) null);
          if (this._foregroundFxImage == null)
            return;
          this._foregroundFxImage.put_Source((ImageSource) null);
        }
        else
        {
          await this.UpdateBackgroundFx(rtb);
          await this.UpdateForegroundFx(rtb);
        }
      }
    }

    private async Task UpdateBackgroundFx(RenderTargetBitmap rtb)
    {
      if (((FrameworkElement) this._renderedGrid).ActualHeight < 1.0 || this._backgroundFxImage == null)
        return;
      if (this.BackgroundFx == null)
      {
        this._backgroundFxImage.put_Source((ImageSource) null);
      }
      else
      {
        int pw = rtb.PixelWidth;
        int ph = rtb.PixelHeight;
        WriteableBitmap wb = this._backgroundFxImage.Source as WriteableBitmap;
        if (wb == null || ((BitmapSource) wb).PixelWidth != pw || ((BitmapSource) wb).PixelHeight != ph)
          wb = new WriteableBitmap(pw, ph);
        await this.OnProcessBackgroundImage(rtb, wb, pw, ph);
        this._backgroundFxImage.put_Source((ImageSource) wb);
      }
    }

    private async Task UpdateForegroundFx(RenderTargetBitmap rtb)
    {
      if (((FrameworkElement) this._renderedGrid).ActualHeight < 1.0 || this._foregroundFxImage == null)
        return;
      if (this.ForegroundFx == null)
      {
        this._foregroundFxImage.put_Source((ImageSource) null);
      }
      else
      {
        int pw = rtb.PixelWidth;
        int ph = rtb.PixelHeight;
        WriteableBitmap wb = this._foregroundFxImage.Source as WriteableBitmap;
        if (wb == null || ((BitmapSource) wb).PixelWidth != pw || ((BitmapSource) wb).PixelHeight != ph)
          wb = new WriteableBitmap(pw, ph);
        await this.ProcessForegroundImage(rtb, wb, pw, ph);
        this._foregroundFxImage.put_Source((ImageSource) wb);
      }
    }

    private Task OnProcessBackgroundImage(
      RenderTargetBitmap rtb,
      WriteableBitmap wb,
      int pw,
      int ph)
    {
      return this.BackgroundFx.ProcessBitmap(rtb, wb, pw, ph);
    }

    private Task ProcessForegroundImage(
      RenderTargetBitmap rtb,
      WriteableBitmap wb,
      int pw,
      int ph)
    {
      return this.ForegroundFx.ProcessBitmap(rtb, wb, pw, ph);
    }
  }
}
