// Decompiled with JetBrains decompiler
// Type: RykenTube.TypeConstructor
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;

namespace RykenTube
{
  public class TypeConstructor
  {
    private static Dictionary<string, Type> typeValues = new Dictionary<string, Type>()
    {
      {
        "i",
        typeof (int)
      },
      {
        "st",
        typeof (string)
      },
      {
        "s",
        typeof (SignedInUserClient)
      },
      {
        "u",
        typeof (UserClient)
      },
      {
        "p",
        typeof (PlaylistClient)
      },
      {
        "f",
        typeof (FeedClient)
      },
      {
        "ti",
        typeof (YouTubeTime)
      },
      {
        "ytf",
        typeof (YouTubeFeed)
      },
      {
        "c",
        typeof (Category)
      },
      {
        "uf",
        typeof (UserFeed)
      },
      {
        "subs",
        typeof (SubscriptionsPageClient)
      }
    };
    private const string Separator = "s.-";
    private const string ParamSeparator = "p.-";

    public Type Type { get; set; }

    public object[] Params { get; set; }

    public TypeConstructor()
    {
    }

    public TypeConstructor(Type type, params object[] param)
    {
      this.Type = type;
      this.Params = param;
    }

    public object Construct()
    {
      if (this.Type != null)
      {
        try
        {
          return Activator.CreateInstance(this.Type, this.Params);
        }
        catch (Exception ex)
        {
        }
      }
      return (object) null;
    }

    public override string ToString()
    {
      string str = this.Type == null ? typeof (object).ToString() : this.typeToString(this.Type);
      if (this.Params != null)
      {
        str += "s.-";
        for (int index = 0; index < this.Params.Length; ++index)
        {
          object obj = this.Params[index];
          if (obj != null)
            str = str + this.paramToString(obj) + "p.-" + this.typeToString(obj.GetType()) + (index == this.Params.Length - 1 ? "" : "s.-");
          else
            str = str + "x:Nullp.-x:Null" + (index == this.Params.Length - 1 ? "" : "s.-");
        }
      }
      return str;
    }

    private string paramToString(object param)
    {
      try
      {
        if (Enum.IsDefined(param.GetType(), param))
        {
          if (Enum.GetUnderlyingType(param.GetType()) == typeof (int))
            return ((int) param).ToString();
        }
      }
      catch
      {
      }
      return param.ToString();
    }

    private string typeToString(Type t)
    {
      foreach (KeyValuePair<string, Type> typeValue in TypeConstructor.typeValues)
      {
        if (typeValue.Value == t)
          return typeValue.Key;
      }
      return t.ToString();
    }

    public static TypeConstructor FromString(string s)
    {
      TypeConstructor typeConstructor = new TypeConstructor();
      string[] strArray = s.Split("s.-");
      if (strArray.Length != 0)
      {
        try
        {
          typeConstructor.Type = TypeConstructor.typeValues.ContainsKey(strArray[0]) ? TypeConstructor.typeValues[strArray[0]] : Type.GetType(strArray[0]);
        }
        catch
        {
          return (TypeConstructor) null;
        }
        if (strArray.Length > 1)
        {
          typeConstructor.Params = new object[strArray.Length - 1];
          for (int index = 1; index < strArray.Length; ++index)
            typeConstructor.Params[index - 1] = TypeConstructor.fromParam(strArray[index]);
        }
        else
          typeConstructor.Params = new object[0];
      }
      return typeConstructor;
    }

    private static object fromParam(string paramString)
    {
      string[] strArray = paramString.Split("p.-");
      return strArray.Length > 1 ? TypeConstructor.fromParam(strArray[0], strArray[1]) : (object) null;
    }

    private static object fromParam(string param, string type)
    {
      try
      {
        if (type == "x:Null")
        {
          string type1 = (string) null;
          return TypeConstructor.fromParam(param, type1);
        }
        Type type2 = !TypeConstructor.typeValues.ContainsKey(type) ? Type.GetType(type, true) : TypeConstructor.typeValues[type];
        return TypeConstructor.fromParam(param, type2);
      }
      catch
      {
        return (object) null;
      }
    }

    private static object fromParam(string param, Type type)
    {
      if (param == "x:Null")
        param = (string) null;
      try
      {
        if (type == typeof (int))
          return (object) int.Parse(param);
        if (type == typeof (string))
          return (object) param;
        try
        {
          if (!Enum.IsDefined(type, (object) param))
            return Enum.Parse(type, param);
          int result = 0;
          if (int.TryParse(param, out result))
          {
            Enum.GetUnderlyingType(type);
            Type type1 = typeof (int);
          }
          return Enum.Parse(type, param);
        }
        catch
        {
        }
        return (object) null;
      }
      catch
      {
        return (object) null;
      }
    }

    private static int toInt(string param) => param.Length;
  }
}
