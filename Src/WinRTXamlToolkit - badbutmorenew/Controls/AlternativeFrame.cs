// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.AlternativeFrame
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "PART_PagePresentersPanel", Type = typeof (Panel))]
  [StyleTypedProperty(Property = "PagePresenterStyle", StyleTargetType = typeof (ContentPresenter))]
  public class AlternativeFrame : ContentControl
  {
    private const int WaitForImagesToLoadTimeout = 1000;
    private const int DefaultCacheSize = 10;
    private const string PagePresentersPanelName = "PART_PagePresentersPanel";
    private readonly TaskCompletionSource<bool> _waitForApplyTemplateTaskSource = new TaskCompletionSource<bool>((object) false);
    private readonly Dictionary<JournalEntry, ContentPresenter> _preloadedPageCache;
    private readonly FrameCache _frameCache = new FrameCache(10);
    private ContentPresenter _currentPagePresenter;
    private Panel _pagePresentersPanel;
    private bool _isNavigating;
    public static readonly DependencyProperty CacheSizeProperty = DependencyProperty.Register(nameof (CacheSize), (Type) typeof (int), (Type) typeof (AlternativeFrame), new PropertyMetadata((object) 10, new PropertyChangedCallback(AlternativeFrame.OnCacheSizeChanged)));
    public static readonly DependencyProperty PagePresenterStyleProperty = DependencyProperty.Register(nameof (PagePresenterStyle), (Type) typeof (Style), (Type) typeof (AlternativeFrame), new PropertyMetadata((object) null, new PropertyChangedCallback(AlternativeFrame.OnPagePresenterStyleChanged)));
    public static readonly DependencyProperty PageTransitionProperty = DependencyProperty.Register(nameof (PageTransition), (Type) typeof (PageTransition), (Type) typeof (AlternativeFrame), new PropertyMetadata((object) null));
    public static readonly DependencyProperty CanGoBackProperty = DependencyProperty.Register(nameof (CanGoBack), (Type) typeof (bool), (Type) typeof (AlternativeFrame), new PropertyMetadata((object) false));
    public static readonly DependencyProperty CanGoForwardProperty = DependencyProperty.Register(nameof (CanGoForward), (Type) typeof (bool), (Type) typeof (AlternativeFrame), new PropertyMetadata((object) false));
    public static readonly DependencyProperty CanNavigateProperty = DependencyProperty.Register(nameof (CanNavigate), (Type) typeof (bool), (Type) typeof (AlternativeFrame), new PropertyMetadata((object) true));
    public static readonly DependencyProperty ShouldWaitForImagesToLoadProperty = DependencyProperty.Register(nameof (ShouldWaitForImagesToLoad), (Type) typeof (bool?), (Type) typeof (AlternativeFrame), new PropertyMetadata((object) null));

    public event AlternativeNavigationEventHandler Navigated;

    public event AlternativeNavigatingCancelEventHandler Navigating;

    public Stack<JournalEntry> BackStack { get; private set; }

    public JournalEntry CurrentJournalEntry { get; private set; }

    public Stack<JournalEntry> ForwardStack { get; private set; }

    public Type CurrentSourcePageType => this.CurrentJournalEntry.SourcePageType;

    public int CacheSize
    {
      get => (int) ((DependencyObject) this).GetValue(AlternativeFrame.CacheSizeProperty);
      set => ((DependencyObject) this).SetValue(AlternativeFrame.CacheSizeProperty, (object) value);
    }

    private static void OnCacheSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      AlternativeFrame alternativeFrame = (AlternativeFrame) d;
      int oldValue = (int) e.OldValue;
      int cacheSize = alternativeFrame.CacheSize;
      alternativeFrame.OnCacheSizeChanged(oldValue, cacheSize);
    }

    private void OnCacheSizeChanged(int oldCacheSize, int newCacheSize) => this._frameCache.CacheSize = newCacheSize;

    public Style PagePresenterStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(AlternativeFrame.PagePresenterStyleProperty);
      set => ((DependencyObject) this).SetValue(AlternativeFrame.PagePresenterStyleProperty, (object) value);
    }

    private static void OnPagePresenterStyleChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      AlternativeFrame alternativeFrame = (AlternativeFrame) d;
      Style oldValue = (Style) e.OldValue;
      Style pagePresenterStyle = alternativeFrame.PagePresenterStyle;
      alternativeFrame.OnPagePresenterStyleChanged(oldValue, pagePresenterStyle);
    }

    protected virtual void OnPagePresenterStyleChanged(
      Style oldPagePresenterStyle,
      Style newPagePresenterStyle)
    {
    }

    public PageTransition PageTransition
    {
      get => (PageTransition) ((DependencyObject) this).GetValue(AlternativeFrame.PageTransitionProperty);
      set => ((DependencyObject) this).SetValue(AlternativeFrame.PageTransitionProperty, (object) value);
    }

    public bool CanGoBack
    {
      get => (bool) ((DependencyObject) this).GetValue(AlternativeFrame.CanGoBackProperty);
      private set => ((DependencyObject) this).SetValue(AlternativeFrame.CanGoBackProperty, (object) value);
    }

    public bool CanGoForward
    {
      get => (bool) ((DependencyObject) this).GetValue(AlternativeFrame.CanGoForwardProperty);
      private set => ((DependencyObject) this).SetValue(AlternativeFrame.CanGoForwardProperty, (object) value);
    }

    public bool CanNavigate
    {
      get => (bool) ((DependencyObject) this).GetValue(AlternativeFrame.CanNavigateProperty);
      private set => ((DependencyObject) this).SetValue(AlternativeFrame.CanNavigateProperty, (object) value);
    }

    public bool? ShouldWaitForImagesToLoad
    {
      get => (bool?) ((DependencyObject) this).GetValue(AlternativeFrame.ShouldWaitForImagesToLoadProperty);
      set => ((DependencyObject) this).SetValue(AlternativeFrame.ShouldWaitForImagesToLoadProperty, (object) value);
    }

    public int BackStackDepth => this.BackStack.Count;

    public int ForwardStackDepth => this.ForwardStack.Count;

    public AlternativeFrame()
    {
      this.BackStack = new Stack<JournalEntry>();
      this.ForwardStack = new Stack<JournalEntry>();
      this._preloadedPageCache = new Dictionary<JournalEntry, ContentPresenter>();
      ((Control) this).put_DefaultStyleKey((object) typeof (AlternativeFrame));
    }

    public async Task<bool> Preload(Type sourcePageType, object parameter)
    {
      JournalEntry je = new JournalEntry()
      {
        SourcePageType = sourcePageType,
        Parameter = parameter
      };
      if (this._preloadedPageCache.ContainsKey(je))
        return true;
      ContentPresenter contentPresenter = new ContentPresenter();
      ((FrameworkElement) contentPresenter).put_Style(this.PagePresenterStyle);
      ContentPresenter cp = contentPresenter;
      AlternativePage newPage = this._frameCache.Get(sourcePageType);
      if (newPage == null)
        return false;
      newPage.Frame = this;
      cp.put_Content((object) newPage);
      ((UIElement) cp).put_Opacity(0.005);
      Canvas.SetZIndex((UIElement) cp, int.MinValue);
      ((IList<UIElement>) this._pagePresentersPanel.Children).Insert(0, (UIElement) cp);
      this._preloadedPageCache.Add(je, cp);
      await newPage.PreloadInternal(parameter);
      ((UIElement) cp).put_Opacity(0.0);
      return true;
    }

    public async Task UnloadPreloaded(Type sourcePageType, object parameter)
    {
      JournalEntry je = new JournalEntry()
      {
        SourcePageType = sourcePageType,
        Parameter = parameter
      };
      if (!this._preloadedPageCache.ContainsKey(je))
        return;
      ContentPresenter cp = this._preloadedPageCache[je];
      AlternativePage page = (AlternativePage) cp.Content;
      await page.UnloadPreloadedInternal();
      this._frameCache.Store(page);
      ((ICollection<UIElement>) this._pagePresentersPanel.Children).Remove((UIElement) cp);
      this._preloadedPageCache.Remove(je);
    }

    public async Task UnloadAllPreloaded()
    {
      foreach (KeyValuePair<JournalEntry, ContentPresenter> kvp in this._preloadedPageCache)
      {
        ((ICollection<UIElement>) this._pagePresentersPanel.Children).Remove((UIElement) kvp.Value);
        AlternativePage page = (AlternativePage) kvp.Value.Content;
        await page.UnloadPreloadedInternal();
        this._frameCache.Store(page);
      }
      this._preloadedPageCache.Clear();
    }

    public async Task<bool> Navigate(Type sourcePageType) => await this.Navigate(sourcePageType, (object) null);

    public async Task<bool> Navigate(Type sourcePageType, object parameter)
    {
      if (this._isNavigating)
        throw new InvalidOperationException("Navigation already in progress.");
      if (!this.CanNavigate)
        throw new InvalidOperationException("Navigate() call failed. CanNavigate is false.");
      return await this.NavigateCore(sourcePageType, parameter, (NavigationMode) 0);
    }

    public async Task<bool> GoBack()
    {
      if (this._isNavigating)
        throw new InvalidOperationException("Navigation already in progress.");
      if (!this.CanGoBack)
        throw new InvalidOperationException("GoBack() call failed. CanGoBack is false.");
      if (!this.CanNavigate)
        throw new InvalidOperationException("GoBack() call failed. CanNavigate is false.");
      JournalEntry backJournalEntry = this.BackStack.Peek();
      return await this.NavigateCore(backJournalEntry.SourcePageType, backJournalEntry.Parameter, (NavigationMode) 1);
    }

    public async Task<bool> GoForward()
    {
      if (this._isNavigating)
        throw new InvalidOperationException("Navigation already in progress.");
      if (!this.CanGoForward)
        throw new InvalidOperationException("GoForward() call failed. CanGoForward is false.");
      if (!this.CanNavigate)
        throw new InvalidOperationException("GoForward() call failed. CanNavigate is false.");
      JournalEntry forwardJournalEntry = this.ForwardStack.Peek();
      return await this.NavigateCore(forwardJournalEntry.SourcePageType, forwardJournalEntry.Parameter, (NavigationMode) 2);
    }

    public async Task SetNavigationState(string navigationState)
    {
      try
      {
        int parseIndex = 2;
        int totalPages = AlternativeFrame.ParseNumber(navigationState, ref parseIndex);
        this.BackStack.Clear();
        this.ForwardStack.Clear();
        this.CurrentJournalEntry = (JournalEntry) null;
        if (this._currentPagePresenter != null)
        {
          if (this._currentPagePresenter.Content is AlternativePage content)
            this._frameCache.Store(content);
          ((ICollection<UIElement>) this._pagePresentersPanel.Children).Remove((UIElement) this._currentPagePresenter);
          this._currentPagePresenter = (ContentPresenter) null;
        }
        if (totalPages == 0)
        {
          this.CurrentJournalEntry = (JournalEntry) null;
          await this.UnloadAllPreloaded();
        }
        else
        {
          int backStackDepth = AlternativeFrame.ParseNumber(navigationState, ref parseIndex);
          List<JournalEntry> reversedForwardStack = new List<JournalEntry>(totalPages - backStackDepth);
          for (int index = 0; index < totalPages; ++index)
          {
            JournalEntry journalEntry = this.ParseJournalEntry(navigationState, ref parseIndex);
            if (index < backStackDepth)
              this.BackStack.Push(journalEntry);
            else
              reversedForwardStack.Add(journalEntry);
          }
          reversedForwardStack.Reverse();
          using (List<JournalEntry>.Enumerator enumerator = reversedForwardStack.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ForwardStack.Push(enumerator.Current);
          }
          JournalEntry forwardJournalEntry = this.ForwardStack.Peek();
          int num = await this.NavigateCore(forwardJournalEntry.SourcePageType, forwardJournalEntry.Parameter, (NavigationMode) 2) ? 1 : 0;
        }
      }
      catch (Exception ex)
      {
        throw new ArgumentException("Could not deserialize frame navigation state.", nameof (navigationState), ex);
      }
    }

    private JournalEntry ParseJournalEntry(string parsedString, ref int parseIndex)
    {
      int number1 = AlternativeFrame.ParseNumber(parsedString, ref parseIndex);
      Type type = Type.GetType(parsedString.Substring(parseIndex, number1));
      parseIndex += number1 + 1;
      int number2 = AlternativeFrame.ParseNumber(parsedString, ref parseIndex);
      if (number2 == 0)
        return new JournalEntry() { SourcePageType = type };
      int number3 = AlternativeFrame.ParseNumber(parsedString, ref parseIndex);
      string str = parsedString.Substring(parseIndex, number3);
      parseIndex += number3 + 1;
      object obj;
      switch (number2 - 1)
      {
        case 0:
          obj = (object) byte.Parse(str);
          break;
        case 1:
          obj = (object) short.Parse(str);
          break;
        case 2:
          obj = (object) ushort.Parse(str);
          break;
        case 3:
          obj = (object) int.Parse(str);
          break;
        case 4:
          obj = (object) uint.Parse(str);
          break;
        case 5:
          obj = (object) long.Parse(str);
          break;
        case 6:
          obj = (object) ulong.Parse(str);
          break;
        case 7:
          obj = (object) float.Parse(str);
          break;
        case 8:
          obj = (object) double.Parse(str);
          break;
        case 9:
          obj = (object) str[0];
          break;
        case 10:
          obj = (object) bool.Parse(str);
          break;
        case 11:
          obj = (object) str;
          break;
        case 15:
          obj = (object) Guid.Parse(str);
          break;
        default:
          throw new ArgumentException("Parsing JournalEntry failed - unknown parameter type.");
      }
      return new JournalEntry()
      {
        SourcePageType = type,
        Parameter = obj
      };
    }

    private static int ParseNumber(string parsedString, ref int parseIndex)
    {
      int num1 = parsedString.IndexOf(',', parseIndex);
      int num2 = num1 >= 0 ? num1 : parsedString.Length;
      string s = parsedString.Substring(parseIndex, num2 - parseIndex);
      parseIndex = num2 + 1;
      return int.Parse(s);
    }

    public string GetNavigationState()
    {
      int num = this.BackStack.Count + this.ForwardStack.Count;
      if (this.CurrentJournalEntry != null)
        ++num;
      if (num == 0)
        return "1,0";
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("1,{0},{1}", (object) num, (object) this.BackStackDepth);
      foreach (JournalEntry journalEntry in (IEnumerable<JournalEntry>) this.BackStack.Reverse<JournalEntry>())
        this.AppendEntryToNavigationStateString(sb, journalEntry);
      if (this.CurrentJournalEntry != null)
        this.AppendEntryToNavigationStateString(sb, this.CurrentJournalEntry);
      using (Stack<JournalEntry>.Enumerator enumerator = this.ForwardStack.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          JournalEntry current = enumerator.Current;
          this.AppendEntryToNavigationStateString(sb, current);
        }
      }
      return sb.ToString();
    }

    private void AppendEntryToNavigationStateString(StringBuilder sb, JournalEntry journalEntry)
    {
      string assemblyQualifiedName = journalEntry.SourcePageType.AssemblyQualifiedName;
      sb.AppendFormat(",{0},{1}", (object) assemblyQualifiedName.Length, (object) assemblyQualifiedName);
      if (journalEntry.Parameter == null)
      {
        sb.Append(",0");
      }
      else
      {
        string valueString = journalEntry.Parameter.ToString();
        if (journalEntry.Parameter is byte)
          this.AddParameterToNavigationStateString(sb, 1, valueString);
        else if (journalEntry.Parameter is short)
          this.AddParameterToNavigationStateString(sb, 2, valueString);
        else if (journalEntry.Parameter is ushort)
          this.AddParameterToNavigationStateString(sb, 3, valueString);
        else if (journalEntry.Parameter is int)
          this.AddParameterToNavigationStateString(sb, 4, valueString);
        else if (journalEntry.Parameter is uint)
          this.AddParameterToNavigationStateString(sb, 5, valueString);
        else if (journalEntry.Parameter is long)
          this.AddParameterToNavigationStateString(sb, 6, valueString);
        else if (journalEntry.Parameter is ulong)
          this.AddParameterToNavigationStateString(sb, 7, valueString);
        else if (journalEntry.Parameter is float)
          this.AddParameterToNavigationStateString(sb, 8, valueString);
        else if (journalEntry.Parameter is double)
          this.AddParameterToNavigationStateString(sb, 9, valueString);
        else if (journalEntry.Parameter is char)
          this.AddParameterToNavigationStateString(sb, 10, valueString);
        else if (journalEntry.Parameter is bool)
          this.AddParameterToNavigationStateString(sb, 11, valueString);
        else if (journalEntry.Parameter is string)
        {
          this.AddParameterToNavigationStateString(sb, 12, valueString);
        }
        else
        {
          if (!(journalEntry.Parameter is Guid))
            return;
          this.AddParameterToNavigationStateString(sb, 16, valueString);
        }
      }
    }

    private void AddParameterToNavigationStateString(
      StringBuilder sb,
      int typeCode,
      string valueString)
    {
      int length = valueString.Length;
      if (length == 0)
        sb.AppendFormat(",{0},0", (object) typeCode);
      else
        sb.AppendFormat(",{0},{1},{2}", (object) typeCode, (object) length, (object) valueString);
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._pagePresentersPanel = (Panel) ((Control) this).GetTemplateChild("PART_PagePresentersPanel");
      this._waitForApplyTemplateTaskSource.SetResult(true);
    }

    private async Task<bool> NavigateCore(
      Type sourcePageType,
      object parameter,
      NavigationMode navigationMode)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AlternativeFrame.\u003C\u003Ec__DisplayClass31 cDisplayClass31 = new AlternativeFrame.\u003C\u003Ec__DisplayClass31();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass31.sourcePageType = sourcePageType;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass31.parameter = parameter;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass31.navigationMode = navigationMode;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass31.\u003C\u003E4__this = this;
      this._isNavigating = true;
      this.CanNavigate = false;
      this.CanGoBack = false;
      this.CanGoForward = false;
      ((UIElement) this).put_IsHitTestVisible(false);
      if (!((DependencyObject) this).Dispatcher.HasThreadAccess)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AlternativeFrame.\u003C\u003Ec__DisplayClass33 cDisplayClass33 = new AlternativeFrame.\u003C\u003Ec__DisplayClass33();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass33.CS\u0024\u003C\u003E8__locals32 = cDisplayClass31;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass33.navigateCoreTask = (Task<bool>) null;
        // ISSUE: method pointer
        await ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 1, new DispatchedHandler((object) cDisplayClass33, __methodptr(\u003CNavigateCore\u003Eb__30)));
        // ISSUE: reference to a compiler-generated field
        return await cDisplayClass33.navigateCoreTask;
      }
      try
      {
        int num = await this._waitForApplyTemplateTaskSource.Task ? 1 : 0;
        await ((FrameworkElement) this).WaitForLoadedAsync();
        AlternativePage currentPage = (AlternativePage) null;
        if (this._currentPagePresenter != null)
        {
          currentPage = (AlternativePage) this._currentPagePresenter.Content;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          AlternativeNavigatingCancelEventArgs cancelArgs = new AlternativeNavigatingCancelEventArgs(cDisplayClass31.navigationMode, cDisplayClass31.sourcePageType);
          await this.OnNavigating(cancelArgs);
          if (!cancelArgs.Cancel)
            await currentPage.OnNavigatingFromInternal(cancelArgs);
          if (cancelArgs.Cancel)
            return false;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        JournalEntry je = new JournalEntry()
        {
          SourcePageType = cDisplayClass31.sourcePageType,
          Parameter = cDisplayClass31.parameter
        };
        AlternativePage newPage;
        ContentPresenter newPagePresenter;
        if (this._preloadedPageCache.ContainsKey(je))
        {
          newPagePresenter = this._preloadedPageCache[je];
          newPage = (AlternativePage) newPagePresenter.Content;
          this._preloadedPageCache.Remove(je);
        }
        else
        {
          newPage = this._frameCache.Get(je.SourcePageType);
          if (newPage == null)
            throw new InvalidOperationException("Pages used in AlternativeFrame need to be of AlternativePage type.");
          newPage.Frame = this;
          ContentPresenter contentPresenter = new ContentPresenter();
          ((FrameworkElement) contentPresenter).put_Style(this.PagePresenterStyle);
          newPagePresenter = contentPresenter;
          newPagePresenter.put_Content((object) newPage);
          ((ICollection<UIElement>) this._pagePresentersPanel.Children).Add((UIElement) newPagePresenter);
        }
        ((UIElement) newPagePresenter).put_Opacity(0.005);
        await this.UnloadAllPreloaded();
        // ISSUE: reference to a compiler-generated field
        AlternativeNavigationEventArgs args = new AlternativeNavigationEventArgs((object) newPage.Content, cDisplayClass31.navigationMode, je.Parameter, je.SourcePageType);
        await newPage.OnNavigatingToInternal(args);
        // ISSUE: reference to a compiler-generated field
        switch ((int) cDisplayClass31.navigationMode)
        {
          case 0:
            this.ForwardStack.Clear();
            if (this.CurrentJournalEntry != null)
            {
              this.BackStack.Push(this.CurrentJournalEntry);
              break;
            }
            break;
          case 1:
            this.BackStack.Pop();
            if (this.CurrentJournalEntry != null)
            {
              this.ForwardStack.Push(this.CurrentJournalEntry);
              break;
            }
            break;
          case 2:
            this.ForwardStack.Pop();
            if (this.CurrentJournalEntry != null)
            {
              this.BackStack.Push(this.CurrentJournalEntry);
              break;
            }
            break;
        }
        this.CurrentJournalEntry = je;
        await this.OnNavigated(args);
        if (currentPage != null)
          await currentPage.OnNavigatedFromInternal(args);
        await newPage.OnNavigatedToInternal(args);
        await ((FrameworkElement) newPagePresenter).WaitForLoadedAsync();
        await ((FrameworkElement) newPagePresenter).WaitForNonZeroSizeAsync();
        bool? waitForImagesToLoad1 = this.ShouldWaitForImagesToLoad;
        if ((!waitForImagesToLoad1.GetValueOrDefault() ? 0 : (waitForImagesToLoad1.HasValue ? 1 : 0)) != 0)
        {
          bool? waitForImagesToLoad2 = newPage.ShouldWaitForImagesToLoad;
          if ((waitForImagesToLoad2.GetValueOrDefault() ? 1 : (!waitForImagesToLoad2.HasValue ? 1 : 0)) != 0)
            goto label_36;
        }
        bool? waitForImagesToLoad3 = newPage.ShouldWaitForImagesToLoad;
        if ((!waitForImagesToLoad3.GetValueOrDefault() ? 0 : (waitForImagesToLoad3.HasValue ? 1 : 0)) != 0)
        {
          bool? waitForImagesToLoad4 = this.ShouldWaitForImagesToLoad;
          if ((waitForImagesToLoad4.GetValueOrDefault() ? 1 : (!waitForImagesToLoad4.HasValue ? 1 : 0)) == 0)
            goto label_38;
        }
        else
          goto label_38;
label_36:
        await ((FrameworkElement) newPage).WaitForImagesToLoad(1000);
label_38:
        ((UIElement) newPagePresenter).put_Opacity(1.0);
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass31.navigationMode == 1)
          await this.TransitionBackward(currentPage, newPage, this._currentPagePresenter, newPagePresenter);
        else
          await this.TransitionForward(currentPage, newPage, this._currentPagePresenter, newPagePresenter);
        if (this._currentPagePresenter != null)
        {
          ((ICollection<UIElement>) this._pagePresentersPanel.Children).Remove((UIElement) this._currentPagePresenter);
          this._frameCache.Store(currentPage);
        }
        this._currentPagePresenter = newPagePresenter;
        return true;
      }
      finally
      {
        ((UIElement) this).put_IsHitTestVisible(true);
        this._isNavigating = false;
        this.CanNavigate = true;
        this.CanGoBack = this.BackStack.Count > 0;
        this.CanGoForward = this.ForwardStack.Count > 0;
      }
    }

    private async Task OnNavigating(AlternativeNavigatingCancelEventArgs args)
    {
      AlternativeNavigatingCancelEventHandler handler = this.Navigating;
      if (handler == null)
        return;
      await handler((object) this, args);
    }

    private async Task OnNavigated(AlternativeNavigationEventArgs args)
    {
      AlternativeNavigationEventHandler handler = this.Navigated;
      if (handler == null)
        return;
      await handler((object) this, args);
    }

    private async Task TransitionForward(
      AlternativePage currentPage,
      AlternativePage newPage,
      ContentPresenter previousPagePresenter,
      ContentPresenter newPagePresenter)
    {
      PageTransition transition = newPage != null ? newPage.PageTransition ?? this.PageTransition : this.PageTransition;
      if (transition == null)
        return;
      if (currentPage != null)
        await currentPage.OnTransitioningFromInternal();
      if (newPage != null)
        await newPage.OnTransitioningToInternal();
      await transition.TransitionForward((DependencyObject) previousPagePresenter, (DependencyObject) newPagePresenter);
      if (currentPage != null)
        await currentPage.OnTransitionedFromInternal();
      if (newPage == null)
        return;
      await newPage.OnTransitionedToInternal();
    }

    private async Task TransitionBackward(
      AlternativePage currentPage,
      AlternativePage newPage,
      ContentPresenter previousPagePresenter,
      ContentPresenter newPagePresenter)
    {
      PageTransition transition = currentPage != null ? currentPage.PageTransition ?? this.PageTransition : this.PageTransition;
      if (transition == null)
        return;
      if (currentPage != null)
        await currentPage.OnTransitioningFromInternal();
      if (newPage != null)
        await newPage.OnTransitioningToInternal();
      await transition.TransitionBackward((DependencyObject) previousPagePresenter, (DependencyObject) newPagePresenter);
      if (currentPage != null)
        await currentPage.OnTransitionedFromInternal();
      if (newPage == null)
        return;
      await newPage.OnTransitionedToInternal();
    }
  }
}
