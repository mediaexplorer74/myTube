// Decompiled with JetBrains decompiler
// Type: myTube.Program
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Xaml;

namespace myTube
{
  public static class Program
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    private static void Main(string[] args)
    {
      App app;
      Application.Start((ApplicationInitializationCallback) (p => app = new App()));
    }
  }
}
