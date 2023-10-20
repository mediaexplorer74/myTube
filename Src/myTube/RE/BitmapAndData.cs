// Decompiled with JetBrains decompiler
// Type: myTube.BitmapAndData
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
  public class BitmapAndData
  {
    public BitmapImage Image;
    public Uri Uri;
    private TaskCompletionSource<bool> tcs;
    private bool isCanceled;

    public System.Threading.Tasks.Task<bool> Task => this.tcs.Task;

    public event EventHandler Canceled;

    public bool IsCanceled => this.isCanceled;

    public BitmapAndData() => this.tcs = new TaskCompletionSource<bool>();

    public void Cancel()
    {
      this.isCanceled = true;
      this.SetCompletionResult(false);
      if (this.Canceled == null)
        return;
      this.Canceled((object) this, (EventArgs) null);
    }

    public void SetCompletionResult(bool result) => this.tcs.TrySetResult(result);
  }
}
