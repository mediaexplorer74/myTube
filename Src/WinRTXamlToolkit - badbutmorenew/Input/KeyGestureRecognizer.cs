// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Input.KeyGestureRecognizer
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Input
{
  public class KeyGestureRecognizer : IDisposable
  {
    private Window window;
    private KeyGesture gesture;
    private int combinationsMatched;

    public event EventHandler GestureRecognized;

    private void RaiseGestureRecognized()
    {
      EventHandler gestureRecognized = this.GestureRecognized;
      if (gestureRecognized == null)
        return;
      gestureRecognized((object) this, EventArgs.Empty);
    }

    public KeyGestureRecognizer(KeyGesture gesture)
    {
      if (gesture == null)
        throw new ArgumentNullException(nameof (gesture));
      this.gesture = gesture.Count != 0 ? gesture : throw new ArgumentException("The gesture needs to consist of at least one key or key combination.", nameof (gesture));
      this.window = Window.Current;
      CoreWindow coreWindow = this.window.CoreWindow;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<CoreWindow, KeyEventArgs>>((Func<TypedEventHandler<CoreWindow, KeyEventArgs>, EventRegistrationToken>) new Func<TypedEventHandler<CoreWindow, KeyEventArgs>, EventRegistrationToken>(coreWindow.add_KeyDown), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(coreWindow.remove_KeyDown), new TypedEventHandler<CoreWindow, KeyEventArgs>((object) this, __methodptr(CoreWindowOnKeyDown)));
    }

    public void Dispose()
    {
      if (!this.window.Dispatcher.HasThreadAccess)
      {
        // ISSUE: method pointer
        this.window.Dispatcher.RunAsync((CoreDispatcherPriority) 1, new DispatchedHandler((object) this, __methodptr(Dispose)));
      }
      else
      {
        // ISSUE: method pointer
        WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<CoreWindow, KeyEventArgs>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this.window.CoreWindow.remove_KeyDown), new TypedEventHandler<CoreWindow, KeyEventArgs>((object) this, __methodptr(CoreWindowOnKeyDown)));
        this.window = (Window) null;
        this.gesture = (KeyGesture) null;
      }
    }

    private KeyGestureRecognizer.MatchKind CheckKeyCombination(
      KeyCombination combination,
      VirtualKey keyAdded,
      KeyCombination precedingCombination = null)
    {
      CoreVirtualKeyStates virtualKeyStates = (CoreVirtualKeyStates) 1;
      bool flag1 = (this.window.CoreWindow.GetKeyState((VirtualKey) 17) & virtualKeyStates) == virtualKeyStates;
      bool flag2 = (this.window.CoreWindow.GetKeyState((VirtualKey) 18) & virtualKeyStates) == virtualKeyStates;
      bool flag3 = (this.window.CoreWindow.GetKeyState((VirtualKey) 16) & virtualKeyStates) == virtualKeyStates;
      if (keyAdded.IsModifier())
        return KeyGestureRecognizer.MatchKind.Incomplete;
      if (!combination.Contains(keyAdded))
        return KeyGestureRecognizer.MatchKind.Mismatch;
      foreach (VirtualKey virtualKey in (List<VirtualKey>) combination)
      {
        if (virtualKey != keyAdded && (this.window.CoreWindow.GetKeyState(virtualKey) & virtualKeyStates) != virtualKeyStates)
          return KeyGestureRecognizer.MatchKind.Mismatch;
      }
      return flag1 && !combination.Contains((VirtualKey) 17) && (precedingCombination == null || !precedingCombination.Contains((VirtualKey) 17)) || flag2 && (!combination.Contains((VirtualKey) 18) || precedingCombination == null || !precedingCombination.Contains((VirtualKey) 18)) || flag3 && (!combination.Contains((VirtualKey) 16) || precedingCombination == null || !precedingCombination.Contains((VirtualKey) 16)) ? KeyGestureRecognizer.MatchKind.Mismatch : KeyGestureRecognizer.MatchKind.Match;
    }

    private void CoreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
    {
      KeyCombination precedingCombination = (KeyCombination) null;
      if (this.combinationsMatched > 0)
        precedingCombination = this.gesture[0];
      switch (this.CheckKeyCombination(this.gesture[this.combinationsMatched], args.VirtualKey, precedingCombination))
      {
        case KeyGestureRecognizer.MatchKind.Mismatch:
          this.combinationsMatched = 0;
          break;
        case KeyGestureRecognizer.MatchKind.Match:
          ++this.combinationsMatched;
          if (this.combinationsMatched != this.gesture.Count)
            break;
          this.RaiseGestureRecognized();
          this.combinationsMatched = 0;
          break;
      }
    }

    private enum MatchKind
    {
      Mismatch,
      Incomplete,
      Match,
    }
  }
}
