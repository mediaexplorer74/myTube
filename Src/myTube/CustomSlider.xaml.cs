// myTube.CustomSlider

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
using Windows.UI;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
    public sealed partial class CustomSlider : UserControl
    {
        public DependencyProperty SliderBackgroundProperty 
            = DependencyProperty.Register(nameof(SliderBackground), typeof(Brush), typeof(CustomSlider), 
                new PropertyMetadata((object)new SolidColorBrush(Colors.White)));
        public DependencyProperty SliderForegroundProperty 
            = DependencyProperty.Register(nameof(SliderForeground), typeof(Brush), typeof(CustomSlider), 
                new PropertyMetadata((object)null));

        public DependencyProperty ThumbForegroundProperty 
            = DependencyProperty.Register(nameof(ThumbForeground), typeof(Brush), typeof(CustomSlider), 
                new PropertyMetadata((object)null));

        public DependencyProperty MinimumProperty 
            = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(CustomSlider), 
                new PropertyMetadata((object)0));

        public DependencyProperty MaximumProperty 
            = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(CustomSlider), 
                new PropertyMetadata((object)100));

        public DependencyProperty ValueProperty 
            = DependencyProperty.Register(nameof(Value), typeof(double), typeof(CustomSlider), 
                new PropertyMetadata((object)50.0, new PropertyChangedCallback(CustomSlider.OnValueChanged)));

        private bool isChanging;

        //private ScaleTransform recTrans;
        //private TranslateTransform thumbTrans;        
        //private FrameworkElement thumbRec;

        public event EventHandler<SliderValueChangedEventArgs> ValueChanged;

        public Brush ThumbForeground
        {
            get => (Brush)((DependencyObject)this).GetValue(this.ThumbForegroundProperty);
            set => ((DependencyObject)this).SetValue(this.ThumbForegroundProperty, (object)value);
        }

        public double Minimum
        {
            get => (double)((DependencyObject)this).GetValue(this.MinimumProperty);
            set => ((DependencyObject)this).SetValue(this.MinimumProperty, (object)value);
        }

        public double Maximum
        {
            get => (double)((DependencyObject)this).GetValue(this.MaximumProperty);
            set => ((DependencyObject)this).SetValue(this.MaximumProperty, (object)value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomSlider sender = d as CustomSlider;
            try
            {
                sender.setPos((double)e.NewValue);
            }
            catch
            {
            }
            if (sender.ValueChanged == null)
                return;
            try
            {
                sender.ValueChanged((object)sender, new SliderValueChangedEventArgs()
                {
                    OldValue = (double)e.OldValue,
                    NewValue = (double)e.NewValue
                });
            }
            catch
            {
            }
        }

        private void setPos(double val)
        {
            double Dec = MyMath.BetweenValue(this.Minimum, this.Maximum, val);
            this.recTrans.ScaleX = Dec;
            this.thumbTrans.X = (MyMath.Between(0.0, ((FrameworkElement)this).ActualWidth 
                - ((FrameworkElement)this.thumbRec).Width, Dec));
        }

        public double Value
        {
            get => (double)((DependencyObject)this).GetValue(this.ValueProperty);
            set => ((DependencyObject)this).SetValue(this.ValueProperty, (object)value);
        }

        public Brush SliderBackground
        {
            get => (Brush)((DependencyObject)this).GetValue(this.SliderBackgroundProperty);
            set => ((DependencyObject)this).SetValue(this.SliderBackgroundProperty, (object)value);
        }

        public Brush SliderForeground
        {
            get => (Brush)((DependencyObject)this).GetValue(this.SliderBackgroundProperty);
            set => ((DependencyObject)this).SetValue(this.SliderBackgroundProperty, (object)value);
        }

        public bool IsChanging => this.isChanging;

        public CustomSlider()
        {
            this.InitializeComponent();
            ((UIElement)this).ManipulationMode = ManipulationModes.TranslateX;

            this.ManipulationStarted += CustomSlider_ManipulationStarted;
            this.ManipulationDelta += CustomSlider_ManipulationDelta;
            this.ManipulationCompleted += CustomSlider_ManipulationCompleted;
            this.SizeChanged += CustomSlider_SizeChanged;
        }

        private void CustomSlider_ManipulationCompleted(
          object sender,
          ManipulationCompletedRoutedEventArgs e)
        {
            this.isChanging = false;
        }

        private void CustomSlider_SizeChanged(object sender, SizeChangedEventArgs e) 
            => this.setPos(this.Value);

        public void SetRelativeValue(double rel)
        {
            this.Value = MyMath.Between(this.Minimum, this.Maximum, rel);
            this.setPos(this.Value);
        }

        private void CustomSlider_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.Value = MyMath.Clamp(this.Value + e.Delta.Translation.X 
                / ((FrameworkElement)this).ActualWidth * (this.Maximum - this.Minimum), 
                this.Minimum, this.Maximum);
            e.Handled = true;
            this.isChanging = true;
        }

        private void CustomSlider_ManipulationStarted(
          object sender,
          ManipulationStartedRoutedEventArgs e)
        {
            double x = e.Position.X;
            e.Handled = true;
            this.Value = MyMath.Between(this.Minimum, this.Maximum, 
                MyMath.BetweenValue(0.0, ((FrameworkElement)this).ActualWidth, x));
            this.isChanging = true;
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            this.Value = MyMath.Between(this.Minimum, this.Maximum, MyMath.BetweenValue(0.0, 
                ((FrameworkElement)this).ActualWidth, e.GetPosition((UIElement)this).X));
            e.Handled = true;
            base.OnTapped(e);
        }     
    }
}
