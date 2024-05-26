// WatsonRegistrationUtility.WatsonRegistrationManager
// Assembly: WatsonRegistrationUtility, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\WatsonRegistrationUtility.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace WatsonRegistrationUtility
{
  [Version(1)]
  [Static(typeof (__IWatsonRegistrationManagerStatics), 1)]
  [Activatable(1)]
  public sealed class WatsonRegistrationManager : __IWatsonRegistrationManagerPublicNonVirtuals
  {
    //[Overload("CreateInstance1")]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern WatsonRegistrationManager();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void Start([In] string appSecret);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetCorrelationId([In] string correlationId);
  }
}
