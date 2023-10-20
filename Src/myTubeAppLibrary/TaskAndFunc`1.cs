// Decompiled with JetBrains decompiler
// Type: myTube.TaskAndFunc`1
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Threading.Tasks;

namespace myTube
{
  internal class TaskAndFunc<T> : TaskAndFuncBase
  {
    private Func<System.Threading.Tasks.Task<T>> func;
    private TaskCompletionSource<T> tcs;
    private bool processed;

    public System.Threading.Tasks.Task<T> Task => this.tcs.Task;

    public TaskAndFunc(Func<System.Threading.Tasks.Task<T>> func)
    {
      this.func = func;
      this.tcs = new TaskCompletionSource<T>();
    }

    public override async System.Threading.Tasks.Task Process()
    {
      if (this.processed)
        return;
      this.processed = true;
      try
      {
        this.tcs.TrySetResult(await this.func());
      }
      catch (Exception ex)
      {
        this.tcs.TrySetException(ex);
      }
    }
  }
}
