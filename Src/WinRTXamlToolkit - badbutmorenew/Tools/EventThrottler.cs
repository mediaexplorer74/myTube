// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.EventThrottler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Tools
{
  public class EventThrottler
  {
    private Func<Task> next;
    private bool isRunning;

    public async void Run(Func<Task> action)
    {
      if (this.isRunning)
      {
        this.next = action;
      }
      else
      {
        this.isRunning = true;
        try
        {
          await action();
          while (this.next != null)
          {
            Func<Task> nextCopy = this.next;
            this.next = (Func<Task>) null;
            await nextCopy();
          }
        }
        finally
        {
          this.isRunning = false;
        }
      }
    }
  }
}
