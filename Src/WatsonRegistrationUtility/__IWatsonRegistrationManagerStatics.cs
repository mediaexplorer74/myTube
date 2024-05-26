// WatsonRegistrationUtility.__IWatsonRegistrationManagerStatics
// Assembly: WatsonRegistrationUtility, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\WatsonRegistrationUtility.winmd

using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;

namespace WatsonRegistrationUtility
{
  //[Guid(2515208965, 7638, 13439, 173, 93, 125, 49, 5, 219, 173, 186)]
  [Version(1)]
  [ExclusiveTo(typeof (WatsonRegistrationManager))]
  internal interface __IWatsonRegistrationManagerStatics
  {
    void Start([In] string appSecret);

    void SetCorrelationId([In] string correlationId);
  }
}
