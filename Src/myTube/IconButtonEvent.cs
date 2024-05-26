// myTube.IconButtonEvent

using System;
using System.Threading.Tasks;

namespace myTube
{
  public class IconButtonEvent : IconButtonInfo
  {
    public event EventHandler<IconButtonEventArgs> Selected;

    public async Task CallSelected(object sender)
    {
      if (this.Selected == null)
        return;
      IconButtonEventArgs e = new IconButtonEventArgs()
      {
        OriginalSender = sender
      };
      this.Selected((object) this, e);
      await e.WaitForClose();
    }
  }
}
