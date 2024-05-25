// Decompiled with JetBrains decompiler
// Type: WatsonRegistrationUtility.WatsonRegistrationManager
// Assembly: WatsonRegistrationUtility, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: AD5C2DB1-83CB-472E-8543-879CBB01917C
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\WatsonRegistrationUtility.winmd

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

#nullable disable
namespace WatsonRegistrationUtility
{
  [Threading]
  [Version(1)]
  [MarshalingBehavior]
  [Static(typeof (__IWatsonRegistrationManagerStatics), 1)]
  [Activatable(1)]
  public sealed class WatsonRegistrationManager : __IWatsonRegistrationManagerPublicNonVirtuals
  {
    [Overload("CreateInstance1")]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public extern WatsonRegistrationManager();

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void Start([In] string appSecret);

    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    public static extern void SetCorrelationId([In] string correlationId);
  }
}
