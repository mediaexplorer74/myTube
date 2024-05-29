// myTube.Popups.DevicePickerFlyout

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.Popups
{
  public sealed partial class DevicePickerFlyout : UserControl
  {
        //TEMP
    private DispatcherTimer timer = new DispatcherTimer();
    private DataTemplate itemTemplate = new DataTemplate();
    private ListView listView = new ListView();
    private ProgressBar progress = new ProgressBar();
    private ContentControl dontCastControl = new ContentControl();
    

    public event EventHandler<DeviceInformation> ItemSelected;

    public object ItemsSource
    {
      set => ((ItemsControl) this.listView).ItemsSource = value;
    }

    public DevicePickerFlyout()
    {
      //this.InitializeComponent();
   

     ((FrameworkElement)this).Unloaded += this.DevicePickerFlyout_Unloaded;
            
     DispatcherTimer dispatcherTimer = new DispatcherTimer();
      dispatcherTimer.Interval = TimeSpan.FromSeconds(12.0);
      this.timer = dispatcherTimer;
      DispatcherTimer timer = this.timer;

        this.timer.Tick += this.Timer_Tick;

        this.Loaded += this.DevicePickerFlyout_Loaded;
    }

    private void DevicePickerFlyout_Loaded(object sender, RoutedEventArgs e) => this.timer.Start();

    private void Timer_Tick(object sender, object e)
    {
      this.timer.Stop();
      ((UIElement) this.progress).Visibility = (Visibility) 1;
    }

    private void DevicePickerFlyout_Unloaded(object sender, RoutedEventArgs e)
    {
    }

    private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ItemSelected == null || !(((Selector) this.listView).SelectedItem is DeviceViewModel selectedItem))
        return;
      this.ItemSelected((object) this, selectedItem.Device);
    }

    private void dontCastControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.ItemSelected == null)
        return;
      this.ItemSelected((object) this, (DeviceInformation) null);
    }
   
  }
}
