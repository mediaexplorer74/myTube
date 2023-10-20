// Decompiled with JetBrains decompiler
// Type: myTube.TestPages.TestListPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TestFramework;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace myTube.TestPages
{
  public sealed class TestListPage : Page, IComponentConnector
  {
    private Stack<object> backStack = new Stack<object>();
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer logScroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock testLog;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl runAllButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl goUpButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl itemsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public TestListPage()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(TestListPage_DataContextChanged)));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.TestListPage_Loaded));
    }

    private void TestListPage_Loaded(object sender, RoutedEventArgs e) => Test.Logged += new EventHandler<TestLoggedEventArgs>(this.Test_Logged);

    private async void Test_Logged(object sender, TestLoggedEventArgs e)
    {
      Run run1 = new Run();
      run1.put_Text(e.Text);
      Run run2 = run1;
      switch (e.Type)
      {
        case LogType.Error:
          ((TextElement) run2).put_FontWeight(FontWeights.Normal);
          ((TextElement) run2).put_Foreground((Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 60, (byte) 60)));
          break;
        case LogType.Warning:
          ((TextElement) run2).put_Foreground((Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 220, (byte) 30)));
          break;
        case LogType.Success:
          ((TextElement) run2).put_FontWeight(FontWeights.Bold);
          ((TextElement) run2).put_Foreground((Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 30, byte.MaxValue, (byte) 30)));
          break;
        case LogType.Failure:
          ((TextElement) run2).put_FontWeight(FontWeights.Bold);
          ((TextElement) run2).put_Foreground((Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 0, (byte) 0)));
          break;
      }
      ((ICollection<Inline>) this.testLog.Inlines).Add((Inline) new LineBreak());
      ((ICollection<Inline>) this.testLog.Inlines).Add((Inline) run2);
      ((UIElement) this.logScroll).UpdateLayout();
      this.logScroll.ChangeView(new double?(), new double?(this.logScroll.ScrollableHeight), new float?());
    }

    private void TestListPage_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (((FrameworkElement) this).DataContext == null)
      {
        ((ICollection<object>) this.itemsControl.Items).Clear();
        Assembly assembly = Assembly.Load(new AssemblyName("myTube.WindowsPhone"));
        try
        {
          foreach (TypeInfo definedType in assembly.DefinedTypes)
          {
            try
            {
              if (definedType.Namespace != null)
              {
                if (definedType.Namespace.Contains("Tests.Backend"))
                {
                  Type type = definedType.AsType();
                  if (Test.GetTestMethodInfo(type).Count > 0)
                    ((ICollection<object>) this.itemsControl.Items).Add((object) type);
                }
              }
            }
            catch
            {
            }
          }
        }
        catch
        {
        }
      }
      else
      {
        if (!(((FrameworkElement) this).DataContext is Type))
          return;
        Type dataContext = ((FrameworkElement) this).DataContext as Type;
        ((ICollection<object>) this.itemsControl.Items).Clear();
        using (List<TestMethodInfo>.Enumerator enumerator = Test.GetTestMethodInfo(dataContext).GetEnumerator())
        {
          while (enumerator.MoveNext())
            ((ICollection<object>) this.itemsControl.Items).Add((object) enumerator.Current);
        }
      }
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      ((FrameworkElement) this).put_DataContext(e.Parameter);
      base.OnNavigatedTo(e);
    }

    private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement))
        return;
      if (frameworkElement.DataContext is TestMethodInfo)
      {
        this.RunTest(frameworkElement.DataContext as TestMethodInfo);
      }
      else
      {
        if (!(frameworkElement.DataContext is Type))
          return;
        this.ChangeDataContext(frameworkElement.DataContext);
      }
    }

    private void ChangeDataContext(object con)
    {
      this.backStack.Push(((FrameworkElement) this).DataContext);
      ((FrameworkElement) this).put_DataContext(con);
    }

    private void goUpButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      try
      {
        ((FrameworkElement) this).put_DataContext(this.backStack.Pop());
      }
      catch
      {
        ((FrameworkElement) this).put_DataContext((object) null);
      }
    }

    private async Task RunTest(Type type)
    {
      ((ICollection<Inline>) this.testLog.Inlines).Clear();
      TestResult testResult = await Test.Run(type);
      ((FrameworkElement) this).put_DataContext(((FrameworkElement) this).DataContext);
    }

    private async Task RunTest(TestMethodInfo info)
    {
      ((ICollection<Inline>) this.testLog.Inlines).Clear();
      this.overCanvas.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) this.logScroll), false);
      try
      {
        TestResult testResult = await Test.Run(info);
        ((FrameworkElement) this).put_DataContext(((FrameworkElement) this).DataContext);
      }
      catch
      {
      }
    }

    private async void runAllButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      ((ICollection<Inline>) this.testLog.Inlines).Clear();
      this.overCanvas.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) this.logScroll), false);
      try
      {
        TestResult testResult = await Test.Run(Enumerable.ToArray<object>((IEnumerable<object>) this.itemsControl.Items));
      }
      catch
      {
      }
    }

    private void TextBlock_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      TextBlock textBlock = sender as TextBlock;
      if (sender == null)
        return;
      object newValue = args.NewValue;
      if (newValue == null)
        return;
      if (newValue is Type)
        textBlock.put_Text((newValue as Type).Name);
      else
        textBlock.put_Text(newValue.ToString());
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///TestPages/TestListPage.xaml"), (ComponentResourceLocation) 0);
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.logScroll = (ScrollViewer) ((FrameworkElement) this).FindName("logScroll");
      this.testLog = (TextBlock) ((FrameworkElement) this).FindName("testLog");
      this.runAllButton = (ContentControl) ((FrameworkElement) this).FindName("runAllButton");
      this.goUpButton = (ContentControl) ((FrameworkElement) this).FindName("goUpButton");
      this.itemsControl = (ItemsControl) ((FrameworkElement) this).FindName("itemsControl");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.Grid_Tapped_1));
          break;
        case 2:
          FrameworkElement frameworkElement = (FrameworkElement) target;
          // ISSUE: method pointer
          WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(frameworkElement.add_DataContextChanged), new Action<EventRegistrationToken>(frameworkElement.remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(TextBlock_DataContextChanged)));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.runAllButton_Tapped));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.goUpButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
