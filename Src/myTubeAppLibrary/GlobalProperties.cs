// Decompiled with JetBrains decompiler
// Type: myTube.GlobalProperties
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using Windows.Networking.Connectivity;

namespace myTube
{
  public static class GlobalProperties
  {
    public static bool IsMobileDataConnection
    {
      get
      {
        try
        {
          return NetworkInformation.GetInternetConnectionProfile().IsWwanConnectionProfile;
        }
        catch
        {
          return false;
        }
      }
    }

    public static bool HasInternetConnection
    {
      get
      {
        try
        {
          return NetworkInformation.GetInternetConnectionProfile() != null;
        }
        catch
        {
          return false;
        }
      }
    }
  }
}
