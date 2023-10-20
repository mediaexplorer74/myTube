// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.TryCatchRetry
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;

namespace WinRTXamlToolkit.Tools
{
  public static class TryCatchRetry
  {
    public static void Run<T>(Action action, int retries = 1, bool throwOnFail = false) where T : Exception
    {
      int num = 0;
label_1:
      ++num;
      try
      {
        action();
      }
      catch (T ex)
      {
        if (Debugger.IsAttached)
          Debugger.Break();
        if (num > retries)
        {
          if (!throwOnFail)
            return;
          throw;
        }
        else
          goto label_1;
      }
    }

    public static async Task RunWithDelayAsync<T>(
      Task task,
      TimeSpan delay,
      int retries = 1,
      bool throwOnFail = false)
      where T : Exception
    {
      int attempts = 0;
      while (true)
      {
        ++attempts;
        try
        {
          await task;
          break;
        }
        catch (T ex)
        {
          if (Debugger.IsAttached)
            Debugger.Break();
          if (attempts > retries)
          {
            if (!throwOnFail)
              break;
            throw;
          }
        }
        await Task.Delay((TimeSpan) delay);
      }
    }

    public static async Task<TResult> RunWithDelayAsync<TException, TResult>(
      Task<TResult> task,
      TimeSpan delay,
      int retries = 1,
      bool throwOnFail = false)
      where TException : Exception
    {
      int attempts = 0;
      while (true)
      {
        ++attempts;
        try
        {
          return await task;
        }
        catch (TException ex)
        {
          if (attempts > retries)
          {
            if (!throwOnFail)
              return default (TResult);
            throw;
          }
        }
        await Task.Delay((TimeSpan) delay);
      }
    }

    public static async Task<TResult> RunWithDelayAsync<TException, TResult>(
      IAsyncOperation<TResult> operation,
      TimeSpan delay,
      int retries = 1,
      bool throwOnFail = false)
      where TException : Exception
    {
      int attempts = 0;
      while (true)
      {
        ++attempts;
        try
        {
          return await operation;
        }
        catch (TException ex)
        {
          if (attempts > retries)
          {
            if (!throwOnFail)
              return default (TResult);
            throw;
          }
        }
        await Task.Delay((TimeSpan) delay);
      }
    }
  }
}
