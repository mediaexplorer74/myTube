// myTube.VideoContextMenu

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
    public partial class VideoContextMenu : UserControl
    {
        public DependencyProperty ItemsSourceProperty
                = DependencyProperty.Register(nameof(ItemsSource),
                    typeof(List<IconButtonEvent>), typeof(VideoContextMenu),
                    new PropertyMetadata((object)null));
        public DependencyProperty SelectButtonEnabledProperty
                = DependencyProperty.Register(nameof(SelectButtonEnabled),
                    typeof(bool), typeof(VideoContextMenu),
                    new PropertyMetadata((object)false));
        public DependencyProperty CancelButtonEnabledProperty
                = DependencyProperty.Register(nameof(CancelButtonEnabled),
                    typeof(bool), typeof(VideoContextMenu),
                    new PropertyMetadata((object)false));

        public FrameworkElement SelectedElement;
        private TaskCompletionSource<object> waitForObjectTcs;
     
        //TEMP
        //private UserControl userControl;     
     
        //private TransitionCollection transitions;
        
        //private ListView itemsControl;

        //private ContentControl closeButton;      
        //private Rectangle backgroundRec;        

        public VideoContextMenu()
        {
            this.InitializeComponent();

            this.Loaded += VideoContextMenu_Loaded;
            this.Unloaded += VideoContextMenu_Unloaded;
        }

        public List<IconButtonEvent> ItemsSource
        {
            get => (List<IconButtonEvent>)((DependencyObject)this).GetValue(this.ItemsSourceProperty);
            set => ((DependencyObject)this).SetValue(this.ItemsSourceProperty, (object)value);
        }

        public bool SelectButtonEnabled
        {
            get => (bool)((DependencyObject)this).GetValue(this.SelectButtonEnabledProperty);
            set => ((DependencyObject)this).SetValue(this.SelectButtonEnabledProperty, (object)value);
        }

        public bool CancelButtonEnabled
        {
            get => (bool)((DependencyObject)this).GetValue(this.CancelButtonEnabledProperty);
            set => ((DependencyObject)this).SetValue(this.CancelButtonEnabledProperty, (object)value);
        }
              

        private void VideoContextMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.waitForObjectTcs == null)
                return;
            this.waitForObjectTcs.TrySetResult((object)null);
        }

        private void VideoContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public event EventHandler SelectTapped;

        public event EventHandler CancelTapped;

        public void SetTransitionOffset(double horizontal, double vertical)
        {
            foreach (Transition transition in (IEnumerable<Transition>)this.transitions)
            {
                if (transition is EntranceThemeTransition)
                {
                    (transition as EntranceThemeTransition).FromHorizontalOffset = (horizontal);
                    (transition as EntranceThemeTransition).FromVerticalOffset = (vertical);
                    (transition as EntranceThemeTransition).IsStaggeringEnabled = (true);
                }
            }
        }

        public Task<object> WaitForDataContext()
        {
            if (this.waitForObjectTcs == null)
                this.waitForObjectTcs = new TaskCompletionSource<object>();
            return this.waitForObjectTcs.Task;
        }

        private async void ItemTapped(object sender, TappedRoutedEventArgs e)
        {
            ((UIElement)this).IsHitTestVisible = (false);
            FrameworkElement s = sender as FrameworkElement;
            if (s == null || !(s.DataContext is IconButtonEvent))
                return;
            ((UIElement)s).IsHitTestVisible = (false);
            Task selectedTask = (s.DataContext as IconButtonEvent)
                      .CallSelected(((FrameworkElement)this).DataContext ?? (object)this.SelectedElement);
            if (this.waitForObjectTcs != null)
            {
                this.waitForObjectTcs.TrySetResult((s.DataContext as IconButtonEvent).DataContext);
                this.waitForObjectTcs = (TaskCompletionSource<object>)null;
            }
            Storyboard ani = new Storyboard();
            foreach (UIElement child in (IEnumerable<UIElement>)this.itemsControl.ItemsPanelRoot.Children)
            {
                if ((child as FrameworkElement).DataContext != s.DataContext)
                    ani.Add((Timeline)Ani.DoubleAni((DependencyObject)child, "Opacity", 0.0, 0.1));
                child.IsHitTestVisible = (false);
            }
            await Task.Delay(100);
            ani.Begin();
            Storyboard storyboard1 = ani;

           
            EventHandler<object> handler = async (_param1_1, _param2_1) =>
            {
                await selectedTask;
                Storyboard storyboard2 = Ani.Begin((DependencyObject)s, "Opacity", 0.0, 0.1);
                storyboard2.Completed += (_param1_2, _param2_2) =>
                {
                    Helper.FindParent<Popup>((FrameworkElement)this, 100).IsOpen = false;
                };
            };

            storyboard1.Completed += handler;

            ani = (Storyboard)null;
        }

        private void userControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = e.NewSize.Width;
            if (width > 700.0)
                ((FrameworkElement)this.itemsControl).Margin = (new Thickness(76.0, 0.0, 76.0, 0.0));
            else if (width > 600.0)
                ((FrameworkElement)this.itemsControl).Margin = (new Thickness(57.0, 0.0, 57.0, 0.0));
            else if (width > 400.0)
                ((FrameworkElement)this.itemsControl).Margin = (new Thickness(38.0, 0.0, 38.0, 0.0));
            else
                ((FrameworkElement)this.itemsControl).Margin = (new Thickness(19.0, 0.0, 19.0, 0.0));
        }

        private void IconTextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.SelectTapped == null)
                return;
            this.SelectTapped(sender, new EventArgs());
        }

        private void closeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.CancelTapped == null)
                return;
            this.CancelTapped(sender, new EventArgs());
        }


    }
}

