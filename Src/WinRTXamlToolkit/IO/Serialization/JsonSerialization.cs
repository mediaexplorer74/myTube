// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.IO.Serialization.JsonSerialization
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using WinRTXamlToolkit.IO.Extensions;

namespace WinRTXamlToolkit.IO.Serialization
{
  public static class JsonSerialization
  {
    public static async Task SerializeAsJson<T>(
      this T objectGraph,
      string fileName,
      StorageFolder folder = null,
      CreationCollisionOption options = 2)
    {
      folder = folder ?? ApplicationData.Current.LocalFolder;
      try
      {
        StorageFile file = await folder.CreateFileAsync(fileName, options);
        using (Stream stream = await WindowsRuntimeStorageExtensions.OpenStreamForWriteAsync((IStorageFile) file))
          new DataContractJsonSerializer((Type) typeof (T)).WriteObject(stream, (object) (T) objectGraph);
      }
      catch (Exception ex)
      {
        if (Debugger.IsAttached)
          Debugger.Break();
        throw;
      }
    }

    public static string SerializeAsJson<T>(this T graph)
    {
      if ((object) graph == null)
        return (string) null;
      DataContractJsonSerializer contractJsonSerializer = new DataContractJsonSerializer((Type) typeof (T));
      MemoryStream memoryStream = new MemoryStream();
      contractJsonSerializer.WriteObject((Stream) memoryStream, (object) graph);
      byte[] array = memoryStream.ToArray();
      return Encoding.UTF8.GetString(array, 0, array.Length);
    }

    public static async Task<T> LoadFromJsonFile<T>(string fileName, StorageFolder folder = null)
    {
      string json = await StringIOExtensions.ReadFromFile(fileName, folder);
      return JsonSerialization.LoadFromJsonString<T>(json);
    }

    public static T LoadFromJsonString<T>(string json) => (T) new DataContractJsonSerializer((Type) typeof (T)).ReadObject((Stream) new MemoryStream(Encoding.UTF8.GetBytes(json)));
  }
}
