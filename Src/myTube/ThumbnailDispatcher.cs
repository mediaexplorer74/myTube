// myTube.ThumbnailDispatcher

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace myTube
{
  public class ThumbnailDispatcher
  {
    private const string Tag = "ThumbnailDispatcher";
    private Queue<BitmapAndData> data;
    private BitmapAndData currentData;
    private HttpClient client;
    private bool processing;
    private Stopwatch lastProcessWatch = new Stopwatch();

    public ThumbnailDispatcher()
    {
      this.data = new Queue<BitmapAndData>();
      this.client = new HttpClient();
    }

    private Task<bool> AddData(BitmapAndData bit)
    {
      this.data.Enqueue(bit);
      if (!this.processing)
        this.startProcess();
      return bit.Task;
    }

    public Task<bool> AddData(BitmapImage bit, Uri uri)
    {
      foreach (BitmapAndData bitmapAndData in Enumerable.Where<BitmapAndData>((IEnumerable<BitmapAndData>) this.data, (Func<BitmapAndData, bool>) (b => b.Image == bit)))
        bitmapAndData.Cancel();
      bit.UriSource = (Uri) null;
      return this.AddData(new BitmapAndData()
      {
        Image = bit,
        Uri = uri
      });
    }

    public void Reset()
    {
      if (this.currentData != null)
        this.currentData.Cancel();
      using (Queue<BitmapAndData>.Enumerator enumerator = this.data.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Cancel();
      }
      this.data.Clear();
    }

    private async void startProcess()
    {
      if (this.processing)
        return;
      this.processing = true;
      int orogiCount = this.data.Count;
      TimeSpan maxTime = TimeSpan.FromSeconds(5.0);
      TimeSpan midTime = TimeSpan.FromSeconds(2.5);
      Stopwatch watch = new Stopwatch();
      watch.Start();
      while (this.data.Count > 0)
      {
        if (this.data.Count > orogiCount)
          orogiCount = this.data.Count;
        try
        {
          BitmapAndData bitmapData = this.data.Dequeue();
          if (!bitmapData.IsCanceled)
          {
            this.currentData = bitmapData;
            int timeout = 1500;
            if (watch.Elapsed > maxTime)
              timeout = 100;
            else if (watch.Elapsed > midTime)
              timeout = 1000;
            await this.process(bitmapData, timeout);
          }
        }
        catch
        {
        }
      }
      this.currentData = (BitmapAndData) null;
      watch.Stop();
      Helper.Write((object) nameof (ThumbnailDispatcher), (object)
          ("Finished processing " + (object) orogiCount + " thumbnails"));
      this.processing = false;
    }

    private Task process(BitmapAndData bitmapData, int timeout)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      if (bitmapData.IsCanceled)
      {
        tcs.SetResult(true);
        return (Task) tcs.Task;
      }
      if (bitmapData.Image != null)
      {
        RoutedEventHandler opened = (RoutedEventHandler) null;
        EventHandler canceled = (EventHandler) null;
        ExceptionRoutedEventHandler failed = (ExceptionRoutedEventHandler) null;

        Timer timer = (Timer) null;
        
        opened = (RoutedEventHandler) ((s, e) =>
        {
          tcs.TrySetResult(true);
          bitmapData.SetCompletionResult(true);

            // WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(bitmapData.Image.remove_ImageOpened), opened);
            // WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>(new Action<EventRegistrationToken>(bitmapData.Image.remove_ImageFailed), failed);
            bitmapData.Image.ImageOpened -= opened;
            bitmapData.Image.ImageFailed -= failed;

            bitmapData.Canceled -= canceled;
          if (timer == null)
            return;
          timer.Dispose();
          timer = (Timer) null;
        });
        failed = (ExceptionRoutedEventHandler) ((s, e) =>
        {
          tcs.TrySetResult(false);
          bitmapData.SetCompletionResult(false);

            //WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(bitmapData.Image.remove_ImageOpened), opened);
            //WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>(new Action<EventRegistrationToken>(bitmapData.Image.remove_ImageFailed), failed);
            bitmapData.Image.ImageOpened -= opened;
            bitmapData.Image.ImageFailed -= failed;

            bitmapData.Canceled -= canceled;
          if (timer == null)
            return;
          timer.Dispose();
          timer = (Timer) null;
        });
        canceled = (EventHandler) ((s, e) =>
        {
          tcs.TrySetResult(false);
          bitmapData.SetCompletionResult(false);

          bitmapData.Image.ImageOpened -= opened;
          bitmapData.Image.ImageFailed -= failed;

          bitmapData.Canceled -= canceled;
          if (timer == null)
            return;
          timer.Dispose();
          timer = (Timer) null;
        });
        bitmapData.Canceled += canceled;
        
        BitmapImage image1 = bitmapData.Image;     

        image1.ImageOpened += opened;

        BitmapImage image2 = bitmapData.Image;

        image2.ImageFailed += failed;
                
        if (bitmapData.Uri != (Uri) null)
        {
          bitmapData.Image.UriSource = (Uri) null;
          bitmapData.Image.UriSource = bitmapData.Uri;
        }
        int time = 0;

        DispatchedHandler dispatchedHandler;

        timer = new Timer ((TimerCallback) (state =>
        {
          time += 10;

            // ISSUE: method pointer
            //WindowsRuntimeSystemExtensions.AsTask(CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //    (CoreDispatcherPriority) 1, dispatchedHandler 
            //    ?? (dispatchedHandler = new DispatchedHandler((object)
            //    this, __methodptr(Cprocesss))))).Wait();
            //await
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            CoreDispatcherPriority.High, dispatchedHandler = new DispatchedHandler(callback));
           
        }), 
        (object) 0, 10, 10 
        );
      }

      return (Task) tcs.Task;
    }

    private void callback()
    {
        //
    }
  }
}




