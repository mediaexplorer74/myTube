// Decompiled with JetBrains decompiler
// Type: myTube.Activities.ActivityListTemplateSelector
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace myTube.Activities
{
  public class ActivityListTemplateSelector : DataTemplateSelector
  {
    private ActivitiesList list;

    public ActivityListTemplateSelector(ActivitiesList list) => this.list = list;

    protected virtual DataTemplate SelectTemplateCore(object item) => item is YouTubeEntry youTubeEntry ? this.list.GetDataTemplate(youTubeEntry.ActivityType) : base.SelectTemplateCore(item);

    protected virtual DataTemplate SelectTemplateCore(object item, DependencyObject container) => item is YouTubeEntry youTubeEntry ? this.list.GetDataTemplate(youTubeEntry.ActivityType) : base.SelectTemplateCore(item, container);
  }
}
