// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Interactivity.Interaction
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Interactivity
{
  public static class Interaction
  {
    public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("Behaviors", (Type) typeof (BehaviorCollection), (Type) typeof (Interaction), new PropertyMetadata(DesignMode.DesignModeEnabled ? (object) new BehaviorCollection() : (object) (BehaviorCollection) null, new PropertyChangedCallback(Interaction.BehaviorsChanged)));

    [EditorBrowsable(EditorBrowsableState.Always)]
    public static BehaviorCollection GetBehaviors(DependencyObject obj)
    {
      if (!(obj.GetValue(Interaction.BehaviorsProperty) is BehaviorCollection behaviors))
      {
        behaviors = new BehaviorCollection();
        Interaction.SetBehaviors(obj, behaviors);
      }
      return behaviors;
    }

    private static void SetBehaviors(DependencyObject obj, BehaviorCollection value) => obj.SetValue(Interaction.BehaviorsProperty, (object) value);

    private static void BehaviorsChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
      if (!(sender is DependencyObject dependencyObject))
        return;
      if (args.OldValue is BehaviorCollection oldValue)
        oldValue.Detach();
      if (!(args.NewValue is BehaviorCollection newValue))
        return;
      newValue.Attach(dependencyObject);
    }
  }
}
