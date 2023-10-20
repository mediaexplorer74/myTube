// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Message
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;

namespace myTube.Cloud
{
  public class Message : DataObject
  {
    public string Title { get; set; }

    public string Body { get; set; }

    public string MinVersion { get; set; }

    public string MaxVersion { get; set; }

    public string PollId { get; set; }

    public long Date { get; set; }

    public List<MessageAction> Actions { get; set; } = new List<MessageAction>();

    [DefaultValue(UserMode.Normal)]
    [JsonConverter(typeof (StringEnumConverter))]
    public UserMode UserMode { get; set; }
  }
}
