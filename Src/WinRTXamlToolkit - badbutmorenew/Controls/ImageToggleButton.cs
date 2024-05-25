// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.ImageToggleButton
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "PART_CheckedStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_NormalStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_HoverStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_HoverStateRecycledNormalStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_HoverStateRecycledPressedStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_PressedStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_DisabledStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_CheckedHoverStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_CheckedHoverStateRecycledCheckedStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_CheckedHoverStateRecycledCheckedPressedStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_CheckedPressedStateImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_CheckedDisabledStateImage", Type = typeof (Image))]
  public class ImageToggleButton : ToggleButton
  {
    private const string NormalStateImageName = "PART_NormalStateImage";
    private const string HoverStateImageName = "PART_HoverStateImage";
    private const string HoverStateRecycledNormalStateImageName = "PART_HoverStateRecycledNormalStateImage";
    private const string HoverStateRecycledPressedStateImageName = "PART_HoverStateRecycledPressedStateImage";
    private const string PressedStateImageName = "PART_PressedStateImage";
    private const string DisabledStateImageName = "PART_DisabledStateImage";
    private const string CheckedStateImageName = "PART_CheckedStateImage";
    private const string CheckedHoverStateImageName = "PART_CheckedHoverStateImage";
    private const string CheckedHoverStateRecycledCheckedStateImageName = "PART_CheckedHoverStateRecycledCheckedStateImage";
    private const string CheckedHoverStateRecycledCheckedPressedStateImageName = "PART_CheckedHoverStateRecycledCheckedPressedStateImage";
    private const string CheckedPressedStateImageName = "PART_CheckedPressedStateImage";
    private const string CheckedDisabledStateImageName = "PART_CheckedDisabledStateImage";
    private Image _normalStateImage;
    private Image _hoverStateImage;
    private Image _hoverStateRecycledCheckedStateImage;
    private Image _hoverStateRecycledCheckedPressedStateImage;
    private Image _pressedStateImage;
    private Image _disabledStateImage;
    private Image _checkedStateImage;
    private Image _checkedHoverStateImage;
    private Image _checkedHoverStateRecycledCheckedStateImage;
    private Image _checkedHoverStateRecycledCheckedPressedStateImage;
    private Image _checkedPressedStateImage;
    private Image _checkedDisabledStateImage;
    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof (Stretch), (Type) typeof (Stretch), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) (Stretch) 0));
    public static readonly DependencyProperty RecyclePressedStateImageForHoverProperty = DependencyProperty.Register(nameof (RecyclePressedStateImageForHover), (Type) typeof (bool), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) false, new PropertyChangedCallback(ImageToggleButton.OnRecyclePressedStateImageForHoverChanged)));
    public static readonly DependencyProperty RecycleUncheckedStateImagesForCheckedStatesProperty = DependencyProperty.Register(nameof (RecycleUncheckedStateImagesForCheckedStates), (Type) typeof (bool), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) true, new PropertyChangedCallback(ImageToggleButton.OnRecycleUncheckedStateImagesForCheckedStatesChanged)));
    public static readonly DependencyProperty NormalStateImageSourceProperty = DependencyProperty.Register(nameof (NormalStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnNormalStateImageSourceChanged)));
    public static readonly DependencyProperty HoverStateImageSourceProperty = DependencyProperty.Register(nameof (HoverStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnHoverStateImageSourceChanged)));
    public static readonly DependencyProperty PressedStateImageSourceProperty = DependencyProperty.Register(nameof (PressedStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnPressedStateImageSourceChanged)));
    public static readonly DependencyProperty DisabledStateImageSourceProperty = DependencyProperty.Register(nameof (DisabledStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnDisabledStateImageSourceChanged)));
    public static readonly DependencyProperty CheckedStateImageSourceProperty = DependencyProperty.Register(nameof (CheckedStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnCheckedStateImageSourceChanged)));
    public static readonly DependencyProperty CheckedHoverStateImageSourceProperty = DependencyProperty.Register(nameof (CheckedHoverStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnCheckedHoverStateImageSourceChanged)));
    public static readonly DependencyProperty CheckedPressedStateImageSourceProperty = DependencyProperty.Register(nameof (CheckedPressedStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnCheckedPressedStateImageSourceChanged)));
    public static readonly DependencyProperty CheckedDisabledStateImageSourceProperty = DependencyProperty.Register(nameof (CheckedDisabledStateImageSource), (Type) typeof (ImageSource), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnCheckedDisabledStateImageSourceChanged)));
    public static readonly DependencyProperty NormalStateImageUriSourceProperty = DependencyProperty.Register(nameof (NormalStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnNormalStateImageUriSourceChanged)));
    public static readonly DependencyProperty HoverStateImageUriSourceProperty = DependencyProperty.Register(nameof (HoverStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnHoverStateImageUriSourceChanged)));
    public static readonly DependencyProperty PressedStateImageUriSourceProperty = DependencyProperty.Register(nameof (PressedStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnPressedStateImageUriSourceChanged)));
    public static readonly DependencyProperty DisabledStateImageUriSourceProperty = DependencyProperty.Register(nameof (DisabledStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnDisabledStateImageUriSourceChanged)));
    public static readonly DependencyProperty CheckedStateImageUriSourceProperty = DependencyProperty.Register(nameof (CheckedStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnCheckedStateImageUriSourceChanged)));
    public static readonly DependencyProperty CheckedHoverStateImageUriSourceProperty = DependencyProperty.Register(nameof (CheckedHoverStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnCheckedHoverStateImageUriSourceChanged)));
    public static readonly DependencyProperty CheckedPressedStateImageUriSourceProperty = DependencyProperty.Register(nameof (CheckedPressedStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnCheckedPressedStateImageUriSourceChanged)));
    public static readonly DependencyProperty CheckedDisabledStateImageUriSourceProperty = DependencyProperty.Register(nameof (CheckedDisabledStateImageUriSource), (Type) typeof (Uri), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageToggleButton.OnCheckedDisabledStateImageUriSourceChanged)));
    public static readonly DependencyProperty GenerateMissingImagesProperty = DependencyProperty.Register(nameof (GenerateMissingImages), (Type) typeof (bool), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) false, new PropertyChangedCallback(ImageToggleButton.OnGenerateMissingImagesChanged)));
    public static readonly DependencyProperty GeneratedHoverStateLightenAmountProperty = DependencyProperty.Register(nameof (GeneratedHoverStateLightenAmount), (Type) typeof (double), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) 0.25, new PropertyChangedCallback(ImageToggleButton.OnGeneratedHoverStateLightenAmountChanged)));
    public static readonly DependencyProperty GeneratedPressedStateLightenAmountProperty = DependencyProperty.Register(nameof (GeneratedPressedStateLightenAmount), (Type) typeof (double), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) 0.5, new PropertyChangedCallback(ImageToggleButton.OnGeneratedPressedStateLightenAmountChanged)));
    public static readonly DependencyProperty GeneratedDisabledStateGrayscaleAmountProperty = DependencyProperty.Register(nameof (GeneratedDisabledStateGrayscaleAmount), (Type) typeof (double), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ImageToggleButton.OnGeneratedDisabledStateGrayscaleAmountChanged)));
    public static readonly DependencyProperty GeneratedCheckedStateLightenAmountProperty = DependencyProperty.Register(nameof (GeneratedCheckedStateLightenAmount), (Type) typeof (double), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) 0.5, new PropertyChangedCallback(ImageToggleButton.OnGeneratedCheckedStateLightenAmountChanged)));
    public static readonly DependencyProperty GeneratedCheckedHoverStateLightenAmountProperty = DependencyProperty.Register(nameof (GeneratedCheckedHoverStateLightenAmount), (Type) typeof (double), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) 0.65, new PropertyChangedCallback(ImageToggleButton.OnGeneratedCheckedHoverStateLightenAmountChanged)));
    public static readonly DependencyProperty GeneratedCheckedPressedStateLightenAmountProperty = DependencyProperty.Register(nameof (GeneratedCheckedPressedStateLightenAmount), (Type) typeof (double), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) 0.8, new PropertyChangedCallback(ImageToggleButton.OnGeneratedCheckedPressedStateLightenAmountChanged)));
    public static readonly DependencyProperty GeneratedCheckedDisabledStateGrayscaleAmountProperty = DependencyProperty.Register(nameof (GeneratedCheckedDisabledStateGrayscaleAmount), (Type) typeof (double), (Type) typeof (ImageToggleButton), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ImageToggleButton.OnGeneratedCheckedDisabledStateGrayscaleAmountChanged)));

    public Stretch Stretch
    {
      get => (Stretch) ((DependencyObject) this).GetValue(ImageToggleButton.StretchProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.StretchProperty, (object) value);
    }

    public bool RecyclePressedStateImageForHover
    {
      get => (bool) ((DependencyObject) this).GetValue(ImageToggleButton.RecyclePressedStateImageForHoverProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.RecyclePressedStateImageForHoverProperty, (object) value);
    }

    private static void OnRecyclePressedStateImageForHoverChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      bool oldValue = (bool) e.OldValue;
      bool stateImageForHover = imageToggleButton.RecyclePressedStateImageForHover;
      imageToggleButton.OnRecyclePressedStateImageForHoverChanged(oldValue, stateImageForHover);
    }

    protected virtual void OnRecyclePressedStateImageForHoverChanged(
      bool oldRecyclePressedStateImageForHover,
      bool newRecyclePressedStateImageForHover)
    {
      this.UpdateRecycledHoverStateImages();
    }

    public bool RecycleUncheckedStateImagesForCheckedStates
    {
      get => (bool) ((DependencyObject) this).GetValue(ImageToggleButton.RecycleUncheckedStateImagesForCheckedStatesProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.RecycleUncheckedStateImagesForCheckedStatesProperty, (object) value);
    }

    private static void OnRecycleUncheckedStateImagesForCheckedStatesChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      bool oldValue = (bool) e.OldValue;
      bool forCheckedStates = imageToggleButton.RecycleUncheckedStateImagesForCheckedStates;
      imageToggleButton.OnRecycleUncheckedStateImagesForCheckedStatesChanged(oldValue, forCheckedStates);
    }

    protected virtual void OnRecycleUncheckedStateImagesForCheckedStatesChanged(
      bool oldRecycleUncheckedStateImagesForCheckedStates,
      bool newRecycleUncheckedStateImagesForCheckedStates)
    {
      this.UpdateCheckedStateImage();
      this.UpdateCheckedHoverStateImage();
      this.UpdateCheckedPressedStateImage();
      this.UpdateCheckedDisabledStateImage();
    }

    public ImageSource NormalStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageToggleButton.NormalStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.NormalStateImageSourceProperty, (object) value);
    }

    private static void OnNormalStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageToggleButton.NormalStateImageSource;
      imageToggleButton.OnNormalStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnNormalStateImageSourceChanged(
      ImageSource oldNormalStateImageSource,
      ImageSource newNormalStateImageSource)
    {
      this.UpdateNormalStateImage();
      this.UpdateHoverStateImage();
      this.UpdateRecycledHoverStateImages();
      this.UpdatePressedStateImage();
      this.UpdateDisabledStateImage();
      this.UpdateCheckedStateImage();
      this.UpdateCheckedHoverStateImage();
      this.UpdateRecycledCheckedHoverStateImages();
      this.UpdateCheckedPressedStateImage();
      this.UpdateCheckedDisabledStateImage();
    }

    public ImageSource HoverStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageToggleButton.HoverStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.HoverStateImageSourceProperty, (object) value);
    }

    private static void OnHoverStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageToggleButton.HoverStateImageSource;
      imageToggleButton.OnHoverStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnHoverStateImageSourceChanged(
      ImageSource oldHoverStateImageSource,
      ImageSource newHoverStateImageSource)
    {
      this.UpdateHoverStateImage();
      this.UpdateCheckedHoverStateImage();
    }

    public ImageSource PressedStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageToggleButton.PressedStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.PressedStateImageSourceProperty, (object) value);
    }

    private static void OnPressedStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageToggleButton.PressedStateImageSource;
      imageToggleButton.OnPressedStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnPressedStateImageSourceChanged(
      ImageSource oldPressedStateImageSource,
      ImageSource newPressedStateImageSource)
    {
      this.UpdatePressedStateImage();
      this.UpdateRecycledHoverStateImages();
      this.UpdateCheckedStateImage();
      this.UpdateCheckedHoverStateImage();
      this.UpdateRecycledCheckedHoverStateImages();
      this.UpdateCheckedPressedStateImage();
      this.UpdateCheckedDisabledStateImage();
    }

    public ImageSource DisabledStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageToggleButton.DisabledStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.DisabledStateImageSourceProperty, (object) value);
    }

    private static void OnDisabledStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageToggleButton.DisabledStateImageSource;
      imageToggleButton.OnDisabledStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnDisabledStateImageSourceChanged(
      ImageSource oldDisabledStateImageSource,
      ImageSource newDisabledStateImageSource)
    {
      if (this._disabledStateImage == null)
        return;
      this._disabledStateImage.put_Source(newDisabledStateImageSource);
    }

    public ImageSource CheckedStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageToggleButton.CheckedStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.CheckedStateImageSourceProperty, (object) value);
    }

    private static void OnCheckedStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageToggleButton.CheckedStateImageSource;
      imageToggleButton.OnCheckedStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnCheckedStateImageSourceChanged(
      ImageSource oldCheckedStateImageSource,
      ImageSource newCheckedStateImageSource)
    {
      this.UpdateCheckedStateImage();
      this.UpdateCheckedHoverStateImage();
      this.UpdateRecycledCheckedHoverStateImages();
      this.UpdateCheckedPressedStateImage();
      this.UpdateCheckedDisabledStateImage();
    }

    public ImageSource CheckedHoverStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageToggleButton.CheckedHoverStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.CheckedHoverStateImageSourceProperty, (object) value);
    }

    private static void OnCheckedHoverStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageToggleButton.CheckedHoverStateImageSource;
      imageToggleButton.OnCheckedHoverStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnCheckedHoverStateImageSourceChanged(
      ImageSource oldCheckedHoverStateImageSource,
      ImageSource newCheckedHoverStateImageSource)
    {
      this.UpdateCheckedHoverStateImage();
    }

    public ImageSource CheckedPressedStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageToggleButton.CheckedPressedStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.CheckedPressedStateImageSourceProperty, (object) value);
    }

    private static void OnCheckedPressedStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageToggleButton.CheckedPressedStateImageSource;
      imageToggleButton.OnCheckedPressedStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnCheckedPressedStateImageSourceChanged(
      ImageSource oldCheckedPressedStateImageSource,
      ImageSource newCheckedPressedStateImageSource)
    {
      this.UpdateCheckedPressedStateImage();
      this.UpdateRecycledCheckedHoverStateImages();
    }

    public ImageSource CheckedDisabledStateImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(ImageToggleButton.CheckedDisabledStateImageSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.CheckedDisabledStateImageSourceProperty, (object) value);
    }

    private static void OnCheckedDisabledStateImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource stateImageSource = imageToggleButton.CheckedDisabledStateImageSource;
      imageToggleButton.OnCheckedDisabledStateImageSourceChanged(oldValue, stateImageSource);
    }

    protected virtual void OnCheckedDisabledStateImageSourceChanged(
      ImageSource oldCheckedDisabledStateImageSource,
      ImageSource newCheckedDisabledStateImageSource)
    {
      this.UpdateCheckedDisabledStateImage();
    }

    public Uri NormalStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageToggleButton.NormalStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.NormalStateImageUriSourceProperty, (object) value);
    }

    private static void OnNormalStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageToggleButton.NormalStateImageUriSource;
      imageToggleButton.OnNormalStateImageUriSourceChanged(oldValue, stateImageUriSource);
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
      get => (Uri) ((DependencyObject) this).GetValue(ImageToggleButton.HoverStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.HoverStateImageUriSourceProperty, (object) value);
    }

    private static void OnHoverStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageToggleButton.HoverStateImageUriSource;
      imageToggleButton.OnHoverStateImageUriSourceChanged(oldValue, stateImageUriSource);
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
      get => (Uri) ((DependencyObject) this).GetValue(ImageToggleButton.PressedStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.PressedStateImageUriSourceProperty, (object) value);
    }

    private static void OnPressedStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageToggleButton.PressedStateImageUriSource;
      imageToggleButton.OnPressedStateImageUriSourceChanged(oldValue, stateImageUriSource);
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
      get => (Uri) ((DependencyObject) this).GetValue(ImageToggleButton.DisabledStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.DisabledStateImageUriSourceProperty, (object) value);
    }

    private static void OnDisabledStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageToggleButton.DisabledStateImageUriSource;
      imageToggleButton.OnDisabledStateImageUriSourceChanged(oldValue, stateImageUriSource);
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

    public Uri CheckedStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageToggleButton.CheckedStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.CheckedStateImageUriSourceProperty, (object) value);
    }

    private static void OnCheckedStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageToggleButton.CheckedStateImageUriSource;
      imageToggleButton.OnCheckedStateImageUriSourceChanged(oldValue, stateImageUriSource);
    }

    private void OnCheckedStateImageUriSourceChanged(
      Uri oldCheckedStateImageUriSource,
      Uri newCheckedStateImageUriSource)
    {
      if (newCheckedStateImageUriSource != (Uri) null)
        this.CheckedStateImageSource = (ImageSource) new BitmapImage((Uri) newCheckedStateImageUriSource);
      else
        this.CheckedStateImageSource = (ImageSource) null;
    }

    public Uri CheckedHoverStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageToggleButton.CheckedHoverStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.CheckedHoverStateImageUriSourceProperty, (object) value);
    }

    private static void OnCheckedHoverStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageToggleButton.CheckedHoverStateImageUriSource;
      imageToggleButton.OnCheckedHoverStateImageUriSourceChanged(oldValue, stateImageUriSource);
    }

    private void OnCheckedHoverStateImageUriSourceChanged(
      Uri oldCheckedHoverStateImageUriSource,
      Uri newCheckedHoverStateImageUriSource)
    {
      if (newCheckedHoverStateImageUriSource != (Uri) null)
        this.CheckedHoverStateImageSource = (ImageSource) new BitmapImage((Uri) newCheckedHoverStateImageUriSource);
      else
        this.CheckedHoverStateImageSource = (ImageSource) null;
    }

    public Uri CheckedPressedStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageToggleButton.CheckedPressedStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.CheckedPressedStateImageUriSourceProperty, (object) value);
    }

    private static void OnCheckedPressedStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageToggleButton.CheckedPressedStateImageUriSource;
      imageToggleButton.OnCheckedPressedStateImageUriSourceChanged(oldValue, stateImageUriSource);
    }

    private void OnCheckedPressedStateImageUriSourceChanged(
      Uri oldCheckedPressedStateImageUriSource,
      Uri newCheckedPressedStateImageUriSource)
    {
      if (newCheckedPressedStateImageUriSource != (Uri) null)
        this.CheckedPressedStateImageSource = (ImageSource) new BitmapImage((Uri) newCheckedPressedStateImageUriSource);
      else
        this.CheckedPressedStateImageSource = (ImageSource) null;
    }

    public Uri CheckedDisabledStateImageUriSource
    {
      get => (Uri) ((DependencyObject) this).GetValue(ImageToggleButton.CheckedDisabledStateImageUriSourceProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.CheckedDisabledStateImageUriSourceProperty, (object) value);
    }

    private static void OnCheckedDisabledStateImageUriSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      Uri oldValue = (Uri) e.OldValue;
      Uri stateImageUriSource = imageToggleButton.CheckedDisabledStateImageUriSource;
      imageToggleButton.OnCheckedDisabledStateImageUriSourceChanged(oldValue, stateImageUriSource);
    }

    private void OnCheckedDisabledStateImageUriSourceChanged(
      Uri oldCheckedDisabledStateImageUriSource,
      Uri newCheckedDisabledStateImageUriSource)
    {
      if (newCheckedDisabledStateImageUriSource != (Uri) null)
        this.CheckedDisabledStateImageSource = (ImageSource) new BitmapImage((Uri) newCheckedDisabledStateImageUriSource);
      else
        this.CheckedDisabledStateImageSource = (ImageSource) null;
    }

    public bool GenerateMissingImages
    {
      get => (bool) ((DependencyObject) this).GetValue(ImageToggleButton.GenerateMissingImagesProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.GenerateMissingImagesProperty, (object) value);
    }

    private static void OnGenerateMissingImagesChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      bool oldValue = (bool) e.OldValue;
      bool generateMissingImages = imageToggleButton.GenerateMissingImages;
      imageToggleButton.OnGenerateMissingImagesChanged(oldValue, generateMissingImages);
    }

    protected virtual void OnGenerateMissingImagesChanged(
      bool oldGenerateMissingImages,
      bool newGenerateMissingImages)
    {
      this.UpdateHoverStateImage();
      this.UpdatePressedStateImage();
      this.UpdateDisabledStateImage();
      this.UpdateCheckedStateImage();
      this.UpdateCheckedHoverStateImage();
      this.UpdateCheckedPressedStateImage();
      this.UpdateCheckedDisabledStateImage();
    }

    public double GeneratedHoverStateLightenAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageToggleButton.GeneratedHoverStateLightenAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.GeneratedHoverStateLightenAmountProperty, (object) value);
    }

    private static void OnGeneratedHoverStateLightenAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      double oldValue = (double) e.OldValue;
      double stateLightenAmount = imageToggleButton.GeneratedHoverStateLightenAmount;
      imageToggleButton.OnGeneratedHoverStateLightenAmountChanged(oldValue, stateLightenAmount);
    }

    protected virtual void OnGeneratedHoverStateLightenAmountChanged(
      double oldGeneratedHoverStateLightenAmount,
      double newGeneratedHoverStateLightenAmount)
    {
      this.UpdateHoverStateImage();
    }

    public double GeneratedPressedStateLightenAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageToggleButton.GeneratedPressedStateLightenAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.GeneratedPressedStateLightenAmountProperty, (object) value);
    }

    private static void OnGeneratedPressedStateLightenAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      double oldValue = (double) e.OldValue;
      double stateLightenAmount = imageToggleButton.GeneratedPressedStateLightenAmount;
      imageToggleButton.OnGeneratedPressedStateLightenAmountChanged(oldValue, stateLightenAmount);
    }

    protected virtual void OnGeneratedPressedStateLightenAmountChanged(
      double oldGeneratedPressedStateLightenAmount,
      double newGeneratedPressedStateLightenAmount)
    {
      this.UpdatePressedStateImage();
    }

    public double GeneratedDisabledStateGrayscaleAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageToggleButton.GeneratedDisabledStateGrayscaleAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.GeneratedDisabledStateGrayscaleAmountProperty, (object) value);
    }

    private static void OnGeneratedDisabledStateGrayscaleAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      double oldValue = (double) e.OldValue;
      double stateGrayscaleAmount = imageToggleButton.GeneratedDisabledStateGrayscaleAmount;
      imageToggleButton.OnGeneratedDisabledStateGrayscaleAmountChanged(oldValue, stateGrayscaleAmount);
    }

    protected virtual void OnGeneratedDisabledStateGrayscaleAmountChanged(
      double oldGeneratedDisabledStateGrayscaleAmount,
      double newGeneratedDisabledStateGrayscaleAmount)
    {
      this.UpdateDisabledStateImage();
    }

    public double GeneratedCheckedStateLightenAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageToggleButton.GeneratedCheckedStateLightenAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.GeneratedCheckedStateLightenAmountProperty, (object) value);
    }

    private static void OnGeneratedCheckedStateLightenAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      double oldValue = (double) e.OldValue;
      double stateLightenAmount = imageToggleButton.GeneratedCheckedStateLightenAmount;
      imageToggleButton.OnGeneratedCheckedStateLightenAmountChanged(oldValue, stateLightenAmount);
    }

    protected virtual void OnGeneratedCheckedStateLightenAmountChanged(
      double oldGeneratedCheckedStateLightenAmount,
      double newGeneratedCheckedStateLightenAmount)
    {
      this.UpdateCheckedStateImage();
    }

    public double GeneratedCheckedHoverStateLightenAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageToggleButton.GeneratedCheckedHoverStateLightenAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.GeneratedCheckedHoverStateLightenAmountProperty, (object) value);
    }

    private static void OnGeneratedCheckedHoverStateLightenAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      double oldValue = (double) e.OldValue;
      double stateLightenAmount = imageToggleButton.GeneratedCheckedHoverStateLightenAmount;
      imageToggleButton.OnGeneratedCheckedHoverStateLightenAmountChanged(oldValue, stateLightenAmount);
    }

    protected virtual void OnGeneratedCheckedHoverStateLightenAmountChanged(
      double oldGeneratedCheckedHoverStateLightenAmount,
      double newGeneratedCheckedHoverStateLightenAmount)
    {
      this.UpdateCheckedHoverStateImage();
    }

    public double GeneratedCheckedPressedStateLightenAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageToggleButton.GeneratedCheckedPressedStateLightenAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.GeneratedCheckedPressedStateLightenAmountProperty, (object) value);
    }

    private static void OnGeneratedCheckedPressedStateLightenAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      double oldValue = (double) e.OldValue;
      double stateLightenAmount = imageToggleButton.GeneratedCheckedPressedStateLightenAmount;
      imageToggleButton.OnGeneratedCheckedPressedStateLightenAmountChanged(oldValue, stateLightenAmount);
    }

    protected virtual void OnGeneratedCheckedPressedStateLightenAmountChanged(
      double oldGeneratedCheckedPressedStateLightenAmount,
      double newGeneratedCheckedPressedStateLightenAmount)
    {
      this.UpdateCheckedPressedStateImage();
    }

    public double GeneratedCheckedDisabledStateGrayscaleAmount
    {
      get => (double) ((DependencyObject) this).GetValue(ImageToggleButton.GeneratedCheckedDisabledStateGrayscaleAmountProperty);
      set => ((DependencyObject) this).SetValue(ImageToggleButton.GeneratedCheckedDisabledStateGrayscaleAmountProperty, (object) value);
    }

    private static void OnGeneratedCheckedDisabledStateGrayscaleAmountChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ImageToggleButton imageToggleButton = (ImageToggleButton) d;
      double oldValue = (double) e.OldValue;
      double stateGrayscaleAmount = imageToggleButton.GeneratedCheckedDisabledStateGrayscaleAmount;
      imageToggleButton.OnGeneratedCheckedDisabledStateGrayscaleAmountChanged(oldValue, stateGrayscaleAmount);
    }

    protected virtual void OnGeneratedCheckedDisabledStateGrayscaleAmountChanged(
      double oldGeneratedCheckedDisabledStateGrayscaleAmount,
      double newGeneratedCheckedDisabledStateGrayscaleAmount)
    {
      this.UpdateCheckedDisabledStateImage();
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

    private async void GenerateCheckedStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      WriteableBitmap wb = new WriteableBitmap(1, 1);
      WriteableBitmap writeableBitmap = await wb.FromBitmapImage((BitmapImage) this.NormalStateImageSource);
      await wb.WaitForLoadedAsync();
      wb.Lighten(this.GeneratedCheckedStateLightenAmount);
      this._checkedStateImage.put_Source((ImageSource) wb);
    }

    private async void GenerateCheckedHoverStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      WriteableBitmap wb = new WriteableBitmap(1, 1);
      if (this.CheckedStateImageSource != null)
      {
        WriteableBitmap writeableBitmap1 = await wb.FromBitmapImage((BitmapImage) this.CheckedStateImageSource);
      }
      else if (this.NormalStateImageSource != null)
      {
        WriteableBitmap writeableBitmap2 = await wb.FromBitmapImage((BitmapImage) this.NormalStateImageSource);
      }
      await wb.WaitForLoadedAsync();
      wb.Lighten(this.GeneratedCheckedHoverStateLightenAmount);
      this._checkedHoverStateImage.put_Source((ImageSource) wb);
    }

    private async void GenerateCheckedPressedStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      WriteableBitmap wb = new WriteableBitmap(1, 1);
      if (this.CheckedStateImageSource != null)
      {
        WriteableBitmap writeableBitmap1 = await wb.FromBitmapImage((BitmapImage) this.CheckedStateImageSource);
      }
      else if (this.NormalStateImageSource != null)
      {
        WriteableBitmap writeableBitmap2 = await wb.FromBitmapImage((BitmapImage) this.NormalStateImageSource);
      }
      await wb.WaitForLoadedAsync();
      wb.Lighten(this.GeneratedCheckedPressedStateLightenAmount);
      this._checkedPressedStateImage.put_Source((ImageSource) wb);
    }

    private async void GenerateCheckedDisabledStateImage()
    {
      if (DesignMode.DesignModeEnabled)
        return;
      WriteableBitmap wb = new WriteableBitmap(1, 1);
      if (this.CheckedStateImageSource != null)
      {
        WriteableBitmap writeableBitmap = await wb.FromBitmapImage((BitmapImage) this.CheckedStateImageSource);
        await wb.WaitForLoadedAsync();
        wb.Grayscale(this.GeneratedCheckedDisabledStateGrayscaleAmount);
      }
      else if (this.PressedStateImageSource != null)
      {
        WriteableBitmap writeableBitmap = await wb.FromBitmapImage((BitmapImage) this.PressedStateImageSource);
        await wb.WaitForLoadedAsync();
        wb.Grayscale(this.GeneratedCheckedDisabledStateGrayscaleAmount);
      }
      else if (this.NormalStateImageSource != null)
      {
        WriteableBitmap writeableBitmap = await wb.FromBitmapImage((BitmapImage) this.NormalStateImageSource);
        await wb.WaitForLoadedAsync();
        wb.Grayscale(this.GeneratedCheckedDisabledStateGrayscaleAmount);
      }
      this._checkedDisabledStateImage.put_Source((ImageSource) wb);
    }

    public ImageToggleButton() => ((Control) this).put_DefaultStyleKey((object) typeof (ImageToggleButton));

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._normalStateImage = ((Control) this).GetTemplateChild("PART_NormalStateImage") as Image;
      this._hoverStateImage = ((Control) this).GetTemplateChild("PART_HoverStateImage") as Image;
      this._hoverStateRecycledCheckedStateImage = ((Control) this).GetTemplateChild("PART_HoverStateRecycledNormalStateImage") as Image;
      this._hoverStateRecycledCheckedPressedStateImage = ((Control) this).GetTemplateChild("PART_HoverStateRecycledPressedStateImage") as Image;
      this._pressedStateImage = ((Control) this).GetTemplateChild("PART_PressedStateImage") as Image;
      this._disabledStateImage = ((Control) this).GetTemplateChild("PART_DisabledStateImage") as Image;
      this._checkedStateImage = ((Control) this).GetTemplateChild("PART_CheckedStateImage") as Image;
      this._checkedHoverStateImage = ((Control) this).GetTemplateChild("PART_CheckedHoverStateImage") as Image;
      this._checkedHoverStateRecycledCheckedStateImage = ((Control) this).GetTemplateChild("PART_CheckedHoverStateRecycledCheckedStateImage") as Image;
      this._checkedHoverStateRecycledCheckedPressedStateImage = ((Control) this).GetTemplateChild("PART_CheckedHoverStateRecycledCheckedPressedStateImage") as Image;
      this._checkedPressedStateImage = ((Control) this).GetTemplateChild("PART_CheckedPressedStateImage") as Image;
      this._checkedDisabledStateImage = ((Control) this).GetTemplateChild("PART_CheckedDisabledStateImage") as Image;
      this.UpdateNormalStateImage();
      this.UpdateHoverStateImage();
      this.UpdateRecycledHoverStateImages();
      this.UpdatePressedStateImage();
      this.UpdateDisabledStateImage();
      this.UpdateCheckedStateImage();
      this.UpdateCheckedHoverStateImage();
      this.UpdateRecycledCheckedHoverStateImages();
      this.UpdateCheckedPressedStateImage();
      this.UpdateCheckedDisabledStateImage();
    }

    private void UpdateNormalStateImage()
    {
      if (this._normalStateImage == null)
        return;
      this._normalStateImage.put_Source(this.NormalStateImageSource);
    }

    private void UpdateHoverStateImage()
    {
      if (this._hoverStateImage == null)
        return;
      if (!this.GenerateMissingImages || this.NormalStateImageSource == null)
        this._hoverStateImage.put_Source(this.HoverStateImageSource);
      else
        this.GenerateHoverStateImage();
      if (this._hoverStateImage.Source != null)
        return;
      this._hoverStateImage.put_Source(this.NormalStateImageSource);
    }

    private void UpdateRecycledHoverStateImages()
    {
      if (this._hoverStateRecycledCheckedStateImage != null)
      {
        if (this.RecyclePressedStateImageForHover && this.NormalStateImageSource != null)
          this._hoverStateRecycledCheckedStateImage.put_Source(this.NormalStateImageSource);
        else
          this._hoverStateRecycledCheckedStateImage.put_Source((ImageSource) null);
      }
      if (this._hoverStateRecycledCheckedPressedStateImage == null)
        return;
      if (this.RecyclePressedStateImageForHover && this.PressedStateImageSource != null)
        this._hoverStateRecycledCheckedPressedStateImage.put_Source(this.PressedStateImageSource);
      else
        this._hoverStateRecycledCheckedPressedStateImage.put_Source((ImageSource) null);
    }

    private void UpdatePressedStateImage()
    {
      if (this._pressedStateImage == null)
        return;
      if (!this.GenerateMissingImages || this.NormalStateImageSource == null)
        this._pressedStateImage.put_Source(this.PressedStateImageSource);
      else
        this.GeneratePressedStateImage();
    }

    private void UpdateDisabledStateImage()
    {
      if (this._disabledStateImage == null)
        return;
      if (!this.GenerateMissingImages || this.NormalStateImageSource == null)
        this._disabledStateImage.put_Source(this.DisabledStateImageSource);
      else
        this.GenerateDisabledStateImage();
    }

    private void UpdateCheckedStateImage()
    {
      if (this._checkedStateImage == null)
        return;
      if (this.CheckedStateImageSource != null)
        this._checkedStateImage.put_Source(this.CheckedStateImageSource);
      else if (this.RecycleUncheckedStateImagesForCheckedStates && this.PressedStateImageSource != null)
      {
        this._checkedStateImage.put_Source(this.PressedStateImageSource);
      }
      else
      {
        if (!this.GenerateMissingImages)
          return;
        this.GenerateCheckedStateImage();
      }
    }

    private void UpdateCheckedHoverStateImage()
    {
      if (this._checkedHoverStateImage == null)
        return;
      if (this.CheckedHoverStateImageSource != null)
        this._checkedHoverStateImage.put_Source(this.CheckedHoverStateImageSource);
      else if (this.RecycleUncheckedStateImagesForCheckedStates && this.HoverStateImageSource != null)
        this._checkedHoverStateImage.put_Source(this.HoverStateImageSource);
      else if (this.GenerateMissingImages)
        this.GenerateCheckedHoverStateImage();
      if (this._checkedHoverStateImage.Source != null)
        return;
      this._checkedHoverStateImage.put_Source(this.CheckedStateImageSource);
    }

    private void UpdateRecycledCheckedHoverStateImages()
    {
      if (this._checkedHoverStateRecycledCheckedStateImage != null)
      {
        if (this.RecyclePressedStateImageForHover)
        {
          if (this.CheckedStateImageSource != null)
            this._checkedHoverStateRecycledCheckedStateImage.put_Source(this.CheckedStateImageSource);
        }
        else
          this._checkedHoverStateRecycledCheckedStateImage.put_Source((ImageSource) null);
      }
      if (this._checkedHoverStateRecycledCheckedPressedStateImage == null || !this.RecyclePressedStateImageForHover)
        return;
      if (this.CheckedPressedStateImageSource != null)
        this._checkedHoverStateRecycledCheckedPressedStateImage.put_Source(this.CheckedPressedStateImageSource);
      else
        this._checkedHoverStateRecycledCheckedPressedStateImage.put_Source((ImageSource) null);
    }

    private void UpdateCheckedPressedStateImage()
    {
      if (this._checkedPressedStateImage == null)
        return;
      if (this.CheckedPressedStateImageSource != null)
        this._checkedPressedStateImage.put_Source(this.CheckedPressedStateImageSource);
      else if (this.RecycleUncheckedStateImagesForCheckedStates && this.PressedStateImageSource != null)
      {
        this._checkedPressedStateImage.put_Source(this.PressedStateImageSource);
      }
      else
      {
        if (!this.GenerateMissingImages)
          return;
        this.GenerateCheckedPressedStateImage();
      }
    }

    private void UpdateCheckedDisabledStateImage()
    {
      if (this._disabledStateImage == null)
        return;
      if (this.CheckedDisabledStateImageSource != null)
        this._checkedDisabledStateImage.put_Source(this.CheckedDisabledStateImageSource);
      else if (this.RecycleUncheckedStateImagesForCheckedStates && this.DisabledStateImageSource != null)
      {
        this._checkedDisabledStateImage.put_Source(this.DisabledStateImageSource);
      }
      else
      {
        if (!this.GenerateMissingImages)
          return;
        this.GenerateCheckedDisabledStateImage();
      }
    }
  }
}
