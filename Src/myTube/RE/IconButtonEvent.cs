// Decompiled with JetBrains decompiler
// Type: myTube.IconButtonEvent
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

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
