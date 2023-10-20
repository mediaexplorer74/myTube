// Decompiled with JetBrains decompiler
// Type: myTube.TileData
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.StartScreen;

namespace myTube
{
  public class TileData
  {
    private ApplicationDataCompositeValue values;
    private string key;
    private static Dictionary<string, TileData> tileData = new Dictionary<string, TileData>();

    public string CancellationReason
    {
      get => ((IDictionary<string, object>) this.values).ContainsKey(nameof (CancellationReason)) ? ((IDictionary<string, object>) this.values)[nameof (CancellationReason)] as string : "";
      set => ((IDictionary<string, object>) this.values)[nameof (CancellationReason)] = (object) value;
    }

    public bool SignedIn
    {
      get => ((IDictionary<string, object>) this.values).ContainsKey(nameof (SignedIn)) && (bool) ((IDictionary<string, object>) this.values)[nameof (SignedIn)];
      set => ((IDictionary<string, object>) this.values)[nameof (SignedIn)] = (object) value;
    }

    public int SecondsWaitedToSignIn
    {
      get => ((IDictionary<string, object>) this.values).ContainsKey(nameof (SecondsWaitedToSignIn)) ? (int) ((IDictionary<string, object>) this.values)[nameof (SecondsWaitedToSignIn)] : 0;
      set => ((IDictionary<string, object>) this.values)[nameof (SecondsWaitedToSignIn)] = (object) value;
    }

    public string Log
    {
      get => ((IDictionary<string, object>) this.values).ContainsKey(nameof (Log)) ? ((IDictionary<string, object>) this.values)[nameof (Log)] as string : "";
      set => ((IDictionary<string, object>) this.values)[nameof (Log)] = (object) value;
    }

    public string Exception
    {
      get => ((IDictionary<string, object>) this.values).ContainsKey(nameof (Exception)) ? ((IDictionary<string, object>) this.values)[nameof (Exception)] as string : (string) null;
      set => ((IDictionary<string, object>) this.values)[nameof (Exception)] = (object) value;
    }

    public string SignInException
    {
      get => ((IDictionary<string, object>) this.values).ContainsKey(nameof (SignInException)) ? ((IDictionary<string, object>) this.values)[nameof (SignInException)] as string : (string) null;
      set => ((IDictionary<string, object>) this.values)[nameof (SignInException)] = (object) value;
    }

    public DateTimeOffset LastRun
    {
      get => ((IDictionary<string, object>) this.values).ContainsKey(nameof (LastRun)) ? (DateTimeOffset) ((IDictionary<string, object>) this.values)[nameof (LastRun)] : DateTimeOffset.MinValue;
      set => ((IDictionary<string, object>) this.values)[nameof (LastRun)] = (object) value;
    }

    public int RenderTaskCount
    {
      get => ((IDictionary<string, object>) this.values).ContainsKey("RenderTasks") ? (int) ((IDictionary<string, object>) this.values)["RenderTasks"] : 0;
      set => ((IDictionary<string, object>) this.values)["RenderTasks"] = (object) value;
    }

    internal TileData(ApplicationDataCompositeValue vals, string key)
    {
      this.values = vals;
      this.key = key;
    }

    public void Save() => ((IDictionary<string, object>) ApplicationData.Current.LocalSettings.Values)[this.key] = (object) this.values;

    public override string ToString() => this.LastRun.ToString() + "\n\n" + this.Log + "\n\nSaved " + (object) this.RenderTaskCount + " new tile images\n\nWaited " + (object) this.SecondsWaitedToSignIn + " seconds to sigin in. " + (this.SignedIn ? (object) "Signed in." : (object) "Did not sign in.") + (string.IsNullOrEmpty(this.Exception) ? (object) "" : (object) ("\n\nException:\n" + this.Exception)) + (string.IsNullOrEmpty(this.SignInException) ? (object) "" : (object) ("\n\nSign in Exception:\n" + this.SignInException)) + (string.IsNullOrEmpty(this.CancellationReason) ? (object) "" : (object) ("\n\nTask canceled: " + this.CancellationReason));

    public static async void Clean()
    {
      try
      {
        ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
        string[] keys = ((IDictionary<string, object>) local.Values).Keys.ToArray<string>();
        for (int i = 0; i < keys.Length; ++i)
        {
          string key = keys[i];
          if (key.StartsWith("tileLog"))
          {
            bool hasId = false;
            foreach (SecondaryTile secondaryTile in (IEnumerable<SecondaryTile>) await SecondaryTile.FindAllAsync())
            {
              if (key.Contains(secondaryTile.TileId))
              {
                hasId = true;
                break;
              }
            }
            if (!hasId)
            {
              ((IDictionary<string, object>) local.Values).Remove(key);
              continue;
            }
          }
          key = (string) null;
        }
        local = (ApplicationDataContainer) null;
        keys = (string[]) null;
      }
      catch
      {
      }
    }

    public static TileData GetTileData(string tileId)
    {
      if (TileData.tileData.ContainsKey(tileId))
        return TileData.tileData[tileId];
      ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
      string key = "tileLog" + tileId;
      if (((IDictionary<string, object>) localSettings.Values).ContainsKey(key))
      {
        TileData tileData = new TileData((ApplicationDataCompositeValue) ((IDictionary<string, object>) localSettings.Values)[key], key);
        TileData.tileData.Add(tileId, tileData);
        return tileData;
      }
      ApplicationDataCompositeValue vals = new ApplicationDataCompositeValue();
      ((IDictionary<string, object>) localSettings.Values)[key] = (object) vals;
      TileData tileData1 = new TileData(vals, key);
      TileData.tileData.Add(tileId, tileData1);
      return tileData1;
    }
  }
}
