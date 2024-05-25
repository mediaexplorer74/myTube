// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.AutoCompleteTextBox
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "AutocompleteItemsContainer", Type = typeof (ItemsControl))]
  [TemplateVisualState(Name = "PointerOver", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  [TemplatePart(Name = "TextBox", Type = typeof (TextBox))]
  [TemplatePart(Name = "AutoCompletePresenter", Type = typeof (Popup))]
  public sealed class AutoCompleteTextBox : Control
  {
    private const string TextBoxPropertyName = "TextBox";
    private const string AutoCompletePresenterPropertyName = "AutoCompletePresenter";
    private const string AutocompleteItemsContainerPropertyName = "AutocompleteItemsContainer";
    private const string GroupCommon = "CommonStates";
    private const string StateDisabled = "Disabled";
    private const string StateNormal = "Normal";
    private const string StatePointerOver = "PointerOver";
    private UpDownTextBox textBox;
    private Popup autoCompletePresenter;
    private SimpleThemingListBox autocompleteItemsContainer;
    private AutoCompleteTextBox.SuggestionPopupDisplaySide suggestionPopupDisplaySide;
    private static readonly DependencyProperty _ScrollBarBackgroundProperty = DependencyProperty.Register(nameof (ScrollBarBackground), (Type) typeof (Brush), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _ScrollBarBorderBrushProperty = DependencyProperty.Register(nameof (ScrollBarBorderBrush), (Type) typeof (Brush), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _ScrollBarBorderThicknessProperty = DependencyProperty.Register(nameof (ScrollBarBorderThickness), (Type) typeof (Thickness), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) new Thickness(0.0)));
    private static readonly DependencyProperty _ItemPointerOverBackgroundThemeBrushProperty = DependencyProperty.Register(nameof (ItemPointerOverBackgroundThemeBrush), (Type) typeof (Brush), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _ItemPointerOverForegroundThemeBrushProperty = DependencyProperty.Register(nameof (ItemPointerOverForegroundThemeBrush), (Type) typeof (Brush), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _MaximumVisibleSuggestionsProperty = DependencyProperty.Register(nameof (MaximumVisibleSuggestions), (Type) typeof (int), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) 3));
    private static readonly DependencyProperty _AutoCompleteServiceProperty = DependencyProperty.Register(nameof (AutoCompleteService), (Type) typeof (object), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) new AutoCompleteTextBox.DamerauLevenshteinDistance()));
    private static readonly DependencyProperty _WordDictionaryProperty = DependencyProperty.Register(nameof (WordDictionary), (Type) typeof (object), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _TextProperty = DependencyProperty.Register(nameof (Text), (Type) typeof (string), (Type) typeof (AutoCompleteTextBox), new PropertyMetadata((object) string.Empty));
    private bool isRaisedByUserTyping = true;
    private bool isLoaded;

    public static DependencyProperty ScrollBarBackgroundProperty => AutoCompleteTextBox._ScrollBarBackgroundProperty;

    public Brush ScrollBarBackground
    {
      get => (Brush) ((DependencyObject) this).GetValue(AutoCompleteTextBox.ScrollBarBackgroundProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.ScrollBarBackgroundProperty, (object) value);
    }

    public static DependencyProperty ScrollBarBorderBrushProperty => AutoCompleteTextBox._ScrollBarBorderBrushProperty;

    public Brush ScrollBarBorderBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(AutoCompleteTextBox.ScrollBarBorderBrushProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.ScrollBarBorderBrushProperty, (object) value);
    }

    public static DependencyProperty ScrollBarBorderThicknessProperty => AutoCompleteTextBox._ScrollBarBorderThicknessProperty;

    public Thickness ScrollBarBorderThickness
    {
      get => (Thickness) ((DependencyObject) this).GetValue(AutoCompleteTextBox.ScrollBarBorderThicknessProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.ScrollBarBorderThicknessProperty, (object) value);
    }

    public static DependencyProperty ItemPointerOverBackgroundThemeBrushProperty => AutoCompleteTextBox._ItemPointerOverBackgroundThemeBrushProperty;

    public Brush ItemPointerOverBackgroundThemeBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(AutoCompleteTextBox.ItemPointerOverBackgroundThemeBrushProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.ItemPointerOverBackgroundThemeBrushProperty, (object) value);
    }

    public static DependencyProperty ItemPointerOverForegroundThemeBrushProperty => AutoCompleteTextBox._ItemPointerOverForegroundThemeBrushProperty;

    public Brush ItemPointerOverForegroundThemeBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(AutoCompleteTextBox.ItemPointerOverForegroundThemeBrushProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.ItemPointerOverForegroundThemeBrushProperty, (object) value);
    }

    public static DependencyProperty MaximumVisibleSuggestionsProperty => AutoCompleteTextBox._MaximumVisibleSuggestionsProperty;

    public int MaximumVisibleSuggestions
    {
      get => (int) ((DependencyObject) this).GetValue(AutoCompleteTextBox.MaximumVisibleSuggestionsProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.MaximumVisibleSuggestionsProperty, (object) value);
    }

    public static DependencyProperty AutoCompleteServiceProperty => AutoCompleteTextBox._AutoCompleteServiceProperty;

    public object AutoCompleteService
    {
      get => ((DependencyObject) this).GetValue(AutoCompleteTextBox.AutoCompleteServiceProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.AutoCompleteServiceProperty, value);
    }

    public static DependencyProperty WordDictionaryProperty => AutoCompleteTextBox._WordDictionaryProperty;

    public object WordDictionary
    {
      get => ((DependencyObject) this).GetValue(AutoCompleteTextBox.WordDictionaryProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.WordDictionaryProperty, value);
    }

    public static DependencyProperty TextProperty => AutoCompleteTextBox._TextProperty;

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(AutoCompleteTextBox.TextProperty);
      set => ((DependencyObject) this).SetValue(AutoCompleteTextBox.TextProperty, (object) value);
    }

    public AutoCompleteTextBox()
    {
      this.put_DefaultStyleKey((object) typeof (AutoCompleteTextBox));
      AutoCompleteTextBox autoCompleteTextBox1 = this;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) autoCompleteTextBox1).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) autoCompleteTextBox1).remove_SizeChanged), new SizeChangedEventHandler(this.OnSizeChanged));
      AutoCompleteTextBox autoCompleteTextBox2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) autoCompleteTextBox2).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) autoCompleteTextBox2).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      AutoCompleteTextBox autoCompleteTextBox3 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) autoCompleteTextBox3).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) autoCompleteTextBox3).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
      if (this.autoCompletePresenter != null)
        ((FrameworkElement) this.autoCompletePresenter).put_Width(((FrameworkElement) this).ActualWidth);
      if (this.autocompleteItemsContainer == null)
        return;
      ((FrameworkElement) this.autocompleteItemsContainer).put_Width(((FrameworkElement) this).ActualWidth);
    }

    protected virtual void OnApplyTemplate()
    {
      this.UnsubscribeEvents();
      this.textBox = this.GetTemplateChild("TextBox") as UpDownTextBox;
      this.autoCompletePresenter = this.GetTemplateChild("AutoCompletePresenter") as Popup;
      ((FrameworkElement) this.autoCompletePresenter).put_Width(((FrameworkElement) this).ActualWidth);
      this.autocompleteItemsContainer = this.GetTemplateChild("AutocompleteItemsContainer") as SimpleThemingListBox;
      ((FrameworkElement) this.autocompleteItemsContainer).put_Width(((FrameworkElement) this).ActualWidth);
      this.SetBorderThickness();
      ((FrameworkElement) this).OnApplyTemplate();
      this.SubscribeEvents();
    }

    protected virtual void OnPointerEntered(PointerRoutedEventArgs e)
    {
      base.OnPointerEntered(e);
      this.UpdateState(AutoCompleteTextBox.VisualControlState.PointerOver);
    }

    protected virtual void OnPointerExited(PointerRoutedEventArgs e)
    {
      base.OnPointerExited(e);
      this.UpdateState(AutoCompleteTextBox.VisualControlState.Normal);
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      this.isLoaded = true;
      this.SubscribeEvents();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      this.isLoaded = false;
      this.UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
      if (!this.isLoaded)
        return;
      if (this.textBox != null)
      {
        UpDownTextBox textBox1 = this.textBox;
        WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>((Func<TextChangedEventHandler, EventRegistrationToken>) new Func<TextChangedEventHandler, EventRegistrationToken>(((TextBox) textBox1).add_TextChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((TextBox) textBox1).remove_TextChanged), new TextChangedEventHandler(this.OnTextChanged));
        UpDownTextBox textBox2 = this.textBox;
        WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(((UIElement) textBox2).add_KeyDown), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) textBox2).remove_KeyDown), new KeyEventHandler(this.OnTextBoxKeyDown));
        this.textBox.UpPressed += new EventHandler(this.OnTextBoxUpPressed);
        this.textBox.DownPressed += new EventHandler(this.OnTextBoxDownPressed);
      }
      if (this.autoCompletePresenter != null)
      {
        Popup completePresenter = this.autoCompletePresenter;
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(completePresenter.add_Closed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(completePresenter.remove_Closed), new EventHandler<object>(this.OnAutoCompleteClosed));
      }
      if (this.autocompleteItemsContainer != null)
      {
        SimpleThemingListBox autocompleteItemsContainer = this.autocompleteItemsContainer;
        WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>((Func<SelectionChangedEventHandler, EventRegistrationToken>) new Func<SelectionChangedEventHandler, EventRegistrationToken>(((Selector) autocompleteItemsContainer).add_SelectionChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Selector) autocompleteItemsContainer).remove_SelectionChanged), new SelectionChangedEventHandler(this.OnAutocompleteItemsContainerItemSelectionChanged));
      }
      AutoCompleteTextBox autoCompleteTextBox = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) autoCompleteTextBox).add_LostFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) autoCompleteTextBox).remove_LostFocus), new RoutedEventHandler(this.OnLostFocus));
    }

    private void UnsubscribeEvents()
    {
      if (this.textBox != null)
      {
        WindowsRuntimeMarshal.RemoveEventHandler<TextChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((TextBox) this.textBox).remove_TextChanged), new TextChangedEventHandler(this.OnTextChanged));
        WindowsRuntimeMarshal.RemoveEventHandler<KeyEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this.textBox).remove_KeyDown), new KeyEventHandler(this.OnTextBoxKeyDown));
        this.textBox.UpPressed -= new EventHandler(this.OnTextBoxUpPressed);
        this.textBox.DownPressed -= new EventHandler(this.OnTextBoxDownPressed);
      }
      if (this.autoCompletePresenter != null)
        WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this.autoCompletePresenter.remove_Closed), new EventHandler<object>(this.OnAutoCompleteClosed));
      if (this.autocompleteItemsContainer != null)
        WindowsRuntimeMarshal.RemoveEventHandler<SelectionChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Selector) this.autocompleteItemsContainer).remove_SelectionChanged), new SelectionChangedEventHandler(this.OnAutocompleteItemsContainerItemSelectionChanged));
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this).remove_LostFocus), new RoutedEventHandler(this.OnLostFocus));
    }

    private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
    {
      if (e.Key == 27)
        this.HideAutoCompleteSuggestions();
      else if (e.Key == 113)
      {
        this.TryOpenAutoComplete(this.Text);
      }
      else
      {
        if (e.Key != 13 || this.autoCompletePresenter == null || !this.autoCompletePresenter.IsOpen)
          return;
        this.HideAutoCompleteSuggestions();
      }
    }

    private void OnTextBoxDownPressed(object sender, EventArgs e)
    {
      if (this.autoCompletePresenter == null || this.autocompleteItemsContainer == null)
        return;
      if (!this.autoCompletePresenter.IsOpen)
      {
        this.TryOpenAutoComplete(this.Text);
        if (!(((ItemsControl) this.autocompleteItemsContainer).ItemsSource is ICollection<string> itemsSource) || itemsSource.Count <= 0)
          return;
        ((Selector) this.autocompleteItemsContainer).put_SelectedIndex(0);
      }
      else
      {
        if (!(((ItemsControl) this.autocompleteItemsContainer).ItemsSource is ICollection<string> itemsSource) || itemsSource.Count <= ((Selector) this.autocompleteItemsContainer).SelectedIndex + 1)
          return;
        SimpleThemingListBox autocompleteItemsContainer = this.autocompleteItemsContainer;
        ((Selector) autocompleteItemsContainer).put_SelectedIndex(((Selector) autocompleteItemsContainer).SelectedIndex + 1);
      }
    }

    private void OnTextBoxUpPressed(object sender, EventArgs e)
    {
      if (this.autoCompletePresenter == null || this.autocompleteItemsContainer == null)
        return;
      if (!this.autoCompletePresenter.IsOpen)
      {
        this.TryOpenAutoComplete(this.Text);
        if (!(((ItemsControl) this.autocompleteItemsContainer).ItemsSource is ICollection<string> itemsSource) || itemsSource.Count <= 0)
          return;
        ((Selector) this.autocompleteItemsContainer).put_SelectedIndex(itemsSource.Count - 1);
      }
      else
      {
        if (!(((ItemsControl) this.autocompleteItemsContainer).ItemsSource is ICollection<string> itemsSource) || itemsSource.Count <= 0)
          return;
        if (((Selector) this.autocompleteItemsContainer).SelectedIndex > 0)
        {
          SimpleThemingListBox autocompleteItemsContainer = this.autocompleteItemsContainer;
          ((Selector) autocompleteItemsContainer).put_SelectedIndex(((Selector) autocompleteItemsContainer).SelectedIndex - 1);
        }
        else
        {
          if (((Selector) this.autocompleteItemsContainer).SelectedIndex != -1)
            return;
          ((Selector) this.autocompleteItemsContainer).put_SelectedIndex(itemsSource.Count - 1);
        }
      }
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
      if (FocusManager.GetFocusedElement() is DependencyObject focusedElement && this.autocompleteItemsContainer != null && ((IEnumerable<DependencyObject>) focusedElement.GetAncestors()).Contains<DependencyObject>((DependencyObject) this.autocompleteItemsContainer))
        return;
      this.HideAutoCompleteSuggestions();
    }

    private void OnAutoCompleteClosed(object sender, object e) => this.SetBorderThickness();

    private void OnAutocompleteItemsContainerItemSelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      if (!(((Selector) this.autocompleteItemsContainer).SelectedItem is string selectedItem))
        return;
      this.isRaisedByUserTyping = false;
      this.Text = selectedItem;
      this.textBox.put_SelectionStart(0);
      this.textBox.put_SelectionLength(selectedItem.Length);
      ((Control) this.textBox).Focus((FocusState) 3);
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
      ((FrameworkElement) this.textBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
      if (this.isRaisedByUserTyping)
        this.TryOpenAutoComplete(this.Text);
      else
        this.isRaisedByUserTyping = true;
    }

    public void TryOpenAutoComplete(string textToComplete)
    {
      AutoCompleteTextBox.IAutoCompletable autoCompleteService = this.AutoCompleteService as AutoCompleteTextBox.IAutoCompletable;
      ICollection<string> wordDictionary = this.WordDictionary as ICollection<string>;
      if (autoCompleteService == null || wordDictionary == null)
        return;
      IList<string> suggestedWords = autoCompleteService.GetSuggestedWords(textToComplete, wordDictionary);
      if (((IEnumerable<string>) suggestedWords).Any<string>())
        this.ShowAutoCompletePopup((ICollection<string>) suggestedWords);
      else
        this.HideAutoCompleteSuggestions();
    }

    private void ShowAutoCompletePopup(ICollection<string> suggestedWords)
    {
      ((ItemsControl) this.autocompleteItemsContainer).put_ItemsSource((object) suggestedWords);
      this.autoCompletePresenter.put_IsOpen(true);
      this.AdjustSuggestionItemContainerSize(suggestedWords.Count);
      this.SetCompletionDisplaySide();
      this.SetBorderThickness();
    }

    public void HideAutoCompleteSuggestions()
    {
      this.autoCompletePresenter.put_IsOpen(false);
      ((ItemsControl) this.autocompleteItemsContainer).put_ItemsSource((object) null);
      this.SetBorderThickness();
    }

    private void AdjustSuggestionItemContainerSize(int suggestedWordsCount) => ((FrameworkElement) this.autocompleteItemsContainer).put_Height((suggestedWordsCount > this.MaximumVisibleSuggestions ? (double) this.MaximumVisibleSuggestions : (double) suggestedWordsCount) * this.autocompleteItemsContainer.GetItemHeight() + 5.0);

    private void SetCompletionDisplaySide()
    {
      Thickness thickness = new Thickness(0.0);
      this.suggestionPopupDisplaySide = this.GetDisplaySide();
      switch (this.suggestionPopupDisplaySide)
      {
        case AutoCompleteTextBox.SuggestionPopupDisplaySide.Top:
          double completeListBoxHeight = this.CalculateAutoCompleteListBoxHeight();
          thickness.Top = -completeListBoxHeight;
          break;
        case AutoCompleteTextBox.SuggestionPopupDisplaySide.Bottom:
          thickness.Top = ((FrameworkElement) this.textBox).ActualHeight;
          break;
      }
      ((FrameworkElement) this.autoCompletePresenter).put_Margin(thickness);
    }

    private AutoCompleteTextBox.SuggestionPopupDisplaySide GetDisplaySide() => this.GetSuggestionPopupVerticalEndCoordinate() > ((Rect) Window.Current.Bounds).Bottom ? AutoCompleteTextBox.SuggestionPopupDisplaySide.Top : AutoCompleteTextBox.SuggestionPopupDisplaySide.Bottom;

    private double GetSuggestionPopupVerticalEndCoordinate() => ((Point) ((UIElement) this.textBox).TransformToVisual(Window.Current.Content).TransformPoint((Point) new Point(0.0, 0.0))).Y + ((FrameworkElement) this.textBox).ActualHeight + this.CalculateAutoCompleteListBoxHeight();

    private double CalculateAutoCompleteListBoxHeight() => !(((ItemsControl) this.autocompleteItemsContainer).ItemsSource is ICollection<string> itemsSource) || !((IEnumerable<string>) itemsSource).Any<string>() ? 0.0 : this.autocompleteItemsContainer.GetItemHeight() * (itemsSource.Count > this.MaximumVisibleSuggestions ? (double) this.MaximumVisibleSuggestions : (double) itemsSource.Count);

    private void SetBorderThickness()
    {
      Thickness thickness1;
      Thickness thickness2;
      if (this.autoCompletePresenter.IsOpen)
      {
        if (this.suggestionPopupDisplaySide == AutoCompleteTextBox.SuggestionPopupDisplaySide.Bottom)
        {
          thickness1 = new Thickness(1.0, 1.0, 1.0, 0.0);
          thickness2 = new Thickness(1.0, 0.0, 1.0, 1.0);
        }
        else if (this.suggestionPopupDisplaySide == AutoCompleteTextBox.SuggestionPopupDisplaySide.Top)
        {
          thickness1 = new Thickness(1.0, 0.0, 1.0, 1.0);
          thickness2 = new Thickness(1.0, 1.0, 1.0, 0.0);
        }
      }
      else
      {
        thickness1 = new Thickness(1.0);
        thickness2 = new Thickness(0.0);
      }
      ((Control) this.textBox).put_BorderThickness(thickness1);
      ((Control) this.autocompleteItemsContainer).put_BorderThickness(thickness2);
    }

    private void UpdateState(AutoCompleteTextBox.VisualControlState newState)
    {
      switch (newState)
      {
        case AutoCompleteTextBox.VisualControlState.Disabled:
          VisualStateManager.GoToState((Control) this, "Disabled", false);
          break;
        case AutoCompleteTextBox.VisualControlState.Normal:
          VisualStateManager.GoToState((Control) this, "Normal", false);
          break;
        case AutoCompleteTextBox.VisualControlState.PointerOver:
          VisualStateManager.GoToState((Control) this, "PointerOver", false);
          break;
      }
    }

    public interface IAutoCompletable
    {
      IList<string> GetSuggestedWords(
        string wordToSuggest,
        ICollection<string> suggestionDictionary);
    }

    public abstract class AutoCompletable : AutoCompleteTextBox.IAutoCompletable
    {
      private int maximumSuggestionCount = 25;

      public int MaximumSuggestionCount
      {
        get => this.maximumSuggestionCount;
        set => this.maximumSuggestionCount = value;
      }

      public abstract IList<string> GetSuggestedWords(
        string wordToSuggest,
        ICollection<string> suggestionDictionary);
    }

    public class CommonSubstringSuggestion : AutoCompleteTextBox.AutoCompletable
    {
      private AutoCompleteTextBox.ScoredString GetScoreByLongestCommonSubstring(
        string wordToSuggest,
        string suggestion)
      {
        int[,] numArray = new int[wordToSuggest.Length + 1, suggestion.Length + 1];
        int val1 = int.MinValue;
        for (int index1 = 1; index1 <= wordToSuggest.Length; ++index1)
        {
          for (int index2 = 1; index2 <= suggestion.Length; ++index2)
          {
            if ((int) wordToSuggest[index1 - 1] == (int) suggestion[index2 - 1])
            {
              numArray[index1, index2] = numArray[index1 - 1, index2 - 1] + 1;
              val1 = Math.Max(val1, numArray[index1, index2]);
            }
          }
        }
        return new AutoCompleteTextBox.ScoredString()
        {
          Text = suggestion,
          Score = val1
        };
      }

      public override IList<string> GetSuggestedWords(
        string wordToSuggest,
        ICollection<string> suggestionDictionary)
      {
        IEnumerable<AutoCompleteTextBox.ScoredString> source = (IEnumerable<AutoCompleteTextBox.ScoredString>) ((IEnumerable<string>) suggestionDictionary).Select<string, AutoCompleteTextBox.ScoredString>((Func<string, AutoCompleteTextBox.ScoredString>) (suggestion => this.GetScoreByLongestCommonSubstring(wordToSuggest, suggestion)));
        int maximalScore = ((IEnumerable<AutoCompleteTextBox.ScoredString>) source).Max<AutoCompleteTextBox.ScoredString>((Func<AutoCompleteTextBox.ScoredString, int>) (scoredString => scoredString.Score));
        return (IList<string>) ((IEnumerable<AutoCompleteTextBox.ScoredString>) source).Where<AutoCompleteTextBox.ScoredString>((Func<AutoCompleteTextBox.ScoredString, bool>) (stringWithScore => stringWithScore.Score > 0 && maximalScore - stringWithScore.Score <= 1)).OrderBy<AutoCompleteTextBox.ScoredString, int>((Func<AutoCompleteTextBox.ScoredString, int>) (scoreString => scoreString.Score), (IComparer<int>) Comparer<int>.Create((Comparison<int>) ((first, second) => second.CompareTo(first)))).Take<AutoCompleteTextBox.ScoredString>(this.MaximumSuggestionCount).Select<AutoCompleteTextBox.ScoredString, string>((Func<AutoCompleteTextBox.ScoredString, string>) (stringWithScore => stringWithScore.Text)).ToList<string>();
      }
    }

    public class DamerauLevenshteinDistance : AutoCompleteTextBox.AutoCompletable
    {
      private AutoCompleteTextBox.ScoredString CalculateDistance(string first, string second)
      {
        int num1 = 1;
        int[,] numArray = new int[first.Length + 1, second.Length + 1];
        for (int index = 0; index < first.Length + 1; ++index)
          numArray[index, 0] = index;
        for (int index = 0; index < second.Length + 1; ++index)
          numArray[0, index] = index;
        for (int index1 = 1; index1 < first.Length + 1; ++index1)
        {
          for (int index2 = 1; index2 < second.Length + 1; ++index2)
          {
            char lower1 = char.ToLower(first[index1 - 1]);
            char lower2 = char.ToLower(second[index2 - 1]);
            int num2 = !lower1.Equals(lower2) ? 1 : 0;
            numArray[index1, index2] = Math.Min(numArray[index1 - 1, index2] + num1, Math.Min(numArray[index1, index2 - 1] + num1, numArray[index1 - 1, index2 - 1] + num2));
            if (index1 > 1 && index2 > 1 && (int) lower1 == (int) second[index2 - 2] && (int) first[index1 - 2] == (int) lower2)
              numArray[index1, index2] = Math.Min(numArray[index1, index2], numArray[index1 - 2, index2 - 2] + num2);
          }
        }
        return new AutoCompleteTextBox.ScoredString()
        {
          Text = second,
          Score = numArray[first.Length, second.Length]
        };
      }

      public override IList<string> GetSuggestedWords(
        string wordToSuggest,
        ICollection<string> suggestionDictionary)
      {
        IEnumerable<AutoCompleteTextBox.ScoredString> source = (IEnumerable<AutoCompleteTextBox.ScoredString>) ((IEnumerable<string>) suggestionDictionary).Select<string, AutoCompleteTextBox.ScoredString>((Func<string, AutoCompleteTextBox.ScoredString>) (suggestion => this.CalculateDistance(wordToSuggest, suggestion)));
        int maximalScore = ((IEnumerable<AutoCompleteTextBox.ScoredString>) source).Min<AutoCompleteTextBox.ScoredString>((Func<AutoCompleteTextBox.ScoredString, int>) (scoredString => scoredString.Score));
        IComparer<int> comparer = (IComparer<int>) Comparer<int>.Create((Comparison<int>) ((firstScore, secondScore) => firstScore.CompareTo(secondScore)));
        return (IList<string>) ((IEnumerable<AutoCompleteTextBox.ScoredString>) source).Where<AutoCompleteTextBox.ScoredString>((Func<AutoCompleteTextBox.ScoredString, bool>) (scoredString => scoredString.Score < wordToSuggest.Length && Math.Abs(maximalScore - scoredString.Score) <= 1)).OrderBy<AutoCompleteTextBox.ScoredString, int>((Func<AutoCompleteTextBox.ScoredString, int>) (scoredString => scoredString.Score), (IComparer<int>) comparer).Take<AutoCompleteTextBox.ScoredString>(this.MaximumSuggestionCount).Select<AutoCompleteTextBox.ScoredString, string>((Func<AutoCompleteTextBox.ScoredString, string>) (scoredString => scoredString.Text)).ToList<string>();
      }
    }

    public class PrefixSuggestion : AutoCompleteTextBox.AutoCompletable
    {
      private AutoCompleteTextBox.ScoredString GetSuggestionPrefixScore(
        string wordToSuggest,
        string suggestion)
      {
        int num1 = Math.Min(wordToSuggest.Length, suggestion.Length);
        AutoCompleteTextBox.ScoredString suggestionPrefixScore = new AutoCompleteTextBox.ScoredString()
        {
          Text = suggestion
        };
        int num2 = 0;
        for (int index = 0; index < num1 && (int) wordToSuggest[index] == (int) suggestion[index]; ++index)
          ++num2;
        suggestionPrefixScore.Score = num2;
        return suggestionPrefixScore;
      }

      public override IList<string> GetSuggestedWords(
        string wordToSuggest,
        ICollection<string> suggestionDictionary)
      {
        IEnumerable<AutoCompleteTextBox.ScoredString> source = (IEnumerable<AutoCompleteTextBox.ScoredString>) ((IEnumerable<string>) suggestionDictionary).Select<string, AutoCompleteTextBox.ScoredString>((Func<string, AutoCompleteTextBox.ScoredString>) (suggestion => this.GetSuggestionPrefixScore(wordToSuggest, suggestion)));
        int maximalScore = ((IEnumerable<AutoCompleteTextBox.ScoredString>) source).Max<AutoCompleteTextBox.ScoredString>((Func<AutoCompleteTextBox.ScoredString, int>) (scoredString => scoredString.Score));
        IComparer<int> comparer = (IComparer<int>) Comparer<int>.Create((Comparison<int>) ((firstScore, secondScore) => secondScore.CompareTo(firstScore)));
        return (IList<string>) ((IEnumerable<AutoCompleteTextBox.ScoredString>) source).Where<AutoCompleteTextBox.ScoredString>((Func<AutoCompleteTextBox.ScoredString, bool>) (scoredString => scoredString.Score != 0 && maximalScore - scoredString.Score <= 1)).OrderBy<AutoCompleteTextBox.ScoredString, int>((Func<AutoCompleteTextBox.ScoredString, int>) (scoredString => scoredString.Score), (IComparer<int>) comparer).Take<AutoCompleteTextBox.ScoredString>(this.MaximumSuggestionCount).Select<AutoCompleteTextBox.ScoredString, string>((Func<AutoCompleteTextBox.ScoredString, string>) (scoredString => scoredString.Text)).ToList<string>();
      }
    }

    public class ScoredString
    {
      public string Text { get; set; }

      public int Score { get; set; }
    }

    public enum SuggestionPopupDisplaySide
    {
      Top,
      Bottom,
    }

    public enum VisualControlState
    {
      Disabled,
      Normal,
      PointerOver,
    }
  }
}
