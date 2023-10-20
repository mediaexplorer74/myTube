// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Async.AsyncAutoResetEvent
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
  public class AsyncAutoResetEvent
  {
    private static readonly Task CompletedTask = (Task) Task.FromResult<bool>(true);
    private readonly Queue<TaskCompletionSource<bool>> _waits = new Queue<TaskCompletionSource<bool>>();
    private bool _signaled;

    public AsyncAutoResetEvent(bool initialState = false) => this._signaled = initialState;

    public Task WaitAsync()
    {
      lock (this._waits)
      {
        if (this._signaled)
        {
          this._signaled = false;
          return AsyncAutoResetEvent.CompletedTask;
        }
        TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
        this._waits.Enqueue(completionSource);
        return (Task) completionSource.Task;
      }
    }

    public void Reset() => this._signaled = false;

    public void Set()
    {
      TaskCompletionSource<bool> completionSource = (TaskCompletionSource<bool>) null;
      lock (this._waits)
      {
        if (this._waits.Count > 0)
          completionSource = this._waits.Dequeue();
        else if (!this._signaled)
          this._signaled = true;
      }
      completionSource?.SetResult(true);
    }
  }
}
