// Windows10TileTask.IMainTileTaskStatic
// Assembly: Windows10TileTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\Windows10TileTask.winmd

using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace Windows10TileTask
{
  [Guid(3628217455, 23900, 24220, 85, 200, 90, 185, 229, 88, 236, 111)]
  [Version(16777216)]
  [ExclusiveTo(typeof (MainTileTask))]
  internal interface IMainTileTaskStatic
  {
    //[MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    IAsyncAction UpdateMainTile();
  }
}
