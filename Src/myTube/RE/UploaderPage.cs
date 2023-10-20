// Decompiled with JetBrains decompiler
// Type: myTube.UploaderPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class UploaderPage : Page, IComponentConnector
  {
    private ObservableCollection<string> tags;
    private IStorageFile selectedFile;
    private bool addTagEnabled = true;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private BitmapImage videoBitmap;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid viewGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer uploadsScroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock nouploads;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl uploadsList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel internalGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl openButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid thumbGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox titleBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox descriptionBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox categoryBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox privacyBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox licenseBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl moreButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel morePanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl submitButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox uploadDateCheckBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DatePicker publishAtDatePicker;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TimePicker publishAtTimePicker;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox embeddableCheckBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox extendedStatsCheckBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox tagBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl addTagButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl tagsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Control16by9 thumbControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock videoTitle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public UploaderPage()
    {
      this.InitializeComponent();
      this.tags = new ObservableCollection<string>();
      this.tagsControl.put_ItemsSource((object) this.tags);
      this.enableAddTag(false);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.UploaderPage_Loaded));
    }

    private async void UploaderPage_Loaded(object sender, RoutedEventArgs e)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      ((ICollection<object>) this.uploadsList.Items).Clear();
      if (App.GlobalObjects.UploadsManager.Uploads.Count > 0)
      {
        ((UIElement) this.nouploads).put_Visibility((Visibility) 1);
        using (List<UploadInfo>.Enumerator enumerator = App.GlobalObjects.UploadsManager.Uploads.GetEnumerator())
        {
          while (enumerator.MoveNext())
            ((ICollection<object>) this.uploadsList.Items).Add((object) enumerator.Current);
        }
      }
      else
        ((UIElement) this.nouploads).put_Visibility((Visibility) 0);
    }

    private async void openButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      FileOpenPicker fileOpenPicker = new FileOpenPicker();
      fileOpenPicker.put_SuggestedStartLocation((PickerLocationId) 7);
      fileOpenPicker.FileTypeFilter.Add(".mp4");
      fileOpenPicker.FileTypeFilter.Add(".avi");
      fileOpenPicker.FileTypeFilter.Add(".mpg");
      fileOpenPicker.FileTypeFilter.Add(".mpeg");
      fileOpenPicker.FileTypeFilter.Add(".m4v");
      fileOpenPicker.FileTypeFilter.Add(".mov");
      try
      {
        fileOpenPicker.PickSingleFileAndContinue();
      }
      catch
      {
      }
    }

    public async Task SelectFile(IStorageFile file)
    {
      try
      {
        if (file == null)
          return;
        try
        {
          StorageItemThumbnail thumbnailAsync = await (file as StorageFile).GetThumbnailAsync((ThumbnailMode) 1, 256U);
          if (thumbnailAsync != null)
          {
            ((UIElement) this.thumbGrid).put_Visibility((Visibility) 0);
            ((UIElement) this.openButton).put_Visibility((Visibility) 1);
            ((UIElement) this.submitButton).put_Visibility((Visibility) 0);
            ((BitmapSource) this.videoBitmap).SetSource((IRandomAccessStream) thumbnailAsync);
          }
        }
        catch
        {
          ((UIElement) this.thumbGrid).put_Visibility((Visibility) 0);
          ((UIElement) this.openButton).put_Visibility((Visibility) 1);
          ((UIElement) this.submitButton).put_Visibility((Visibility) 0);
          this.videoBitmap.put_UriSource(new Uri("ms-appx:///Images/NoThumb.jpg"));
        }
        this.videoTitle.put_Text(((IStorageItem) file).Name);
        this.selectedFile = file;
      }
      catch
      {
      }
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.Parameter is IStorageFile)
        this.SelectFile(e.Parameter as IStorageFile);
      if (e.NavigationMode != 1)
      {
        this.overCanvas.ScrollToPage(0, true);
        this.scroll.ChangeView(new double?(), new double?(0.0), new float?(), true);
      }
      base.OnNavigatedTo(e);
    }

    private async void submitButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.selectedFile == null)
      {
        this.openButton_Tapped(sender, e);
        this.scroll.ChangeView(new double?(), new double?(0.0), new float?());
      }
      else if (string.IsNullOrWhiteSpace(this.titleBox.Text))
      {
        IUICommand iuiCommand1 = await new MessageDialog("Please enter a title.", "Not yet").ShowAsync();
      }
      else if (string.IsNullOrWhiteSpace(this.descriptionBox.Text))
      {
        IUICommand iuiCommand2 = await new MessageDialog("Please enter a description.", "Not yet").ShowAsync();
        ((Control) this.descriptionBox).Focus((FocusState) 2);
      }
      else if (((Collection<string>) this.tags).Count == 0)
      {
        IUICommand iuiCommand3 = await new MessageDialog("Please add at least one tag.", "Not yet").ShowAsync();
      }
      else if (((Selector) this.categoryBox).SelectedItem == null)
      {
        IUICommand iuiCommand4 = await new MessageDialog("Please select a category", "Not yet").ShowAsync();
      }
      else
      {
        ((UIElement) this.submitButton).put_IsHitTestVisible(false);
        Ani.Begin((DependencyObject) this.submitButton, "Opacity", 0.5, 0.3);
        YouTubeEntry entry = new YouTubeEntry();
        entry.ID = (string) null;
        entry.Author = (string) null;
        entry.AuthorDisplayName = (string) null;
        entry.Title = this.titleBox.Text.Trim();
        entry.Description = this.descriptionBox.Text.Trim();
        entry.Embeddable = ((ToggleButton) this.embeddableCheckBox).IsChecked.Value;
        YouTubeEntry youTubeEntry = entry;
        bool? isChecked = ((ToggleButton) this.extendedStatsCheckBox).IsChecked;
        int num = isChecked.Value ? 1 : 0;
        youTubeEntry.PublicStatsVisible = num != 0;
        entry.Category = (((Selector) this.categoryBox).SelectedItem as CategoryTextInfo).Category;
        switch (((Selector) this.privacyBox).SelectedIndex)
        {
          case 0:
            entry.PrivacyStatus = PrivacyStatus.Public;
            break;
          case 1:
            entry.PrivacyStatus = PrivacyStatus.Private;
            break;
          case 2:
            entry.PrivacyStatus = PrivacyStatus.Unlisted;
            break;
        }
        switch (((Selector) this.licenseBox).SelectedIndex)
        {
          case 0:
            entry.License = License.YouTube;
            break;
          case 1:
            entry.License = License.CreativeCommons;
            break;
        }
        isChecked = ((ToggleButton) this.uploadDateCheckBox).IsChecked;
        if (isChecked.Value)
        {
          DateTimeOffset dateTimeOffset = this.publishAtDatePicker.Date + this.publishAtTimePicker.Time;
          entry.PublishAt = !(dateTimeOffset < DateTimeOffset.Now + TimeSpan.FromMinutes(10.0)) ? new DateTimeOffset?(dateTimeOffset.ToUniversalTime()) : new DateTimeOffset?();
        }
        else
          entry.PublishAt = new DateTimeOffset?();
        foreach (string tag in (Collection<string>) this.tags)
          entry.Tags.Add(tag);
        try
        {
          BasicProperties basicPropertiesAsync = await ((IStorageItem) this.selectedFile).GetBasicPropertiesAsync();
          ((IList<object>) this.uploadsList.Items).Insert(0, (object) await App.GlobalObjects.UploadsManager.GetUploadInfo(await App.GlobalObjects.UploadsManager.StartUpload(this.selectedFile, await YouTube.GetUploadUri(entry, this.selectedFile.ContentType, basicPropertiesAsync.Size), entry.Title)));
          ((UIElement) this.nouploads).put_Visibility((Visibility) 1);
          TextBox titleBox = this.titleBox;
          TextBox descriptionBox = this.descriptionBox;
          string str1;
          this.tagBox.put_Text(str1 = "");
          string str2;
          string str3 = str2 = str1;
          descriptionBox.put_Text(str2);
          string str4 = str3;
          titleBox.put_Text(str4);
          ((UIElement) this.openButton).put_Visibility((Visibility) 0);
          ((UIElement) this.thumbGrid).put_Visibility((Visibility) 1);
          this.selectedFile = (IStorageFile) null;
          ((Collection<string>) this.tags).Clear();
          this.overCanvas.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) this.uploadsScroll), false);
        }
        catch (Exception ex)
        {
          new MessageDialog("We weren't able to upload this video" + (Settings.UserMode >= UserMode.Beta ? "\n" + (object) ex : ""), "Error").ShowAsync();
        }
        ((UIElement) this.submitButton).put_IsHitTestVisible(true);
        Ani.Begin((DependencyObject) this.submitButton, "Opacity", 1.0, 0.3);
      }
    }

    private void addTagButton_Tapped(object sender, TappedRoutedEventArgs e) => this.addTag();

    private void tagBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
      if (e.Key != 13)
        return;
      e.put_Handled(true);
      this.addTag();
    }

    private void addTag()
    {
      if (string.IsNullOrWhiteSpace(this.tagBox.Text) || ((Collection<string>) this.tags).Contains(this.tagBox.Text.Trim()))
        return;
      string str1 = this.tagBox.Text.Trim();
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
        ((Collection<string>) this.tags).Add(str2.Trim());
      this.tagBox.put_Text("");
      ((Control) this.tagBox).Focus((FocusState) 2);
    }

    private void enableAddTag(bool enable)
    {
      if (enable)
      {
        if (this.addTagEnabled)
          return;
        bool flag;
        ((UIElement) this.addTagButton).put_IsHitTestVisible(flag = true);
        this.addTagEnabled = flag;
        Ani.Begin((DependencyObject) this.addTagButton, "Opacity", 1.0, 0.2);
      }
      else
      {
        if (!this.addTagEnabled)
          return;
        bool flag;
        ((UIElement) this.addTagButton).put_IsHitTestVisible(flag = false);
        this.addTagEnabled = flag;
        Ani.Begin((DependencyObject) this.addTagButton, "Opacity", 0.5, 0.2);
      }
    }

    private void deleteTagButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement) || !(frameworkElement.DataContext is string dataContext) || !((Collection<string>) this.tags).Contains(dataContext))
        return;
      ((Collection<string>) this.tags).Remove(dataContext);
    }

    private void tagBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.tagBox.Text.Length < 2)
        this.enableAddTag(false);
      else
        this.enableAddTag(true);
    }

    private void moreButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      ((UIElement) this.moreButton).put_Visibility((Visibility) 1);
      ((UIElement) this.morePanel).put_Visibility((Visibility) 0);
    }

    private void titleBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
      if (e.Key != 13)
        return;
      e.put_Handled(true);
      ((Control) this.descriptionBox).Focus((FocusState) 2);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///UploaderPage.xaml"), (ComponentResourceLocation) 0);
      this.videoBitmap = (BitmapImage) ((FrameworkElement) this).FindName("videoBitmap");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.viewGrid = (Grid) ((FrameworkElement) this).FindName("viewGrid");
      this.uploadsScroll = (ScrollViewer) ((FrameworkElement) this).FindName("uploadsScroll");
      this.nouploads = (TextBlock) ((FrameworkElement) this).FindName("nouploads");
      this.uploadsList = (ItemsControl) ((FrameworkElement) this).FindName("uploadsList");
      this.scroll = (ScrollViewer) ((FrameworkElement) this).FindName("scroll");
      this.internalGrid = (StackPanel) ((FrameworkElement) this).FindName("internalGrid");
      this.openButton = (ContentControl) ((FrameworkElement) this).FindName("openButton");
      this.thumbGrid = (Grid) ((FrameworkElement) this).FindName("thumbGrid");
      this.titleBox = (TextBox) ((FrameworkElement) this).FindName("titleBox");
      this.descriptionBox = (TextBox) ((FrameworkElement) this).FindName("descriptionBox");
      this.categoryBox = (ComboBox) ((FrameworkElement) this).FindName("categoryBox");
      this.privacyBox = (ComboBox) ((FrameworkElement) this).FindName("privacyBox");
      this.licenseBox = (ComboBox) ((FrameworkElement) this).FindName("licenseBox");
      this.moreButton = (ContentControl) ((FrameworkElement) this).FindName("moreButton");
      this.morePanel = (StackPanel) ((FrameworkElement) this).FindName("morePanel");
      this.submitButton = (ContentControl) ((FrameworkElement) this).FindName("submitButton");
      this.uploadDateCheckBox = (CheckBox) ((FrameworkElement) this).FindName("uploadDateCheckBox");
      this.publishAtDatePicker = (DatePicker) ((FrameworkElement) this).FindName("publishAtDatePicker");
      this.publishAtTimePicker = (TimePicker) ((FrameworkElement) this).FindName("publishAtTimePicker");
      this.embeddableCheckBox = (CheckBox) ((FrameworkElement) this).FindName("embeddableCheckBox");
      this.extendedStatsCheckBox = (CheckBox) ((FrameworkElement) this).FindName("extendedStatsCheckBox");
      this.tagBox = (TextBox) ((FrameworkElement) this).FindName("tagBox");
      this.addTagButton = (ContentControl) ((FrameworkElement) this).FindName("addTagButton");
      this.tagsControl = (ItemsControl) ((FrameworkElement) this).FindName("tagsControl");
      this.thumbControl = (Control16by9) ((FrameworkElement) this).FindName("thumbControl");
      this.videoTitle = (TextBlock) ((FrameworkElement) this).FindName("videoTitle");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.deleteTagButton_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.openButton_Tapped));
          break;
        case 3:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.openButton_Tapped));
          break;
        case 4:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>(new Func<KeyEventHandler, EventRegistrationToken>(uiElement4.add_KeyDown), new Action<EventRegistrationToken>(uiElement4.remove_KeyDown), new KeyEventHandler(this.titleBox_KeyDown));
          break;
        case 5:
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement5.add_Tapped), new Action<EventRegistrationToken>(uiElement5.remove_Tapped), new TappedEventHandler(this.moreButton_Tapped));
          break;
        case 6:
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement6.add_Tapped), new Action<EventRegistrationToken>(uiElement6.remove_Tapped), new TappedEventHandler(this.submitButton_Tapped));
          break;
        case 7:
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>(new Func<KeyEventHandler, EventRegistrationToken>(uiElement7.add_KeyDown), new Action<EventRegistrationToken>(uiElement7.remove_KeyDown), new KeyEventHandler(this.tagBox_KeyDown));
          TextBox textBox = (TextBox) target;
          WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>(new Func<TextChangedEventHandler, EventRegistrationToken>(textBox.add_TextChanged), new Action<EventRegistrationToken>(textBox.remove_TextChanged), new TextChangedEventHandler(this.tagBox_TextChanged));
          break;
        case 8:
          UIElement uiElement8 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement8.add_Tapped), new Action<EventRegistrationToken>(uiElement8.remove_Tapped), new TappedEventHandler(this.addTagButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
