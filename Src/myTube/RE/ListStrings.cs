// Decompiled with JetBrains decompiler
// Type: myTube.ListStrings
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using Windows.UI.Xaml;

namespace myTube
{
  public class ListStrings : DependencyObject
  {
    public static DependencyProperty DefaultProperty = DependencyProperty.Register(nameof (Default), typeof (string), typeof (ListStrings), new PropertyMetadata((object) null, new PropertyChangedCallback(ListStrings.OnDefaultChanged)));
    public static DependencyProperty SignInProperty = DependencyProperty.Register(nameof (SignIn), typeof (string), typeof (ListStrings), new PropertyMetadata((object) null, new PropertyChangedCallback(ListStrings.OnSignInChanged)));
    public static DependencyProperty LoadingProperty = DependencyProperty.Register(nameof (Loading), typeof (string), typeof (ListStrings), new PropertyMetadata((object) null, new PropertyChangedCallback(ListStrings.OnLoadingChanged)));
    public static DependencyProperty NoItemsProperty = DependencyProperty.Register(nameof (NoItems), typeof (string), typeof (ListStrings), new PropertyMetadata((object) null, new PropertyChangedCallback(ListStrings.OnNoItemsChanged)));
    public static DependencyProperty CurrentProperty = DependencyProperty.Register(nameof (Current), typeof (string), typeof (ListStrings), new PropertyMetadata((object) null));
    public static DependencyProperty StateProperty = DependencyProperty.Register(nameof (State), typeof (ListState), typeof (ListStrings), new PropertyMetadata((object) ListState.Default, new PropertyChangedCallback(ListStrings.OnStatePropertyChanged)));

    private static void OnDefaultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ListStrings.changed(d, e, ListState.Default);

    private static void OnSignInChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ListStrings.changed(d, e, ListState.SignIn);

    private static void OnLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ListStrings.changed(d, e, ListState.Loading);

    private static void OnNoItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ListStrings.changed(d, e, ListState.NoItems);

    private static void changed(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e,
      ListState state)
    {
      ListStrings listStrings = d as ListStrings;
      string newValue = (string) e.NewValue;
      if (listStrings.State != state)
        return;
      listStrings.Current = newValue;
    }

    private static void OnStatePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ListStrings listStrings = d as ListStrings;
      switch ((ListState) e.NewValue)
      {
        case ListState.Default:
          listStrings.Current = listStrings.Default;
          break;
        case ListState.SignIn:
          listStrings.Current = listStrings.SignIn;
          break;
        case ListState.Loading:
          listStrings.Current = listStrings.Loading;
          break;
        case ListState.NoItems:
          listStrings.Current = listStrings.NoItems;
          break;
        default:
          listStrings.Current = listStrings.Default;
          break;
      }
    }

    public string Default
    {
      get => (string) this.GetValue(ListStrings.DefaultProperty);
      set => this.SetValue(ListStrings.DefaultProperty, (object) value);
    }

    public string SignIn
    {
      get => (string) this.GetValue(ListStrings.SignInProperty);
      set => this.SetValue(ListStrings.SignInProperty, (object) value);
    }

    public string Loading
    {
      get => (string) this.GetValue(ListStrings.LoadingProperty);
      set => this.SetValue(ListStrings.LoadingProperty, (object) value);
    }

    public string NoItems
    {
      get => (string) this.GetValue(ListStrings.NoItemsProperty);
      set => this.SetValue(ListStrings.NoItemsProperty, (object) value);
    }

    public string Current
    {
      get => (string) this.GetValue(ListStrings.CurrentProperty);
      private set => this.SetValue(ListStrings.CurrentProperty, (object) value);
    }

    public ListState State
    {
      get => (ListState) this.GetValue(ListStrings.StateProperty);
      set => this.SetValue(ListStrings.StateProperty, (object) value);
    }
  }
}
