// Decompiled with JetBrains decompiler
// Type: RykenTube.CipherLoader
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RykenTube
{
  public class CipherLoader
  {
    private Regex findFirstFunction = new Regex("function *\\(.+?\\).+?\\(.\\.url\\).+?\\.set\\(.+?;");
    private Regex findSetFunction = new Regex("\\.set\\(([a-z]|[A-Z])+?\\..+?,.+?\\);");
    private Regex findSigFunctionName = new Regex("\\([a-zA-Z]{2}?\\(");
    private static string lastUpdateResult = "Updater not yet run";
    private static Stopwatch updateWatch;
    private static UpdaterState updateState = UpdaterState.Unknown;

    private string extratFromBrackets(string code, int index)
    {
      bool flag1 = false;
      bool flag2 = false;
      string str = "";
      int num = 0;
      for (int index1 = index; index1 < code.Length; ++index1)
      {
        char ch = code[index1];
        if (ch == '{')
        {
          ++num;
          if (!flag1)
            flag1 = true;
        }
        if (ch == '}')
          --num;
        if (flag1)
        {
          str += ch.ToString();
          if (num == 0)
          {
            flag2 = true;
            break;
          }
        }
      }
      return flag2 ? str.Remove(0, 1).Remove(str.Length - 2, 1) : code;
    }

    private string extractFromParen(string code, int index)
    {
      bool flag1 = false;
      bool flag2 = false;
      string str = "";
      int num = 0;
      for (int index1 = index; index1 < code.Length; ++index1)
      {
        char ch = code[index1];
        if (ch == '(')
        {
          ++num;
          if (!flag1)
            flag1 = true;
        }
        if (ch == ')')
          --num;
        if (flag1)
        {
          str += ch.ToString();
          if (num == 0)
          {
            flag2 = true;
            break;
          }
        }
      }
      return flag2 ? str.Remove(0, 1).Remove(str.Length - 2, 1) : code;
    }

    public string GetConfigJsonFromHttp(string page)
    {
      int num1 = page.IndexOf("ytplayer.config =");
      if (num1 <= 0)
        return (string) null;
      bool flag1 = false;
      bool flag2 = false;
      string str = "";
      int num2 = 0;
      int startIndex = num1;
      for (int index = num1; index < page.Length; ++index)
      {
        int num3 = (int) page[index];
        if (num3 == 123)
        {
          ++num2;
          if (!flag1)
          {
            startIndex = index;
            flag1 = true;
          }
        }
        if (num3 == 125)
          --num2;
        if (flag1 && num2 == 0)
        {
          str = page.Substring(startIndex, index - startIndex + 1);
          flag2 = true;
          break;
        }
      }
      return flag2 ? str : (string) null;
    }

    public async Task<string> GetConfigJson(string videoID)
    {
      HttpClient httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36");
      try
      {
        return this.GetConfigJsonFromHttp(await httpClient.GetStringAsync(new Uri("https://www.youtube.com/watch?v=" + videoID)));
      }
      catch
      {
        return (string) null;
      }
    }

    public string GetJSLink(string configJson)
    {
      try
      {
        return (string) JObject.Parse(configJson)["assets"][(object) "js"];
      }
      catch
      {
      }
      return (string) null;
    }

    public FunctionAndVariable GetCipherFunctionFromJS(string code)
    {
      Match match1 = this.findFirstFunction.Match(code);
      if (match1.Success)
      {
        Match match2 = this.findSetFunction.Match(match1.Value);
        if (match2.Success)
        {
          Match match3 = this.findSigFunctionName.Match(match2.Value);
          if (match3.Success)
          {
            string str1 = match3.Value.Trim().Replace("(", "").Replace(")", "");
            Match match4 = Regex.Match(code, str1 + "\\s*\\=\\s*function\\s*\\(([a-z]|[A-Z])*\\).+?}");
            if (match4.Success)
            {
              string function = this.extratFromBrackets(match4.Value, 0);
              int index1 = -1;
              string str2 = "";
              for (int index2 = 0; index2 < function.Length; ++index2)
              {
                if (function[index2] != ' ' && function[index2] != ';' && function[index2] != ',' && function[index2] != '.' && function[index2] != '=')
                  str2 += function[index2].ToString();
                else if (function[index2] == '.')
                {
                  if (code.Contains("var " + str2 + "={") && str2 != "a")
                  {
                    index1 = code.IndexOf("var " + str2 + "={");
                    break;
                  }
                  str2 = "";
                }
                else
                  str2 = "";
              }
              string variable = (string) null;
              if (index1 != -1)
                variable = this.extratFromBrackets(code, index1);
              return new FunctionAndVariable(function, variable);
            }
          }
        }
      }
      return (FunctionAndVariable) null;
    }

    public async Task<FunctionAndVariable> GetCipherFunction(string videoID)
    {
      try
      {
        return await this.GetCipherFunctionFromJSLink(this.convertToHttp(this.GetJSLink(await this.GetConfigJson(videoID))));
      }
      catch (Exception ex)
      {
        if (Debugger.IsAttached)
          Debugger.Break();
      }
      return (FunctionAndVariable) null;
    }

    public async Task<FunctionAndVariable> GetCipherFunctionFromJSLink(string link)
    {
      HttpClient httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36");
      return this.GetCipherFunctionFromJS(await httpClient.GetStringAsync(new Uri(link, UriKind.Absolute)));
    }

    public string GetCipherAlgorithm(FunctionAndVariable functionAndVariable)
    {
      string function = functionAndVariable.Function;
      string variable = functionAndVariable.Variable;
      char[] chArray = new char[1]{ ';' };
      string[] strArray1 = function.Split(chArray);
      Dictionary<string, CipherType> dictionary = new Dictionary<string, CipherType>();
      string str1 = variable;
      string[] separator = new string[1]{ "}," };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        string[] strArray2 = str2.Split(':');
        if (str2.Length > 1)
        {
          strArray2[0] = strArray2[0].Trim();
          strArray2[1] = strArray2[1].Trim();
          if (!dictionary.ContainsKey(strArray2[0]))
          {
            if (strArray2[1].Contains("reverse"))
              dictionary.Add(strArray2[0], CipherType.Reverse);
            else if (strArray2[1].Contains("slice"))
              dictionary.Add(strArray2[0], CipherType.Slice);
            else if (strArray2[1].Contains("splice"))
              dictionary.Add(strArray2[0], CipherType.Slice);
            else
              dictionary.Add(strArray2[0], CipherType.D);
          }
        }
      }
      CipherAlgoritm cipherAlgoritm = new CipherAlgoritm();
      bool flag1 = false;
      char ch;
      foreach (string code in strArray1)
      {
        if (code.Contains("reverse"))
        {
          cipherAlgoritm.Add(new CipherCode(CipherType.Reverse));
          flag1 = false;
        }
        else if (code.Contains("slice"))
        {
          string fromParen = this.extractFromParen(code, 0);
          int data = 0;
          ref int local = ref data;
          int.TryParse(fromParen, out local);
          cipherAlgoritm.Add(new CipherCode(CipherType.Slice, (object) data));
          flag1 = false;
        }
        else if (!code.Contains("split") && !code.Contains("join"))
        {
          if (code.Contains("var"))
            flag1 = true;
          else if (flag1)
          {
            string s = "";
            bool flag2 = false;
            if (code.Contains("="))
            {
              for (int index = code.IndexOf('='); index < code.Length; ++index)
              {
                if (!flag2)
                {
                  if (code[index] == '[')
                    flag2 = true;
                }
                else if (code[index] != ']' && code[index] != '%')
                {
                  string str3 = s;
                  ch = code[index];
                  string str4 = ch.ToString();
                  s = str3 + str4;
                }
                else
                  break;
              }
            }
            int result = 0;
            if (int.TryParse(s, out result))
            {
              cipherAlgoritm.Add(new CipherCode(CipherType.D, (object) result));
              flag1 = false;
            }
          }
          else
          {
            CipherType type = CipherType.D;
            if (code.Contains("."))
            {
              string str5 = "";
              bool flag3 = false;
              for (int index = 0; index < code.Length; ++index)
              {
                if (code[index] == '.' && !flag3)
                  flag3 = true;
                else if (flag3)
                {
                  if (code[index] != '(')
                  {
                    string str6 = str5;
                    ch = code[index];
                    string str7 = ch.ToString();
                    str5 = str6 + str7;
                  }
                  else
                  {
                    string key = str5.Trim();
                    if (dictionary.ContainsKey(key))
                    {
                      type = dictionary[key];
                      break;
                    }
                    str5 = "";
                  }
                }
              }
            }
            string fromParen = this.extractFromParen(code, 0);
            if (fromParen.Contains(","))
              fromParen = fromParen.Split(',')[1];
            if (type != CipherType.Reverse)
            {
              int result = 0;
              if (int.TryParse(fromParen, out result))
                cipherAlgoritm.Add(new CipherCode(type, (object) result));
            }
            else
              cipherAlgoritm.Add(new CipherCode(CipherType.Reverse));
            flag1 = false;
          }
        }
      }
      return cipherAlgoritm.ToString();
    }

    public async Task<string> GetCipherAlgorithmComplete(string videoID) => this.GetCipherAlgorithm(await this.GetCipherFunction(videoID));

    public async Task<string> GetCipherAlgorithmFromHttp(string http) => this.GetCipherAlgorithm(await this.GetCipherFunctionFromJSLink(this.convertToHttp(this.GetJSLink(this.GetConfigJsonFromHttp(http)))));

    private string convertToHttp(string link)
    {
      string oldValue = "http://";
      string str1 = "https://";
      string str2 = "www.youtube.com";
      string str3 = "";
      if (link.Contains(oldValue))
        link = link.Replace(oldValue, "");
      if (!link.Contains(str1))
        str3 += str1;
      if (!link.Contains(str2))
        str3 += str2;
      return str3 + link;
    }
  }
}
