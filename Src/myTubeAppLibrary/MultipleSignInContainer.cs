// Decompiled with JetBrains decompiler
// Type: myTube.MultipleSignInContainer
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;

namespace myTube
{
  public class MultipleSignInContainer
  {
    private const string Separator = ";";
    private List<SignInInfo> accounts;

    public int Count => this.accounts.Count;

    public MultipleSignInContainer() => this.accounts = new List<SignInInfo>();

    public SignInInfo this[int i] => this.accounts[i];

    public List<SignInInfo> Accounts => this.accounts;

    public MultipleSignInContainer(string s)
      : this()
    {
      string str = s;
      string[] separator = new string[1]{ ";" };
      foreach (string s1 in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        try
        {
          this.accounts.Add(new SignInInfo(s1));
        }
        catch
        {
        }
      }
      this.accounts.Sort((Comparison<SignInInfo>) ((x, x2) => x.UserName.CompareTo(x2.UserName)));
    }

    public SignInInfo AddAccount(UserInfo info, string refreshToken, string scope)
    {
      string str = "null";
      if (info != null)
        str = info.ID;
      for (int index = 0; index < this.accounts.Count; ++index)
      {
        if (this.accounts[index].UserID == str)
        {
          if (scope == null)
            scope = this.accounts[index].Scope;
          this.accounts.RemoveAt(index);
          --index;
        }
      }
      for (int index = 0; index < this.accounts.Count; ++index)
      {
        if (this.accounts[index].RefreshToken == refreshToken)
        {
          if (scope == null)
            scope = this.accounts[index].Scope;
          if (info == null)
            str = this.accounts[index].UserID;
          this.accounts.RemoveAt(index);
          --index;
        }
      }
      if (info == null)
      {
        UserInfo userInfo = new UserInfo();
        userInfo.ID = str;
        userInfo.UserName = "Google Account";
        userInfo.UserDisplayName = "Google Account";
        info = userInfo;
      }
      SignInInfo signInInfo = new SignInInfo(info, refreshToken, scope);
      this.accounts.Add(signInInfo);
      this.accounts.Sort((Comparison<SignInInfo>) ((x, x2) => x.UserName.CompareTo(x2.UserName)));
      return signInInfo;
    }

    public void RemoveAccount(UserInfo info) => this.RemoveAccount(info.ID);

    public void RemoveAccount(SignInInfo info) => this.RemoveAccount(info.UserID);

    public bool HasAccount(string UserID)
    {
      for (int index = 0; index < this.accounts.Count; ++index)
      {
        if (this.accounts[index].UserID == UserID)
          return true;
      }
      return false;
    }

    public void RemoveAccount(string UserID)
    {
      for (int index = 0; index < this.accounts.Count; ++index)
      {
        if (this.accounts[index].UserID == UserID)
        {
          this.accounts.RemoveAt(index);
          return;
        }
      }
      this.accounts.Sort((Comparison<SignInInfo>) ((x, x2) => x.UserName.CompareTo(x2.UserName)));
    }

    public string ToSaveString()
    {
      string saveString = "";
      for (int index = 0; index < this.accounts.Count; ++index)
      {
        saveString += this.accounts[index].ToSaveString();
        if (index < this.accounts.Count - 1)
          saveString += ";";
      }
      return saveString;
    }

    public void Clear() => this.accounts.Clear();
  }
}
