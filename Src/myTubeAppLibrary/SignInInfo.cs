// Decompiled with JetBrains decompiler
// Type: myTube.SignInInfo
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;

namespace myTube
{
  public class SignInInfo
  {
    private const string Separator = ",";
    public string UserID;
    public string RefreshToken;
    public string Scope = "";

    public string UserName { get; set; } = "";

    public SignInInfo()
    {
    }

    public SignInInfo(string s)
    {
      string[] strArray = s.Split(new string[1]{ "," }, StringSplitOptions.None);
      if (strArray.Length != 0)
        this.UserName = strArray[0];
      if (strArray.Length > 1)
        this.UserID = strArray[1];
      if (strArray.Length > 2)
        this.RefreshToken = strArray[2];
      if (strArray.Length <= 3)
        return;
      this.Scope = strArray[3];
    }

    public SignInInfo(UserInfo inf, string RefreshToken, string scope)
    {
      if (inf != null)
      {
        this.UserName = inf.UserDisplayName;
        this.UserID = inf.ID;
      }
      else
      {
        this.UserName = "signed in";
        this.UserID = "null";
      }
      this.Scope = scope;
      this.RefreshToken = RefreshToken;
    }

    public override string ToString() => this.UserName;

    public string ToSaveString() => this.UserName + "," + this.UserID + "," + this.RefreshToken + "," + this.Scope;
  }
}
