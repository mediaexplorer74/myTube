// Decompiled with JetBrains decompiler
// Type: myTube.ClientAndItem`1
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;

namespace myTube
{
  public class ClientAndItem<T> where T : ClientData<T>
  {
    public YouTubeClient<T> Client { get; set; }

    public T Item { get; set; }
  }
}
