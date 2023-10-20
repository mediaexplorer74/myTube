// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Behaviors.HighlightBehavior
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Common;
using WinRTXamlToolkit.Interactivity;

namespace WinRTXamlToolkit.Controls.Behaviors
{
  public class HighlightBehavior : Behavior<TextBlock>
  {
    public static readonly DependencyProperty SearchStringProperty = DependencyProperty.Register(nameof (SearchString), (Type) typeof (string), (Type) typeof (HighlightBehavior), new PropertyMetadata((object) null, new PropertyChangedCallback(HighlightBehavior.OnSearchStringChanged)));
    public static readonly DependencyProperty IsCaseSensitiveProperty = DependencyProperty.Register(nameof (IsCaseSensitive), (Type) typeof (bool), (Type) typeof (HighlightBehavior), new PropertyMetadata((object) false, new PropertyChangedCallback(HighlightBehavior.OnIsCaseSensitiveChanged)));
    public static readonly DependencyProperty HighlightTemplateProperty = DependencyProperty.Register(nameof (HighlightTemplate), (Type) typeof (DataTemplate), (Type) typeof (HighlightBehavior), new PropertyMetadata((object) null, new PropertyChangedCallback(HighlightBehavior.OnHighlightTemplateChanged)));
    public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register(nameof (HighlightBrush), (Type) typeof (Brush), (Type) typeof (HighlightBehavior), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(HighlightBehavior.OnHighlightBrushChanged)));
    private PropertyChangeEventSource<string> _textChangeEventSource;

    public string SearchString
    {
      get => (string) ((DependencyObject) this).GetValue(HighlightBehavior.SearchStringProperty);
      set => ((DependencyObject) this).SetValue(HighlightBehavior.SearchStringProperty, (object) value);
    }

    private static void OnSearchStringChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HighlightBehavior highlightBehavior = (HighlightBehavior) d;
      string oldValue = (string) e.OldValue;
      string searchString = highlightBehavior.SearchString;
      highlightBehavior.OnSearchStringChanged(oldValue, searchString);
    }

    private void OnSearchStringChanged(string oldSearchString, string newSearchString) => this.UpdateHighlight();

    public bool IsCaseSensitive
    {
      get => (bool) ((DependencyObject) this).GetValue(HighlightBehavior.IsCaseSensitiveProperty);
      set => ((DependencyObject) this).SetValue(HighlightBehavior.IsCaseSensitiveProperty, (object) value);
    }

    private static void OnIsCaseSensitiveChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HighlightBehavior highlightBehavior = (HighlightBehavior) d;
      bool oldValue = (bool) e.OldValue;
      bool isCaseSensitive = highlightBehavior.IsCaseSensitive;
      highlightBehavior.OnIsCaseSensitiveChanged(oldValue, isCaseSensitive);
    }

    private void OnIsCaseSensitiveChanged(bool oldIsCaseSensitive, bool newIsCaseSensitive) => this.UpdateHighlight();

    public DataTemplate HighlightTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(HighlightBehavior.HighlightTemplateProperty);
      set => ((DependencyObject) this).SetValue(HighlightBehavior.HighlightTemplateProperty, (object) value);
    }

    private static void OnHighlightTemplateChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HighlightBehavior highlightBehavior = (HighlightBehavior) d;
      DataTemplate oldValue = (DataTemplate) e.OldValue;
      DataTemplate highlightTemplate = highlightBehavior.HighlightTemplate;
      highlightBehavior.OnHighlightTemplateChanged(oldValue, highlightTemplate);
    }

    private void OnHighlightTemplateChanged(
      DataTemplate oldHighlightTemplate,
      DataTemplate newHighlightTemplate)
    {
      this.UpdateHighlight();
    }

    public Brush HighlightBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(HighlightBehavior.HighlightBrushProperty);
      set => ((DependencyObject) this).SetValue(HighlightBehavior.HighlightBrushProperty, (object) value);
    }

    private static void OnHighlightBrushChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HighlightBehavior highlightBehavior = (HighlightBehavior) d;
      Brush oldValue = (Brush) e.OldValue;
      Brush highlightBrush = highlightBehavior.HighlightBrush;
      highlightBehavior.OnHighlightBrushChanged(oldValue, highlightBrush);
    }

    private void OnHighlightBrushChanged(Brush oldHighlightBrush, Brush newHighlightBrush) => this.UpdateHighlight();

    protected override void OnAttached()
    {
      this.UpdateHighlight();
      this._textChangeEventSource = new PropertyChangeEventSource<string>((DependencyObject) this.AssociatedObject, "Text", (BindingMode) 1);
      this._textChangeEventSource.ValueChanged += new EventHandler<string>(this.TextChanged);
      base.OnAttached();
    }

    protected override void OnDetaching()
    {
      this.ClearHighlight();
      this._textChangeEventSource.ValueChanged -= new EventHandler<string>(this.TextChanged);
      this._textChangeEventSource = (PropertyChangeEventSource<string>) null;
      base.OnDetaching();
    }

    private void TextChanged(object sender, string s) => this.UpdateHighlight();

    public void UpdateHighlight()
    {
      if (this.AssociatedObject == null || string.IsNullOrEmpty(this.AssociatedObject.Text) || string.IsNullOrEmpty(this.SearchString))
      {
        this.ClearHighlight();
      }
      else
      {
        string text = this.AssociatedObject.Text;
        string searchString = this.SearchString;
        int startIndex1 = 0;
        ((ICollection<Inline>) this.AssociatedObject.Inlines).Clear();
        while (true)
        {
          string str1 = text;
          string str2 = searchString;
          int startIndex2 = startIndex1;
          int comparisonType = this.IsCaseSensitive ? 0 : 1;
          int startIndex3;
          if ((startIndex3 = str1.IndexOf(str2, startIndex2, (StringComparison) comparisonType)) >= 0)
          {
            if (startIndex3 > startIndex1)
            {
              Run run = new Run();
              run.put_Text(text.Substring(startIndex1, startIndex3 - startIndex1));
              ((ICollection<Inline>) this.AssociatedObject.Inlines).Add((Inline) run);
            }
            string str3 = text.Substring(startIndex3, searchString.Length);
            Run run1;
            if (this.HighlightTemplate == null)
            {
              Run run2 = new Run();
              run2.put_Text(str3);
              ((TextElement) run2).put_Foreground(this.HighlightBrush);
              run1 = run2;
            }
            else
            {
              run1 = (Run) this.HighlightTemplate.LoadContent();
              run1.put_Text(str3);
            }
            ((ICollection<Inline>) this.AssociatedObject.Inlines).Add((Inline) run1);
            startIndex1 = startIndex3 + searchString.Length;
          }
          else
            break;
        }
        if (startIndex1 >= text.Length)
          return;
        Run run3 = new Run();
        run3.put_Text(text.Substring(startIndex1, text.Length - startIndex1));
        ((ICollection<Inline>) this.AssociatedObject.Inlines).Add((Inline) run3);
      }
    }

    public void ClearHighlight()
    {
      if (this.AssociatedObject == null)
        return;
      string text = this.AssociatedObject.Text;
      ((ICollection<Inline>) this.AssociatedObject.Inlines).Clear();
      InlineCollection inlines = this.AssociatedObject.Inlines;
      Run run1 = new Run();
      run1.put_Text(text);
      Run run2 = run1;
      ((ICollection<Inline>) inlines).Add((Inline) run2);
    }
  }
}
