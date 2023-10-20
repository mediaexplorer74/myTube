// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Comment
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;

namespace myTube.Cloud
{
  public class Comment : DataObject
  {
    public string MessageID { get; set; }

    public string UserID { get; set; }

    public string UserName { get; set; }

    public string Content { get; set; }
  }
}
