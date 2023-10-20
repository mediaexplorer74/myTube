// Decompiled with JetBrains decompiler
// Type: myTube.MyExtensions
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Web.Http;

namespace myTube
{
  public static class MyExtensions
  {
    public static int IndexOfClosest(this float[] array, float value)
    {
      int num1 = -1;
      float num2 = float.MaxValue;
      for (int index = 0; index < array.Length; ++index)
      {
        float num3 = Math.Abs(array[index] - value);
        if ((double) num3 < (double) num2)
        {
          num1 = index;
          num2 = num3;
        }
      }
      return num1;
    }

    public static async Task<int> ShowAsync(this MessageDialog md, params string[] commands)
    {
      foreach (string command in commands)
        md.Commands.Add((IUICommand) new UICommand(command));
      IUICommand iuiCommand = await md.ShowAsync();
      return iuiCommand == null || !md.Commands.Contains(iuiCommand) ? -1 : md.Commands.IndexOf(iuiCommand);
    }

    public static double TotalWeeks(this TimeSpan time) => time.TotalDays / 7.0;

    public static double TotalMonths(this TimeSpan time) => time.TotalDays / 30.5;

    public static double TotalYears(this TimeSpan time) => time.TotalDays / 365.0;

    public static async Task<bool> CanAccessFile(this HttpClient client, Uri uri)
    {
      try
      {
        return (await client.GetAsync(uri, (HttpCompletionOption) 1)).IsSuccessStatusCode;
      }
      catch
      {
        return false;
      }
    }

    public static double ProgressToDouble(this DownloadOperation download)
    {
      try
      {
        double d = (double) download.Progress.BytesReceived / (double) download.Progress.TotalBytesToReceive;
        return double.IsNaN(d) ? 0.0 : d;
      }
      catch
      {
        return 0.0;
      }
    }

    public static void Add<T>(this IList<T> destination, IList<T> source)
    {
      foreach (T obj in (IEnumerable<T>) source)
        destination.Add(obj);
    }

    public static Point Center(this Rect rect) => new Point((rect.Left + rect.Right) / 2.0, (rect.Top + rect.Bottom) / 2.0);

    public static async Task<string> ReadString(this Stream s)
    {
      string val = "";
      using (StreamReader sr = new StreamReader(s))
      {
        try
        {
          val = await sr.ReadToEndAsync();
        }
        catch
        {
        }
      }
      return val;
    }

    public static Point GetPosition(this UIElement el, UIElement from)
    {
      Point position = new Point();
      try
      {
        el.TransformToVisual(from).TryTransform(new Point(), ref position);
        return position;
      }
      catch
      {
        return position;
      }
    }

    public static Rect GetBounds(this FrameworkElement el, UIElement from)
    {
      Rect bounds = new Rect();
      try
      {
        return ((UIElement) el).TransformToVisual(from).TransformBounds(new Rect(0.0, 0.0, el.ActualWidth, el.ActualHeight));
      }
      catch
      {
        return bounds;
      }
    }

    public static bool HasMedia(this MediaElement el) => el.CurrentState != null && el.CurrentState != 1;

    public static Task<bool> WaitForOpened(this MediaElement med)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      if (med.CurrentState != 1)
      {
        tcs.SetResult(true);
        return tcs.Task;
      }
      RoutedEventHandler opened = (RoutedEventHandler) null;
      ExceptionRoutedEventHandler failed = (ExceptionRoutedEventHandler) null;
      failed = (ExceptionRoutedEventHandler) ((o, e) =>
      {
        tcs.SetResult(false);
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(med.remove_MediaOpened), opened);
        WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>(new Action<EventRegistrationToken>(med.remove_MediaFailed), failed);
      });
      opened = (RoutedEventHandler) ((sender, e) =>
      {
        tcs.SetResult(true);
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(med.remove_MediaOpened), opened);
        WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>(new Action<EventRegistrationToken>(med.remove_MediaFailed), failed);
      });
      MediaElement mediaElement1 = med;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(mediaElement1.add_MediaOpened), new Action<EventRegistrationToken>(mediaElement1.remove_MediaOpened), opened);
      MediaElement mediaElement2 = med;
      WindowsRuntimeMarshal.AddEventHandler<ExceptionRoutedEventHandler>(new Func<ExceptionRoutedEventHandler, EventRegistrationToken>(mediaElement2.add_MediaFailed), new Action<EventRegistrationToken>(mediaElement2.remove_MediaFailed), failed);
      return tcs.Task;
    }

    public static Task<bool> WaitForTappedOrUnloaded(this FrameworkElement el, TimeSpan timeout)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      TappedEventHandler tapped = (TappedEventHandler) null;
      RoutedEventHandler unloaded = (RoutedEventHandler) null;
      tapped = (TappedEventHandler) ((s, e) =>
      {
        WindowsRuntimeMarshal.RemoveEventHandler<TappedEventHandler>(new Action<EventRegistrationToken>(((UIElement) el).remove_Tapped), tapped);
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(el.remove_Unloaded), unloaded);
        tcs.SetResult(true);
      });
      unloaded = (RoutedEventHandler) ((s, e) =>
      {
        WindowsRuntimeMarshal.RemoveEventHandler<TappedEventHandler>(new Action<EventRegistrationToken>(((UIElement) el).remove_Tapped), tapped);
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(el.remove_Unloaded), unloaded);
        tcs.SetResult(false);
      });
      return tcs.Task;
    }

    public static void UpdateLayoutOffscreen(this FrameworkElement el, double width, double height)
    {
      ((UIElement) el).InvalidateArrange();
      ((UIElement) el).InvalidateMeasure();
      ((UIElement) el).Measure(new Size(width, height));
      ((UIElement) el).Arrange(new Rect(0.0, 0.0, width, height));
    }

    public static bool ContainsPoint(this FrameworkElement el, FrameworkElement from, Point p)
    {
      Rect bounds = el.GetBounds((UIElement) from);
      return p.X > bounds.X && p.Y > bounds.Y && p.X < bounds.Right && p.Y < bounds.Bottom;
    }

    public static bool ContainsPoint(this FrameworkElement el, PointerRoutedEventArgs e)
    {
      Point position = e.GetCurrentPoint((UIElement) el).Position;
      return el.ContainsPoint(position);
    }

    public static bool ContainsPoint(this FrameworkElement el, TappedRoutedEventArgs e)
    {
      Point position = e.GetPosition((UIElement) el);
      return el.ContainsPoint(position);
    }

    public static bool ContainsPoint(this FrameworkElement el, Point p) => p.X > 0.0 && p.Y > 0.0 && p.X < el.ActualWidth && p.Y < el.ActualHeight;

    public static async Task<bool> FileExists(this StorageFolder folder, string path)
    {
      try
      {
        StorageFile fileAsync = await folder.GetFileAsync(path);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static async Task<bool> FolderExists(this StorageFolder folder, string path)
    {
      try
      {
        StorageFolder folderAsync = await folder.GetFolderAsync(path);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
