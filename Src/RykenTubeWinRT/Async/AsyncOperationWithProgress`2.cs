// Decompiled with JetBrains decompiler
// Type: RykenTube.Async.AsyncOperationWithProgress`2
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace RykenTube.Async
{
  public class AsyncOperationWithProgress<T1, T2> : IAsyncOperationWithProgress<T1, T2>, IAsyncInfo
  {
    public AsyncOperationWithProgress(T2 progress)
    {
    }

    public AsyncOperationWithProgressCompletedHandler<T1, T2> Completed
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public Exception ErrorCode => throw new NotImplementedException();

    public virtual uint Id => 0;

    public AsyncOperationProgressHandler<T1, T2> Progress { get; set; }

    public AsyncStatus Status => throw new NotImplementedException();

    public void Cancel() => throw new NotImplementedException();

    public void Close() => throw new NotImplementedException();

    public T1 GetResults() => throw new NotImplementedException();

    //[SpecialName]
    //void IAsyncOperationWithProgress<T1, T2>.Progress (//put_Progress(
    //  AsyncOperationProgressHandler<T1, T2> handler)
    //{
    //  this.Progress = handler;
    // }

    //[SpecialName]
    //void IAsyncOperationWithProgress<T1, T2>.Completed ( //put_Completed(
    //   AsyncOperationWithProgressCompletedHandler<T1, T2> handler)
    //{
    //  this.Completed = handler;
    //}
  }
}
