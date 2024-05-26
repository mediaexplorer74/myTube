// Windows10TileTask.Windows10TileTask
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
  public sealed class Windows10TileTask : IBackgroundTask, IStringable
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern Windows10TileTask();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Run([In] IBackgroundTaskInstance taskInstance);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern string IStringable.ToString();
  }
}
