// Decompiled with JetBrains decompiler
// Type: myTube.AddPlaylistControl
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Helpers;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace myTube
{
  public sealed class AddPlaylistControl : UserControl, IComponentConnector
  {
    public static DependencyProperty ModeProperty = DependencyProperty.Register(nameof (Mode), typeof (PlaylistEditMode), typeof (AddPlaylistControl), new PropertyMetadata((object) PlaylistEditMode.Create, new PropertyChangedCallback(AddPlaylistControl.OnModeChanged)));
    private TranslateTransform trans = new TranslateTransform();
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl parent;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualStateGroup NormalStates;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Create;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Edit;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox titleBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox descriptionBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox privateBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button createButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button cancelButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      string str;
      switch ((PlaylistEditMode) e.NewValue)
      {
        case PlaylistEditMode.Create:
          str = "Create";
          break;
        case PlaylistEditMode.Edit:
          str = "Edit";
          break;
        default:
          str = "Create";
          break;
      }
      VisualStateManager.GoToState((Control) (d as AddPlaylistControl), str, false);
    }

    private YouTubeEntry Entry => ((FrameworkElement) this).DataContext is YouTubeEntry ? ((FrameworkElement) this).DataContext as YouTubeEntry : (YouTubeEntry) null;

    private PlaylistEntry Playlist => ((FrameworkElement) this).DataContext is PlaylistEntry ? ((FrameworkElement) this).DataContext as PlaylistEntry : (PlaylistEntry) null;

    public PlaylistEditMode Mode
    {
      get => (PlaylistEditMode) ((DependencyObject) this).GetValue(AddPlaylistControl.ModeProperty);
      set => ((DependencyObject) this).SetValue(AddPlaylistControl.ModeProperty, (object) value);
    }

    public AddPlaylistControl()
    {
      this.InitializeComponent();
      ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.AddPlaylistControl_Loaded));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.AddPlaylistControl_Unloaded));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(AddPlaylistControl_DataContextChanged)));
      ((FrameworkElement) this).put_RequestedTheme(Settings.Theme);
    }

    private void AddPlaylistControl_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (this.Mode != PlaylistEditMode.Edit || this.Playlist == null)
        return;
      this.titleBox.put_Text(this.Playlist.Title);
      this.descriptionBox.put_Text(this.Playlist.Description);
      ((ToggleButton) this.privateBox).put_IsChecked(new bool?(this.Playlist.Private));
    }

    private void AddPlaylistControl_Unloaded(object sender, RoutedEventArgs e) => InputPaneHelper.Deregister((FrameworkElement) this);

    private void AddPlaylistControl_Loaded(object sender, RoutedEventArgs e)
    {
      ((UIElement) this).put_RenderTransform((Transform) this.trans);
      InputPaneHelper.Register((FrameworkElement) this, this.trans);
      ((Control) this.titleBox).Focus((FocusState) 3);
    }

    private void HidePopup() => DefaultPage.Current.ClosePopup();

    private async void createButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Ani.Begin((DependencyObject) this.createButton, "Opacity", 0.5, 0.2);
      ((UIElement) this.createButton).put_IsHitTestVisible(false);
      Ani.Begin((DependencyObject) this.cancelButton, "Opacity", 0.5, 0.2);
      ((UIElement) this.cancelButton).put_IsHitTestVisible(false);
      if (string.IsNullOrWhiteSpace(this.titleBox.Text))
      {
        ((Control) this.titleBox).Focus((FocusState) 3);
      }
      else
      {
        switch (this.Mode)
        {
          case PlaylistEditMode.Create:
            try
            {
              int num = await YouTube.CreatePlaylist(this.titleBox.Text, this.descriptionBox.Text, ((ToggleButton) this.privateBox).IsChecked.Value, this.Entry != null ? this.Entry.ID : (string) null) ? 1 : 0;
              this.HidePopup();
              break;
            }
            catch
            {
              break;
            }
          case PlaylistEditMode.Edit:
            try
            {
              if (this.Playlist != null)
              {
                this.Playlist.Title = this.titleBox.Text;
                this.Playlist.Description = this.descriptionBox.Text;
                PlaylistEntry playlist = this.Playlist;
                bool? isChecked = ((ToggleButton) this.privateBox).IsChecked;
                int num1 = isChecked.Value ? 1 : 0;
                playlist.Privacy = (PrivacyStatus) num1;
                string id = this.Playlist.ID;
                string text1 = this.titleBox.Text;
                string text2 = this.descriptionBox.Text;
                isChecked = ((ToggleButton) this.privateBox).IsChecked;
                int num2 = isChecked.Value ? 1 : 0;
                int num3 = await YouTube.EditPlaylist(id, text1, text2, num2 != 0) ? 1 : 0;
                this.HidePopup();
                break;
              }
              break;
            }
            catch
            {
              break;
            }
        }
      }
      ((UIElement) this.createButton).put_IsHitTestVisible(true);
      Ani.Begin((DependencyObject) this.createButton, "Opacity", 1.0, 0.2);
    }

    private async void cancelButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Ani.Begin((DependencyObject) this.createButton, "Opacity", 0.5, 0.2);
      ((UIElement) this.createButton).put_IsHitTestVisible(false);
      Ani.Begin((DependencyObject) this.cancelButton, "Opacity", 0.5, 0.2);
      ((UIElement) this.cancelButton).put_IsHitTestVisible(false);
      if (this.Mode == PlaylistEditMode.Edit && this.Playlist != null)
      {
        try
        {
          int num = await YouTube.DeletePlaylist(this.Playlist.ID) ? 1 : 0;
          this.Playlist.Exists = false;
          App.Instance.RootFrame.GoBack();
          App.GlobalObjects.OfflinePlaylistsCollection.RemoveOfflinePlaylist(this.Playlist.ID);
        }
        catch
        {
        }
      }
      this.HidePopup();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///AddPlaylistControl.xaml"), (ComponentResourceLocation) 0);
      this.parent = (UserControl) ((FrameworkElement) this).FindName("parent");
      this.NormalStates = (VisualStateGroup) ((FrameworkElement) this).FindName("NormalStates");
      this.Create = (VisualState) ((FrameworkElement) this).FindName("Create");
      this.Edit = (VisualState) ((FrameworkElement) this).FindName("Edit");
      this.titleBox = (TextBox) ((FrameworkElement) this).FindName("titleBox");
      this.descriptionBox = (TextBox) ((FrameworkElement) this).FindName("descriptionBox");
      this.privateBox = (CheckBox) ((FrameworkElement) this).FindName("privateBox");
      this.createButton = (Button) ((FrameworkElement) this).FindName("createButton");
      this.cancelButton = (Button) ((FrameworkElement) this).FindName("cancelButton");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.createButton_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.cancelButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
