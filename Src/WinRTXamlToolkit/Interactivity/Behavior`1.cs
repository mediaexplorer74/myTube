// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Interactivity.Behavior`1
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Interactivity
{
  public abstract class Behavior<T> : Behavior where T : DependencyObject
  {
    protected Behavior() => this._associatedType = typeof (T);

    public T AssociatedObject
    {
      get => (T) this._associatedObject;
      internal set => this._associatedObject = (DependencyObject) value;
    }
  }
}
