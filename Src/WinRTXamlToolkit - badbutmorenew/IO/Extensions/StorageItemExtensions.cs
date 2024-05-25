// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.IO.Extensions.StorageItemExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace WinRTXamlToolkit.IO.Extensions
{
  public static class StorageItemExtensions
  {
    public static async Task<ulong> GetFreeSpace(this IStorageItem sf)
    {
      BasicProperties properties = await sf.GetBasicPropertiesAsync();
      IDictionary<string, object> filteredProperties = await (IAsyncOperation<IDictionary<string, object>>) properties.RetrievePropertiesAsync((IEnumerable<string>) new string[1]
      {
        "System.FreeSpace"
      });
      object freeSpace = filteredProperties["System.FreeSpace"];
      return (ulong) freeSpace;
    }

    public static async Task<ulong> GetSize(this IStorageItem file)
    {
      BasicProperties props = await file.GetBasicPropertiesAsync();
      ulong sizeInB = props.Size;
      return sizeInB;
    }

    public static string GetSizeString(this long sizeInB) => ((ulong) sizeInB).GetSizeString();

    public static string GetSizeString(
      this ulong sizeInB,
      double promoteLimit = 1024.0,
      double decimalLimit = 10.0,
      string separator = " ")
    {
      if ((double) sizeInB < promoteLimit)
        return string.Format("{0}{1}B", (object) sizeInB, (object) separator);
      double num1 = (double) sizeInB / 1024.0;
      if (num1 < decimalLimit)
        return string.Format("{0:F1}{1}KB", (object) num1, (object) separator);
      if (num1 < promoteLimit)
        return string.Format("{0:F0}{1}KB", (object) num1, (object) separator);
      double num2 = num1 / 1024.0;
      if (num2 < decimalLimit)
        return string.Format("{0:F1}{1}MB", (object) num2, (object) separator);
      if (num2 < promoteLimit)
        return string.Format("{0:F0}{1}MB", (object) num2, (object) separator);
      double num3 = num2 / 1024.0;
      if (num3 < decimalLimit)
        return string.Format("{0:F1}{1}GB", (object) num3, (object) separator);
      if (num3 < promoteLimit)
        return string.Format("{0:F0}{1}GB", (object) num3, (object) separator);
      double num4 = num3 / 1024.0;
      return num4 < decimalLimit ? string.Format("{0:F1}{1}TB", (object) num4, (object) separator) : string.Format("{0:F0}{1}TB", (object) num4, (object) separator);
    }
  }
}
