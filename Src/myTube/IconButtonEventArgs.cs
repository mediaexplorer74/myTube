// myTube.IconButtonEventArgs

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
