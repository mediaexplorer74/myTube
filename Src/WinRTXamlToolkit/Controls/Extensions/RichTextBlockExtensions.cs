// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.RichTextBlockExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class RichTextBlockExtensions
  {
    public static readonly DependencyProperty PlainTextProperty = DependencyProperty.RegisterAttached("PlainText", (Type) typeof (string), (Type) typeof (RichTextBlockExtensions), new PropertyMetadata((object) "", new PropertyChangedCallback(RichTextBlockExtensions.OnPlainTextChanged)));
    public static readonly DependencyProperty LinkedHtmlFragmentProperty = DependencyProperty.RegisterAttached("LinkedHtmlFragment", (Type) typeof (string), (Type) typeof (RichTextBlockExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(RichTextBlockExtensions.OnLinkedHtmlFragmentChanged)));

    public static string GetPlainText(DependencyObject d) => (string) d.GetValue(RichTextBlockExtensions.PlainTextProperty);

    public static void SetPlainText(DependencyObject d, string value) => d.SetValue(RichTextBlockExtensions.PlainTextProperty, (object) value);

    private static void OnPlainTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      string oldValue = (string) e.OldValue;
      string str = (string) d.GetValue(RichTextBlockExtensions.PlainTextProperty);
      ((ICollection<Block>) ((RichTextBlock) d).Blocks).Clear();
      Paragraph paragraph = new Paragraph();
      InlineCollection inlines = paragraph.Inlines;
      Run run1 = new Run();
      run1.put_Text(str);
      Run run2 = run1;
      ((ICollection<Inline>) inlines).Add((Inline) run2);
      ((ICollection<Block>) ((RichTextBlock) d).Blocks).Add((Block) paragraph);
    }

    public static string GetLinkedHtmlFragment(DependencyObject d) => (string) d.GetValue(RichTextBlockExtensions.LinkedHtmlFragmentProperty);

    public static void SetLinkedHtmlFragment(DependencyObject d, string value) => d.SetValue(RichTextBlockExtensions.LinkedHtmlFragmentProperty, (object) value);

    private static void OnLinkedHtmlFragmentChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      string oldValue = (string) e.OldValue;
      string htmlFragment = (string) d.GetValue(RichTextBlockExtensions.LinkedHtmlFragmentProperty);
      ((RichTextBlock) d).SetLinkedHtmlFragmentString(htmlFragment);
    }

    public static void SetLinkedHtmlFragmentString(
      this RichTextBlock richTextBlock,
      string htmlFragment)
    {
      ((ICollection<Block>) richTextBlock.Blocks).Clear();
      if (string.IsNullOrEmpty(htmlFragment))
        return;
      Regex regex = new Regex("\\<a\\s(href\\=\"|[^\\>]+?\\shref\\=\")(?<link>[^\"]+)\".*?\\>(?<text>.*?)(\\<\\/a\\>|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
      int startIndex = 0;
      IEnumerator enumerator = (IEnumerator) regex.Matches(htmlFragment).GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Match current = (Match) enumerator.Current;
          if (current.Index > startIndex)
          {
            richTextBlock.AppendText(htmlFragment.Substring(startIndex, current.Index - startIndex));
            startIndex = current.Index + current.Length;
            richTextBlock.AppendLink(current.Groups["text"].Value, new Uri(current.Groups["link"].Value));
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      if (startIndex >= htmlFragment.Length)
        return;
      richTextBlock.AppendText(htmlFragment.Substring(startIndex));
    }

    public static void AppendText(this RichTextBlock richTextBlock, string text)
    {
      if (((ICollection<Block>) richTextBlock.Blocks).Count == 0 || !(((IList<Block>) richTextBlock.Blocks)[((ICollection<Block>) richTextBlock.Blocks).Count - 1] is Paragraph paragraph))
      {
        paragraph = new Paragraph();
        ((ICollection<Block>) richTextBlock.Blocks).Add((Block) paragraph);
      }
      InlineCollection inlines = paragraph.Inlines;
      Run run1 = new Run();
      run1.put_Text(text);
      Run run2 = run1;
      ((ICollection<Inline>) inlines).Add((Inline) run2);
    }

    public static void AppendLink(this RichTextBlock richTextBlock, string text, Uri uri)
    {
      if (((ICollection<Block>) richTextBlock.Blocks).Count == 0 || !(((IList<Block>) richTextBlock.Blocks)[((ICollection<Block>) richTextBlock.Blocks).Count - 1] is Paragraph paragraph))
      {
        paragraph = new Paragraph();
        ((ICollection<Block>) richTextBlock.Blocks).Add((Block) paragraph);
      }
      HyperlinkButton hyperlinkButton1 = new HyperlinkButton();
      ((ContentControl) hyperlinkButton1).put_Content((object) text);
      hyperlinkButton1.put_NavigateUri((Uri) uri);
      HyperlinkButton hyperlinkButton2 = hyperlinkButton1;
      InlineCollection inlines = paragraph.Inlines;
      InlineUIContainer inlineUiContainer1 = new InlineUIContainer();
      inlineUiContainer1.put_Child((UIElement) hyperlinkButton2);
      InlineUIContainer inlineUiContainer2 = inlineUiContainer1;
      ((ICollection<Inline>) inlines).Add((Inline) inlineUiContainer2);
    }
  }
}
