// Decompiled with JetBrains decompiler
// Type: myTube.BetaPages.TileInfoPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.BetaPages
{
  public sealed class TileInfoPage : Page, IComponentConnector
  {
    private SecondaryTile lastTile;
    private bool setIssue;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl fileList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run taskText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run issuesText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private SecondaryTile Tile => ((FrameworkElement) this).DataContext as SecondaryTile;

    public TileInfoPage()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(TileInfoPage_DataContextChanged)));
    }

    private async void TileInfoPage_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      args.put_Handled(true);
      if (this.Tile == null || this.lastTile == this.Tile)
        return;
      this.lastTile = this.Tile;
      List<StorageFile> files = Enumerable.ToList<StorageFile>((IEnumerable<StorageFile>) await (await TileHelper.GetFolderForTile(this.Tile.TileId)).GetFilesAsync());
      this.ResetIssues();
      if (files.Count == 0)
        this.AddIssue("There are no files in the folder for this tile. This may lead to a completely blank tile.");
      if (files.Count > 10)
        this.AddIssue("There are " + (object) files.Count + " files in the tile folder. Usually a maximum of 10 files is expected. Perhaps old images aren't being deleted.");
      if (files.Count < 6)
        this.AddIssue("There is a small amount of files in the tile folder. Is this a a video tile, or a list with very few videos?");
      ((ICollection<object>) this.fileList.Items).Clear();
      List<string> paths = new List<string>();
      int count = 0;
      TileData tileData = TileData.GetTileData(this.Tile.TileId);
      TileArgs tileArgs = new TileArgs(this.Tile.Arguments);
      this.taskText.put_Text(tileData.ToString());
      if (!string.IsNullOrEmpty(tileData.Exception))
        this.AddIssue("There was an error running the background task. Check the background task status info for exception details.");
      if (tileArgs.ShouldSignInFirst && !tileData.SignedIn && tileData.LastRun != DateTimeOffset.MinValue)
        this.AddIssue("The background task didn't sign into YouTube when it last run. Signing in is required to load the video information for this task.");
      if (tileData.LastRun != DateTimeOffset.MinValue && DateTimeOffset.Now - tileData.LastRun > TimeSpan.FromHours(6.0))
        this.AddIssue("It has been more than 6 hours since this background task last run. Are background tasks enabled for myTube in battery saver?");
      ToAgoString ago = new ToAgoString();
      foreach (StorageFile file in files)
      {
        ++count;
        if (!paths.Contains(file.Name))
          paths.Add(file.Name);
        else
          this.AddIssue("There are duplicate files in the folder for this tile.");
        object d = (object) new ExpandoObject();
        // ISSUE: reference to a compiler-generated field
        if (TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Title", typeof (TileInfoPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__0, d, file.Name);
        BasicProperties basicPropertiesAsync = await file.GetBasicPropertiesAsync();
        if (basicPropertiesAsync.Size < 1024UL && file.Name.EndsWith(".png"))
          this.AddIssue("One or more image files are unusually small. This may lead to some sides of the tile being empty.");
        // ISSUE: reference to a compiler-generated field
        if (TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, ulong, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Size", typeof (TileInfoPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__1.Target((CallSite) TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__1, d, basicPropertiesAsync.Size / 1024UL);
        // ISSUE: reference to a compiler-generated field
        if (TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Action<CallSite, ItemCollection, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (TileInfoPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__2.Target((CallSite) TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__2, this.fileList.Items, d);
        Helper.Write((object) nameof (TileInfoPage), (object) ("Added file info for " + (object) count + " tiles"));
        // ISSUE: reference to a compiler-generated field
        if (TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Ago", typeof (TileInfoPage), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__3.Target((CallSite) TileInfoPage.\u003C\u003Eo__4.\u003C\u003Ep__3, d, ago.Convert((object) basicPropertiesAsync.DateModified.DateTime, typeof (string), (object) "toUTC", "en"));
        d = (object) null;
      }
      if (((ICollection<object>) this.fileList.Items).Count > files.Count)
        this.AddIssue("There are duplicate files displayed in the list. Please ignore any duplicates");
      files = (List<StorageFile>) null;
      paths = (List<string>) null;
      ago = (ToAgoString) null;
    }

    private void ResetIssues()
    {
      this.setIssue = false;
      this.issuesText.put_Text("No issues");
    }

    private void AddIssue(string issue)
    {
      if (!this.setIssue)
      {
        this.issuesText.put_Text("");
        this.setIssue = true;
      }
      else
      {
        Run issuesText = this.issuesText;
        issuesText.put_Text(issuesText.Text + "\n\n");
      }
      if (this.issuesText.Text.Contains(issue))
        return;
      Run issuesText1 = this.issuesText;
      issuesText1.put_Text(issuesText1.Text + issue);
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (!(e.Parameter is SecondaryTile))
        return;
      ((FrameworkElement) this).put_DataContext(e.Parameter);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///BetaPages/TileInfoPage.xaml"), (ComponentResourceLocation) 0);
      this.fileList = (ItemsControl) ((FrameworkElement) this).FindName("fileList");
      this.taskText = (Run) ((FrameworkElement) this).FindName("taskText");
      this.issuesText = (Run) ((FrameworkElement) this).FindName("issuesText");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
