﻿// Decompiled with JetBrains decompiler
// Type: ChannelNotificationsTask.IRegularTaskStatic
// Assembly: ChannelNotificationsTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime
// MVID: 41BF73AA-BC72-4F06-8835-20BEA97ACF00
// Assembly location: C:\Users\Admin\Desktop\RE\myTubeBeta_3.9.23.0\ChannelNotificationsTask.winmd

using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Foundation.Metadata;

#nullable disable
namespace ChannelNotificationsTask
{
  [Guid(2857456021, 27957, 23120, 75, 170, 54, 174, 245, 105, 18, 148)]
  [Version(16777216)]
  [ExclusiveTo(typeof (RegularTask))]
  internal interface IRegularTaskStatic
  {
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    IAsyncAction UpdateNotifications();
  }
}
