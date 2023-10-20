// Decompiled with JetBrains decompiler
// Type: myTube.SharedSettings
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using Windows.Storage;

namespace myTube
{
  public static class SharedSettings
  {
    private static ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
    private static ApplicationDataContainer roaming = ApplicationData.Current.RoamingSettings;

    public static bool NotificationsOnlyOnWifi
    {
      get
      {
        try
        {
          return !((IDictionary<string, object>) SharedSettings.local.Values).ContainsKey("notificationsOnlyOnWifi") || (bool) ((IDictionary<string, object>) SharedSettings.local.Values)["notificationsOnlyOnWifi"];
        }
        catch
        {
          return true;
        }
      }
      set => ((IDictionary<string, object>) SharedSettings.local.Values)["notificationsOnlyOnWifi"] = (object) value;
    }

    public static string NotificationsStatus
    {
      get => ((IDictionary<string, object>) SharedSettings.local.Values).ContainsKey("NotificationStatus") ? ((IDictionary<string, object>) SharedSettings.local.Values)["NotificationStatus"] as string : "Not run";
      set => ((IDictionary<string, object>) SharedSettings.local.Values)["NotificationStatus"] = (object) value;
    }

    public static bool SyncContacts
    {
      get => SharedSettings.local.GetValue<bool>(nameof (SyncContacts));
      set => SharedSettings.local.SetValue(nameof (SyncContacts), (object) value);
    }

    public static string SingularTaskStatus
    {
      get => SharedSettings.local.GetValue<string>(nameof (SingularTaskStatus), "Not run");
      set => SharedSettings.local.SetValue(nameof (SingularTaskStatus), (object) value);
    }

    public static string LiveTileTaskStatus
    {
      get => SharedSettings.local.GetValue<string>(nameof (LiveTileTaskStatus), "Not run");
      set => SharedSettings.local.SetValue(nameof (LiveTileTaskStatus), (object) value);
    }

    public static PlaylistPosition WatchLaterAddToPosition
    {
      get => SharedSettings.roaming.GetEnum<PlaylistPosition>("watchLaterPos", PlaylistPosition.End);
      set => SharedSettings.roaming.SetEnum("watchLaterPos", (object) value);
    }

    public static string LastBackgroundAudioLog
    {
      get => SharedSettings.local.GetValue<string>(nameof (LastBackgroundAudioLog), "Not run");
      set => SharedSettings.local.SetValue(nameof (LastBackgroundAudioLog), (object) value);
    }

    public static SignInInfo CurrentAccount
    {
      get
      {
        string s = SharedSettings.roaming.GetValue<string>("currentAccount");
        return s == null ? (SignInInfo) null : new SignInInfo(s);
      }
      set
      {
        if (value != null)
          SharedSettings.roaming.SetValue("currentAccount", (object) value.ToSaveString());
        else
          ((IDictionary<string, object>) SharedSettings.roaming.Values).Remove("currentAccount");
      }
    }

    public static MultipleSignInContainer Accounts
    {
      get
      {
        string s = SharedSettings.roaming.GetValue<string>("accounts");
        return s == null ? new MultipleSignInContainer() : new MultipleSignInContainer(s);
      }
      set => SharedSettings.roaming.SetValue("accounts", (object) value.ToSaveString());
    }

    public static string RefreshToken
    {
      get => SharedSettings.roaming.GetValue<string>("refreshToken");
      set => SharedSettings.roaming.SetValue("refreshToken", (object) value);
    }

    public static bool LiveMainTile
    {
      get => SharedSettings.local.GetValue<bool>(nameof (LiveMainTile), true);
      set => SharedSettings.local.SetValue(nameof (LiveMainTile), (object) value);
    }

    public static RegionInfo Region
    {
      get
      {
        string countryCode = SharedSettings.roaming.GetValue<string>(nameof (Region));
        return !string.IsNullOrWhiteSpace(countryCode)
                    ? RegionInfo.GetFromCode(countryCode)
                    : RegionInfo.Global;
      }
      set => SharedSettings.roaming.SetValue(nameof (Region), (object) value.CountryCode);
    }

    public static bool MainTilePinned
    {
      get => SharedSettings.local.GetValue<bool>(nameof (MainTilePinned), true);
      set => SharedSettings.local.SetValue(nameof (MainTilePinned), (object) value);
    }

    public static DateTimeOffset BackgroundAudioTaskLastStartedAt
    {
      get => SharedSettings.local.GetValue<DateTimeOffset>(nameof (BackgroundAudioTaskLastStartedAt), new DateTimeOffset(1991, 10, 14, 0, 0, 0, TimeSpan.FromSeconds(0.0)));
      set => SharedSettings.local.SetValue(nameof (BackgroundAudioTaskLastStartedAt), (object) value);
    }

    private static T GetEnum<T>(this ApplicationDataContainer settings, string name, T fallback = default (T)) where T : struct
    {
      T result;
      return ((IDictionary<string, object>) settings.Values).ContainsKey(name) && Enum.TryParse<T>(((IDictionary<string, object>) settings.Values)[name].ToString(), out result) ? result : fallback;
    }

        private static void SetEnum(this ApplicationDataContainer settings, string name, object value)
        {
            ((IDictionary<string, object>)settings.Values)[name] = (object)value.ToString();
        }

        private static T GetValue<T>(this ApplicationDataContainer settings, string name, T fallback = default)
        {
            return string.IsNullOrWhiteSpace(name) || !((IDictionary<string, object>)settings.Values).ContainsKey(name) ? fallback : (T)((IDictionary<string, object>)settings.Values)[name];
        }

        private static void SetValue(this ApplicationDataContainer settings, string name, object value)
        {
            ((IDictionary<string, object>)settings.Values)[name] = value;
        }
    }
}
