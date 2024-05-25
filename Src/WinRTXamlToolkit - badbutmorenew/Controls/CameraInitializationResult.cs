// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.CameraInitializationResult
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;

namespace WinRTXamlToolkit.Controls
{
  public class CameraInitializationResult
  {
    public Exception Error { get; private set; }

    public string ErrorMessage { get; private set; }

    public bool Success { get; private set; }

    internal CameraInitializationResult(bool success, string error = null, Exception exception = null)
    {
      this.Error = exception;
      this.ErrorMessage = error;
      this.Success = success;
    }
  }
}
