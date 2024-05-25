// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.TextBlockExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class TextBlockExtensions
  {
    public static Rect GetCharacterRect(this TextBlock tb, int characterIndex)
    {
      double actualWidth = ((FrameworkElement) tb).ActualWidth;
      double actualHeight = ((FrameworkElement) tb).ActualHeight;
      int num1 = 0;
      bool flag = false;
      Rect rect = new Rect(-100000.0, 0.0, 0.0, 0.0);
      Rect characterRect = (Rect) tb.ContentStart.GetPositionAtOffset(num1, (LogicalDirection) 1).GetCharacterRect((LogicalDirection) 1);
      for (int index = 0; index <= characterIndex; ++index)
      {
        for (; characterRect == rect || characterRect.X - rect.X < 4.0; characterRect = (Rect) tb.ContentStart.GetPositionAtOffset(num1, (LogicalDirection) 1).GetCharacterRect((LogicalDirection) 1))
        {
          ++num1;
          if (num1 > tb.ContentEnd.Offset)
            break;
        }
        rect = characterRect;
        if (index == characterIndex)
        {
          for (; characterRect == rect || characterRect.X - rect.X < 4.0; characterRect = (Rect) tb.ContentStart.GetPositionAtOffset(num1, (LogicalDirection) 1).GetCharacterRect((LogicalDirection) 1))
          {
            ++num1;
            if (num1 > tb.ContentEnd.Offset)
              break;
          }
          int num2 = num1 + 1;
          double x = rect.X;
          double y = rect.Y;
          double width = Math.Max(characterRect.X - rect.X, 0.0);
          double height = rect.Height;
          double num3 = (x + width / 2.0) / actualWidth;
          if (!flag)
            ;
          return new Rect(x, y, width, height);
        }
      }
      return new Rect();
    }
  }
}
