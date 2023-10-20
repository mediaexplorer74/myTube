// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.DelayedLoadControl
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "PART_ContentBorder", Type = typeof (Border))]
  public class DelayedLoadControl : Control
  {
    private const string ContentBorderName = "PART_ContentBorder";
    private Border _contentBorder;
    private int _loadRequestId;
    public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof (Delay), (Type) typeof (TimeSpan), (Type) typeof (DelayedLoadControl), new PropertyMetadata((object) TimeSpan.FromSeconds(0.0)));
    public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof (ContentTemplate), (Type) typeof (DataTemplate), (Type) typeof (DelayedLoadControl), new PropertyMetadata((object) null));

    public TimeSpan Delay
    {
      get => (TimeSpan) ((DependencyObject) this).GetValue(DelayedLoadControl.DelayProperty);
      set => ((DependencyObject) this).SetValue(DelayedLoadControl.DelayProperty, (object) value);
    }

    public DataTemplate ContentTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(DelayedLoadControl.ContentTemplateProperty);
      set => ((DependencyObject) this).SetValue(DelayedLoadControl.ContentTemplateProperty, (object) value);
    }

    public DelayedLoadControl()
    {
      this.put_DefaultStyleKey((object) typeof (DelayedLoadControl));
      DelayedLoadControl delayedLoadControl1 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) delayedLoadControl1).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) delayedLoadControl1).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      DelayedLoadControl delayedLoadControl2 = this;
      WindowsRuntimeMarshal.AddEventHandler<DependencyPropertyChangedEventHandler>((Func<DependencyPropertyChangedEventHandler, EventRegistrationToken>) new Func<DependencyPropertyChangedEventHandler, EventRegistrationToken>(((Control) delayedLoadControl2).add_IsEnabledChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Control) delayedLoadControl2).remove_IsEnabledChanged), new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged));
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._contentBorder = this.GetTemplateChild("PART_ContentBorder") as Border;
    }

    private void OnIsEnabledChanged(
      object sender,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      ++this._loadRequestId;
      if (!this.IsEnabled)
        return;
      this.DelayedLoad();
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      if (!this.IsEnabled)
        return;
      this.DelayedLoad();
    }

    private async void DelayedLoad()
    {
      ++this._loadRequestId;
      int handledRequestId = this._loadRequestId;
      await Task.Delay((TimeSpan) this.Delay);
      if (handledRequestId != this._loadRequestId || this._contentBorder.Child != null)
        return;
      this._contentBorder.put_Child((UIElement) this.ContentTemplate.LoadContent());
    }
  }
}
