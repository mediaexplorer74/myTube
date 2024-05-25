// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.BackgroundTimer
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Tools
{
  public class BackgroundTimer : IDisposable
  {
    private ManualResetEvent _stopRequestEvent;
    private ManualResetEvent _stoppedEvent;
    private TimeSpan _interval;
    private bool _adjustDelays = true;
    private bool _isEnabled;

    public event EventHandler<object> Tick;

    public TimeSpan Interval
    {
      get => this._interval;
      set
      {
        if (this.IsEnabled)
        {
          this.Stop();
          this._interval = value;
          this.Start();
        }
        else
          this._interval = value;
      }
    }

    public bool AdjustDelays
    {
      get => this._adjustDelays;
      set
      {
        if (this.IsEnabled)
        {
          this.Stop();
          this._adjustDelays = value;
          this.Start();
        }
        else
          this._adjustDelays = value;
      }
    }

    public bool IsEnabled
    {
      get => this._isEnabled;
      set
      {
        if (this._isEnabled == value)
          return;
        if (value)
          this.Start();
        else
          this.Stop();
      }
    }

    public BackgroundTimer()
    {
      this._stopRequestEvent = new ManualResetEvent(false);
      this._stoppedEvent = new ManualResetEvent(false);
    }

    public void Start()
    {
      if (this._isEnabled)
        return;
      this._isEnabled = true;
      this._stopRequestEvent.Reset();
      if (this._adjustDelays)
        Task.Run((Action) new Action(this.RunAdjusted));
      else
        Task.Run((Action) new Action(this.Run));
    }

    public void Stop()
    {
      if (!this._isEnabled)
        return;
      this._isEnabled = false;
      this._stoppedEvent.Reset();
      this._stopRequestEvent.Set();
      ((WaitHandle) this._stoppedEvent).WaitOne();
    }

    public void StopNonBlocking()
    {
      if (!this._isEnabled)
        return;
      this._isEnabled = false;
      this._stopRequestEvent.Set();
    }

    private void Run()
    {
      while (this._isEnabled)
      {
        ((WaitHandle) this._stopRequestEvent).WaitOne(this._interval);
        if (this._isEnabled && this.Tick != null)
          this.Tick((object) this, (object) null);
      }
      this._stoppedEvent.Set();
    }

    private void RunAdjusted()
    {
      DateTime now = DateTime.Now;
      long num = 0;
      while (this._isEnabled)
      {
        TimeSpan timeSpan = DateTime.Now - now;
        TimeSpan timeout = TimeSpan.FromSeconds(this._interval.TotalSeconds * (double) (num + 1L)) - timeSpan;
        if (timeout > TimeSpan.Zero)
          ((WaitHandle) this._stopRequestEvent).WaitOne(timeout);
        if (this._isEnabled && this.Tick != null)
          this.Tick((object) this, (object) null);
        ++num;
      }
      this._stoppedEvent.Set();
    }

    public void Dispose()
    {
      if (this._stopRequestEvent != null)
      {
        ((WaitHandle) this._stopRequestEvent).Dispose();
        this._stopRequestEvent = (ManualResetEvent) null;
      }
      if (this._stoppedEvent == null)
        return;
      ((WaitHandle) this._stoppedEvent).Dispose();
      this._stoppedEvent = (ManualResetEvent) null;
    }

    ~BackgroundTimer() => this.Dispose();
  }
}
