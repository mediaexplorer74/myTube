﻿// Decompiled with JetBrains decompiler
// Type: Windows10TileTask.ISingularBackgroundTaskClass
// Assembly: Windows10TileTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: E65E81AD-1A99-4C5D-8812-143365A94573
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\Windows10TileTask.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

#nullable disable
namespace Windows10TileTask
{
  [Guid(3626136828, 36349, 22140, 73, 239, 61, 96, 19, 205, 32, 178)]
  [Version(16777216)]
  [ExclusiveTo(typeof (SingularBackgroundTask))]
  internal interface ISingularBackgroundTaskClass
  {
    bool SetYouTubeParams { [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] get; [MethodImpl(MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }
  }
}