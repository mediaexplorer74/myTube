// Decompiled with JetBrains decompiler
// Type: Windows10TileTask.IMainTileTaskStatic
// Assembly: Windows10TileTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: E65E81AD-1A99-4C5D-8812-143365A94573
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\Windows10TileTask.winmd

using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;

#nullable disable
namespace Windows10TileTask
{
  [Guid(3628217455, 23900, 24220, 85, 200, 90, 185, 229, 88, 236, 111)]
  [Version(16777216)]
  [ExclusiveTo(typeof (MainTileTask))]
  internal interface IMainTileTaskStatic
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    IAsyncAction UpdateMainTile();
  }
}
