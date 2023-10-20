// Decompiled with JetBrains decompiler
// Type: myTube.TilePinner
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
  public static class TilePinner
  {
    private const string Tag = "TilePinner";

    public static string TileBackgroundTaskName { get; set; }

    public static bool ShouldSetupSecondaryTasks { get; set; } = true;

    public static async Task PinListTile(
      TileArgs args,
      TypeConstructor constructor,
      string title,
      Func<FrameworkElement, RenderTargetBitmap, Task<RenderTargetBitmap>> renderTask)
    {
      if (string.IsNullOrWhiteSpace(TilePinner.TileBackgroundTaskName))
        throw new InvalidOperationException("TileBackgroundTaskName not set");
      SecondaryTile tile = await TileHelper.CreateTile(constructor, title, args);
      List<TileNotification> nots = await TileHelper.UpdateSecondaryTile(constructor, renderTask, false);
      if (await tile.RequestCreateAsync())
      {
        TileUpdater forSecondaryTile = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId);
        forSecondaryTile.Clear();
        forSecondaryTile.EnableNotificationQueue(true);
        foreach (TileNotification tileNotification in nots)
          forSecondaryTile.Update(tileNotification);
        await TilePinner.SetUpBackgroundTasks();
      }
      else
        await TileHelper.CleanUpFolders();
    }

    public static BackgroundTaskRegistration RegisterBackgroundTask(
      string entryPoint,
      string name,
      IBackgroundTrigger trigger,
      IBackgroundCondition condition)
    {
      foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> allTask in (IEnumerable<KeyValuePair<Guid, IBackgroundTaskRegistration>>) BackgroundTaskRegistration.AllTasks)
      {
        if (allTask.Value.Name == name)
          return (BackgroundTaskRegistration) allTask.Value;
      }
      BackgroundTaskBuilder backgroundTaskBuilder = new BackgroundTaskBuilder();
      backgroundTaskBuilder.TaskEntryPoint = entryPoint;
      backgroundTaskBuilder.Name = name;
      backgroundTaskBuilder.SetTrigger(trigger);
      if (condition != null)
        backgroundTaskBuilder.AddCondition(condition);
      return backgroundTaskBuilder.Register();
    }

    public static async Task SetUpBackgroundTasks()
    {
      IReadOnlyList<SecondaryTile> allAsync = await SecondaryTile.FindAllAsync();
      Helper.Write((object) nameof (TilePinner), (object) "Listing all registered background tasks");
      IReadOnlyDictionary<Guid, IBackgroundTaskRegistration> allTasks = BackgroundTaskRegistration.AllTasks;
      if (allTasks != null)
      {
        try
        {
          foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> keyValuePair in (IEnumerable<KeyValuePair<Guid, IBackgroundTaskRegistration>>) allTasks)
          {
            try
            {
              if (keyValuePair.Value.Name.StartsWith("Typed"))
              {
                bool flag = true;
                if (TilePinner.ShouldSetupSecondaryTasks)
                {
                  foreach (SecondaryTile secondaryTile in (IEnumerable<SecondaryTile>) allAsync)
                  {
                    if (secondaryTile.TileId == keyValuePair.Value.Name)
                    {
                      flag = false;
                      break;
                    }
                  }
                }
                if (flag)
                {
                  Helper.Write((object) nameof (TilePinner), (object) "Unregistering tile task");
                  keyValuePair.Value.Unregister(true);
                }
              }
            }
            catch
            {
            }
          }
        }
        catch
        {
        }
      }
      TimeTrigger trigger = new TimeTrigger(30U, false);
      TimeTrigger timeTrigger = new TimeTrigger(30U, false);
      foreach (SecondaryTile secondaryTile in (IEnumerable<SecondaryTile>) allAsync)
      {
        if (secondaryTile.TileId.StartsWith("Typed") && TilePinner.ShouldSetupSecondaryTasks)
          TilePinner.RegisterBackgroundTask(TilePinner.TileBackgroundTaskName, secondaryTile.TileId, (IBackgroundTrigger) trigger, (IBackgroundCondition) null);
      }
    }

    public static async Task PinVideoTile(YouTubeEntry entry, Type pageType)
    {
      if (SecondaryTile.Exists(TileHelper.CreateTileID(entry)))
        return;
      TileArgs tileArgs = new TileArgs(pageType, entry.ID);
      TileNotification tileNotification = await TileHelper.GetAdaptiveTileNotification(entry);
      if (await (await TileHelper.CreateTile(entry, tileArgs)).RequestCreateAsync())
        await TileHelper.UpdateSecondaryTile(entry);
      tileArgs = (TileArgs) null;
    }
  }
}
