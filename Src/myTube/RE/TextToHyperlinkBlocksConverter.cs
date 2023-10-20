// Decompiled with JetBrains decompiler
// Type: myTube.TextToHyperlinkBlocksConverter
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace myTube
{
  public class TextToHyperlinkBlocksConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language) => (object) this.Convert(value.ToString());

    public Paragraph Convert(string value)
    {
      Paragraph paragraph = new Paragraph();
      string str1 = "";
      bool flag = false;
      int startIndex = 0;
      for (int index = 0; index < value.Length; ++index)
      {
        char ch = value[index];
        switch (ch)
        {
          case '\n':
          case ' ':
          case '(':
          case ')':
            string url = value.Substring(startIndex, index - startIndex);
            if (!flag)
            {
              int num;
              if ((num = url.IndexOf("http:")) != -1 || (num = url.IndexOf("https:")) != -1 || (num = url.IndexOf("www")) != -1)
              {
                string str2 = url.Substring(0, num);
                InlineCollection inlines = paragraph.Inlines;
                Run run = new Run();
                run.put_Text(str2);
                ((ICollection<Inline>) inlines).Add((Inline) run);
                url = url.Substring(num);
                flag = true;
              }
              else
                break;
            }
            startIndex = index + 1;
            if (!string.IsNullOrWhiteSpace(url))
            {
              if (!flag)
              {
                InlineCollection inlines = paragraph.Inlines;
                Run run = new Run();
                run.put_Text(url);
                ((ICollection<Inline>) inlines).Add((Inline) run);
              }
              else
              {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                TextToHyperlinkBlocksConverter.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10 = new TextToHyperlinkBlocksConverter.\u003C\u003Ec__DisplayClass1_0();
                Hyperlink hyperlink1 = new Hyperlink();
                // ISSUE: reference to a compiler-generated field
                cDisplayClass10.linkInfo = YouTubeURLHelper.GetUrlType(url);
                // ISSUE: reference to a compiler-generated field
                if (cDisplayClass10.linkInfo.Type != YouTubeURLType.None)
                {
                  Hyperlink hyperlink2 = hyperlink1;
                  // ISSUE: method pointer
                  WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>>(new Func<TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>, EventRegistrationToken>(hyperlink2.add_Click), new Action<EventRegistrationToken>(hyperlink2.remove_Click), new TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>((object) cDisplayClass10, __methodptr(\u003CConvert\u003Eb__0)));
                  ((TextElement) hyperlink1).put_Foreground((Brush) App.Instance.GetThemeResource("SubtleBrush"));
                  InlineCollection inlines = ((Span) hyperlink1).Inlines;
                  Run run = new Run();
                  run.put_Text(url);
                  ((ICollection<Inline>) inlines).Add((Inline) run);
                  ((ICollection<Inline>) paragraph.Inlines).Add((Inline) hyperlink1);
                }
                else
                {
                  try
                  {
                    try
                    {
                      hyperlink1.put_NavigateUri(new Uri(url.IndexOf("www.") == 0 ? "http://" + url : url, UriKind.Absolute));
                      ((TextElement) hyperlink1).put_Foreground((Brush) App.Instance.GetThemeResource("SubtleBrush"));
                      InlineCollection inlines = ((Span) hyperlink1).Inlines;
                      Run run = new Run();
                      run.put_Text(url);
                      ((ICollection<Inline>) inlines).Add((Inline) run);
                      ((ICollection<Inline>) paragraph.Inlines).Add((Inline) hyperlink1);
                    }
                    catch
                    {
                      InlineCollection inlines = paragraph.Inlines;
                      Run run = new Run();
                      run.put_Text(url);
                      ((ICollection<Inline>) inlines).Add((Inline) run);
                    }
                  }
                  catch
                  {
                  }
                }
                flag = false;
              }
            }
            str1 = "";
            InlineCollection inlines1 = paragraph.Inlines;
            Run run1 = new Run();
            run1.put_Text(ch.ToString());
            ((ICollection<Inline>) inlines1).Add((Inline) run1);
            break;
        }
      }
      string str3 = value.Substring(startIndex);
      if (!string.IsNullOrWhiteSpace(str3))
      {
        if (!flag)
        {
          InlineCollection inlines = paragraph.Inlines;
          Run run = new Run();
          run.put_Text(str3);
          ((ICollection<Inline>) inlines).Add((Inline) run);
        }
        else
        {
          try
          {
            Hyperlink hyperlink3 = new Hyperlink();
            hyperlink3.put_NavigateUri(new Uri(str3.IndexOf("www.") == 0 ? "http://" + str3 : str3, UriKind.Absolute));
            Hyperlink hyperlink4 = hyperlink3;
            InlineCollection inlines = ((Span) hyperlink4).Inlines;
            Run run = new Run();
            run.put_Text(str3);
            ((ICollection<Inline>) inlines).Add((Inline) run);
            ((TextElement) hyperlink4).put_Foreground((Brush) App.Instance.GetThemeResource("SubtleBrush"));
            ((ICollection<Inline>) paragraph.Inlines).Add((Inline) hyperlink4);
          }
          catch
          {
            InlineCollection inlines = paragraph.Inlines;
            Run run = new Run();
            run.put_Text(str3);
            ((ICollection<Inline>) inlines).Add((Inline) run);
          }
        }
      }
      return paragraph;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
