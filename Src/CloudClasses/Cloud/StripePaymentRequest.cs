// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.StripePaymentRequest
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

namespace myTube.Cloud
{
  public class StripePaymentRequest
  {
    public string Token { get; set; }

    public string Title { get; set; }

    public string Currency { get; set; }

    public double Amount { get; set; }
  }
}
