# myTube 3.9.125 — R&D Plan
### "Fahrenheit 451 Edition" — A Tribute to W10M / Metro UI

> *"They kept the fires burning. We kept the code."*
> Solo dev · UWP · C# · MIT · 2024–∞

---

## Block 1 — Research: What Was myTube?

### Origins & Identity

myTube was created by **Christopher Blackman (Ryken, Ryken Studio)** — one of the most celebrated third-party app developers in the Windows Phone ecosystem. At its peak (~2014–2017) it was arguably the best YouTube client on any mobile platform: fluid animations, Live Tile support, background audio, AMOLED-friendly dark theme, and deep OS integration that the official Google app never bothered to provide.

It shipped in two major eras:

| Era | Build series | Target OS | Key trait |
|---|---|---|---|
| Classic | 2.7.x | WP8 / WP8.1 | Silverlight-era XAML, pivot navigation |
| Modern | 3.9.x | W10M / UWP | XAML Islands, adaptive triggers, Live Tiles v2 |

Your fork (`3.9.125`) is a **"synthez"** — a deliberate cross-splice of the 2.7.x architecture (which is more readable post-decompile) with the 3.9.x background task and notification layer. That's the right approach.

### Why It Died

- YouTube Data API v3 replaced the older unofficial endpoints that myTube leaned on.
- Google broke WebView-based OAuth login flows (the "damaged webbrowser google auth" bug in your draft).
- Microsoft quietly killed Silverlight/WP8 toolchain support, and W10M reached EOL in 2019.
- Ryken moved on (understandably). The Store listing went dark.

### What You're Actually Reconstructing

From your draft + the known codebase, the feature set to target (in priority order):

1. Video search & playback (the non-negotiable core)
2. Channel browsing & subscriptions
3. Background audio (already a RE'd module)
4. Live Tile updates (already a RE'd task)
5. Channel push notifications (already RE'd)
6. Toast interactions (already RE'd)
7. Account / OAuth (the hard one — see Block 3)

---

## Block 2 — Code Architecture

### Module Map

```
myTube.sln
│
├── myTube                        [UWP App Head — WinRT, XAML, C#]
│   ├── Pages/                    TextPage (partial ✓), HomePage (TODO)
│   ├── Controls/                 OverCanvas feature (experimental ✓)
│   └── App.xaml.cs               Lifecycle, activation handlers
│
├── myTubeAppLibrary              [Core domain logic — RE from 2.7.x]
│   ├── ViewModels/               MVVM wiring (INotifyPropertyChanged)
│   ├── Models/                   VideoItem, ChannelItem, PlaylistItem
│   └── Services/                 YouTubeService, SettingsService
│
├── RykenTubeWinRT                [YouTube API abstraction — RE from 2.7.x]
│   ├── YouTubeClient.cs          Data API v3 wrapper
│   ├── VideoParser.cs            JSON → model mapping
│   └── SearchQuery.cs            Query builder
│
├── WinRTXamlToolkit              [UI helpers — RE from 2.7.x]
│   └── (animation helpers, converters, behaviors)
│
├── XMLHelper                     [Serialization — RE from 2.7.x]
│
├── CloudClasses                  [Settings sync / roaming — RE from 2.7.x]
│
├── BackgroundAudio               [BG audio task — RE from 3.9.x] ← Priority
├── ChannelNotificationsTask      [Push polling task — RE from 3.9.x]
├── ToastActionBackgroundTask     [Toast reply/action handler — RE from 3.9.x]
├── Windows10TileTask             [Live Tile updater — RE from 3.9.x]
├── WatsonRegistrationUtility     [Crash telemetry — RE from 3.9.x]
│
└── TestFramework                 [Unit tests — skeleton]
```

### Key APIs to Integrate

**YouTube Data API v3** — the only viable official path.

```csharp
// Minimum endpoints needed
GET /youtube/v3/search          // search, homepage feed (via activity)
GET /youtube/v3/videos          // video detail, stream metadata
GET /youtube/v3/channels        // channel info
GET /youtube/v3/subscriptions   // user subscriptions (requires OAuth)
GET /youtube/v3/playlists       // playlists
```

**Important quota note:** YouTube Data API v3 has a **10,000 unit/day** free quota. Search costs 100 units per call. Design your `RykenTubeWinRT` layer with aggressive caching (in-memory + `ApplicationData.Current.LocalFolder`) from day one.

**OAuth 2.0 for Desktop/UWP** — the broken part. Solution:

```csharp
// Use Windows.Security.Authentication.Web.WebAuthenticationBroker
// with PKCE flow — NOT embedded WebView (that's what broke in 2.7.x)

var result = await WebAuthenticationBroker.AuthenticateAsync(
    WebAuthenticationOptions.None,
    authUri,          // accounts.google.com/o/oauth2/v2/auth
    callbackUri       // ms-app://your-sid/
);
```

This approach uses the system browser, survives Google's third-party cookie/WebView restrictions, and works on both PC and W10M (build 14393+).

**MediaElement / MediaPlayer API** — for playback:

```csharp
// Prefer Windows.Media.Playback.MediaPlayer (3.9.x era)
// over the older MediaElement (2.7.x era)
// MediaPlayer supports background audio properly on UWP
var player = BackgroundMediaPlayer.Current; // still valid on 14393
```

**Background Tasks** — your 3.9.x RE modules already cover this. Key manifest declarations needed: `BackgroundTask` (audio, timer, push), `Protocol` activation.

---

## Block 3 — Technical Limitations & Test Environment Setup

### Hard Constraints

| Constraint | Detail |
|---|---|
| Min build | 14393 (Anniversary Update, Aug 2016) |
| Toolkit | Visual Studio 2019 or 2022 (with UWP workload + Windows 10 SDK 14393) |
| .NET | .NET Native + .NET Standard 2.0 for shared libs |
| YouTube | Data API v3 only — no scraping (ToS), no youtube-dl libs on Store builds |
| Auth | WebAuthenticationBroker + PKCE — no embedded WebView login |
| Background audio | Requires `BackgroundMediaPlayer` + manifest declaration |
| Store | Dev sideload (sideload .appx) is the realistic target — Store submission requires Google API key review |

### Test Environment

**Recommended layered setup:**

```
Layer 1 — Development machine
  VS 2022 + UWP workload
  Windows 10 SDK 10.0.14393.0 (minimum) + latest SDK installed side-by-side
  API key in local.settings / secrets.json (NEVER commit to git)

Layer 2 — PC emulation
  UWP app running on desktop in "phone-like" window (set custom app window size ~360×640)
  Windows Mobile Emulator (if you still have it — Hyper-V based, x86)

Layer 3 — Real hardware (the fun part)
  Lumia 950 / 950 XL — the best W10M test device (Snapdragon 808/810, build 14393 stable)
  Lumia 640 XL — good mid-range test
  Deploy via: Device Portal (http://phone-ip:8080) or VS remote deploy
```

**Secrets management** for the API key:

```csharp
// secrets.cs (git-ignored)
public static class ApiSecrets {
    public const string YouTubeApiKey = "YOUR_KEY_HERE";
    public const string OAuthClientId = "YOUR_CLIENT_ID.apps.googleusercontent.com";
}
```

### Known Pitfalls

- **IL2CPP / .NET Native compilation** strips reflection — any `dynamic`/`JsonConvert` magic needs `rd.xml` runtime directives.
- **BackgroundMediaPlayer** was deprecated in Creators Update (1703) in favor of `MediaPlayer` with `SystemMediaTransportControls`. Plan a shim.
- **Live Tiles** require the `Windows10TileTask` to stay under ~25 seconds execution budget.
- **Quota exhaustion:** Cache channel/subscription data locally; refresh only on explicit pull-to-refresh.

---

## Block 4 — Development Plan (Waterfall for Solo Dev)

### Philosophy

Solo waterfall with **hard gates** — no moving to the next phase until the current one has a working, testable artifact. No scope creep inside a phase.

```
Phase 0 — Foundation       [~2 weeks]
Phase 1 — Playback Core    [~3 weeks]
Phase 2 — Browse & Search  [~3 weeks]
Phase 3 — Account & Auth   [~2 weeks]
Phase 4 — Background Tasks [~2 weeks]
Phase 5 — Metro UI Polish  [~3 weeks]
Phase 6 — Community Prep   [~1 week]
```

---

### Phase 0 — Foundation

**Goal:** Clean solution that builds on 14393, CI green, secrets safe.

- [ ] Pin Windows 10 SDK 14393 as `TargetMinVersion` in all project files
- [ ] Add `secrets.cs` + `.gitignore` entry
- [ ] Wire up `RykenTubeWinRT` with a hardcoded test video ID → confirm raw JSON response
- [ ] Stub all ViewModels with `INotifyPropertyChanged` (use a base class — don't repeat boilerplate)
- [ ] Set up `TestFramework` project with one passing MSTest

**C# tip:** Create a shared `BindableBase`:

```csharp
public abstract class BindableBase : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;
    protected bool SetProperty<T>(ref T field, T value,
        [CallerMemberName] string name = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        return true;
    }
}
```

---

### Phase 1 — Playback Core

**Gate:** A video URL → plays audio+video on both PC and device.

- [ ] `RykenTubeWinRT.VideoParser` — parse YouTube Data API v3 `/videos` response
- [ ] Resolve actual stream URL (see note below)
- [ ] `MediaPlayer` integration in `myTube` app head
- [ ] Basic `VideoPage.xaml` — just player + title, no chrome

**Stream URL note:** YouTube Data API v3 does NOT return stream URLs directly. You need `YoutubeExplode` (NuGet, MIT license) or your own stream extractor. `YoutubeExplode` is the cleanest option for a UWP target and does NOT require any Google API key for stream extraction:

```csharp
var youtube = new YoutubeClient();
var video = await youtube.Videos.GetAsync(videoId);
var manifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
var stream = manifest.GetMuxedStreams().GetWithHighestVideoQuality();
mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(stream.Url));
```

---

### Phase 2 — Browse & Search

**Gate:** Home feed + search results displayed in Metro-style list.

- [ ] `HomePage.xaml` — the missing page from your draft. Suggested layout: Hero video (large tile) + horizontally scrolling category rows (Pivot or Hub)
- [ ] `SearchPage.xaml` — search bar + `GridView` / `ListView` of `VideoItem` tiles
- [ ] `RykenTubeWinRT.SearchQuery` implementation against `/search` endpoint
- [ ] Quota-aware cache layer (`ApplicationData.Current.LocalFolder` + JSON)
- [ ] `WinRTXamlToolkit` converters wired up (duration formatting, thumbnail loading)

**XAML tip — Metro-authentic tile item template:**

```xml
<DataTemplate x:Key="VideoTileTemplate">
  <Grid Width="160" Margin="4">
    <Grid.RowDefinitions>
      <RowDefinition Height="90"/>
      <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>
    <Image Source="{Binding ThumbnailUrl}" Stretch="UniformToFill"/>
    <Border Background="#CC000000" VerticalAlignment="Bottom"
            Padding="4,2" Grid.Row="0">
      <TextBlock Text="{Binding Duration}" Foreground="White"
                 FontSize="11" HorizontalAlignment="Right"/>
    </Border>
    <TextBlock Grid.Row="1" Text="{Binding Title}" MaxLines="2"
               TextWrapping="Wrap" FontSize="13" Margin="0,4,0,0"/>
  </Grid>
</DataTemplate>
```

---

### Phase 3 — Account & OAuth

**Gate:** User can sign in, see subscriptions, and sign out cleanly.

- [ ] `WebAuthenticationBroker` PKCE flow (see Block 2)
- [ ] Token storage in `PasswordVault` (the correct secure storage on UWP)
- [ ] Token refresh logic (Google access tokens expire in 1 hour)
- [ ] `SubscriptionsPage.xaml` — list of subscribed channels
- [ ] `CloudClasses` — roaming settings for account preferences

```csharp
// Secure token storage
var vault = new PasswordVault();
vault.Add(new PasswordCredential("myTube", "oauth_token", accessToken));
// Retrieve:
var cred = vault.Retrieve("myTube", "oauth_token");
```

---

### Phase 4 — Background Tasks

**Gate:** Music plays with screen off; Live Tile updates; notifications arrive.

- [ ] `BackgroundAudio` — wire `SystemMediaTransportControls` for lock screen controls
- [ ] `Windows10TileTask` — fetch latest subscription video, update tile with `TileUpdateManager`
- [ ] `ChannelNotificationsTask` — poll for new uploads, fire toast via `ToastNotificationManager`
- [ ] `ToastActionBackgroundTask` — handle "Watch later" / "Open" actions from toast

**Manifest snippet for background audio:**

```xml
<Extensions>
  <Extension Category="windows.backgroundTasks"
             EntryPoint="BackgroundAudio.AudioTask">
    <BackgroundTasks>
      <Task Type="audio"/>
    </BackgroundTasks>
  </Extension>
</Extensions>
```

---

### Phase 5 — Metro UI Polish

**Gate:** The app *feels* like a lost W10M gem, not a PC port.

This is your tribute phase. The aesthetic contract:

- Accent color system tied to system accent (user's chosen color, like W10M did)
- Fluent/Metro transitions: `DrillInThemeAnimation`, `EntranceThemeTransition`
- `OverCanvas` feature (your experimental work) — use for video detail overlay, Metro-style peek
- Adaptive triggers: phone layout (single column) vs tablet/PC (hub layout)
- Dark theme by default — respect `RequestedTheme` from system
- Typography: Segoe UI / Segoe WP — authentic to the era

```xml
<!-- Adaptive layout trigger -->
<VisualStateManager.VisualStateGroups>
  <VisualStateGroup>
    <VisualState x:Name="PhoneLayout">
      <VisualState.StateTriggers>
        <AdaptiveTrigger MinWindowWidth="0"/>
      </VisualState.StateTriggers>
      <!-- phone-specific setters -->
    </VisualState>
    <VisualState x:Name="TabletLayout">
      <VisualState.StateTriggers>
        <AdaptiveTrigger MinWindowWidth="720"/>
      </VisualState.StateTriggers>
    </VisualState>
  </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

---

### Phase 6 — Community Prep

**Gate:** A stranger can clone the repo, get a YouTube API key, and run the app.

- [ ] `README.md` rewrite (English + Russian — the W10M community is very active in RU)
- [ ] `SETUP.md` — API key registration walkthrough, sideload instructions
- [ ] GitHub Release with signed `.appx` (sideload package)
- [ ] Screenshots in Metro style (dark, accent-colored)

---

## Block 5 — Intro: Deployment & the W10M Fan Community

### Deployment Reality

The Microsoft Store submission for YouTube clients is a **legal minefield** (Google's ToS, API terms). Realistic distribution channels:

| Channel | Effort | Reach |
|---|---|---|
| GitHub Releases (`.appx` sideload) | Low | Developers, enthusiasts |
| [Appx.lol](https://appx.lol) / community stores | Medium | Broader W10M users |
| [W10M Fan Discord](https://discord.gg/windowsphone) | Low | Direct community |
| XDA Developers thread | Medium | Longtail discovery |
| Reddit r/windowsphone | Low | Awareness |

For sideloading to work, device must have **Developer Mode** enabled (Settings → Update & Security → For developers). Document this clearly.

### Bringing It to the Community

The W10M / Metro fan community is small, passionate, and organized. Key entry points:

- **Ryken's own Discord** (linked from `github.com/Ryken100/mytube`) — this is the most direct audience. A message saying "I'm RE'ing myTube as a tribute, MIT, open for contributions" will land warmly.
- **XDA thread** — open a development thread. WP/W10M subforum still has active readers.
- **r/windowsphone** — announce when you have a sideloadable build.

### Framing the Release

The "Fahrenheit 451" narrative you've described is genuinely compelling and worth using in the README/announcement. Something like:

> *"When the platform was discontinued, the apps didn't just stop working — they disappeared from stores, from memory, from the conversation. This is a reconstruction: not a port, not an emulation, but an attempt to recover something that deserved to survive."*

That tone will resonate with the W10M community far more than a changelog.

### Contributing & Sustaining

Since you're solo and this is RnD/tribute:

- Keep `CONTRIBUTING.md` honest: "Issues and PRs welcome but no guarantees on timelines."
- Tag the repo with topics: `uwp`, `windows-phone`, `youtube-client`, `w10m`, `metro-ui`, `retro`.
- Consider a `COMPAT.md` listing which Lumia devices have been tested — community members will fill this in.

---

## Quick Reference: Priority Stack for Next Session

```
1. Phase 0 — get it building cleanly on 14393
2. Integrate YoutubeExplode for stream resolution
3. Implement WebAuthenticationBroker OAuth (unblocks all account features)
4. Rebuild HomePage.xaml (the most visible missing piece)
5. Wire BackgroundAudio task (signature W10M feature)
```

---

*[m][e] 2024 — MIT — As is. RnD. DIY. Keep the fires burning.* 🔥
