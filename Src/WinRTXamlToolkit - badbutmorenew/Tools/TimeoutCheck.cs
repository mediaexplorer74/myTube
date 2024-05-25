// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.TimeoutCheck
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Tools
{
  public class TimeoutCheck : IDisposable
  {
    private readonly DispatcherTimer _timeoutTimer;
    private CoreDispatcher _dispatcher;

    public bool ThrowOnTimeout { get; set; }

    public bool BreakOnTimeout { get; set; }

    public bool AttachOnTimeout { get; set; }

    public bool DebugOnly { get; set; }

    public object Subject { get; set; }

    public event EventHandler<object> Timeout;

    public TimeSpan Interval
    {
      get => (TimeSpan) this._timeoutTimer.Interval;
      set => this._timeoutTimer.put_Interval((TimeSpan) value);
    }

    public TimeoutCheck(object subject, int timeoutInMilliseconds = 1000, bool autoStart = true)
    {
      this.DebugOnly = true;
      this.BreakOnTimeout = true;
      this.Subject = subject;
      this._timeoutTimer = new DispatcherTimer();
      DispatcherTimer timeoutTimer = this._timeoutTimer;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(timeoutTimer.add_Tick), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(timeoutTimer.remove_Tick), new EventHandler<object>(this.OnTimeout));
      this.Interval = TimeSpan.FromMilliseconds((double) timeoutInMilliseconds);
      if (!autoStart)
        return;
      this.Start();
    }

    private void OnTimeout(object sender, object o)
    {
      EventHandler<object> timeout = this.Timeout;
      if (timeout != null)
        timeout((object) this, this.Subject);
      if (this.DebugOnly)
        return;
      if (this.BreakOnTimeout && Debugger.IsAttached)
        Debugger.Break();
      else if (this.AttachOnTimeout)
        Debugger.Launch();
      else if (this.ThrowOnTimeout)
        throw new TimeoutException("Timeout occured for " + this.Subject);
    }

    public void Start()
    {
      this._dispatcher = Window.Current.Dispatcher;
      this._timeoutTimer.Start();
    }

    public void Stop()
    {
      if (this._dispatcher.HasThreadAccess)
      {
        this._timeoutTimer.Stop();
      }
      else
      {
        // ISSUE: method pointer
        this._dispatcher.RunAsync((CoreDispatcherPriority) 1, new DispatchedHandler((object) this, __methodptr(Stop)));
      }
    }

    ~TimeoutCheck() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void Dispose(bool disposing) => this.Stop();
  }
}
