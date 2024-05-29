// Decompiled with JetBrains decompiler
// Windows10TileTask.SingularBackgroundTask
// Assembly: Windows10TileTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\Windows10TileTask.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace Windows10TileTask
{
  [Version(16777216)]
  [Activatable(16777216)]
  public sealed class SingularBackgroundTask : 
    IBackgroundTask,
    ISingularBackgroundTaskClass,
    IStringable
  {
    //[MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern SingularBackgroundTask();

    //[MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Run([In] IBackgroundTaskInstance taskInstance);

    public extern bool SetYouTubeParams
        {
            get;
            set;
        }

    
    extern string IStringable.ToString();
  }
}
