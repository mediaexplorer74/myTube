// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.MessageDialogExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class MessageDialogExtensions
  {
    private static TaskCompletionSource<MessageDialog> _currentDialogShowRequest;

    public static IAsyncOperation<IUICommand> ShowTwoOptionsDialog(
      string text,
      string leftButtonText,
      string rightButtonText,
      Action leftButtonAction,
      Action rightButtonAction)
    {
      MessageDialog dialog = new MessageDialog(text);
      dialog.AddButton(leftButtonText, leftButtonAction);
      dialog.AddButton(rightButtonText, rightButtonAction);
      dialog.put_DefaultCommandIndex(1U);
      return dialog.ShowAsync();
    }

    public static void AddButton(this MessageDialog dialog, string caption, Action action)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: method pointer
      UICommand uiCommand = new UICommand(caption, new UICommandInvokedHandler((object) new MessageDialogExtensions.\u003C\u003Ec__DisplayClass1()
      {
        action = action
      }, __methodptr(\u003CAddButton\u003Eb__0)));
      ((ICollection<IUICommand>) dialog.Commands).Add((IUICommand) uiCommand);
    }

    public static async Task<IUICommand> ShowAsyncQueue(this MessageDialog dialog)
    {
      if (!Window.Current.Dispatcher.HasThreadAccess)
        throw new InvalidOperationException("This method can only be invoked from UI thread.");
      while (MessageDialogExtensions._currentDialogShowRequest != null)
      {
        MessageDialog task = await MessageDialogExtensions._currentDialogShowRequest.Task;
      }
      TaskCompletionSource<MessageDialog> request = MessageDialogExtensions._currentDialogShowRequest = new TaskCompletionSource<MessageDialog>();
      IUICommand result = await dialog.ShowAsync();
      MessageDialogExtensions._currentDialogShowRequest = (TaskCompletionSource<MessageDialog>) null;
      request.SetResult(dialog);
      return result;
    }

    public static async Task<IUICommand> ShowAsyncIfPossible(this MessageDialog dialog)
    {
      if (!Window.Current.Dispatcher.HasThreadAccess)
        throw new InvalidOperationException("This method can only be invoked from UI thread.");
      if (MessageDialogExtensions._currentDialogShowRequest != null)
        return (IUICommand) null;
      TaskCompletionSource<MessageDialog> request = MessageDialogExtensions._currentDialogShowRequest = new TaskCompletionSource<MessageDialog>();
      IUICommand result = await dialog.ShowAsync();
      MessageDialogExtensions._currentDialogShowRequest = (TaskCompletionSource<MessageDialog>) null;
      request.SetResult(dialog);
      return result;
    }
  }
}
