// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.StringItem
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;

namespace myTube.Cloud
{
  public class StringItem : DataObject
  {
    public string Text { get; set; }

    public string Language { get; set; }

    public string Type { get; set; }

    public string AppName { get; set; }
  }
}
