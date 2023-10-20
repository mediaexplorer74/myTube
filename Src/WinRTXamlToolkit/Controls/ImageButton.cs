// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.ImageButton
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "PART_NormalStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_DisabledStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_HoverStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_HoverStateRecycledNormalStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_HoverStateRecycledPressedStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_PressedStateImage", Type = typeof (Image))]
  public class ImageButton : Button
  {
    private const string NormalStateImageName = "PART_NormalStateImage";
    private const string HoverStateImageName = "PART_HoverStateImage";
    private const string HoverStateRecycledNormalStateImageName = "PART_HoverStateRecycledNormalStateImage";
    private const string HoverStateRecycledPressedStateImageName = "PART_HoverStateRecycledPressedStateImage";
    private const string PressedStateImageName = "PART_PressedStateImage";
    private const string DisabledStateImageName = "PART_DisabledStateImage";
    private Image _normalStateImage;
    private Image _hoverStateImage;
    private Image _hoverStateRecycledNormalStateImage;
    private Image _hoverStateRecycledPressedStateImage;
    private Image _pressedStateImage;
    private Image _disabledStateImage;
    private readonly TaskCompletionSource<bool> _waitForApplyTemplateTaskSource = new TaskCompletionSource<bool>((object) false);
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof (Stretch), (Type) typeof (Stretch), (Type) typeof (ImageButton), new PropertyMetadata((object) (Stretch) 0));
    public static readonly DependencyProperty RecyclePressedStateImageForHoverProperty = DependencyProperty.Register(nameof (RecyclePressedStateImageForHover), (Type) typeof (bool), (Type) typeof (ImageButton), new PropertyMetadata((object) false, new PropertyChangedCallback(ImageButton.OnRecyclePressedStateImageForHoverChanged)));
    public static readonly DependencyProperty NormalStateImageSourceProperty = DependencyProperty.Register(nameof (NormalStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.OnNormalStateImageSourceChanged)));
    public static readonly DependencyProperty HoverStateImageSourceProperty = DependencyProperty.Register(nameof (HoverStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.OnHoverStateImageSourceChanged)));
    public static readonly DependencyProperty PressedStateImageSourceProperty = DependencyProperty.Register(nameof (PressedStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.OnPressedStateImageSourceChanged)));
    public static readonly DependencyProperty DisabledStateImageSourceProperty = DependencyProperty.Register(nameof (DisabledStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.OnDisabledStateImageSourceChanged)));
    public static readonly DependencyProperty NormalStateImageUriSourceProperty = DependencyProperty.Register(nameof (NormalStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.OnNormalStateImageUriSourceChanged)));
    public static readonly DependencyProperty HoverStateImageUriSourceProperty = DependencyProperty.Register(nameof (HoverStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.OnHoverStateImageUriSourceChanged)));
    public static readonly DependencyProperty PressedStateImageUriSourceProperty = DependencyProperty.Register(nameof (PressedStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.OnPressedStateImageUriSourceChanged)));
    public static readonly DependencyProperty DisabledStateImageUriSourceProperty = DependencyProperty.Register(nameof (DisabledStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageButton.OnDisabledStateImageUriSourceChanged)));
    public static readonly DependencyProperty GenerateMissingImagesProperty = DependencyProperty.Register(nameof (GenerateMissingImages), (Type) typeof (bool), (Type) typeof (ImageButton), new PropertyMetadata((object) false, new PropertyChangedCallback(ImageButton.OnGenerateMissingImagesChanged)));
    public static readonly DependencyProperty GeneratedHoverStateLightenAmountProperty = DependencyProperty.Register(nameof (GeneratedHoverStateLightenAmount), (Type) typeof (double), (Type) typeof (ImageButton), new PropertyMetadata((object) 0.25, new PropertyChangedCallback(ImageButton.OnGeneratedHoverStateLightenAmountChanged)));
    public static readonly DependencyProperty GeneratedPressedStateLightenAmountProperty = DependencyProperty.Register(nameof (GeneratedPressedStateLightenAmount), (Type) typeof (double), (Type) typeof (ImageButton), new PropertyMetadata((object) 0.5, new PropertyChangedCallback(ImageButton.OnGeneratedPressedStateLightenAmountChanged)));
    public static readonly DependencyProperty GeneratedDisabledStateGrayscaleAmountProperty = DependencyProperty.Register(nameof (GeneratedDisabledStateGrayscaleAmount), (Type) typeof (double), (Type) typeof (ImageButton), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ImageButton.OnGeneratedDisabledStateGrayscaleAmountChanged)));

    public Stretch Stretch
    {
      get => (Stretch) ((DependencyObject) this).GetValue(ImageButton.StretchProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.StretchProperty, (object) value);
    }

    public bool RecyclePressedStateImageForHover
    {
      get => (bool) ((DependencyObject) this).GetValue(ImageButton.RecyclePressedStateImageForHoverProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.RecyclePressedStateImageForHoverProperty, (object) value);
    }

    private static void OnRecyclePressedStateImageForHoverChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      bool oldValue = (bool) e.OldValue;
      bool stateImageForHover = imageButton.RecyclePressedStateImageForHover;
      imageButton.OnRecyclePressedStateImageForHoverChanged(oldValue, stateImageForHover);
    }

    protected virtual void OnRecyclePressedStateImageForHoverChanged(
      bool oldRecyclePressedStateImageForHover,
      bool newRecyclePressedStateImageForHover)
    {
      this.UpdateRecycledHoverStateImages();
    }

    public ImageSource NormalStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageButton.NormalStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.NormalStateImageSourceProperty, (object) value);
    }

    private static void OnNormalStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageButton.NormalStateImageSource;
      imageButton.OnNormalStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnNormalStateImageSourceChanged(
      ImageSource oldNormalStateImageSource,
      ImageSource newNormalStateImageSource)
    {
      if (newNormalStateImageSource == null)
        Debugger.Break();
      this.UpdateNormalStateImage();
      this.UpdateHoverStateImage();
      this.UpdateRecycledHoverStateImages();
      this.UpdatePressedStateImage();
      this.UpdateDisabledStateImage();
    }

    public ImageSource HoverStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageButton.HoverStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.HoverStateImageSourceProperty, (object) value);
    }

    private static void OnHoverStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageButton.HoverStateImageSource;
      imageButton.OnHoverStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnHoverStateImageSourceChanged(
      ImageSource oldHoverStateImageSource,
      ImageSource newHoverStateImageSource)
    {
      this.UpdateHoverStateImage();
    }

    public ImageSource PressedStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageButton.PressedStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.PressedStateImageSourceProperty, (object) value);
    }

    private static void OnPressedStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageButton.PressedStateImageSource;
      imageButton.OnPressedStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnPressedStateImageSourceChanged(
      ImageSource oldPressedStateImageSource,
      ImageSource newPressedStateImageSource)
    {
      this.UpdatePressedStateImage();
      this.UpdateRecycledHoverStateImages();
    }

    public ImageSource DisabledStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageButton.DisabledStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.DisabledStateImageSourceProperty, (object) value);
    }

    private static void OnDisabledStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageButton.DisabledStateImageSource;
      imageButton.OnDisabledStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnDisabledStateImageSourceChanged(
      ImageSource oldDisabledStateImageSource,
      ImageSource newDisabledStateImageSource)
    {
      if (this._disabledStateImage == null)
        return;
      this._disabledStateImage.put_Source(newDisabledStateImageSource);
    }

    public Uri NormalStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageButton.NormalStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.NormalStateImageUriSourceProperty, (object) value);
    }

    private static void OnNormalStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageButton.NormalStateImageUriSource;
      imageButton.OnNormalStateImageUriSourceChanged(oldValue, stateImageUriSource);
    }

    private void OnNormalStateImageUriSourceChanged(
      Uri oldNormalStateImageUriSource,
      Uri newNormalStateImageUriSource)
    {
      if (newNormalStateImageUriSource != (Uri) null)
        this.NormalStateImageSource = (ImageSource) new BitmapImage((Uri) newNormalStateImageUriSource);
      else
        this.NormalStateImageSource = (ImageSource) null;
    }

    public Uri HoverStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageButton.HoverStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.HoverStateImageUriSourceProperty, (object) value);
    }

    private static void OnHoverStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageButton.HoverStateImageUriSource;
      imageButton.OnHoverStateImageUriSourceChanged(oldValue, stateImageUriSource);
    }

    private void OnHoverStateImageUriSourceChanged(
      Uri oldHoverStateImageUriSource,
      Uri newHoverStateImageUriSource)
    {
      if (newHoverStateImageUriSource != (Uri) null)
        this.HoverStateImageSource = (ImageSource) new BitmapImage((Uri) newHoverStateImageUriSource);
      else
        this.HoverStateImageSource = (ImageSource) null;
    }

    public Uri PressedStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageButton.PressedStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.PressedStateImageUriSourceProperty, (object) value);
    }

    private static void OnPressedStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageButton.PressedStateImageUriSource;
      imageButton.OnPressedStateImageUriSourceChanged(oldValue, stateImageUriSource);
    }

    private void OnPressedStateImageUriSourceChanged(
      Uri oldPressedStateImageUriSource,
      Uri newPressedStateImageUriSource)
    {
      if (newPressedStateImageUriSource != (Uri) null)
        this.PressedStateImageSource = (ImageSource) new BitmapImage((Uri) newPressedStateImageUriSource);
      else
        this.PressedStateImageSource = (ImageSource) null;
    }

    public Uri DisabledStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageButton.DisabledStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.DisabledStateImageUriSourceProperty, (object) value);
    }

    private static void OnDisabledStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageButton.DisabledStateImageUriSource;
      imageButton.OnDisabledStateImageUriSourceChanged(oldValue, stateImageUriSource);
    }

    private void OnDisabledStateImageUriSourceChanged(
      Uri oldDisabledStateImageUriSource,
      Uri newDisabledStateImageUriSource)
    {
      if (newDisabledStateImageUriSource != (Uri) null)
        this.DisabledStateImageSource = (ImageSource) new BitmapImage((Uri) newDisabledStateImageUriSource);
      else
        this.DisabledStateImageSource = (ImageSource) null;
    }

    public bool GenerateMissingImages
    {
      get => (bool) ((DependencyObject) this).GetValue(ImageButton.GenerateMissingImagesProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.GenerateMissingImagesProperty, (object) value);
    }

    private static void OnGenerateMissingImagesChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      bool oldValue = (bool) e.OldValue;
      bool generateMissingImages = imageButton.GenerateMissingImages;
      imageButton.OnGenerateMissingImagesChanged(oldValue, generateMissingImages);
    }

    protected virtual void OnGenerateMissingImagesChanged(
      bool oldGenerateMissingImages,
      bool newGenerateMissingImages)
    {
      this.UpdateHoverStateImage();
      this.UpdatePressedStateImage();
      this.UpdateDisabledStateImage();
    }

    public double GeneratedHoverStateLightenAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageButton.GeneratedHoverStateLightenAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.GeneratedHoverStateLightenAmountProperty, (object) value);
    }

    private static void OnGeneratedHoverStateLightenAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      double oldValue = (double) e.OldValue;
      double stateLightenAmount = imageButton.GeneratedHoverStateLightenAmount;
      imageButton.OnGeneratedHoverStateLightenAmountChanged(oldValue, stateLightenAmount);
    }

    protected virtual void OnGeneratedHoverStateLightenAmountChanged(
      double oldGeneratedHoverStateLightenAmount,
      double newGeneratedHoverStateLightenAmount)
    {
      this.UpdateHoverStateImage();
    }

    public double GeneratedPressedStateLightenAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageButton.GeneratedPressedStateLightenAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.GeneratedPressedStateLightenAmountProperty, (object) value);
    }

    private static void OnGeneratedPressedStateLightenAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      double oldValue = (double) e.OldValue;
      double stateLightenAmount = imageButton.GeneratedPressedStateLightenAmount;
      imageButton.OnGeneratedPressedStateLightenAmountChanged(oldValue, stateLightenAmount);
    }

    protected virtual void OnGeneratedPressedStateLightenAmountChanged(
      double oldGeneratedPressedStateLightenAmount,
      double newGeneratedPressedStateLightenAmount)
    {
      this.UpdatePressedStateImage();
    }

    public double GeneratedDisabledStateGrayscaleAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageButton.GeneratedDisabledStateGrayscaleAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageButton.GeneratedDisabledStateGrayscaleAmountProperty, (object) value);
    }

    private static void OnGeneratedDisabledStateGrayscaleAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageButton imageButton = (ImageButton) d;
      double oldValue = (double) e.OldValue;
      double stateGrayscaleAmount = imageButton.GeneratedDisabledStateGrayscaleAmount;
      imageButton.OnGeneratedDisabledStateGrayscaleAmountChanged(oldValue, stateGrayscaleAmount);
    }

    protected virtual void OnGeneratedDisabledStateGrayscaleAmountChanged(
      double oldGeneratedDisabledStateGrayscaleAmount,
      double newGeneratedDisabledStateGrayscaleAmount)
    {
      this.UpdateDisabledStateImage();
    }

    private async void GenerateHoverStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      WriteableBitmap wb = new WriteableBitmap(1, 1);
      WriteableBitmap writeableBitmap = await wb.FromBitmapImage((BitmapImage) this.NormalStateImageSource);
      await wb.WaitForLoadedAsync();
      wb.Lighten(this.GeneratedHoverStateLightenAmount);
      this._hoverStateImage.put_Source((ImageSource) wb);
    }

    private async void GeneratePressedStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      WriteableBitmap wb = new WriteableBitmap(1, 1);
      WriteableBitmap writeableBitmap = await wb.FromBitmapImage((BitmapImage) this.NormalStateImageSource);
      await wb.WaitForLoadedAsync();
      wb.Lighten(this.GeneratedPressedStateLightenAmount);
      this._pressedStateImage.put_Source((ImageSource) wb);
    }

    private async void GenerateDisabledStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      WriteableBitmap wb = new WriteableBitmap(1, 1);
      WriteableBitmap writeableBitmap = await wb.FromBitmapImage((BitmapImage) this.NormalStateImageSource);
      await wb.WaitForLoadedAsync();
      wb.Grayscale(this.GeneratedDisabledStateGrayscaleAmount);
      this._disabledStateImage.put_Source((ImageSource) wb);
    }

    public ImageButton() => ((Control) this).put_DefaultStyleKey((object) typeof (ImageButton));

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._normalStateImage = (Image) ((Control) this).GetTemplateChild("PART_NormalStateImage");
      this._hoverStateImage = (Image) ((Control) this).GetTemplateChild("PART_HoverStateImage");
      this._hoverStateRecycledNormalStateImage = (Image) ((Control) this).GetTemplateChild("PART_HoverStateRecycledNormalStateImage");
      this._hoverStateRecycledPressedStateImage = (Image) ((Control) this).GetTemplateChild("PART_HoverStateRecycledPressedStateImage");
      this._pressedStateImage = (Image) ((Control) this).GetTemplateChild("PART_PressedStateImage");
      this._disabledStateImage = (Image) ((Control) this).GetTemplateChild("PART_DisabledStateImage");
      this._waitForApplyTemplateTaskSource.SetResult(true);
    }

    private async void UpdateNormalStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      int num = await this._waitForApplyTemplateTaskSource.Task ? 1 : 0;
      this._normalStateImage.put_Source(this.NormalStateImageSource);
    }

    private async void UpdateHoverStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      int num = await this._waitForApplyTemplateTaskSource.Task ? 1 : 0;
      if (this.HoverStateImageSource != null)
        this._hoverStateImage.put_Source(this.HoverStateImageSource);
      else if (this.GenerateMissingImages && this.NormalStateImageSource != null)
        this.GenerateHoverStateImage();
      if (this._hoverStateImage.Source != null)
        return;
      this._hoverStateImage.put_Source(this.NormalStateImageSource);
    }

    private async void UpdateRecycledHoverStateImages()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      int num = await this._waitForApplyTemplateTaskSource.Task ? 1 : 0;
      if (this.RecyclePressedStateImageForHover && this.NormalStateImageSource != null)
        this._hoverStateRecycledNormalStateImage.put_Source(this.NormalStateImageSource);
      else
        this._hoverStateRecycledNormalStateImage.put_Source((ImageSource) null);
      if (this.RecyclePressedStateImageForHover && this.PressedStateImageSource != null)
        this._hoverStateRecycledPressedStateImage.put_Source(this.PressedStateImageSource);
      else
        this._hoverStateRecycledPressedStateImage.put_Source((ImageSource) null);
    }

    private async void UpdatePressedStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      int num = await this._waitForApplyTemplateTaskSource.Task ? 1 : 0;
      if (this.PressedStateImageSource != null)
      {
        this._pressedStateImage.put_Source(this.PressedStateImageSource);
      }
      else
      {
        if (!this.GenerateMissingImages || this.NormalStateImageSource == null)
          return;
        this.GeneratePressedStateImage();
      }
    }

    private async void UpdateDisabledStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      int num = await this._waitForApplyTemplateTaskSource.Task ? 1 : 0;
      if (this.DisabledStateImageSource != null)
      {
        this._disabledStateImage.put_Source(this.DisabledStateImageSource);
      }
      else
      {
        if (!this.GenerateMissingImages || this.NormalStateImageSource == null)
          return;
        this.GenerateDisabledStateImage();
      }
    }
  }
}
