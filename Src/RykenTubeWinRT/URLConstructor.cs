// Decompiled with JetBrains decompiler
// Type: RykenTube.URLConstructor
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RykenTube
{
  public class URLConstructor
  {
    private Dictionary<string, string> values;
    private static char[] andSplitArray = new char[1]{ '&' };
    private bool urlencode;
    private URLDisplayMode displayMode;
    private string baseAddress;

    public bool UriEncode
    {
      get => this.urlencode;
      set => this.urlencode = value;
    }

    public string this[string key]
    {
      get => this.GetValue(key);
      set => this.SetValue(key, (object) value);
    }

    public Dictionary<string, string>.KeyCollection Keys => this.values.Keys;

    public URLDisplayMode DisplayMode
    {
      get => this.displayMode;
      set => this.displayMode = value;
    }

    public string BaseAddress
    {
      get => this.baseAddress;
      set => this.setBaseAddress(value);
    }

    public URLConstructor() => this.values = new Dictionary<string, string>();

    public URLConstructor(Uri uri)
      : this(uri.ToString())
    {
    }

    public URLConstructor(string url)
      : this()
    {
      this.setBaseAddress(url);
    }

    private void setBaseAddress(string url)
    {
      string[] strArray = url.Split('?');
      this.baseAddress = strArray.Length <= 1 ? (!strArray[0].Contains<char>('=') ? strArray[0] : "") : strArray[0];
      string source = "";
      for (int index = strArray.Length > 1 ? 1 : 0; index < strArray.Length; ++index)
        source = source + "&" + strArray[index];
      if (string.IsNullOrWhiteSpace(source) || !source.Contains<char>('='))
        return;
      foreach (string key in source.Split(URLConstructor.andSplitArray, StringSplitOptions.RemoveEmptyEntries))
      {
        if (key.Contains("="))
        {
          int length = key.IndexOf("=");
          this.SetValue(key.Substring(0, length), (object) WebUtility.UrlDecode(key.Substring(length + 1)));
        }
        else
          this.SetValue(key, (object) "");
      }
    }

    public bool ContainsKey(string key) => this.values.ContainsKey(key);

    public string GetValue(string key) => this.values.ContainsKey(key) ? this.values[key] : (string) null;

    public void SetValue(string key, object value)
    {
      if (this.values.ContainsKey(key))
        this.values[key] = value.ToString();
      else
        this.values.Add(key, value.ToString());
    }

    public void RemoveValue(string key)
    {
      if (!this.values.ContainsKey(key))
        return;
      this.values.Remove(key);
    }

    public override string ToString() => this.ToString(this.displayMode, this.UriEncode);

    public string ToString(URLDisplayMode mode, bool uriEncode)
    {
      string str = string.IsNullOrEmpty(this.baseAddress) ? "" : this.baseAddress + "?";
      KeyValuePair<string, string>[] array = this.values.ToArray<KeyValuePair<string, string>>();
      for (int index = 0; index < array.Length; ++index)
      {
        KeyValuePair<string, string> keyValuePair = array[index];
        if (keyValuePair.Key != null)
        {
          switch (mode)
          {
            case URLDisplayMode.ExcludeNullValues:
              if (keyValuePair.Value != null)
              {
                if (index != 0)
                  str += "&";
                str = str + keyValuePair.Key + "=" + (uriEncode ? WebUtility.UrlEncode(keyValuePair.Value) : keyValuePair.Value);
                continue;
              }
              continue;
            case URLDisplayMode.ShowNullAsEmptyString:
              if (index != 0)
                str += "&";
              str = keyValuePair.Value != null ? str + keyValuePair.Key + "=" + (uriEncode ? WebUtility.UrlEncode(keyValuePair.Value) : keyValuePair.Value) : str + keyValuePair.Key + "=";
              continue;
            case URLDisplayMode.IncludeNullValues:
              if (index != 0)
                str += "&";
              str = keyValuePair.Value != null ? str + keyValuePair.Key + "=" + (uriEncode ? WebUtility.UrlEncode(keyValuePair.Value) : keyValuePair.Value) : str + keyValuePair.Key + "=null";
              continue;
            default:
              continue;
          }
        }
      }
      return str;
    }

    public Uri ToUri(UriKind uriKind) => this.ToUri(uriKind, this.displayMode);

    public Uri ToUri() => this.ToUri(UriKind.RelativeOrAbsolute, this.displayMode);

    public Uri ToUri(UriKind uriKind, URLDisplayMode mode) => new Uri(this.ToString(mode, this.UriEncode), uriKind);

    public URLConstructor Clone()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (KeyValuePair<string, string> keyValuePair in this.values)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
      return new URLConstructor()
      {
        baseAddress = this.baseAddress,
        values = dictionary,
        displayMode = this.displayMode,
        urlencode = this.urlencode
      };
    }
  }
}
