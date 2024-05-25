using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace myTube
{
    internal class DefaultPage : CustomFrame
    {
        internal static object Current;
        internal CustomFrame Frame;

        public DefaultPage()
        {
        }

        internal static void SetPopupArrangeMethod(DependencyObject popup2, Func<Point> func)
        {
            throw new NotImplementedException();
        }

        internal void OpenBrowser()
        {
            throw new NotImplementedException();
        }
    }
}