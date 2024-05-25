// Decompiled with JetBrains decompiler
// Type: BackgroundAudio.AudioPlayer
// Assembly: BackgroundAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 4D30EF3B-0907-4AD1-8CB4-1D29D106F4CC
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\BackgroundAudio.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace BackgroundAudio
{
  //[MarshalingBehavior]
  //[Threading]
  [Version(16777216)]
  [Activatable(16777216)]
  public sealed class AudioPlayer : IBackgroundTask, IStringable
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern AudioPlayer();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern void Run([In] IBackgroundTaskInstance taskInstance);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    extern string IStringable.ToString();
  }
}
