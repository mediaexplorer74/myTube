// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.CameraCaptureControl
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.IO.Extensions;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "PART_CaptureElement", Type = typeof (CaptureElement))]
  [TemplatePart(Name = "PART_RecordingAnimation", Type = typeof (Storyboard))]
  [TemplatePart(Name = "PART_CountdownControl", Type = typeof (CountdownControl))]
  [TemplatePart(Name = "PART_WebCamSelectorPopup", Type = typeof (Popup))]
  [TemplatePart(Name = "PART_WebCamSelector", Type = typeof (Selector))]
  [TemplatePart(Name = "PART_RecordingIndicator", Type = typeof (Panel))]
  public class CameraCaptureControl : Control
  {
    private const string CaptureElementName = "PART_CaptureElement";
    private const string WebCamSelectorPopupName = "PART_WebCamSelectorPopup";
    private const string WebCamSelectorName = "PART_WebCamSelector";
    private const string RecordingIndicatorName = "PART_RecordingIndicator";
    private const string RecordingAnimationName = "PART_RecordingAnimation";
    private const string CountdownControlName = "PART_CountdownControl";
    private const string FlashAnimationName = "PART_FlashAnimation";
    private CaptureElement _captureElement;
    private Popup _webCamSelectorPopup;
    private Selector _webCamSelector;
    private Panel _recordingIndicator;
    private Storyboard _recordingAnimation;
    private CountdownControl _countdownControl;
    private Storyboard _flashAnimation;
    private CameraCaptureControl.CameraCaptureControlStates _internalState;
    private DeviceInformation[] _audioCaptureDevices;
    private DeviceInformation[] _videoCaptureDevices;
    private int _currentAudioCaptureDeviceIndex = -1;
    private int _currentVideoCaptureDeviceIndex = -1;
    private DeviceInformation _preferredVideoCaptureDevice;
    private MediaCapture _mediaCapture;
    private TaskCompletionSource<bool> _recordingTaskSource;
    public static readonly DependencyProperty ShowOnLoadProperty = DependencyProperty.Register(nameof (ShowOnLoad), (Type) typeof (bool), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) true));
    public static readonly DependencyProperty PreferredCameraTypeProperty = DependencyProperty.Register(nameof (PreferredCameraType), (Type) typeof (Panel), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) (Panel) 0));
    public static readonly DependencyProperty VideoDeviceProperty = DependencyProperty.Register(nameof (VideoDevice), (Type) typeof (DeviceInformation), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty AudioDeviceProperty = DependencyProperty.Register(nameof (AudioDevice), (Type) typeof (DeviceInformation), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty VideoDeviceIdProperty = DependencyProperty.Register(nameof (VideoDeviceId), (Type) typeof (string), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) null, new PropertyChangedCallback(CameraCaptureControl.OnVideoDeviceIdChanged)));
    public static readonly DependencyProperty AudioDeviceIdProperty = DependencyProperty.Register(nameof (AudioDeviceId), (Type) typeof (string), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) null, new PropertyChangedCallback(CameraCaptureControl.OnAudioDeviceIdChanged)));
    public static readonly DependencyProperty VideoDeviceNameProperty = DependencyProperty.Register(nameof (VideoDeviceName), (Type) typeof (string), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty AudioDeviceNameProperty = DependencyProperty.Register(nameof (AudioDeviceName), (Type) typeof (string), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PickVideoDeviceAutomaticallyProperty = DependencyProperty.Register(nameof (PickVideoDeviceAutomatically), (Type) typeof (bool), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) true));
    public static readonly DependencyProperty PickAudioDeviceAutomaticallyProperty = DependencyProperty.Register(nameof (PickAudioDeviceAutomatically), (Type) typeof (bool), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) true));
    public static readonly DependencyProperty StreamingCaptureModeProperty = DependencyProperty.Register(nameof (StreamingCaptureMode), (Type) typeof (StreamingCaptureMode), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) (StreamingCaptureMode) 2, new PropertyChangedCallback(CameraCaptureControl.OnStreamingCaptureModeChanged)));
    public static readonly DependencyProperty VideoEncodingQualityProperty = DependencyProperty.Register(nameof (VideoEncodingQuality), (Type) typeof (VideoEncodingQuality), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) (VideoEncodingQuality) 6));
    private StorageFile _videoFile;
    public static readonly DependencyProperty VideoDeviceEnclosureLocationProperty = DependencyProperty.Register(nameof (VideoDeviceEnclosureLocation), (Type) typeof (EnclosureLocation), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PhotoCaptureCountdownSecondsProperty = DependencyProperty.Register(nameof (PhotoCaptureCountdownSeconds), (Type) typeof (int), (Type) typeof (CameraCaptureControl), new PropertyMetadata((object) 3));
    private TaskCompletionSource<CameraInitializationResult> _initializationTaskSource;

    private MediaCapture MediaCapture
    {
      get => this._mediaCapture;
      set => this._mediaCapture = value;
    }

    public event CameraCaptureControl.CameraFailedHandler CameraFailed;

    private void OnCameraFailed(object sender, MediaCaptureFailedEventArgs e)
    {
      if (this.CameraFailed == null)
        return;
      this.CameraFailed(sender, e);
    }

    public bool ShowOnLoad
    {
      get => (bool) ((DependencyObject) this).GetValue(CameraCaptureControl.ShowOnLoadProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.ShowOnLoadProperty, (object) value);
    }

    public Panel PreferredCameraType
    {
      get => (Panel) ((DependencyObject) this).GetValue(CameraCaptureControl.PreferredCameraTypeProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.PreferredCameraTypeProperty, (object) value);
    }

    public DeviceInformation VideoDevice
    {
      get => (DeviceInformation) ((DependencyObject) this).GetValue(CameraCaptureControl.VideoDeviceProperty);
      private set => ((DependencyObject) this).SetValue(CameraCaptureControl.VideoDeviceProperty, (object) value);
    }

    public DeviceInformation AudioDevice
    {
      get => (DeviceInformation) ((DependencyObject) this).GetValue(CameraCaptureControl.AudioDeviceProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.AudioDeviceProperty, (object) value);
    }

    public string VideoDeviceId
    {
      get => (string) ((DependencyObject) this).GetValue(CameraCaptureControl.VideoDeviceIdProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.VideoDeviceIdProperty, (object) value);
    }

    private static void OnVideoDeviceIdChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CameraCaptureControl cameraCaptureControl = (CameraCaptureControl) d;
      string oldValue = (string) e.OldValue;
      string videoDeviceId = cameraCaptureControl.VideoDeviceId;
      cameraCaptureControl.OnVideoDeviceIdChanged(oldValue, videoDeviceId);
    }

    protected void OnVideoDeviceIdChanged(string oldVideoDeviceId, string newVideoDeviceId)
    {
    }

    public string AudioDeviceId
    {
      get => (string) ((DependencyObject) this).GetValue(CameraCaptureControl.AudioDeviceIdProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.AudioDeviceIdProperty, (object) value);
    }

    private static void OnAudioDeviceIdChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CameraCaptureControl cameraCaptureControl = (CameraCaptureControl) d;
      string oldValue = (string) e.OldValue;
      string audioDeviceId = cameraCaptureControl.AudioDeviceId;
      cameraCaptureControl.OnAudioDeviceIdChanged(oldValue, audioDeviceId);
    }

    protected virtual void OnAudioDeviceIdChanged(string oldAudioDeviceId, string newAudioDeviceId)
    {
    }

    public string VideoDeviceName
    {
      get => (string) ((DependencyObject) this).GetValue(CameraCaptureControl.VideoDeviceNameProperty);
      private set => ((DependencyObject) this).SetValue(CameraCaptureControl.VideoDeviceNameProperty, (object) value);
    }

    public string AudioDeviceName
    {
      get => (string) ((DependencyObject) this).GetValue(CameraCaptureControl.AudioDeviceNameProperty);
      private set => ((DependencyObject) this).SetValue(CameraCaptureControl.AudioDeviceNameProperty, (object) value);
    }

    public bool PickVideoDeviceAutomatically
    {
      get => (bool) ((DependencyObject) this).GetValue(CameraCaptureControl.PickVideoDeviceAutomaticallyProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.PickVideoDeviceAutomaticallyProperty, (object) value);
    }

    public bool PickAudioDeviceAutomatically
    {
      get => (bool) ((DependencyObject) this).GetValue(CameraCaptureControl.PickAudioDeviceAutomaticallyProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.PickAudioDeviceAutomaticallyProperty, (object) value);
    }

    public StreamingCaptureMode StreamingCaptureMode
    {
      get => (StreamingCaptureMode) ((DependencyObject) this).GetValue(CameraCaptureControl.StreamingCaptureModeProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.StreamingCaptureModeProperty, (object) value);
    }

    private static void OnStreamingCaptureModeChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CameraCaptureControl cameraCaptureControl = (CameraCaptureControl) d;
      StreamingCaptureMode oldValue = (StreamingCaptureMode) e.OldValue;
      StreamingCaptureMode streamingCaptureMode = cameraCaptureControl.StreamingCaptureMode;
      cameraCaptureControl.OnStreamingCaptureModeChanged(oldValue, streamingCaptureMode);
    }

    protected virtual void OnStreamingCaptureModeChanged(
      StreamingCaptureMode oldStreamingCaptureMode,
      StreamingCaptureMode newStreamingCaptureMode)
    {
    }

    public VideoEncodingQuality VideoEncodingQuality
    {
      get => (VideoEncodingQuality) ((DependencyObject) this).GetValue(CameraCaptureControl.VideoEncodingQualityProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.VideoEncodingQualityProperty, (object) value);
    }

    public EnclosureLocation VideoDeviceEnclosureLocation
    {
      get => (EnclosureLocation) ((DependencyObject) this).GetValue(CameraCaptureControl.VideoDeviceEnclosureLocationProperty);
      private set => ((DependencyObject) this).SetValue(CameraCaptureControl.VideoDeviceEnclosureLocationProperty, (object) value);
    }

    public int PhotoCaptureCountdownSeconds
    {
      get => (int) ((DependencyObject) this).GetValue(CameraCaptureControl.PhotoCaptureCountdownSecondsProperty);
      set => ((DependencyObject) this).SetValue(CameraCaptureControl.PhotoCaptureCountdownSecondsProperty, (object) value);
    }

    public CameraCaptureControl()
    {
      this.put_DefaultStyleKey((object) typeof (CameraCaptureControl));
      CameraCaptureControl cameraCaptureControl1 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) cameraCaptureControl1).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) cameraCaptureControl1).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      CameraCaptureControl cameraCaptureControl2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) cameraCaptureControl2).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) cameraCaptureControl2).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
    }

    private async Task<CameraInitializationResult> InitializeAsync()
    {
      CameraInitializationResult result;
      try
      {
        if (this._initializationTaskSource != null)
        {
          CameraInitializationResult task = await this._initializationTaskSource.Task;
          this._initializationTaskSource = new TaskCompletionSource<CameraInitializationResult>();
        }
        this._internalState = CameraCaptureControl.CameraCaptureControlStates.Initializing;
        this._initializationTaskSource = new TaskCompletionSource<CameraInitializationResult>();
        if (this._currentAudioCaptureDeviceIndex < 0 && (this.StreamingCaptureMode == null || this.StreamingCaptureMode == 1))
        {
          await this.FindAudioCaptureDevicesAsync();
          if (this._audioCaptureDevices.Length == 0)
          {
            if (this.StreamingCaptureMode == 1)
            {
              this._internalState = CameraCaptureControl.CameraCaptureControlStates.Hidden;
              Debugger.Break();
              result = new CameraInitializationResult(false, "No audio devices found.");
              this._initializationTaskSource.SetResult(result);
              return result;
            }
          }
          else if (this.AudioDeviceId != null)
          {
            DeviceInformation deviceInformation = ((IEnumerable<DeviceInformation>) this._audioCaptureDevices).FirstOrDefault<DeviceInformation>((Func<DeviceInformation, bool>) (d => d.Id == this.AudioDeviceId));
            if (deviceInformation != null)
              this._currentAudioCaptureDeviceIndex = Array.IndexOf<DeviceInformation>(this._audioCaptureDevices, deviceInformation);
          }
          else if (this.PickAudioDeviceAutomatically)
          {
            this._currentAudioCaptureDeviceIndex = 0;
          }
          else
          {
            int num = await this.ShowMicrophoneSelector() ? 1 : 0;
          }
        }
        if (this._currentVideoCaptureDeviceIndex < 0 && (this.StreamingCaptureMode == null || this.StreamingCaptureMode == 2))
        {
          await this.FindVideoCaptureDevicesAsync();
          if (this._videoCaptureDevices.Length == 0)
          {
            if (this.StreamingCaptureMode == 2)
            {
              Debugger.Break();
              this._internalState = CameraCaptureControl.CameraCaptureControlStates.Hidden;
              result = new CameraInitializationResult(false, "No video devices found.");
              this._initializationTaskSource.SetResult(result);
              return result;
            }
            result = new CameraInitializationResult(true);
            this._initializationTaskSource.SetResult(result);
            return result;
          }
          if (this._videoCaptureDevices.Length == 1)
          {
            this._currentVideoCaptureDeviceIndex = 0;
            result = await this.StartPreviewAsync();
            this._initializationTaskSource.SetResult(result);
            return result;
          }
          if (this.VideoDeviceId != null)
          {
            DeviceInformation device = ((IEnumerable<DeviceInformation>) this._videoCaptureDevices).FirstOrDefault<DeviceInformation>((Func<DeviceInformation, bool>) (d => d.Id == this.VideoDeviceId));
            if (device != null)
            {
              this._currentVideoCaptureDeviceIndex = Array.IndexOf<DeviceInformation>(this._videoCaptureDevices, device);
              result = await this.StartPreviewAsync();
              this._initializationTaskSource.SetResult(result);
              return result;
            }
          }
          if (this._preferredVideoCaptureDevice != null)
          {
            this._currentVideoCaptureDeviceIndex = Array.IndexOf<DeviceInformation>(this._videoCaptureDevices, this._preferredVideoCaptureDevice);
            result = await this.StartPreviewAsync();
            this._initializationTaskSource.SetResult(result);
            return result;
          }
          if (this.PickVideoDeviceAutomatically)
          {
            this._currentVideoCaptureDeviceIndex = 0;
            result = await this.StartPreviewAsync();
            this._initializationTaskSource.SetResult(result);
            return result;
          }
          if (await this.ShowWebCamSelector())
          {
            result = new CameraInitializationResult(true);
            this._initializationTaskSource.SetResult(result);
            return result;
          }
          result = new CameraInitializationResult(false, "Unable to select video device.");
          this._initializationTaskSource.SetResult(result);
          return result;
        }
        result = await this.StartPreviewAsync();
        this._initializationTaskSource.SetResult(result);
        return result;
      }
      catch (Exception ex)
      {
        result = new CameraInitializationResult(false, "An unkown error has occured.", ex);
        if (this._initializationTaskSource != null)
          this._initializationTaskSource.SetResult(result);
        return result;
      }
    }

    public async Task<CameraInitializationResult> ShowAsync()
    {
      await ((FrameworkElement) this).WaitForLoadedAsync();
      await ((FrameworkElement) this).WaitForNonZeroSizeAsync();
      CameraInitializationResult result = await this.InitializeAsync();
      if (!result.Success)
        return result;
      try
      {
        this._internalState = CameraCaptureControl.CameraCaptureControlStates.Shown;
        if (this._captureElement != null)
        {
          this._captureElement.put_Source(this.MediaCapture);
          await this.MediaCapture.StartPreviewAsync();
        }
        return result;
      }
      catch (Exception ex)
      {
        return new CameraInitializationResult(false, "Camera display failed", ex);
      }
    }

    public async Task HideAsync()
    {
      this._internalState = CameraCaptureControl.CameraCaptureControlStates.Deinitializing;
      if (this.MediaCapture != null)
      {
        try
        {
          await this.MediaCapture.StopPreviewAsync();
        }
        catch
        {
        }
      }
      if (this._captureElement != null)
        this._captureElement.put_Source((MediaCapture) null);
      this._internalState = CameraCaptureControl.CameraCaptureControlStates.Hidden;
    }

    public async Task<StorageFile> CapturePhotoToStorageFileAsync(
      StorageFolder folder = null,
      string fileName = null,
      string defaultExtension = ".jpg")
    {
      if (this._countdownControl != null && this.PhotoCaptureCountdownSeconds > 0)
      {
        ((UIElement) this._countdownControl).FadeInCustom();
        await this._countdownControl.StartCountdownAsync(this.PhotoCaptureCountdownSeconds);
        ((UIElement) this._countdownControl).FadeOutCustom();
      }
      if (this._flashAnimation != null)
        this._flashAnimation.Begin();
      if (folder == null)
        folder = KnownFolders.PicturesLibrary;
      if (fileName == null)
        fileName = await folder.CreateTempFileNameAsync(defaultExtension);
      StorageFile photoFile = await folder.CreateFileAsync(fileName, (CreationCollisionOption) 2);
      ImageEncodingProperties imageEncodingProperties;
      switch (Path.GetExtension(fileName))
      {
        case ".png":
          imageEncodingProperties = ImageEncodingProperties.CreatePng();
          break;
        default:
          imageEncodingProperties = ImageEncodingProperties.CreateJpeg();
          break;
      }
      try
      {
        await this.MediaCapture.CapturePhotoToStorageFileAsync(imageEncodingProperties, (IStorageFile) photoFile);
      }
      catch
      {
        this.OnCameraFailed((object) null, (MediaCaptureFailedEventArgs) null);
        return (StorageFile) null;
      }
      return photoFile;
    }

    public async Task CapturePhotoToStreamAsync(
      IRandomAccessStream stream,
      ImageEncodingProperties imageEncodingProperties)
    {
      await this.MediaCapture.CapturePhotoToStreamAsync(imageEncodingProperties, stream);
    }

    public async Task<StorageFile> StartVideoCaptureAsync(StorageFolder folder = null, string fileName = null)
    {
      if (this._internalState == CameraCaptureControl.CameraCaptureControlStates.Recording)
      {
        int num = await this._recordingTaskSource.Task ? 1 : 0;
        return this._videoFile;
      }
      if (this._internalState != CameraCaptureControl.CameraCaptureControlStates.Shown)
      {
        CameraInitializationResult result = await this.ShowAsync();
        if (!result.Success)
          return (StorageFile) null;
      }
      this._internalState = CameraCaptureControl.CameraCaptureControlStates.Recording;
      this._recordingTaskSource = new TaskCompletionSource<bool>((object) false);
      if (this.MediaCapture == null)
        throw new InvalidOperationException();
      if (folder == null)
        folder = KnownFolders.VideosLibrary;
      if (fileName == null)
        fileName = await folder.CreateTempFileNameAsync(".mp4");
      this._videoFile = await folder.CreateFileAsync(fileName, (CreationCollisionOption) 2);
      if (this._recordingIndicator != null)
        ((UIElement) this._recordingIndicator).put_Visibility((Visibility) 0);
      if (this._recordingAnimation != null)
        this._recordingAnimation.Begin();
      MediaEncodingProfile encodingProfile = Path.GetExtension(fileName).Equals(".wmv", StringComparison.OrdinalIgnoreCase) ? MediaEncodingProfile.CreateWmv(this.VideoEncodingQuality) : MediaEncodingProfile.CreateMp4(this.VideoEncodingQuality);
      await this.MediaCapture.StartRecordToStorageFileAsync(encodingProfile, (IStorageFile) this._videoFile);
      int num1 = await this._recordingTaskSource.Task ? 1 : 0;
      this._recordingTaskSource = (TaskCompletionSource<bool>) null;
      if (this._recordingAnimation != null)
        this._recordingAnimation.Stop();
      if (this._recordingIndicator != null)
        ((UIElement) this._recordingIndicator).put_Visibility((Visibility) 1);
      this._internalState = CameraCaptureControl.CameraCaptureControlStates.Shown;
      return this._videoFile;
    }

    public async Task<StorageFile> StopCapture()
    {
      if (this._internalState != CameraCaptureControl.CameraCaptureControlStates.Recording)
      {
        this._recordingTaskSource.SetResult(true);
        return (StorageFile) null;
      }
      await this.MediaCapture.StopRecordAsync();
      this._recordingTaskSource.SetResult(true);
      return this._videoFile;
    }

    public async Task<CameraInitializationResult> CycleCamerasAsync()
    {
      if (this._videoCaptureDevices.Length <= 1)
        return new CameraInitializationResult(true);
      if (this._internalState == CameraCaptureControl.CameraCaptureControlStates.Shown)
      {
        await this.HideAsync();
        this._currentVideoCaptureDeviceIndex = (this._currentVideoCaptureDeviceIndex + 1) % this._videoCaptureDevices.Length;
        return await this.ShowAsync();
      }
      this._currentVideoCaptureDeviceIndex = (this._currentVideoCaptureDeviceIndex + 1) % this._videoCaptureDevices.Length;
      this.VideoDeviceId = this._videoCaptureDevices[this._currentVideoCaptureDeviceIndex].Id;
      return new CameraInitializationResult(true);
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._captureElement = (CaptureElement) this.GetTemplateChild("PART_CaptureElement");
      if (this.MediaCapture != null)
        this._captureElement.put_Source(this.MediaCapture);
      this._webCamSelectorPopup = this.GetTemplateChild("PART_WebCamSelectorPopup") as Popup;
      this._webCamSelector = this.GetTemplateChild("PART_WebCamSelector") as Selector;
      this._recordingIndicator = this.GetTemplateChild("PART_RecordingIndicator") as Panel;
      this._recordingAnimation = this.GetTemplateChild("PART_RecordingAnimation") as Storyboard;
      this._countdownControl = this.GetTemplateChild("PART_CountdownControl") as CountdownControl;
      this._flashAnimation = this.GetTemplateChild("PART_FlashAnimation") as Storyboard;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
      if (!this.ShowOnLoad)
        return;
      CameraInitializationResult initializationResult = await this.ShowAsync();
    }

    private async void OnUnloaded(object sender, RoutedEventArgs e)
    {
      if (this._internalState == CameraCaptureControl.CameraCaptureControlStates.Initializing && this._initializationTaskSource != null)
      {
        CameraInitializationResult task = await this._initializationTaskSource.Task;
      }
      if (this._internalState != CameraCaptureControl.CameraCaptureControlStates.Shown)
        return;
      this.HideAsync();
    }

    private async Task<bool> ShowMicrophoneSelector()
    {
      if (this._webCamSelector == null)
        return false;
      ((ICollection<object>) ((ItemsControl) this._webCamSelector).Items).Clear();
      foreach (DeviceInformation audioCaptureDevice in this._audioCaptureDevices)
      {
        ItemCollection items = ((ItemsControl) this._webCamSelector).Items;
        string str;
        if (audioCaptureDevice.EnclosureLocation != null && audioCaptureDevice.EnclosureLocation.Panel != null)
          str = string.Format("{0} - {1}", (object) audioCaptureDevice.EnclosureLocation.Panel, (object) audioCaptureDevice.Name);
        else
          str = audioCaptureDevice.Name;
        ((ICollection<object>) items).Add((object) str);
      }
      bool flag;
      int num = flag ? 1 : 0;
      if (this._webCamSelectorPopup != null)
        this._webCamSelectorPopup.put_IsOpen(true);
      SelectionChangedEventArgs changedEventArgs = await this._webCamSelector.WaitForSelectionChangedAsync();
      this._currentAudioCaptureDeviceIndex = this._webCamSelector.SelectedIndex;
      if (this._webCamSelectorPopup != null)
        this._webCamSelectorPopup.put_IsOpen(false);
      CameraInitializationResult initializationResult = await this.StartPreviewAsync();
      return true;
    }

    private async Task<bool> ShowWebCamSelector()
    {
      if (this._webCamSelector == null)
        return false;
      ((ICollection<object>) ((ItemsControl) this._webCamSelector).Items).Clear();
      foreach (DeviceInformation videoCaptureDevice in this._videoCaptureDevices)
      {
        ItemCollection items = ((ItemsControl) this._webCamSelector).Items;
        string str;
        if (videoCaptureDevice.EnclosureLocation != null && videoCaptureDevice.EnclosureLocation.Panel != null)
          str = string.Format("{0} - {1}", (object) videoCaptureDevice.EnclosureLocation.Panel, (object) videoCaptureDevice.Name);
        else
          str = videoCaptureDevice.Name;
        ((ICollection<object>) items).Add((object) str);
      }
      bool flag;
      int num = flag ? 1 : 0;
      if (this._webCamSelectorPopup != null)
        this._webCamSelectorPopup.put_IsOpen(true);
      SelectionChangedEventArgs changedEventArgs = await this._webCamSelector.WaitForSelectionChangedAsync();
      this._currentVideoCaptureDeviceIndex = this._webCamSelector.SelectedIndex;
      if (this._webCamSelectorPopup != null)
        this._webCamSelectorPopup.put_IsOpen(false);
      CameraInitializationResult initializationResult = await this.StartPreviewAsync();
      return true;
    }

    private async Task FindVideoCaptureDevicesAsync()
    {
      if (this._videoCaptureDevices != null)
        return;
      this._videoCaptureDevices = ((IEnumerable<DeviceInformation>) await DeviceInformation.FindAllAsync((DeviceClass) 4)).ToArray<DeviceInformation>();
      if (this.PreferredCameraType == null)
        return;
      foreach (DeviceInformation videoCaptureDevice in this._videoCaptureDevices)
      {
        if (videoCaptureDevice.EnclosureLocation != null && videoCaptureDevice.EnclosureLocation.Panel == this.PreferredCameraType)
        {
          this._preferredVideoCaptureDevice = videoCaptureDevice;
          break;
        }
      }
      bool flag;
      int num = flag ? 1 : 0;
    }

    private async Task FindAudioCaptureDevicesAsync()
    {
      if (this._audioCaptureDevices != null)
        return;
      this._audioCaptureDevices = ((IEnumerable<DeviceInformation>) await DeviceInformation.FindAllAsync((DeviceClass) 1)).ToArray<DeviceInformation>();
    }

    private async Task<CameraInitializationResult> StartPreviewAsync()
    {
      try
      {
        if (this.MediaCapture != null)
          WindowsRuntimeMarshal.RemoveEventHandler<MediaCaptureFailedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this.MediaCapture.remove_Failed), new MediaCaptureFailedEventHandler(this.OnMediaCaptureFailed));
        this.MediaCapture = new MediaCapture();
        MediaCapture mediaCapture = this.MediaCapture;
        WindowsRuntimeMarshal.AddEventHandler<MediaCaptureFailedEventHandler>((Func<MediaCaptureFailedEventHandler, EventRegistrationToken>) new Func<MediaCaptureFailedEventHandler, EventRegistrationToken>(mediaCapture.add_Failed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(mediaCapture.remove_Failed), new MediaCaptureFailedEventHandler(this.OnMediaCaptureFailed));
        if (this._currentVideoCaptureDeviceIndex >= 0)
        {
          this.VideoDevice = this._videoCaptureDevices[this._currentVideoCaptureDeviceIndex];
          this.VideoDeviceId = this.VideoDevice.Id;
          this.VideoDeviceName = this.VideoDevice.Name;
          this.VideoDeviceEnclosureLocation = this.VideoDevice.EnclosureLocation;
        }
        if (this._currentAudioCaptureDeviceIndex >= 0)
        {
          this.AudioDevice = this._audioCaptureDevices[this._currentAudioCaptureDeviceIndex];
          this.AudioDeviceId = this.AudioDevice.Id;
          this.AudioDeviceName = this.AudioDevice.Name;
        }
        if (this.CheckStreamingCaptureModeConfiguration())
          return new CameraInitializationResult(true);
        MediaCaptureInitializationSettings initializationSettings = new MediaCaptureInitializationSettings();
        initializationSettings.put_StreamingCaptureMode(this.StreamingCaptureMode);
        initializationSettings.put_PhotoCaptureSource((PhotoCaptureSource) 0);
        MediaCaptureInitializationSettings settings = initializationSettings;
        if (this.StreamingCaptureMode == null)
        {
          settings.put_VideoDeviceId(this.VideoDeviceId);
          settings.put_AudioDeviceId(this.AudioDeviceId);
        }
        else if (this.StreamingCaptureMode == 2)
          settings.put_VideoDeviceId(this.VideoDeviceId);
        else if (this.StreamingCaptureMode == 1)
          settings.put_AudioDeviceId(this.AudioDeviceId);
        await this.MediaCapture.InitializeAsync(settings);
      }
      catch (Exception ex)
      {
        string error = "An unkown error has occured.";
        if (ex.Message.ToLower().Contains("denied"))
          error = "Camera acccess permission not granted.";
        else if (ex.Message.ToLower().Contains("revoked"))
          error = "Camera acccess permission has been revoked.";
        return new CameraInitializationResult(false, error);
      }
      return new CameraInitializationResult(true);
    }

    private void OnMediaCaptureFailed(
      MediaCapture sender,
      MediaCaptureFailedEventArgs errorEventArgs)
    {
      this.OnCameraFailed((object) sender, errorEventArgs);
    }

    private bool CheckStreamingCaptureModeConfiguration()
    {
      if ((this.StreamingCaptureMode == 2 || this.StreamingCaptureMode == null) && string.IsNullOrEmpty(this.VideoDeviceId))
      {
        if (this.StreamingCaptureMode == 2)
          return true;
        this.StreamingCaptureMode = (StreamingCaptureMode) 1;
      }
      if ((this.StreamingCaptureMode == 1 || this.StreamingCaptureMode == null) && string.IsNullOrEmpty(this.AudioDeviceId))
      {
        if (this.StreamingCaptureMode == 1)
          return true;
        this.StreamingCaptureMode = (StreamingCaptureMode) 2;
      }
      return false;
    }

    private enum CameraCaptureControlStates
    {
      Hidden,
      Initializing,
      Shown,
      Recording,
      Deinitializing,
    }

    public delegate void CameraFailedHandler(object sender, MediaCaptureFailedEventArgs e);
  }
}
