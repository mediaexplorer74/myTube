// Decompiled with JetBrains decompiler
// Type: myTube.UploadProgressPanel
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace myTube
{
  public sealed class UploadProgressPanel : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScaleTransform progressScale;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl cancelButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock percentText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public UploadInfo Upload => ((FrameworkElement) this).DataContext as UploadInfo;

    public UploadProgressPanel()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(UploadProgressPanel_DataContextChanged)));
    }

    private void Progress(UploadOperation upload)
    {
      if (upload.Progress.TotalBytesToSend > 0UL)
        this.progressTo((double) upload.Progress.BytesSent / (double) upload.Progress.TotalBytesToSend);
      else
        this.progressTo(0.0);
      if (upload.Progress.Status == 7)
      {
        this.percentText.put_Text(App.Strings["common.error", "error"].ToLower());
        ((UIElement) this.cancelButton).put_IsHitTestVisible(true);
        Ani.Begin((DependencyObject) this.cancelButton, "Opacity", 1.0, 0.2);
      }
      else if (upload.Progress.Status == 6)
      {
        this.percentText.put_Text(App.Strings["common.canceled", "canceled"].ToLower());
        ((UIElement) this.cancelButton).put_IsHitTestVisible(false);
        Ani.Begin((DependencyObject) this.cancelButton, "Opacity", 0.5, 0.2);
      }
      else if (upload.Progress.Status == 5)
      {
        ((UIElement) this.cancelButton).put_IsHitTestVisible(false);
        Ani.Begin((DependencyObject) this.cancelButton, "Opacity", 0.5, 0.2);
      }
      else
      {
        ((UIElement) this.cancelButton).put_IsHitTestVisible(true);
        Ani.Begin((DependencyObject) this.cancelButton, "Opacity", 1.0, 0.2);
      }
    }

    private void progressTo(double p)
    {
      Ani.Begin((DependencyObject) this.progressScale, "ScaleX", p, 0.2, 2.0);
      if (p < 1.0)
        this.percentText.put_Text(Math.Round(p * 100.0, 0).ToString() + "%");
      else
        this.percentText.put_Text(App.Strings["common.done", "done"].ToLower());
    }

    private async void UploadProgressPanel_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      if (this.Upload == null)
        return;
      UploadOperation uploadOperation = await App.GlobalObjects.UploadsManager.GetUploadOperation(this.Upload);
      if (uploadOperation != null)
      {
        WindowsRuntimeSystemExtensions.AsTask<UploadOperation, UploadOperation>(uploadOperation.AttachAsync(), (IProgress<UploadOperation>) new System.Progress<UploadOperation>(new Action<UploadOperation>(this.Progress)));
        this.Progress(uploadOperation);
      }
      else
      {
        ((UIElement) this.cancelButton).put_IsHitTestVisible(false);
        Ani.Begin((DependencyObject) this.cancelButton, "Opacity", 0.5, 0.2);
        this.percentText.put_Text(App.Strings["common.done", "done"].ToLower());
      }
    }

    private async void ContentControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.Upload == null)
        return;
      await App.GlobalObjects.UploadsManager.CancelUpload(this.Upload);
      this.percentText.put_Text(App.Strings["common.canceled", "canceled"].ToLower());
      ((UIElement) this.cancelButton).put_IsHitTestVisible(false);
      Ani.Begin((DependencyObject) this.cancelButton, "Opacity", 0.5, 0.2);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///UploadProgressPanel.xaml"), (ComponentResourceLocation) 0);
      this.progressScale = (ScaleTransform) ((FrameworkElement) this).FindName("progressScale");
      this.cancelButton = (ContentControl) ((FrameworkElement) this).FindName("cancelButton");
      this.percentText = (TextBlock) ((FrameworkElement) this).FindName("percentText");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.ContentControl_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
