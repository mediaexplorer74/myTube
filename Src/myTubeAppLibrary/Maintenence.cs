// Decompiled with JetBrains decompiler
// Type: myTube.Maintenence
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System.Threading.Tasks;

namespace myTube
{
  public static class Maintenence
  {
    public static async Task Run() => await TileHelper.CleanUpFolders();
  }
}
