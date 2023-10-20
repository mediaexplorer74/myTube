// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.PollData
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace myTube.Cloud
{
  public class PollData : DataObject, INotifyPropertyChanged
  {
    public ObservableCollection<string> PollChoicesList { get; set; }

    public ObservableCollection<int> PollVotesList { get; set; }

    [JsonIgnore]
    public int Total
    {
      get
      {
        int total = 0;
        foreach (int pollVotes in (Collection<int>) this.PollVotesList)
          total += pollVotes;
        return total;
      }
    }

    [JsonIgnore]
    public ObservableCollection<myTube.Cloud.SubPollData> SubPollData
    {
      get
      {
        ObservableCollection<myTube.Cloud.SubPollData> subPollData = new ObservableCollection<myTube.Cloud.SubPollData>();
        int total = this.Total;
        for (int index = 0; index < this.PollChoicesList.Count; ++index)
          subPollData.Add(new myTube.Cloud.SubPollData()
          {
            Title = this.PollChoicesList[index],
            Votes = this.PollVotesList[index],
            Total = total
          });
        return subPollData;
      }
    }

    public PollData()
    {
      this.PollVotesList = new ObservableCollection<int>();
      this.PollChoicesList = new ObservableCollection<string>();
    }

    public void NotifyChanged(string prop) => this.opc(prop);

    public void PopulateVotes()
    {
      while (this.PollVotesList.Count < this.PollChoicesList.Count)
        this.PollVotesList.Add(0);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void opc([CallerMemberName] string p = null)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(p));
    }
  }
}
