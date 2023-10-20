// Decompiled with JetBrains decompiler
// Type: myTube.BetaPages.StringFormatPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using myTube.Cloud.Clients;
using myTube.Helpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace myTube.BetaPages
{
  public sealed class StringFormatPage : Page, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate collectionTemp;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate newSectionTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate editSectionTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton uploadButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Border newSection;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Border newString;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public StringFormatPage() => this.InitializeComponent();

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.Parameter is StringFormatCollection)
      {
        StringFormatCollection s = e.Parameter as StringFormatCollection;
        this.overCanvas.Title = s.Name;
        OverCanvas.SetOverCanvasTitle((DependencyObject) this.scroll, s.Path);
        ((FrameworkElement) this).put_DataContext(e.Parameter);
        if (s == StringFormatCollection.GlobalCollection)
        {
          StringClient stringClient = new StringClient();
          string temp = (string) null;
          string message = (string) null;
          bool seriousError = false;
          try
          {
            temp = await stringClient.GetString("myTube", "template", "Translation");
          }
          catch
          {
            seriousError = true;
            message = "Unable to get the string format";
          }
          if (temp != null)
          {
            try
            {
              s.FromXML(XElement.Parse(temp));
            }
            catch (Exception ex)
            {
              message = "Error setting string format from xml \n" + temp + "\n\n\n" + (object) ex;
            }
          }
          if (message != null)
          {
            IUICommand iuiCommand = await new MessageDialog(message).ShowAsync();
            if (seriousError)
              App.Instance.RootFrame.GoBack();
          }
          temp = (string) null;
          message = (string) null;
        }
        s = (StringFormatCollection) null;
      }
      base.OnNavigatedTo(e);
    }

    private async void newSection_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Border border1 = new Border();
      border1.put_Background((Brush) App.Instance.GetThemeResource("PopupBackground"));
      ((FrameworkElement) border1).put_Width(360.0);
      ((FrameworkElement) border1).put_Height(400.0);
      border1.put_Padding(new Thickness(19.0));
      Border border2 = border1;
      ContentControl contentControl = new ContentControl();
      ((FrameworkElement) contentControl).put_Width(322.0);
      ((Control) contentControl).put_HorizontalContentAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) contentControl).put_VerticalAlignment((VerticalAlignment) 2);
      ((FrameworkElement) contentControl).put_HorizontalAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) contentControl).put_DataContext(((FrameworkElement) this).DataContext);
      contentControl.put_ContentTemplate(this.newSectionTemplate);
      ContentControl c = contentControl;
      border2.put_Child((UIElement) c);
      Popup popup = new Popup();
      Rect bounds = ((FrameworkElement) this.newSection).GetBounds(Window.Current.Content);
      Point position = new Point();
      position.X = bounds.Center().X - ((FrameworkElement) border2).Width / 2.0;
      position.Y = bounds.Top - ((FrameworkElement) border2).Height - 19.0;
      if (position.Y < 0.0)
        position.Y = 0.0;
      popup.put_Child((UIElement) border2);
      Task<bool> showTask = DefaultPage.Current.ShowPopup(popup, position, new Point(0.0, 80.0));
      await Task.Delay(300);
      TranslateTransform trans = new TranslateTransform();
      ((UIElement) c).put_RenderTransform((Transform) trans);
      InputPaneHelper.Register((FrameworkElement) c, trans);
      List<TextBox> textBoxes = Helper.FindChildren<TextBox>((DependencyObject) c, 50);
      foreach (UIElement child in Helper.FindChildren<ComboBox>((DependencyObject) c, 50))
        child.put_Visibility((Visibility) 1);
      List<Button> children = Helper.FindChildren<Button>((DependencyObject) c, 200);
      bool save = false;
      Button button = Enumerable.Last<Button>((IEnumerable<Button>) children);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) button).add_Click), new Action<EventRegistrationToken>(((ButtonBase) button).remove_Click), (RoutedEventHandler) ((_param1, _param2) =>
      {
        bool flag = true;
        foreach (TextBox textBox in textBoxes)
        {
          if (string.IsNullOrWhiteSpace(textBox.Text))
          {
            ((Control) textBox).Focus((FocusState) 3);
            flag = false;
            break;
          }
        }
        if (!flag)
          return;
        save = true;
        DefaultPage.Current.ClosePopup();
      }));
      if (textBoxes.Count == 3)
      {
        int num = await showTask ? 1 : 0;
        if (save && ((FrameworkElement) this).DataContext is StringFormatCollection)
          (((FrameworkElement) this).DataContext as StringFormatCollection).AddStringFormatCollection(textBoxes[1].Text.ToLower(), textBoxes[0].Text, textBoxes[2].Text);
      }
      InputPaneHelper.Deregister((FrameworkElement) c);
    }

    private void newSectionButtonTapped(object sender, SizeChangedEventArgs e)
    {
    }

    private void sectionTapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement) || !(frameworkElement.DataContext is StringFormatCollection dataContext))
        return;
      App.Instance.RootFrame.Navigate(typeof (StringFormatPage), (object) dataContext);
    }

    private async void newString_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Border border1 = new Border();
      border1.put_Background((Brush) App.Instance.GetThemeResource("PopupBackground"));
      ((FrameworkElement) border1).put_Width(360.0);
      ((FrameworkElement) border1).put_Height(400.0);
      border1.put_Padding(new Thickness(19.0));
      Border border2 = border1;
      ContentControl contentControl = new ContentControl();
      ((FrameworkElement) contentControl).put_Width(322.0);
      ((Control) contentControl).put_HorizontalContentAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) contentControl).put_VerticalAlignment((VerticalAlignment) 2);
      ((FrameworkElement) contentControl).put_HorizontalAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) contentControl).put_DataContext(((FrameworkElement) this).DataContext);
      contentControl.put_ContentTemplate(this.newSectionTemplate);
      ContentControl c = contentControl;
      border2.put_Child((UIElement) c);
      Popup popup = new Popup();
      Rect bounds = ((FrameworkElement) this.newSection).GetBounds(Window.Current.Content);
      Point position = new Point();
      position.X = bounds.Center().X - ((FrameworkElement) border2).Width / 2.0;
      position.Y = bounds.Top - ((FrameworkElement) border2).Height - 19.0;
      popup.put_Child((UIElement) border2);
      Task<bool> showTask = DefaultPage.Current.ShowPopup(popup, position, new Point(0.0, 80.0));
      await Task.Delay(300);
      TranslateTransform trans = new TranslateTransform();
      ((UIElement) c).put_RenderTransform((Transform) trans);
      InputPaneHelper.Register((FrameworkElement) c, trans);
      List<TextBox> textBoxes = Helper.FindChildren<TextBox>((DependencyObject) c, 50);
      List<ComboBox> comboBoxes = Helper.FindChildren<ComboBox>((DependencyObject) c, 50);
      foreach (object name in Enum.GetNames(typeof (StringCaseType)))
        ((ICollection<object>) ((ItemsControl) comboBoxes[0]).Items).Add(name);
      ((Selector) comboBoxes[0]).put_SelectedIndex(0);
      List<Button> children = Helper.FindChildren<Button>((DependencyObject) c, 200);
      bool save = false;
      Button button = Enumerable.Last<Button>((IEnumerable<Button>) children);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) button).add_Click), new Action<EventRegistrationToken>(((ButtonBase) button).remove_Click), (RoutedEventHandler) (async (_param1, _param2) =>
      {
        bool allow = true;
        foreach (TextBox textBox in textBoxes)
        {
          if (string.IsNullOrWhiteSpace(textBox.Text))
          {
            ((Control) textBox).Focus((FocusState) 3);
            allow = false;
            break;
          }
        }
        if (textBoxes[1].Text.Contains(".") || textBoxes[1].Text.Contains(","))
        {
          allow = false;
          IUICommand iuiCommand = await new MessageDialog("No fullstops, commas, or other special characters").ShowAsync();
          ((Control) textBoxes[1]).Focus((FocusState) 3);
        }
        if (!allow)
          return;
        save = true;
        DefaultPage.Current.ClosePopup();
      }));
      if (textBoxes.Count >= 3)
      {
        int num = await showTask ? 1 : 0;
        if (save && ((FrameworkElement) this).DataContext is StringFormatCollection)
        {
          StringFormatCollection dataContext = ((FrameworkElement) this).DataContext as StringFormatCollection;
          dataContext.AddStringFormat(new StringFormat()
          {
            Name = textBoxes[0].Text,
            Description = textBoxes[2].Text,
            CaseType = (StringCaseType) ((Selector) comboBoxes[0]).SelectedIndex,
            Path = (string.IsNullOrWhiteSpace(dataContext.Path) ? "" : dataContext.Path + ".") + textBoxes[1].Text.ToLower()
          });
        }
      }
      InputPaneHelper.Deregister((FrameworkElement) c);
    }

    private async void editString_Tapped(object sender, TappedRoutedEventArgs e)
    {
      StringFormatCollection collection = ((FrameworkElement) this).DataContext as StringFormatCollection;
      e.put_Handled(true);
      Border border1 = new Border();
      border1.put_Background((Brush) App.Instance.GetThemeResource("PopupBackground"));
      ((FrameworkElement) border1).put_Width(360.0);
      ((FrameworkElement) border1).put_Height(400.0);
      border1.put_Padding(new Thickness(19.0));
      Border border2 = border1;
      ContentControl contentControl = new ContentControl();
      ((FrameworkElement) contentControl).put_Width(322.0);
      ((Control) contentControl).put_HorizontalContentAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) contentControl).put_VerticalAlignment((VerticalAlignment) 2);
      ((FrameworkElement) contentControl).put_HorizontalAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) contentControl).put_DataContext(((FrameworkElement) this).DataContext);
      contentControl.put_ContentTemplate(this.editSectionTemplate);
      ContentControl c = contentControl;
      border2.put_Child((UIElement) c);
      Popup popup = new Popup();
      Rect bounds = (sender as FrameworkElement).GetBounds(Window.Current.Content);
      Point position = new Point();
      position.X = bounds.X;
      position.Y = bounds.Top - ((FrameworkElement) border2).Height - 19.0;
      if (position.Y < 0.0)
        position.Y = 0.0;
      popup.put_Child((UIElement) border2);
      Task<bool> showTask = DefaultPage.Current.ShowPopup(popup, position, new Point(0.0, 80.0));
      await Task.Delay(300);
      TranslateTransform trans = new TranslateTransform();
      ((UIElement) c).put_RenderTransform((Transform) trans);
      InputPaneHelper.Register((FrameworkElement) c, trans);
      List<TextBox> textBoxes = Helper.FindChildren<TextBox>((DependencyObject) c, 50);
      StringFormat format = (sender as FrameworkElement).DataContext as StringFormat;
      textBoxes[0].put_Text(format.Name);
      textBoxes[1].put_Text(Enumerable.Last<string>((IEnumerable<string>) format.Path.Split('.')));
      textBoxes[2].put_Text(format.Description);
      List<ComboBox> comboBoxes = Helper.FindChildren<ComboBox>((DependencyObject) c, 50);
      foreach (object name in Enum.GetNames(typeof (StringCaseType)))
        ((ICollection<object>) ((ItemsControl) comboBoxes[0]).Items).Add(name);
      ((Selector) comboBoxes[0]).put_SelectedIndex((int) format.CaseType);
      List<Button> list = Enumerable.ToList<Button>(Enumerable.Where<Button>((IEnumerable<Button>) Helper.FindChildren<Button>((DependencyObject) c, 200), (Func<Button, bool>) (butt => ((ContentControl) butt).Content != null)));
      bool save = false;
      Button button1 = list[1];
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) button1).add_Click), new Action<EventRegistrationToken>(((ButtonBase) button1).remove_Click), (RoutedEventHandler) (async (_param1, _param2) =>
      {
        save = false;
        collection.DeleteStringFormat(format);
        DefaultPage.Current.ClosePopup();
      }));
      Button button2 = list[0];
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) button2).add_Click), new Action<EventRegistrationToken>(((ButtonBase) button2).remove_Click), (RoutedEventHandler) (async (_param1, _param2) =>
      {
        bool allow = true;
        foreach (TextBox textBox in textBoxes)
        {
          if (string.IsNullOrWhiteSpace(textBox.Text))
          {
            ((Control) textBox).Focus((FocusState) 3);
            allow = false;
            break;
          }
        }
        if (textBoxes[1].Text.Contains(".") || textBoxes[1].Text.Contains(","))
        {
          allow = false;
          IUICommand iuiCommand = await new MessageDialog("No fullstops, commas, or other special characters").ShowAsync();
          ((Control) textBoxes[1]).Focus((FocusState) 3);
        }
        if (!allow)
          return;
        save = true;
        DefaultPage.Current.ClosePopup();
      }));
      if (textBoxes.Count >= 3)
      {
        int num = await showTask ? 1 : 0;
        if (save && ((FrameworkElement) this).DataContext is StringFormatCollection)
        {
          StringFormatCollection dataContext = ((FrameworkElement) this).DataContext as StringFormatCollection;
          dataContext.DeleteStringFormat(format);
          dataContext.AddStringFormat(new StringFormat()
          {
            Name = textBoxes[0].Text,
            Description = textBoxes[2].Text,
            CaseType = (StringCaseType) ((Selector) comboBoxes[0]).SelectedIndex,
            Path = (string.IsNullOrWhiteSpace(dataContext.Path) ? "" : dataContext.Path + ".") + textBoxes[1].Text.ToLower()
          });
        }
      }
      InputPaneHelper.Deregister((FrameworkElement) c);
    }

    private async void uploadButton_Click(object sender, RoutedEventArgs e)
    {
      ((Control) this.uploadButton).put_IsEnabled(false);
      try
      {
        XElement xml = StringFormatCollection.GlobalCollection.GetXML();
        int num = await new StringClient().AddString(new StringItem()
        {
          Language = "template",
          AppName = "myTube",
          Text = ((object) xml).ToString(),
          Type = "Translation"
        }) ? 1 : 0;
      }
      catch (Exception ex)
      {
        new MessageDialog(ex.ToString(), "Upload error").ShowAsync();
      }
      ((Control) this.uploadButton).put_IsEnabled(true);
    }

    private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
      e.put_Handled(true);
      this.ShowCollectionEditMenu(sender);
    }

    private async void ShowCollectionEditMenu(object sender)
    {
      if (!(sender is FrameworkElement el))
        return;
      StringFormatCollection sfcoll = el.DataContext as StringFormatCollection;
      if (sfcoll == null)
        return;
      Border border1 = new Border();
      border1.put_Background((Brush) App.Instance.GetThemeResource("PopupBackground"));
      ((FrameworkElement) border1).put_Width(360.0);
      ((FrameworkElement) border1).put_Height(400.0);
      border1.put_Padding(new Thickness(19.0));
      Border border2 = border1;
      ContentControl contentControl = new ContentControl();
      ((FrameworkElement) contentControl).put_Width(322.0);
      ((Control) contentControl).put_HorizontalContentAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) contentControl).put_VerticalAlignment((VerticalAlignment) 2);
      ((FrameworkElement) contentControl).put_HorizontalAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) contentControl).put_DataContext(((FrameworkElement) this).DataContext);
      contentControl.put_ContentTemplate(this.editSectionTemplate);
      ContentControl c = contentControl;
      border2.put_Child((UIElement) c);
      Popup popup = new Popup();
      Rect bounds = el.GetBounds(Window.Current.Content);
      Point position = new Point();
      position.X = bounds.Center().X - ((FrameworkElement) border2).Width / 2.0;
      position.Y = bounds.Top - ((FrameworkElement) border2).Height - 19.0;
      if (position.Y < 0.0)
        position.Y = 0.0;
      popup.put_Child((UIElement) border2);
      Task<bool> showTask = DefaultPage.Current.ShowPopup(popup, position, new Point(0.0, 80.0));
      await Task.Delay(300);
      TranslateTransform trans = new TranslateTransform();
      ((UIElement) c).put_RenderTransform((Transform) trans);
      InputPaneHelper.Register((FrameworkElement) c, trans);
      List<TextBox> textBoxes = Helper.FindChildren<TextBox>((DependencyObject) c, 50);
      foreach (UIElement child in Helper.FindChildren<ComboBox>((DependencyObject) c, 50))
        child.put_Visibility((Visibility) 1);
      textBoxes[0].put_Text(sfcoll.Name);
      textBoxes[1].put_Text(Enumerable.Last<string>((IEnumerable<string>) sfcoll.Path.Split('.')));
      textBoxes[2].put_Text(sfcoll.Description);
      List<Button> list = Enumerable.ToList<Button>(Enumerable.Where<Button>((IEnumerable<Button>) Helper.FindChildren<Button>((DependencyObject) c, 5000), (Func<Button, bool>) (butt => ((ContentControl) butt).Content != null)));
      bool save = false;
      Button button1 = list[0];
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) button1).add_Click), new Action<EventRegistrationToken>(((ButtonBase) button1).remove_Click), (RoutedEventHandler) (async (_param1, _param2) =>
      {
        bool allow = true;
        foreach (TextBox textBox in textBoxes)
        {
          if (string.IsNullOrWhiteSpace(textBox.Text))
          {
            ((Control) textBox).Focus((FocusState) 3);
            allow = false;
            break;
          }
        }
        if (textBoxes[1].Text.Contains(".") || textBoxes[1].Text.Contains(",") || textBoxes[1].Text.Contains(" "))
        {
          allow = false;
          IUICommand iuiCommand = await new MessageDialog("No fullstops, commas, spaces or other special characters").ShowAsync();
          ((Control) textBoxes[1]).Focus((FocusState) 3);
        }
        if (!allow)
          return;
        save = true;
        DefaultPage.Current.ClosePopup();
      }));
      Button button2 = list[1];
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) button2).add_Click), new Action<EventRegistrationToken>(((ButtonBase) button2).remove_Click), (RoutedEventHandler) (async (_param1, _param2) =>
      {
        save = false;
        (((FrameworkElement) this).DataContext as StringFormatCollection).DeleteStringFormatCollection(sfcoll);
        DefaultPage.Current.ClosePopup();
      }));
      if (textBoxes.Count >= 3)
      {
        int num = await showTask ? 1 : 0;
        if (save && ((FrameworkElement) this).DataContext is StringFormatCollection)
        {
          StringFormatCollection dataContext = ((FrameworkElement) this).DataContext as StringFormatCollection;
          StringFormatCollection formatCollection = sfcoll;
          formatCollection.Name = textBoxes[0].Text;
          formatCollection.Description = textBoxes[2].Text;
          formatCollection.Path = (string.IsNullOrWhiteSpace(dataContext.Path) ? "" : dataContext.Path + ".") + textBoxes[1].Text.ToLower();
        }
      }
      InputPaneHelper.Deregister((FrameworkElement) c);
      c = (ContentControl) null;
      showTask = (Task<bool>) null;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///BetaPages/StringFormatPage.xaml"), (ComponentResourceLocation) 0);
      this.collectionTemp = (DataTemplate) ((FrameworkElement) this).FindName("collectionTemp");
      this.newSectionTemplate = (DataTemplate) ((FrameworkElement) this).FindName("newSectionTemplate");
      this.editSectionTemplate = (DataTemplate) ((FrameworkElement) this).FindName("editSectionTemplate");
      this.uploadButton = (AppBarButton) ((FrameworkElement) this).FindName("uploadButton");
      this.newSection = (Border) ((FrameworkElement) this).FindName("newSection");
      this.newString = (Border) ((FrameworkElement) this).FindName("newString");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.scroll = (ScrollViewer) ((FrameworkElement) this).FindName("scroll");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          FrameworkElement frameworkElement = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(frameworkElement.add_SizeChanged), new Action<EventRegistrationToken>(frameworkElement.remove_SizeChanged), new SizeChangedEventHandler(this.newSectionButtonTapped));
          break;
        case 2:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.editString_Tapped));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.sectionTapped));
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RightTappedEventHandler>(new Func<RightTappedEventHandler, EventRegistrationToken>(uiElement3.add_RightTapped), new Action<EventRegistrationToken>(uiElement3.remove_RightTapped), new RightTappedEventHandler(this.Grid_RightTapped));
          break;
        case 4:
          ButtonBase buttonBase = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase.add_Click), new Action<EventRegistrationToken>(buttonBase.remove_Click), new RoutedEventHandler(this.uploadButton_Click));
          break;
        case 5:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.newSection_Tapped));
          break;
        case 6:
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement5.add_Tapped), new Action<EventRegistrationToken>(uiElement5.remove_Tapped), new TappedEventHandler(this.newString_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
