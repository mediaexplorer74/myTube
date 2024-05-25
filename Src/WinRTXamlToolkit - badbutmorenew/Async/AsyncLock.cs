// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Async.AsyncLock
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
  public class AsyncLock
  {
    private readonly AsyncSemaphore _semaphore;
    private readonly Task<AsyncLock.Releaser> _releaser;

    public AsyncLock()
    {
      this._semaphore = new AsyncSemaphore(1);
      this._releaser = Task.FromResult<AsyncLock.Releaser>(new AsyncLock.Releaser(this));
    }

    public Task<AsyncLock.Releaser> LockAsync()
    {
      Task task = this._semaphore.WaitAsync();
      return !task.IsCompleted ? task.ContinueWith<AsyncLock.Releaser>((Func<Task, object, AsyncLock.Releaser>) ((_, state) => new AsyncLock.Releaser((AsyncLock) state)), (object) this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default) : this._releaser;
    }

    public struct Releaser : IDisposable
    {
      private readonly AsyncLock _toRelease;

      internal Releaser(AsyncLock toRelease) => this._toRelease = toRelease;

      public void Dispose()
      {
        if (this._toRelease == null)
          return;
        this._toRelease._semaphore.Release();
      }
    }
  }
}
