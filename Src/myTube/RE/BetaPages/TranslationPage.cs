// Decompiled with JetBrains decompiler
// Type: myTube.BetaPages.TranslationPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.BetaPages
{
  public sealed class TranslationPage : Page, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate collectionTemp;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton uploadButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock instructions;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentPresenter content;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run languageRun;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public TranslationPage()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(TranslationPage_DataContextChanged)));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.TranslationPage_Loaded));
    }

    private async void TranslationPage_Loaded(object sender, RoutedEventArgs e)
    {
      this.languageRun.put_Text(App.LanguageName);
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.TranslationPage_Loaded));
    }

    private void TranslationPage_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
    }

    private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      ((Control) this.uploadButton).put_IsEnabled(false);
      if (e.Parameter is StringFormatCollection)
      {
        StringFormatCollection s = e.Parameter as StringFormatCollection;
        this.overCanvas.Title = s.Name;
        ((FrameworkElement) this).put_DataContext(e.Parameter);
        if (s == StringFormatCollection.GlobalCollection)
        {
          ((UIElement) this.instructions).put_Visibility((Visibility) 0);
          StringClient client = new StringClient();
          string message = (string) null;
          bool seriousError = false;
          try
          {
            if (e.NavigationMode != 1)
            {
              string text = await client.GetString("myTube", App.CurrentCulture.TwoLetterISOLanguageName, "Translation");
              if (text != null)
              {
                try
                {
                  App.Strings.SetXML(XElement.Parse(text));
                }
                catch
                {
                  message = "Unable to parse XML for translation file";
                }
              }
            }
          }
          catch
          {
            seriousError = true;
            message = "Unable to get most recent version of translation file";
          }
          string temp = (string) null;
          try
          {
            temp = await client.GetString("myTube", "template", "Translation");
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
          client = (StringClient) null;
          message = (string) null;
          temp = (string) null;
        }
        else
          ((UIElement) this.instructions).put_Visibility((Visibility) 1);
        s = (StringFormatCollection) null;
      }
      ((Control) this.uploadButton).put_IsEnabled(true);
      base.OnNavigatedTo(e);
    }

    private async void uploadButton_Click(object sender, RoutedEventArgs e)
    {
      ((Control) this.uploadButton).put_IsEnabled(false);
      if (App.CurrentCulture.TwoLetterISOLanguageName == "en" && Settings.UserMode < UserMode.Owner)
      {
        new MessageDialog("You cannot translate to this language: " + App.LanguageName, "Upload rejected").ShowAsync();
      }
      else
      {
        UICommand yes = new UICommand("yes");
        UICommand uiCommand = new UICommand("no");
        MessageDialog messageDialog = new MessageDialog("You are about to upload the translation for " + App.LanguageName + ", is this correct?", "Are you sure?");
        messageDialog.Commands.Add((IUICommand) yes);
        messageDialog.Commands.Add((IUICommand) uiCommand);
        if (await messageDialog.ShowAsync() == yes)
        {
          StringFormatCollection.GlobalCollection.TrimAllStrings(App.Strings);
          StringClient stringClient = new StringClient();
          try
          {
            if (await stringClient.AddString(new StringItem()
            {
              AppName = "myTube",
              Language = App.CurrentCulture.TwoLetterISOLanguageName,
              Type = "Translation",
              Text = ((object) App.Strings.XML).ToString()
            }))
              new MessageDialog(App.LanguageName + " translation uploaded successfully.", "Success").ShowAsync();
            else
              new MessageDialog("The service reports that this translation was not uploaded successfully.", "Failure").ShowAsync();
          }
          catch (Exception ex)
          {
            new MessageDialog(ex.ToString(), "Upload error").ShowAsync();
          }
        }
        yes = (UICommand) null;
      }
      ((Control) this.uploadButton).put_IsEnabled(true);
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!(sender is TextBox textBox) || !(((FrameworkElement) textBox).DataContext is StringFormat dataContext) || string.IsNullOrWhiteSpace(textBox.Text))
        return;
      App.Strings[dataContext.Path] = textBox.Text;
    }

    private void TextBox_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is TextBox textBox) || !(((FrameworkElement) textBox).DataContext is StringFormat dataContext))
        return;
      string str = App.Strings[dataContext.Path];
      if (str == null)
        return;
      textBox.put_Text(str);
    }

    private void sectionTapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement) || !(frameworkElement.DataContext is StringFormatCollection dataContext))
        return;
      App.Instance.RootFrame.Navigate(typeof (TranslationPage), (object) dataContext);
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e) => App.Strings.SaveFile(App.CurrentCulture.TwoLetterISOLanguageName + ".xml");

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
    }

    private void titleGridLoaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement) || !(frameworkElement.DataContext is StringFormatCollection dataContext))
        return;
      if (dataContext.AllStringsTranslated(App.Strings))
        ((UIElement) frameworkElement).put_Opacity(0.5);
      else
        ((UIElement) frameworkElement).put_Opacity(1.0);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///BetaPages/TranslationPage.xaml"), (ComponentResourceLocation) 0);
      this.collectionTemp = (DataTemplate) ((FrameworkElement) this).FindName("collectionTemp");
      this.uploadButton = (AppBarButton) ((FrameworkElement) this).FindName("uploadButton");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.scroll = (ScrollViewer) ((FrameworkElement) this).FindName("scroll");
      this.instructions = (TextBlock) ((FrameworkElement) this).FindName("instructions");
      this.content = (ContentPresenter) ((FrameworkElement) this).FindName("content");
      this.languageRun = (Run) ((FrameworkElement) this).FindName("languageRun");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          TextBox textBox = (TextBox) target;
          WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>(new Func<TextChangedEventHandler, EventRegistrationToken>(textBox.add_TextChanged), new Action<EventRegistrationToken>(textBox.remove_TextChanged), new TextChangedEventHandler(this.TextBox_TextChanged));
          FrameworkElement frameworkElement1 = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(frameworkElement1.add_Loaded), new Action<EventRegistrationToken>(frameworkElement1.remove_Loaded), new RoutedEventHandler(this.TextBox_Loaded));
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(uiElement1.add_LostFocus), new Action<EventRegistrationToken>(uiElement1.remove_LostFocus), new RoutedEventHandler(this.TextBox_LostFocus));
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(uiElement2.add_GotFocus), new Action<EventRegistrationToken>(uiElement2.remove_GotFocus), new RoutedEventHandler(this.TextBox_GotFocus));
          break;
        case 2:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.sectionTapped));
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RightTappedEventHandler>(new Func<RightTappedEventHandler, EventRegistrationToken>(uiElement4.add_RightTapped), new Action<EventRegistrationToken>(uiElement4.remove_RightTapped), new RightTappedEventHandler(this.Grid_RightTapped));
          break;
        case 3:
          FrameworkElement frameworkElement2 = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(frameworkElement2.add_Loaded), new Action<EventRegistrationToken>(frameworkElement2.remove_Loaded), new RoutedEventHandler(this.titleGridLoaded));
          break;
        case 4:
          ButtonBase buttonBase = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase.add_Click), new Action<EventRegistrationToken>(buttonBase.remove_Click), new RoutedEventHandler(this.uploadButton_Click));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
