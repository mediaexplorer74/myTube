// ChannelNotificationsTask.RegularTask
// Assembly: ChannelNotificationsTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\ChannelNotificationsTask.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace ChannelNotificationsTask
{
  //[MarshalingBehavior]
  //[Threading]
  [Version(16777216)]
  [Activatable(16777216)]
  [Static(typeof (IRegularTaskStatic), 16777216)]
  public sealed class RegularTask : IBackgroundTask, IStringable
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern RegularTask();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern IAsyncAction UpdateNotifications();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Run([In] IBackgroundTaskInstance taskInstance);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern string IStringable.ToString();
  }
}
