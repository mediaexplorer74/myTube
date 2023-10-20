// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Async.AsyncBarrier
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Async
{
  public class AsyncBarrier
  {
    private readonly int _participantCount;
    private int _remainingParticipants;
    private ConcurrentStack<TaskCompletionSource<bool>> _waiters;

    public AsyncBarrier(int participantCount)
    {
      if (participantCount <= 0)
        throw new ArgumentOutOfRangeException(nameof (participantCount));
      this._remainingParticipants = this._participantCount = participantCount;
      this._waiters = new ConcurrentStack<TaskCompletionSource<bool>>();
    }

    public Task SignalAndWait()
    {
      TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
      this._waiters.Push(completionSource);
      if (Interlocked.Decrement(ref this._remainingParticipants) == 0)
      {
        this._remainingParticipants = this._participantCount;
        ConcurrentStack<TaskCompletionSource<bool>> waiters = this._waiters;
        this._waiters = new ConcurrentStack<TaskCompletionSource<bool>>();
        Parallel.ForEach<TaskCompletionSource<bool>>((IEnumerable<TaskCompletionSource<bool>>) waiters, (Action<TaskCompletionSource<bool>>) (w => w.SetResult(true)));
      }
      return (Task) completionSource.Task;
    }
  }
}
