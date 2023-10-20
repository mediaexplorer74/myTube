// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Async.AsyncCountdownEvent
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
  public class AsyncCountdownEvent
  {
    private readonly AsyncManualResetEvent _amre = new AsyncManualResetEvent();
    private int _count;

    public AsyncCountdownEvent(int initialCount) => this._count = initialCount > 0 ? initialCount : throw new ArgumentOutOfRangeException(nameof (initialCount));

    public Task WaitAsync() => this._amre.WaitAsync();

    public void Signal()
    {
      int num = this._count > 0 ? Interlocked.Decrement(ref this._count) : throw new InvalidOperationException();
      if (num == 0)
        this._amre.Set();
      else if (num < 0)
        throw new InvalidOperationException();
    }

    public Task SignalAndWait()
    {
      this.Signal();
      return this.WaitAsync();
    }
  }
}
