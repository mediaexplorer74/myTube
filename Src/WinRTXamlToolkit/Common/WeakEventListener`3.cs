// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Common.WeakEventListener`3
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;

namespace WinRTXamlToolkit.Common
{
  public class WeakEventListener<TInstance, TSource, TEventArgs> where TInstance : class
  {
    private WeakReference _weakInstance;

    public Action<TInstance, TSource, TEventArgs> OnEventAction { get; set; }

    public Action<WeakEventListener<TInstance, TSource, TEventArgs>> OnDetachAction { get; set; }

    public WeakEventListener(TInstance instance) => this._weakInstance = (object) instance != null ? new WeakReference((object) instance) : throw new ArgumentNullException(nameof (instance));

    public void OnEvent(TSource source, TEventArgs eventArgs)
    {
      TInstance target = (TInstance) this._weakInstance.Target;
      if ((object) target != null)
      {
        if (this.OnEventAction == null)
          return;
        this.OnEventAction(target, source, eventArgs);
      }
      else
        this.Detach();
    }

    public void Detach()
    {
      if (this.OnDetachAction == null)
        return;
      this.OnDetachAction(this);
      this.OnDetachAction = (Action<WeakEventListener<TInstance, TSource, TEventArgs>>) null;
    }
  }
}
