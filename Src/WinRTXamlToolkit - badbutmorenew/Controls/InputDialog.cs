// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.InputDialog
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "TextTextBlock", Type = typeof (TextBlock))]
  [TemplatePart(Name = "LayoutRoot", Type = typeof (Panel))]
  [TemplatePart(Name = "ContentBorder", Type = typeof (Border))]
  [TemplatePart(Name = "InputTextBox", Type = typeof (TextBox))]
  [TemplatePart(Name = "TitleTextBlock", Type = typeof (TextBlock))]
  [StyleTypedProperty(Property = "InputTextStyle", StyleTargetType = typeof (TextBox))]
  [TemplatePart(Name = "ButtonsPanel", Type = typeof (Panel))]
  [TemplateVisualState(GroupName = "PopupStates", Name = "OpenPopupState")]
  [TemplateVisualState(GroupName = "PopupStates", Name = "ClosedPopupState")]
  [StyleTypedProperty(Property = "ButtonStyle", StyleTargetType = typeof (ButtonBase))]
  [StyleTypedProperty(Property = "TitleStyle", StyleTargetType = typeof (TextBlock))]
  [StyleTypedProperty(Property = "TextStyle", StyleTargetType = typeof (TextBlock))]
  public class InputDialog : Control
  {
    private const string PopupStatesGroupName = "PopupStates";
    private const string OpenPopupStateName = "OpenPopupState";
    private const string ClosedPopupStateName = "ClosedPopupState";
    private const string LayoutRootPanelName = "LayoutRoot";
    private const string ContentBorderName = "ContentBorder";
    private const string InputTextBoxName = "InputTextBox";
    private const string TitleTextBlockName = "TitleTextBlock";
    private const string TextTextBlockName = "TextTextBlock";
    private const string ButtonsPanelName = "ButtonsPanel";
    private Panel _layoutRoot;
    private Border _contentBorder;
    private TextBox _inputTextBox;
    private TextBlock _titleTextBlock;
    private TextBlock _textTextBlock;
    private Panel _buttonsPanel;
    private bool _shown;
    private TaskCompletionSource<string> _dismissTaskSource;
    private List<ButtonBase> _buttons;
    private Popup _dialogPopup;
    private Panel _parentPanel;
    private Panel _temporaryParentPanel;
    private ContentControl _parentContentControl;
    public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(nameof (ButtonStyle), (Type) typeof (Style), (Type) typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register(nameof (InputText), (Type) typeof (string), (Type) typeof (InputDialog), new PropertyMetadata((object) "", new PropertyChangedCallback(InputDialog.OnInputTextChanged)));
    public static readonly DependencyProperty AcceptButtonProperty = DependencyProperty.Register(nameof (AcceptButton), (Type) typeof (string), (Type) typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty CancelButtonProperty = DependencyProperty.Register(nameof (CancelButton), (Type) typeof (string), (Type) typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty BackgroundScreenBrushProperty = DependencyProperty.Register(nameof (BackgroundScreenBrush), (Type) typeof (Brush), (Type) typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty BackgroundStripeBrushProperty = DependencyProperty.Register(nameof (BackgroundStripeBrush), (Type) typeof (Brush), (Type) typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty TitleStyleProperty = DependencyProperty.Register(nameof (TitleStyle), (Type) typeof (Style), (Type) typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty TextStyleProperty = DependencyProperty.Register(nameof (TextStyle), (Type) typeof (Style), (Type) typeof (InputDialog), new PropertyMetadata((object) null, new PropertyChangedCallback(InputDialog.OnTextStyleChanged)));
    public static readonly DependencyProperty InputTextStyleProperty = DependencyProperty.Register(nameof (InputTextStyle), (Type) typeof (Style), (Type) typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty IsLightDismissEnabledProperty = DependencyProperty.Register(nameof (IsLightDismissEnabled), (Type) typeof (bool), (Type) typeof (InputDialog), new PropertyMetadata((object) false));
    public static readonly DependencyProperty AwaitsCloseTransitionProperty = DependencyProperty.Register(nameof (AwaitsCloseTransition), (Type) typeof (bool), (Type) typeof (InputDialog), new PropertyMetadata((object) true));
    public static readonly DependencyProperty ButtonsPanelOrientationProperty = DependencyProperty.Register(nameof (ButtonsPanelOrientation), (Type) typeof (Orientation), (Type) typeof (InputDialog), new PropertyMetadata((object) (Orientation) 1));

    public Style ButtonStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(InputDialog.ButtonStyleProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.ButtonStyleProperty, (object) value);
    }

    public string InputText
    {
      get => (string) ((DependencyObject) this).GetValue(InputDialog.InputTextProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.InputTextProperty, (object) value);
    }

    private static void OnInputTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      InputDialog inputDialog = (InputDialog) d;
      string oldValue = (string) e.OldValue;
      string inputText = inputDialog.InputText;
      inputDialog.OnInputTextChanged(oldValue, inputText);
    }

    protected virtual void OnInputTextChanged(string oldInputText, string newInputText)
    {
      if (this._inputTextBox == null)
        return;
      this._inputTextBox.put_Text(newInputText);
    }

    public string AcceptButton
    {
      get => (string) ((DependencyObject) this).GetValue(InputDialog.AcceptButtonProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.AcceptButtonProperty, (object) value);
    }

    public string CancelButton
    {
      get => (string) ((DependencyObject) this).GetValue(InputDialog.CancelButtonProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.CancelButtonProperty, (object) value);
    }

    public Brush BackgroundScreenBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(InputDialog.BackgroundScreenBrushProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.BackgroundScreenBrushProperty, (object) value);
    }

    public Brush BackgroundStripeBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(InputDialog.BackgroundStripeBrushProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.BackgroundStripeBrushProperty, (object) value);
    }

    public Style TitleStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(InputDialog.TitleStyleProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.TitleStyleProperty, (object) value);
    }

    public Style TextStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(InputDialog.TextStyleProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.TextStyleProperty, (object) value);
    }

    private static void OnTextStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      InputDialog inputDialog = (InputDialog) d;
      Style oldValue = (Style) e.OldValue;
      Style textStyle = inputDialog.TextStyle;
      inputDialog.OnTextStyleChanged(oldValue, textStyle);
    }

    protected virtual void OnTextStyleChanged(Style oldTextStyle, Style newTextStyle)
    {
    }

    public Style InputTextStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(InputDialog.InputTextStyleProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.InputTextStyleProperty, (object) value);
    }

    public bool IsLightDismissEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(InputDialog.IsLightDismissEnabledProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.IsLightDismissEnabledProperty, (object) value);
    }

    public bool AwaitsCloseTransition
    {
      get => (bool) ((DependencyObject) this).GetValue(InputDialog.AwaitsCloseTransitionProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.AwaitsCloseTransitionProperty, (object) value);
    }

    public Orientation ButtonsPanelOrientation
    {
      get => (Orientation) ((DependencyObject) this).GetValue(InputDialog.ButtonsPanelOrientationProperty);
      set => ((DependencyObject) this).SetValue(InputDialog.ButtonsPanelOrientationProperty, (object) value);
    }

    public InputDialog()
    {
      this.put_DefaultStyleKey((object) typeof (InputDialog));
      ((UIElement) this).put_Visibility((Visibility) 1);
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._layoutRoot = this.GetTemplateChild("LayoutRoot") as Panel;
      this._contentBorder = this.GetTemplateChild("ContentBorder") as Border;
      this._inputTextBox = this.GetTemplateChild("InputTextBox") as TextBox;
      this._titleTextBlock = this.GetTemplateChild("TitleTextBlock") as TextBlock;
      this._textTextBlock = this.GetTemplateChild("TextTextBlock") as TextBlock;
      this._buttonsPanel = this.GetTemplateChild("ButtonsPanel") as Panel;
      Panel layoutRoot = this._layoutRoot;
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>((Func<TappedEventHandler, EventRegistrationToken>) new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) layoutRoot).add_Tapped), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) layoutRoot).remove_Tapped), new TappedEventHandler(this.OnLayoutRootTapped));
      this._inputTextBox.put_Text(this.InputText);
      TextBox inputTextBox1 = this._inputTextBox;
      WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>((Func<TextChangedEventHandler, EventRegistrationToken>) new Func<TextChangedEventHandler, EventRegistrationToken>(inputTextBox1.add_TextChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(inputTextBox1.remove_TextChanged), new TextChangedEventHandler(this.OnInputTextBoxTextChanged));
      TextBox inputTextBox2 = this._inputTextBox;
      WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(((UIElement) inputTextBox2).add_KeyUp), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) inputTextBox2).remove_KeyUp), new KeyEventHandler(this.OnInputTextBoxKeyUp));
      Border contentBorder = this._contentBorder;
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>((Func<TappedEventHandler, EventRegistrationToken>) new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) contentBorder).add_Tapped), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) contentBorder).remove_Tapped), new TappedEventHandler(this.OnContentBorderTapped));
    }

    private void OnGlobalKeyUp(object sender, KeyRoutedEventArgs e)
    {
      if (e.Key != 27)
        return;
      this.DismissDialog();
      e.put_Handled(true);
    }

    private void OnLayoutRootTapped(object sender, TappedRoutedEventArgs e)
    {
      if (((RoutedEventArgs) e).OriginalSource != sender || !this.IsLightDismissEnabled)
        return;
      this._dismissTaskSource.TrySetResult(this.CancelButton);
      e.put_Handled(true);
    }

    private void OnContentBorderTapped(object sender, TappedRoutedEventArgs e)
    {
      if (((RoutedEventArgs) e).OriginalSource != sender)
        return;
      this.FocusOnButton(this.AcceptButton);
      e.put_Handled(true);
    }

    private void FocusOnButton(string buttonContent)
    {
      ButtonBase buttonBase;
      if (this.AcceptButton != null && (buttonBase = this._buttons.FirstOrDefault<ButtonBase>((Func<ButtonBase, bool>) (b => object.Equals(((ContentControl) b).Content, (object) buttonContent)))) != null)
      {
        ((Control) buttonBase).Focus((FocusState) 3);
      }
      else
      {
        if (this._buttons.Count <= 0)
          return;
        ((Control) this._buttons[0]).Focus((FocusState) 3);
      }
    }

    private void OnInputTextBoxTextChanged(object sender, TextChangedEventArgs e) => this.InputText = this._inputTextBox.Text;

    public async Task<string> ShowAsync(string title, string text, params string[] buttonTexts)
    {
      if (this._shown)
        throw new InvalidOperationException("The dialog is already shown.");
      ((UIElement) this).put_Visibility((Visibility) 0);
      this._shown = true;
      UIElement content = Window.Current.Content;
      WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(content.add_KeyUp), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(content.remove_KeyUp), new KeyEventHandler(this.OnGlobalKeyUp));
      this._dismissTaskSource = new TaskCompletionSource<string>();
      this._parentPanel = ((FrameworkElement) this).Parent as Panel;
      this._parentContentControl = ((FrameworkElement) this).Parent as ContentControl;
      if (this._parentPanel != null)
        ((ICollection<UIElement>) this._parentPanel.Children).Remove((UIElement) this);
      if (this._parentContentControl != null)
        this._parentContentControl.put_Content((object) null);
      Popup popup = new Popup();
      popup.put_Child((UIElement) this);
      this._dialogPopup = popup;
      if (this._parentPanel != null)
      {
        ((ICollection<UIElement>) this._parentPanel.Children).Add((UIElement) this._dialogPopup);
        Panel parentPanel = this._parentPanel;
        WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) parentPanel).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) parentPanel).remove_SizeChanged), new SizeChangedEventHandler(this.OnParentSizeChanged));
      }
      else if (this._parentContentControl != null)
      {
        this._parentContentControl.put_Content((object) this._dialogPopup);
        ContentControl parentContentControl = this._parentContentControl;
        WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) parentContentControl).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) parentContentControl).remove_SizeChanged), new SizeChangedEventHandler(this.OnParentSizeChanged));
      }
      else
      {
        this._temporaryParentPanel = ((DependencyObject) Window.Current.Content).GetFirstDescendantOfType<Panel>();
        if (this._temporaryParentPanel != null)
        {
          ((ICollection<UIElement>) this._temporaryParentPanel.Children).Add((UIElement) this._dialogPopup);
          Panel temporaryParentPanel = this._temporaryParentPanel;
          WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) temporaryParentPanel).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) temporaryParentPanel).remove_SizeChanged), new SizeChangedEventHandler(this.OnParentSizeChanged));
        }
      }
      this._dialogPopup.put_IsOpen(true);
      await ((FrameworkElement) this).WaitForLayoutUpdateAsync();
      this._titleTextBlock.put_Text(title);
      this._textTextBlock.put_Text(text);
      this._buttons = new List<ButtonBase>();
      foreach (string buttonText in buttonTexts)
      {
        Button button = new Button();
        if (this.ButtonStyle != null)
          ((FrameworkElement) button).put_Style(this.ButtonStyle);
        ((ContentControl) button).put_Content((object) buttonText);
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) button).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) button).remove_Click), new RoutedEventHandler(this.OnButtonClick));
        WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(((UIElement) button).add_KeyUp), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) button).remove_KeyUp), new KeyEventHandler(this.OnGlobalKeyUp));
        this._buttons.Add((ButtonBase) button);
        ((ICollection<UIElement>) this._buttonsPanel.Children).Add((UIElement) button);
      }
      bool flag;
      int num = flag ? 1 : 0;
      ((Control) this._inputTextBox).Focus((FocusState) 3);
      this.ResizeLayoutRoot();
      await this.GoToVisualStateAsync((FrameworkElement) this._layoutRoot, "PopupStates", "OpenPopupState");
      string result = await this._dismissTaskSource.Task;
      if (this.AwaitsCloseTransition)
        await this.CloseAsync();
      else
        this.CloseAsync();
      WindowsRuntimeMarshal.RemoveEventHandler<KeyEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(Window.Current.Content.remove_KeyUp), new KeyEventHandler(this.OnGlobalKeyUp));
      return result;
    }

    private void ResizeLayoutRoot()
    {
      object obj = (object) this._parentPanel;
      if (obj == null)
      {
        ContentControl parentContentControl = this._parentContentControl;
        obj = parentContentControl != null ? (object) parentContentControl : (object) this._temporaryParentPanel;
      }
      FrameworkElement frameworkElement = (FrameworkElement) obj;
      ((FrameworkElement) this._layoutRoot).put_Width(frameworkElement.ActualWidth);
      ((FrameworkElement) this._layoutRoot).put_Height(frameworkElement.ActualHeight);
    }

    private void OnParentSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs) => this.ResizeLayoutRoot();

    private async Task CloseAsync()
    {
      if (!this._shown)
        throw new InvalidOperationException("The dialog isn't shown, so it can't be closed.");
      await this.GoToVisualStateAsync((FrameworkElement) this._layoutRoot, "PopupStates", "ClosedPopupState");
      this._dialogPopup.put_IsOpen(false);
      ((ICollection<UIElement>) this._buttonsPanel.Children).Clear();
      foreach (ButtonBase button in this._buttons)
      {
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(button.remove_Click), new RoutedEventHandler(this.OnButtonClick));
        WindowsRuntimeMarshal.RemoveEventHandler<KeyEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) button).remove_KeyUp), new KeyEventHandler(this.OnGlobalKeyUp));
      }
      this._buttons.Clear();
      this._dialogPopup.put_Child((UIElement) null);
      if (this._parentPanel != null)
      {
        ((ICollection<UIElement>) this._parentPanel.Children).Remove((UIElement) this._dialogPopup);
        ((ICollection<UIElement>) this._parentPanel.Children).Add((UIElement) this);
        WindowsRuntimeMarshal.RemoveEventHandler<SizeChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this._parentPanel).remove_SizeChanged), new SizeChangedEventHandler(this.OnParentSizeChanged));
        this._parentPanel = (Panel) null;
      }
      if (this._parentContentControl != null)
      {
        this._parentContentControl.put_Content((object) this);
        WindowsRuntimeMarshal.RemoveEventHandler<SizeChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this._parentContentControl).remove_SizeChanged), new SizeChangedEventHandler(this.OnParentSizeChanged));
        this._parentContentControl = (ContentControl) null;
      }
      if (this._temporaryParentPanel != null)
      {
        ((ICollection<UIElement>) this._temporaryParentPanel.Children).Remove((UIElement) this._dialogPopup);
        WindowsRuntimeMarshal.RemoveEventHandler<SizeChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this._temporaryParentPanel).remove_SizeChanged), new SizeChangedEventHandler(this.OnParentSizeChanged));
        this._temporaryParentPanel = (Panel) null;
      }
      this._dialogPopup = (Popup) null;
      ((UIElement) this).put_Visibility((Visibility) 1);
      this._shown = false;
    }

    private void OnButtonClick(object sender, RoutedEventArgs e) => this._dismissTaskSource.TrySetResult((string) ((ContentControl) sender).Content);

    private void OnInputTextBoxKeyUp(object sender, KeyRoutedEventArgs e)
    {
      this.InputText = this._inputTextBox.Text;
      if (e.Key == 13)
      {
        this.FocusOnButton(this.AcceptButton);
        e.put_Handled(true);
      }
      else
      {
        if (e.Key != 27)
          return;
        this.FocusOnButton(this.CancelButton);
        e.put_Handled(true);
      }
    }

    private void DismissDialog()
    {
      if (this.CancelButton != null)
        this._dismissTaskSource.TrySetResult(this.CancelButton);
      if (this._buttons.Count <= 0)
        return;
      ((Control) this._buttons[0]).Focus((FocusState) 3);
    }
  }
}
