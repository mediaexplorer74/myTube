// myTube.BitmapAndData

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
