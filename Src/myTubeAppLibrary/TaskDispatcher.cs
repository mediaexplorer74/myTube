// Decompiled with JetBrains decompiler
// Type: myTube.TaskDispatcher
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myTube
{
  public class TaskDispatcher
  {
    private Queue<TaskAndFuncBase> tasks;
    private bool processing;
    private object processingLock = new object();
    private object queueLock = new object();

    public TaskDispatcher() => this.tasks = new Queue<TaskAndFuncBase>();

    public Task<T> AddTask<T>(Func<Task<T>> func)
    {
      TaskAndFunc<T> taskAndFunc = new TaskAndFunc<T>(func);
      this.tasks.Enqueue((TaskAndFuncBase) taskAndFunc);
      if (!this.processing)
        this.startProcessing();
      return taskAndFunc.Task;
    }

    public Task<T> AddTask<T>(Func<T> func) => this.AddTask<T>((Func<Task<T>>) (async () => func()));

    private async void startProcessing()
    {
      if (this.processing)
        return;
      this.processing = true;
      while (this.tasks.Count > 0)
      {
        TaskAndFuncBase taskAndFuncBase = this.tasks.Dequeue();
        try
        {
          await taskAndFuncBase.Process();
        }
        catch
        {
        }
      }
      this.processing = false;
    }
  }
}
