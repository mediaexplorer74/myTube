// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.IO.Serialization.XmlSerialization
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using WinRTXamlToolkit.IO.Extensions;

namespace WinRTXamlToolkit.IO.Serialization
{
  public static class XmlSerialization
  {
    public static async Task SerializeAsXmlDataContract<T>(
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
          new DataContractSerializer((Type) typeof (T)).WriteObject(stream, (object) (T) objectGraph);
      }
      catch (Exception ex)
      {
        if (Debugger.IsAttached)
          Debugger.Break();
        throw;
      }
    }

    public static string SerializeAsXmlDataContract<T>(this T objectGraph)
    {
      DataContractSerializer contractSerializer = new DataContractSerializer((Type) typeof (T));
      MemoryStream memoryStream = new MemoryStream();
      contractSerializer.WriteObject((Stream) memoryStream, (object) objectGraph);
      byte[] array = memoryStream.ToArray();
      return Encoding.UTF8.GetString(array, 0, array.Length);
    }

    public static async Task<T> LoadFromXmlDataContractFile<T>(
      string fileName,
      StorageFolder folder = null)
    {
      string xmlString = await StringIOExtensions.ReadFromFile(fileName, folder);
      MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
      DataContractSerializer ser = new DataContractSerializer((Type) typeof (T));
      T result = (T) ser.ReadObject((Stream) ms);
      return result;
    }

    public static async Task SerializeAsXml<T>(
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
          new XmlSerializer((Type) typeof (T)).Serialize(stream, (object) (T) objectGraph);
      }
      catch (Exception ex)
      {
        if (Debugger.IsAttached)
          Debugger.Break();
        throw;
      }
    }

    public static string SerializeAsXml<T>(this T objectGraph)
    {
      XmlSerializer xmlSerializer = new XmlSerializer((Type) typeof (T));
      MemoryStream memoryStream = new MemoryStream();
      xmlSerializer.Serialize((Stream) memoryStream, (object) objectGraph);
      byte[] array = memoryStream.ToArray();
      return Encoding.UTF8.GetString(array, 0, array.Length);
    }

    public static async Task<T> LoadFromXmlFile<T>(string fileName, StorageFolder folder = null)
    {
      string xmlString = await StringIOExtensions.ReadFromFile(fileName, folder);
      MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
      XmlSerializer ser = new XmlSerializer((Type) typeof (T));
      T result = (T) ser.Deserialize((Stream) ms);
      return result;
    }
  }
}
