// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.SubPollData
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using System.ComponentModel;

namespace myTube.Cloud
{
  public class SubPollData : INotifyPropertyChanged
  {
    private int total;
    private int votes;

    public string Title { get; set; }

    public int Total
    {
      get => this.total;
      set
      {
        if (this.total == value)
          return;
        this.total = value;
        this.opc(nameof (Total));
        this.opc("PercentageOfVotes");
        this.opc("PercentageOfVotesNullable");
      }
    }

    public int Votes
    {
      get => this.votes;
      set
      {
        if (value == this.votes)
          return;
        this.votes = value;
        this.opc(nameof (Votes));
        this.opc("PercentageOfVotes");
        this.opc("PercentageOfVotesNullable");
      }
    }

    public double PercentageOfVotes => this.Total == 0 ? 0.0 : (double) this.Votes / (double) this.Total;

    public double? PercentageOfVotesNullable => new double?(this.PercentageOfVotes);

    public event PropertyChangedEventHandler PropertyChanged;

    private void opc(string p)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(p));
    }
  }
}
