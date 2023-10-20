// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Async.AsyncManualResetEvent
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
  public class AsyncManualResetEvent
  {
    private volatile TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

    public Task WaitAsync() => (Task) this._tcs.Task;

    public void Set()
    {
      TaskCompletionSource<bool> tcs = this._tcs;
      Task.Factory.StartNew<bool>((Func<object, bool>) (s => ((TaskCompletionSource<bool>) s).TrySetResult(true)), (object) tcs, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
      tcs.Task.Wait();
    }

    public void Reset()
    {
      TaskCompletionSource<bool> tcs;
      do
      {
        tcs = this._tcs;
      }
      while (tcs.Task.IsCompleted && Interlocked.CompareExchange<TaskCompletionSource<bool>>(ref this._tcs, new TaskCompletionSource<bool>(), tcs) != tcs);
    }
  }
}
