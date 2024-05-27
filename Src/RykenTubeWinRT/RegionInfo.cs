// Decompiled with JetBrains decompiler
// Type: RykenTube.RegionInfo
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RykenTube
{
  public class RegionInfo
  {
    public static readonly RegionInfo Global = new RegionInfo("Worldwide", "US");
    public static readonly RegionInfo Argentina = new RegionInfo(nameof (Argentina), "AR");
    public static readonly RegionInfo Australia = new RegionInfo(nameof (Australia), "AU");
    public static readonly RegionInfo Austria = new RegionInfo(nameof (Austria), "AS");
    public static readonly RegionInfo Brazil = new RegionInfo(nameof (Brazil), "BR");
    public static readonly RegionInfo Bulgaria = new RegionInfo(nameof (Bulgaria), "BG");
    public static readonly RegionInfo Belgium = new RegionInfo(nameof (Belgium), "BE");
    public static readonly RegionInfo Canada = new RegionInfo(nameof (Canada), "CA");
    public static readonly RegionInfo Chile = new RegionInfo(nameof (Chile), "CL");
    public static readonly RegionInfo Colombia = new RegionInfo(nameof (Colombia), "CO");
    public static readonly RegionInfo Croatia = new RegionInfo(nameof (Croatia), "HR");
    public static readonly RegionInfo Czech = new RegionInfo("Czech Republic", "CZ");
    public static readonly RegionInfo Denmark = new RegionInfo(nameof (Denmark), "DK");
    public static readonly RegionInfo Egypt = new RegionInfo(nameof (Egypt), "EG");
    public static readonly RegionInfo Estonia = new RegionInfo(nameof (Estonia), "EE");
    public static readonly RegionInfo Finland = new RegionInfo(nameof (Finland), "FI");
    public static readonly RegionInfo France = new RegionInfo(nameof (France), "FR");
    public static readonly RegionInfo Germany = new RegionInfo(nameof (Germany), "DE");
    public static readonly RegionInfo GreatBritain = new RegionInfo("Great Britain", "GB");
    public static readonly RegionInfo Greece = new RegionInfo(nameof (Greece), "GR");
    public static readonly RegionInfo HongKong = new RegionInfo("Hong Kong", "HK");
    public static readonly RegionInfo Hungary = new RegionInfo(nameof (Hungary), "HU");
    public static readonly RegionInfo Iceland = new RegionInfo(nameof (Iceland), "IS");
    public static readonly RegionInfo India = new RegionInfo(nameof (India), "IN");
    public static readonly RegionInfo Indonesia = new RegionInfo(nameof (Indonesia), "ID");
    public static readonly RegionInfo Ireland = new RegionInfo(nameof (Ireland), "IE");
    public static readonly RegionInfo Israel = new RegionInfo(nameof (Israel), "IL");
    public static readonly RegionInfo Italy = new RegionInfo(nameof (Italy), "IT");
    public static readonly RegionInfo Japan = new RegionInfo(nameof (Japan), "JP");
    public static readonly RegionInfo Jordan = new RegionInfo(nameof (Jordan), "JO");
    public static readonly RegionInfo Kuwait = new RegionInfo(nameof (Kuwait), "KW");
    public static readonly RegionInfo Latvia = new RegionInfo(nameof (Latvia), "LV");
    public static readonly RegionInfo Lebanon = new RegionInfo(nameof (Lebanon), "LB");
    public static readonly RegionInfo Libya = new RegionInfo(nameof (Libya), "LY");
    public static readonly RegionInfo Lithuania = new RegionInfo(nameof (Lithuania), "LT");
    public static readonly RegionInfo Malaysia = new RegionInfo(nameof (Malaysia), "MY");
    public static readonly RegionInfo Mexico = new RegionInfo(nameof (Mexico), "MX");
    public static readonly RegionInfo Morocco = new RegionInfo(nameof (Morocco), "MA");
    public static readonly RegionInfo Netherlands = new RegionInfo(nameof (Netherlands), "NL");
    public static readonly RegionInfo NewZealand = new RegionInfo("New Zealand", "NZ");
    public static readonly RegionInfo Nigeria = new RegionInfo(nameof (Nigeria), "NG");
    public static readonly RegionInfo Peru = new RegionInfo(nameof (Peru), "PE");
    public static readonly RegionInfo Philippines = new RegionInfo("Phillipines", "PH");
    public static readonly RegionInfo Poland = new RegionInfo(nameof (Poland), "PL");
    public static readonly RegionInfo Russia = new RegionInfo(nameof (Russia), "RU");
    public static readonly RegionInfo SaudiArabia = new RegionInfo("Saudi Arabia", "SA");
    public static readonly RegionInfo Singapore = new RegionInfo(nameof (Singapore), "SG");
    public static readonly RegionInfo SouthAfrica = new RegionInfo("South Africa", "ZA");
    public static readonly RegionInfo SouthKorea = new RegionInfo("South Korea", "KR");
    public static readonly RegionInfo Spain = new RegionInfo(nameof (Spain), "ES");
    public static readonly RegionInfo Sweden = new RegionInfo(nameof (Sweden), "SE");
    public static readonly RegionInfo Switzerland = new RegionInfo(nameof (Switzerland), "CH");
    public static readonly RegionInfo Taiwan = new RegionInfo(nameof (Taiwan), "TW");
    public static readonly RegionInfo UAE = new RegionInfo("United Arab Emirates", "AE");
    public static readonly RegionInfo UnitedStates = new RegionInfo("United States", "US");
    public string CountryName = "";
    public string CountryCode = "";
    private static Dictionary<string, RegionInfo> codeInfo;
    private static Dictionary<string, RegionInfo> codeInfo2;
    private static TaskCompletionSource<List<RegionInfo>> regionsLoadedTCS = new TaskCompletionSource<List<RegionInfo>>();

    private static void createCodeInfo()
    {
      if (RegionInfo.codeInfo == null)
      {
        RegionInfo.codeInfo = new Dictionary<string, RegionInfo>();
        foreach (RegionInfo country in RegionInfo.GetCountries())
          RegionInfo.codeInfo.Add(country.CountryCode, country);
      }
      if (RegionInfo.codeInfo2 != null || !RegionInfo.RegionsLoadedTask.IsCompleted)
        return;
      RegionInfo.codeInfo2 = new Dictionary<string, RegionInfo>();
      foreach (RegionInfo regionInfo in RegionInfo.RegionsLoadedTask.Result)
        RegionInfo.codeInfo2.Add(regionInfo.CountryCode, regionInfo);
    }

    public static Task<List<RegionInfo>> RegionsLoadedTask => RegionInfo.regionsLoadedTCS.Task;

    public static RegionInfo GetFromCode(string countryCode)
    {
      RegionInfo.createCodeInfo();
      if (RegionInfo.codeInfo2 != null && RegionInfo.codeInfo2.ContainsKey(countryCode))
        return RegionInfo.codeInfo2[countryCode];
      if (RegionInfo.codeInfo.ContainsKey(countryCode))
        return RegionInfo.codeInfo[countryCode];
      return RegionInfo.codeInfo2.ContainsKey("US") ? RegionInfo.codeInfo2["US"] : RegionInfo.Global;
    }

    public static void SetCountries(List<RegionInfo> regions) => RegionInfo.regionsLoadedTCS.TrySetResult(regions);

    public static List<RegionInfo> GetCountries()
    {
      if (RegionInfo.RegionsLoadedTask.IsCompleted)
        return RegionInfo.RegionsLoadedTask.Result;
      return new List<RegionInfo>()
      {
        RegionInfo.Global,
        RegionInfo.Argentina,
        RegionInfo.Australia,
        RegionInfo.Austria,
        RegionInfo.Belgium,
        RegionInfo.Brazil,
        RegionInfo.Canada,
        RegionInfo.Chile,
        RegionInfo.Colombia,
        RegionInfo.Czech,
        RegionInfo.Egypt,
        RegionInfo.France,
        RegionInfo.Germany,
        RegionInfo.GreatBritain,
        RegionInfo.HongKong,
        RegionInfo.Hungary,
        RegionInfo.India,
        RegionInfo.Ireland,
        RegionInfo.Israel,
        RegionInfo.Italy,
        RegionInfo.Japan,
        RegionInfo.Jordan,
        RegionInfo.Malaysia,
        RegionInfo.Mexico,
        RegionInfo.Morocco,
        RegionInfo.Netherlands,
        RegionInfo.NewZealand,
        RegionInfo.Peru,
        RegionInfo.Philippines,
        RegionInfo.Poland,
        RegionInfo.Russia,
        RegionInfo.SaudiArabia,
        RegionInfo.Singapore,
        RegionInfo.SouthAfrica,
        RegionInfo.SouthKorea,
        RegionInfo.Spain,
        RegionInfo.Sweden,
        RegionInfo.Switzerland,
        RegionInfo.Taiwan,
        RegionInfo.UAE,
        RegionInfo.UnitedStates
      };
    }

    public RegionInfo()
    {
    }

    public RegionInfo(string name, string code)
    {
      this.CountryName = name;
      this.CountryCode = code;
    }

    public static bool operator ==(RegionInfo r1, RegionInfo r2)
    {
      try
      {
        bool flag1 = false;
        bool flag2 = false;
        string countryCode = "";

        try
        {
            if (r1 != null)
                countryCode = r1.CountryCode;
            else
                flag1 = true;
        }
        catch
        {
          flag1 = true;
        }
        try
        {
            if (r2 != null)
                countryCode = r2.CountryCode;
            else
                flag2 = true;
        }
        catch
        {
          flag2 = true;
        }

        if (flag1 & flag2)
          return true;
        return !(flag1 | flag2) && r1.CountryName == r2.CountryName && r1.CountryCode == r2.CountryCode;
      }
      catch
      {
        YouTube.Write((object) nameof (RegionInfo), (object) "Error comparing region info");
        System.Diagnostics.Debug.WriteLine(nameof(RegionInfo), " Error comparing region info");
        return false;
      }
    }

    public static bool operator !=(RegionInfo r1, RegionInfo r2) => !(r1 == r2);

        public override bool Equals(object obj)
        {
            return (object)(obj as RegionInfo) != null && this == obj as RegionInfo;
        }

        public override string ToString() => this.CountryName;
  }
}
