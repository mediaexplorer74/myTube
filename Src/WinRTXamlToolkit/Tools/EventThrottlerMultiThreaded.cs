// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.EventThrottlerMultiThreaded
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Tools
{
  public class EventThrottlerMultiThreaded
  {
    private readonly object key = new object();
    private Func<Task> next;
    private bool isRunning;

    public void Run(Func<Task> action)
    {
      bool flag = false;
      lock (this.key)
      {
        if (this.isRunning)
        {
          this.next = action;
        }
        else
        {
          this.isRunning = true;
          flag = true;
        }
      }
      Action<Task> continuation = (Action<Task>) null;
      continuation = (Action<Task>) (task =>
      {
        Func<Task> func = (Func<Task>) null;
        lock (this.key)
        {
          if (this.next != null)
          {
            func = this.next;
            this.next = (Func<Task>) null;
          }
          else
            this.isRunning = false;
        }
        if (func == null)
          return;
        func().ContinueWith((Action<Task>) continuation, TaskScheduler.FromCurrentSynchronizationContext());
      });
      if (!flag)
        return;
      action().ContinueWith((Action<Task>) continuation, TaskScheduler.FromCurrentSynchronizationContext());
    }
  }
}
