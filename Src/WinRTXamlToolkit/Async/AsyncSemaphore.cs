// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Async.AsyncSemaphore
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
  public class AsyncSemaphore
  {
    private static readonly Task CompletedTask = (Task) Task.FromResult<bool>(true);
    private readonly Queue<TaskCompletionSource<bool>> _waiters = new Queue<TaskCompletionSource<bool>>();
    private int _currentCount;

    public AsyncSemaphore(int initialCount) => this._currentCount = initialCount >= 0 ? initialCount : throw new ArgumentOutOfRangeException(nameof (initialCount));

    public Task WaitAsync()
    {
      lock (this._waiters)
      {
        if (this._currentCount > 0)
        {
          --this._currentCount;
          return AsyncSemaphore.CompletedTask;
        }
        TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
        this._waiters.Enqueue(completionSource);
        return (Task) completionSource.Task;
      }
    }

    public void Release()
    {
      TaskCompletionSource<bool> completionSource = (TaskCompletionSource<bool>) null;
      lock (this._waiters)
      {
        if (this._waiters.Count > 0)
          completionSource = this._waiters.Dequeue();
        else
          ++this._currentCount;
      }
      completionSource?.SetResult(true);
    }
  }
}
