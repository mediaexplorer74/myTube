// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.OrientedSize
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
  internal struct OrientedSize
  {
    private Orientation _orientation;
    private double _direct;
    private double _indirect;

    public Orientation Orientation => this._orientation;

    public double Direct
    {
      get => this._direct;
      set => this._direct = value;
    }

    public double Indirect
    {
      get => this._indirect;
      set => this._indirect = value;
    }

    public double Width
    {
      get => this.Orientation != 1 ? this.Indirect : this.Direct;
      set
      {
        if (this.Orientation == 1)
          this.Direct = value;
        else
          this.Indirect = value;
      }
    }

    public double Height
    {
      get => this.Orientation == 1 ? this.Indirect : this.Direct;
      set
      {
        if (this.Orientation != 1)
          this.Direct = value;
        else
          this.Indirect = value;
      }
    }

    public OrientedSize(Orientation orientation)
      : this(orientation, 0.0, 0.0)
    {
    }

    public OrientedSize(Orientation orientation, double width, double height)
    {
      this._orientation = orientation;
      this._direct = 0.0;
      this._indirect = 0.0;
      this.Width = width;
      this.Height = height;
    }
  }
}
