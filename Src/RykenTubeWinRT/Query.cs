// Decompiled with JetBrains decompiler
// Type: RykenTube.Query
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

namespace RykenTube
{
  public struct Query
  {
    public string Name;
    public string Value;

    public Query(string name, string val)
    {
      this.Name = name;
      this.Value = val;
    }

    public string GetString() => this.Name + "=" + this.Value;

    public override string ToString() => "Query: " + this.Name + " = " + this.Value;
  }
}
