// Decompiled with JetBrains decompiler
// Type: RykenTube.ClientData`1
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace RykenTube
{
  public abstract class ClientData<TClientDataType> : ClientDataBase where TClientDataType : ClientData<TClientDataType>
  {
    public YouTubeClient<TClientDataType> Client { get; set; }

    protected override async Task RefreshOverride()
    {
      ClientData<TClientDataType> clientData = this;
      if (string.IsNullOrWhiteSpace(clientData.ID))
        return;
      EntryClient<TClientDataType> refreshClient = clientData.GetRefreshClient();
      if (refreshClient == null)
        return;
      JToken json = await refreshClient.GetJson(clientData.ID);
      if (json == null)
        return;
      clientData.SetValuesFromJson(json);
    }

    private EntryClient<TClientDataType> GetRefreshClient()
    {
      EntryClient<TClientDataType> refreshClient = (EntryClient<TClientDataType>) null;
      try
      {
        refreshClient = this.GetRefreshClientOverride();
      }
      catch
      {
      }
      if (refreshClient == null)
      {
        if (this.Client != null)
        {
          try
          {
            refreshClient = this.Client.GetRefreshClient();
          }
          catch
          {
          }
        }
      }
      return refreshClient;
    }

    protected abstract EntryClient<TClientDataType> GetRefreshClientOverride();
  }
}
