// Decompiled with JetBrains decompiler
// Type: myTube.Tests.Backend.AuthorizedClient
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System.Threading.Tasks;
using TestFramework;

namespace myTube.Tests.Backend
{
  public class AuthorizedClient
  {
    public AuthorizedClient()
    {
      if (!YouTube.IsSignedIn)
      {
        Test.Error("You are not signed in, so this test cannot start");
        throw new TestFailureException("You are not signed in");
      }
    }

    [TestMethod("Subscriptions")]
    public async Task SubscriptionVideos()
    {
      if ((await new SignedInUserClient(UserFeed.Subscriptions, 5).GetFeed(0)).Length != 0)
        return;
      Test.Warn("Though no errors were thrown, no subscription videos were downloaded.");
    }

    [TestMethod("What to watch")]
    public async Task WhatToWatch()
    {
      if ((await new SignedInUserClient(UserFeed.Recommended, 5).GetFeed(0)).Length != 0)
        return;
      Test.Warn("Though no errors were thrown, no recommended videos were downloaded.");
    }

    [TestMethod("History")]
    public async Task History()
    {
      if ((await new SignedInUserClient(UserFeed.History, 5).GetFeed(0)).Length != 0)
        return;
      Test.Warn("Though no errors were thrown, no history videos were downloaded.");
    }
  }
}
