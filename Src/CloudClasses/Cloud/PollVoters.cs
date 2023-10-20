// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.PollVoters
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace myTube.Cloud
{
  public class PollVoters : DataObject
  {
    [JsonIgnore]
    private object voteLock = new object();

    public List<string>[] Voters { get; set; }

    public string PollId { get; set; }

    public PollVoters()
      : this(20)
    {
    }

    public PollVoters(int howManyChoices)
    {
      this.Voters = new List<string>[howManyChoices];
      for (int index = 0; index < this.Voters.Length; ++index)
        this.Voters[index] = new List<string>();
    }

    public int VoteIndex(string userId)
    {
      for (int index = 0; index < this.Voters.Length; ++index)
      {
        if (this.Voters[index].Contains(userId))
          return index;
      }
      return -1;
    }

    public bool AddVote(int index, string userId)
    {
      for (int index1 = 0; index1 < this.Voters.Length; ++index1)
      {
        if (this.Voters[index1].Contains(userId))
          return false;
      }
      this.Voters[index].Add(userId);
      return true;
    }
  }
}
