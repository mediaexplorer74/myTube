// Decompiled with JetBrains decompiler
// Type: ToastActionBackgroundTask.ToastActionTask
// Assembly: ToastActionBackgroundTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 8C8A76A8-5B60-4755-B3EB-A25B7AD3F906
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\ToastActionBackgroundTask.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Metadata;

#nullable disable
namespace ToastActionBackgroundTask
{
  [MarshalingBehavior]
  [Threading]
  [Version(16777216)]
  [Activatable(16777216)]
  public sealed class ToastActionTask : IBackgroundTask, IStringable
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern ToastActionTask();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Run([In] IBackgroundTaskInstance taskInstance);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern string IStringable.ToString();
  }
}
