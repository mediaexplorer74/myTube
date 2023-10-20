// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Async.AsyncReaderWriterLock
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
  public class AsyncReaderWriterLock
  {
    private readonly Task<AsyncReaderWriterLock.Releaser> _readerReleaser;
    private readonly Task<AsyncReaderWriterLock.Releaser> _writerReleaser;
    private readonly Queue<TaskCompletionSource<AsyncReaderWriterLock.Releaser>> _waitingWriters = new Queue<TaskCompletionSource<AsyncReaderWriterLock.Releaser>>();
    private TaskCompletionSource<AsyncReaderWriterLock.Releaser> _waitingReader = new TaskCompletionSource<AsyncReaderWriterLock.Releaser>();
    private int _readersWaiting;
    private int _status;

    public AsyncReaderWriterLock()
    {
      this._readerReleaser = Task.FromResult<AsyncReaderWriterLock.Releaser>(new AsyncReaderWriterLock.Releaser(this, false));
      this._writerReleaser = Task.FromResult<AsyncReaderWriterLock.Releaser>(new AsyncReaderWriterLock.Releaser(this, true));
    }

    public Task<AsyncReaderWriterLock.Releaser> ReaderLockAsync()
    {
      lock (this._waitingWriters)
      {
        if (this._status >= 0 && this._waitingWriters.Count == 0)
        {
          ++this._status;
          return this._readerReleaser;
        }
        ++this._readersWaiting;
        return this._waitingReader.Task.ContinueWith<AsyncReaderWriterLock.Releaser>((Func<Task<AsyncReaderWriterLock.Releaser>, AsyncReaderWriterLock.Releaser>) (t => t.Result));
      }
    }

    public Task<AsyncReaderWriterLock.Releaser> WriterLockAsync()
    {
      lock (this._waitingWriters)
      {
        if (this._status == 0)
        {
          this._status = -1;
          return this._writerReleaser;
        }
        TaskCompletionSource<AsyncReaderWriterLock.Releaser> completionSource = new TaskCompletionSource<AsyncReaderWriterLock.Releaser>();
        this._waitingWriters.Enqueue(completionSource);
        return completionSource.Task;
      }
    }

    private void ReaderRelease()
    {
      TaskCompletionSource<AsyncReaderWriterLock.Releaser> completionSource = (TaskCompletionSource<AsyncReaderWriterLock.Releaser>) null;
      lock (this._waitingWriters)
      {
        --this._status;
        if (this._status == 0)
        {
          if (this._waitingWriters.Count > 0)
          {
            this._status = -1;
            completionSource = this._waitingWriters.Dequeue();
          }
        }
      }
      completionSource?.SetResult(new AsyncReaderWriterLock.Releaser(this, true));
    }

    private void WriterRelease()
    {
      TaskCompletionSource<AsyncReaderWriterLock.Releaser> completionSource = (TaskCompletionSource<AsyncReaderWriterLock.Releaser>) null;
      bool writer = false;
      lock (this._waitingWriters)
      {
        if (this._waitingWriters.Count > 0)
        {
          completionSource = this._waitingWriters.Dequeue();
          writer = true;
        }
        else if (this._readersWaiting > 0)
        {
          completionSource = this._waitingReader;
          this._status = this._readersWaiting;
          this._readersWaiting = 0;
          this._waitingReader = new TaskCompletionSource<AsyncReaderWriterLock.Releaser>();
        }
        else
          this._status = 0;
      }
      completionSource?.SetResult(new AsyncReaderWriterLock.Releaser(this, writer));
    }

    public struct Releaser : IDisposable
    {
      private readonly AsyncReaderWriterLock _toRelease;
      private readonly bool _writer;

      internal Releaser(AsyncReaderWriterLock toRelease, bool writer)
      {
        this._toRelease = toRelease;
        this._writer = writer;
      }

      public void Dispose()
      {
        if (this._toRelease == null)
          return;
        if (this._writer)
          this._toRelease.WriterRelease();
        else
          this._toRelease.ReaderRelease();
      }
    }
  }
}
