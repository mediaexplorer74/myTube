// Decompiled with JetBrains decompiler
// Type: myTube.CastingException
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;

namespace myTube
{
  public class CastingException : Exception
  {
    public CastingException()
      : this("Unable to cast to the selected device")
    {
    }

    public CastingException(string message)
      : this(message, (Exception) null)
    {
    }

    public CastingException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
