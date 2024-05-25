// myTube.Helper

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
  public static class Helper
  {
    public static int MaxLogsToKeep = 50;
    private static HttpClient client;
    private static object addToLock = new object();
    private static Stopwatch watch;

    public static List<string> Logs { get; private set; }

    static Helper() => Helper.Logs = new List<string>();

    public static event EventHandler<string> Logged;

    private static void createClient()
    {
      if (Helper.client != null)
        return;
      Helper.client = new HttpClient();
    }

    public static async Task<IRandomAccessStream> GetImageStream(Uri uri)
    {
      Helper.createClient();
      Stream streamAsync = await Helper.client.GetStreamAsync(uri);
      MemoryStream memoryStream = new MemoryStream();
      streamAsync.CopyTo((Stream) memoryStream);
      streamAsync.Dispose();
      memoryStream.Position = 0L;
      return memoryStream.AsRandomAccessStream();
    }

    public static void Write(object o)
    {
      string str = o.ToString() + (Helper.watch != null 
                ? (object) (" - " + (object) Helper.watch.Elapsed.TotalSeconds + "s") 
                : (object) "");
      if (Helper.Logged != null)
        Helper.Logged((object) null, str);
      Helper.AddToLog(str);
    }

    private static void AddToLog(string s)
    {
      lock (Helper.addToLock)
      {
        while (Helper.Logs.Count >= Helper.MaxLogsToKeep)
          Helper.Logs.RemoveAt(Helper.Logs.Count - 1);
        Helper.Logs.Insert(0, s);
      }
    }

        public static Task<List<string>> ListLocalFiles()
        {
            return Helper.ListLocalFiles(ApplicationData.Current.LocalFolder, (Func<string, bool>)null);
        }

        public static Task<List<string>> ListLocalFiles(Func<string, bool> Where)
        {
            return Helper.ListLocalFiles(ApplicationData.Current.LocalFolder, Where);
        }

        public static Task<List<string>> ListLocalFiles(StorageFolder sf)
        {
            return Helper.ListLocalFiles(sf, (Func<string, bool>)null);
        }

        public static async Task<List<string>> ListLocalFiles(
      StorageFolder sf,
      Func<string, bool> Where)
    {
      List<string> paths = new List<string>();
      foreach (StorageFile storageFile in (IEnumerable<StorageFile>) await sf.GetFilesAsync())
      {
        if (Where == null || Where(storageFile.Path))
          paths.Add(Helper.ToLocalUriPath(storageFile.Path, ""));
      }
      IReadOnlyList<StorageFolder> foldersAsync = await sf.GetFoldersAsync();
      if (foldersAsync.Count > 0)
      {
        foreach (StorageFolder sf1 in (IEnumerable<StorageFolder>) foldersAsync)
          paths.Add<string>((IList<string>) await Helper.ListLocalFiles(sf1, Where));
      }
      return paths;
    }

        public static void WriteMemory(string tag)
        {
            Helper.Write((object)tag, (object)(".NET memory " + (object)((double)GC.GetTotalMemory(true) / 1048576.0) + " MB"));
        }

        public static void StartTimer()
    {
      Helper.watch = new Stopwatch();
      Helper.watch.Start();
    }

    public static TimeSpan EndTimer()
    {
      if (Helper.watch == null)
        return TimeSpan.Zero;
      Helper.watch.Stop();
      Helper.Write((object) "Helper Timer");
      TimeSpan elapsed = Helper.watch.Elapsed;
      Helper.watch = (Stopwatch) null;
      return elapsed;
    }

        public static void Write(object tag, object o)
        {
            Helper.Write((object)("[" + tag + "] " + o + (Helper.watch != null 
                ? (object)(" - " + (object)Helper.watch.Elapsed.TotalSeconds + "s") 
                : (object)"")));
        }

        public static void Write(object tag, object o, int indentation)
        {
            Helper.Write((object)(Helper.indent(indentation) + "[" + tag + "] " + o + (Helper.watch != null 
                ? (object)(" - " + (object)Helper.watch.Elapsed.TotalSeconds + "s")
                : (object)"")));
        }

        private static string indent(int num)
    {
      string str = "";
      for (int index = 0; index < num; ++index)
        str += "    ";
      return str;
    }

    public static T FindParent<T>(FrameworkElement child, int maxIterations) where T : class
    {
      int num = 0;
      while (num < maxIterations)
      {
        ++num;
        try
        {
          child = (FrameworkElement) child.Parent;
          if (child != null)
          {
            if (child is T)
              return child as T;
          }
        }
        catch
        {
          return default (T);
        }
      }
      return default (T);
    }

    public static double GetResultingOpacity(FrameworkElement child, int maxIterations)
    {
      double opacity = ((UIElement) child).Opacity;
      int num = 0;
      while (num < maxIterations)
      {
        ++num;
        try
        {
          child = VisualTreeHelper.GetParent((DependencyObject) child) as FrameworkElement;
          if (child != null)
            opacity *= ((UIElement) child).Opacity;
          else
            break;
        }
        catch
        {
        }
      }
      return opacity;
    }

    public static double GetResultingOpacity(
      FrameworkElement child,
      FrameworkElement parent,
      int maxIterations)
    {
      double resultingOpacity = 1.0;
      List<FrameworkElement> childrenLine = Helper.FindChildrenLine(parent, child, maxIterations);
      if (childrenLine.Count <= 0)
        return ((UIElement) child).Opacity;
      foreach (FrameworkElement frameworkElement in childrenLine)
        resultingOpacity *= ((UIElement) frameworkElement).Opacity;
      return resultingOpacity;
    }

    public static T FindParentFromTree<T>(FrameworkElement child, int maxIterations) where T : class
    {
      int num = 0;
      while (num < maxIterations)
      {
        ++num;
        try
        {
          child = (FrameworkElement) VisualTreeHelper.GetParent((DependencyObject) child);
          if (child != null)
          {
            if (child is T)
              return child as T;
          }
        }
        catch
        {
          return default (T);
        }
      }
      return default (T);
    }

        public static string ToLocalUriPath(string path)
        {
            return path.Replace(ApplicationData.Current.LocalFolder.Path,
            "ms-appdata:///local").Replace("\\", "/");
        }

        public static string ToLocalUriPath(string path, string replacement)
        {
            return path.Replace(ApplicationData.Current.LocalFolder.Path, replacement).Replace("\\", "/");
        }

        public static string ToFileName(this string s)
    {
      foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
        s = s.Replace(invalidFileNameChar, ' ');
      return s;
    }

    public static T FindChild<T>(DependencyObject parent, int maxIterations) where T : class
    {
      int currentIteration = 0;
      return Helper.FindChild<T>(parent, maxIterations, ref currentIteration);
    }

    public static T FindChild<T>(
      DependencyObject parent,
      int maxIterations,
      ref int currentIteration)
      where T : class
    {
      if (parent is T)
        return parent as T;
      int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
      for (int index = 0; index < childrenCount; ++index)
      {
        DependencyObject child1 = VisualTreeHelper.GetChild(parent, index);
        if (child1 is T)
          return child1 as T;
        ++currentIteration;
        T child2 = Helper.FindChild<T>(child1, maxIterations, ref currentIteration);
        if ((object) child2 != null)
          return child2;
      }
      return default (T);
    }

    public static List<FrameworkElement> FindChildrenLine(
      FrameworkElement parent,
      FrameworkElement child,
      int maxIterations)
    {
      int currentIteration = 0;
      return Helper.FindChildrenLine(parent, child, maxIterations, ref currentIteration);
    }

    private static List<FrameworkElement> FindChildrenLine(
      FrameworkElement parent,
      FrameworkElement child,
      int maxIterations,
      ref int currentIteration)
    {
      List<FrameworkElement> destination = new List<FrameworkElement>();
      int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) parent);
      for (int index = 0; index < childrenCount; ++index)
      {
        ++currentIteration;
        if (currentIteration < maxIterations)
        {
          DependencyObject child1 = VisualTreeHelper.GetChild((DependencyObject) parent, index);
          if (child1 == child)
          {
            destination.Add(parent);
            destination.Add(child1 as FrameworkElement);
            break;
          }
          List<FrameworkElement> childrenLine = Helper.FindChildrenLine(child1 as FrameworkElement, child, 
              maxIterations, ref currentIteration);

          if (childrenLine.Count > 0)
          {
            destination.Add(parent);
            destination.Add<FrameworkElement>((IList<FrameworkElement>) childrenLine);
          }
        }
        else
          break;
      }
      return destination;
    }

    public static List<T> FindChildren<T>(DependencyObject parent, int maxIterations) where T : UIElement
    {
      int currentIteration = 0;
      return Helper.FindChildren<T>(parent, maxIterations, ref currentIteration);
    }

    private static List<T> FindChildren<T>(
      DependencyObject parent,
      int maxIterations,
      ref int currentIteration)
      where T : UIElement
    {
      List<T> destination = new List<T>();
      if (parent is T)
        destination.Add(parent as T);
      int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
      for (int index = 0; index < childrenCount; ++index)
      {
        DependencyObject child = VisualTreeHelper.GetChild(parent, index);
        ++currentIteration;
        int maxIterations1 = maxIterations;
        ref int local = ref currentIteration;
        List<T> children = Helper.FindChildren<T>(child, maxIterations1, ref local);
        if (children.Count > 0)
          destination.Add<T>((IList<T>) children);
      }
      return destination;
    }

    public static async Task<ExceptionRoutedEventArgs> WaitForLoadedAsync(
      this BitmapImage bitmapImage,
      int timeoutInMs = 0)
    {
      TaskCompletionSource<ExceptionRoutedEventArgs> tcs = new TaskCompletionSource<ExceptionRoutedEventArgs>();
      if (((BitmapSource) bitmapImage).PixelWidth > 0 || ((BitmapSource) bitmapImage).PixelHeight > 0)
      {
        tcs.SetResult((ExceptionRoutedEventArgs) null);
        return await tcs.Task;
      }
      RoutedEventHandler reh = (RoutedEventHandler) default;
      ExceptionRoutedEventHandler ereh = (ExceptionRoutedEventHandler) default;
      EventHandler<object> progressCheckTimerTickHandler = (EventHandler<object>) default;

      DispatcherTimer progressCheckTimer = new DispatcherTimer();

      Action dismissWatchmen = (Action) (() =>
      {
        /* WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(bitmapImage.remove_ImageOpened), reh);
        WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>(new Action<EventRegistrationToken>(bitmapImage.remove_ImageFailed), ereh);
        WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>(new Action<EventRegistrationToken>(progressCheckTimer.remove_Tick), progressCheckTimerTickHandler);*/
        bitmapImage.ImageOpened -= reh;
        bitmapImage.ImageFailed -= ereh;
        progressCheckTimer.Tick -= progressCheckTimerTickHandler;
       
        progressCheckTimer.Stop();
      });
      int totalWait = 0;
      progressCheckTimerTickHandler = (EventHandler<object>) ((sender, o) =>
      {
        totalWait += 10;
        if (((BitmapSource) bitmapImage).PixelWidth > 0)
        {
          dismissWatchmen();
          tcs.SetResult((ExceptionRoutedEventArgs) null);
        }
        else
        {
          if (timeoutInMs <= 0 || totalWait < timeoutInMs)
            return;
          dismissWatchmen();
          tcs.SetResult((ExceptionRoutedEventArgs) null);
        }
      });
      progressCheckTimer.Interval = TimeSpan.FromMilliseconds(10.0);
      DispatcherTimer dispatcherTimer = progressCheckTimer;

      //WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(
      //new Func<EventHandler<object>, EventRegistrationToken>(dispatcherTimer.add_Tick), 
      //new Action<EventRegistrationToken>(dispatcherTimer.remove_Tick), progressCheckTimerTickHandler);
      dispatcherTimer.Tick += new EventHandler<object>(progressCheckTimerTickHandler);
      dispatcherTimer.Tick -= progressCheckTimerTickHandler;

      progressCheckTimer.Start();
      reh = (RoutedEventHandler) ((s, e) =>
      {
        dismissWatchmen();
        tcs.SetResult((ExceptionRoutedEventArgs) null);
      });
      ereh = (ExceptionRoutedEventHandler) ((s, e) =>
      {
        dismissWatchmen();
        tcs.SetResult(e);
      });

      BitmapImage bitmapImage1 = bitmapImage;
      //WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(bitmapImage1.add_ImageOpened), new Action<EventRegistrationToken>(bitmapImage1.remove_ImageOpened), reh);
      bitmapImage1.ImageOpened -= reh;

      BitmapImage bitmapImage2 = bitmapImage;
      //WindowsRuntimeMarshal.AddEventHandler<ExceptionRoutedEventHandler>(new Func<ExceptionRoutedEventHandler, EventRegistrationToken>(bitmapImage2.add_ImageFailed), new Action<EventRegistrationToken>(bitmapImage2.remove_ImageFailed), ereh);
      bitmapImage2.ImageFailed -= ereh;

      return await tcs.Task;
    }
  }
}
