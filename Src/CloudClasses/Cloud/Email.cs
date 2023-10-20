// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Email
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using System.Collections.Generic;
using System.Linq;

namespace myTube.Cloud
{
  public class Email : DataObject
  {
    private static string notAllowed = "\\()<>[],:; ";

    public string EmailAddress { get; set; }

    public string UserName { get; set; }

    public List<string> RequestedBetaApps { get; set; }

    public List<string> JoinedBetaApps { get; set; }

    public List<string> JoiningSoon { get; set; }

    public Email()
    {
      this.RequestedBetaApps = new List<string>();
      this.JoinedBetaApps = new List<string>();
      this.JoiningSoon = new List<string>();
    }

    public bool HasApp(string betaId) => this.RequestedBetaApps.Contains(betaId) || this.JoiningSoon.Contains(betaId) || this.JoinedBetaApps.Contains(betaId);

    public static bool IsValidEmailAddress(string addr)
    {
      if (addr.StartsWith(" ") || addr.EndsWith(" ") || !addr.Contains<char>('@') || !addr.Contains<char>('.'))
        return false;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      char ch1 = ' ';
      for (int index = 0; index < addr.Length; ++index)
      {
        char ch2 = addr[index];
        if (ch2 == '"' && flag1)
          return false;
        if (ch2 == '"' && ch1 != '\\')
          flag3 = !flag3;
        if (!flag3)
        {
          if (ch1 == '"' && ch2 != '.' && ch2 != '@' || ch2 == ' ' || Email.notAllowed.Contains<char>(ch2))
            return false;
          if (ch2 == '@')
          {
            if (flag1)
              return false;
            flag1 = true;
          }
          if (ch2 == '.')
          {
            if (ch1 == '.')
              return false;
            if (flag1)
              flag2 = true;
          }
        }
        ch1 = ch2;
      }
      return flag1 & flag2 && !flag3;
    }
  }
}
