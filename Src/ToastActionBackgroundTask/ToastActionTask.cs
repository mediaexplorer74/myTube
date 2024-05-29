// ToastActionBackgroundTask.ToastActionTask
// Assembly: ToastActionBackgroundTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\ToastActionBackgroundTask.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace ToastActionBackgroundTask
{
  //[MarshalingBehavior]
  //[Threading]
  [Version(16777216)]
  [Activatable(16777216)]
  public sealed class ToastActionTask : IBackgroundTask, IStringable
  {
    
    public extern ToastActionTask();

   
    public extern void Run([In] IBackgroundTaskInstance taskInstance);

   
    extern string IStringable.ToString();
  }
}
