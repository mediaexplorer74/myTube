// Decompiled with JetBrains decompiler
// Type: myTube.IconButtonEventArgs
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System.Threading.Tasks;

namespace myTube
{
  public class IconButtonEventArgs
  {
    public object OriginalSender;
    private TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

    public void Close() => this.tcs.SetResult(true);

    public Task WaitForClose() => (Task) this.tcs.Task;
  }
}
