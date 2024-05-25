// Decompiled with JetBrains decompiler
// Type: Windows10TileTask.SingularBackgroundTask
// Assembly: Windows10TileTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: E65E81AD-1A99-4C5D-8812-143365A94573
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\Windows10TileTask.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Metadata;

#nullable disable
namespace Windows10TileTask
{
  [MarshalingBehavior]
  [Threading]
  [Version(16777216)]
  [Activatable(16777216)]
  public sealed class SingularBackgroundTask : 
    IBackgroundTask,
    ISingularBackgroundTaskClass,
    IStringable
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern SingularBackgroundTask();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Run([In] IBackgroundTaskInstance taskInstance);

    public extern bool SetYouTubeParams { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern string IStringable.ToString();
  }
}
