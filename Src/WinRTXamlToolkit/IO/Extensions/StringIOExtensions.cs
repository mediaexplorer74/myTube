// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.IO.Extensions.StringIOExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WinRTXamlToolkit.IO.Extensions
{
  public static class StringIOExtensions
  {
    public static async Task<string> ReadFromFile(string fileName, StorageFolder folder = null)
    {
      folder = folder ?? ApplicationData.Current.LocalFolder;
      StorageFile file = await folder.GetFileAsync(fileName);
      string str;
      using (IRandomAccessStream fs = await file.OpenAsync((FileAccessMode) 0))
      {
        using (IInputStream inStream = fs.GetInputStreamAt(0UL))
        {
          using (DataReader reader = new DataReader(inStream))
          {
            int num = (int) await (IAsyncOperation<uint>) reader.LoadAsync((uint) fs.Size);
            string data = reader.ReadString((uint) fs.Size);
            reader.DetachStream();
            str = data;
          }
        }
      }
      return str;
    }

    public static async Task WriteToFile(
      this string text,
      string fileName,
      StorageFolder folder = null,
      CreationCollisionOption options = 1)
    {
      folder = folder ?? ApplicationData.Current.LocalFolder;
      StorageFile file = await folder.CreateFileAsync(fileName, options);
      using (IRandomAccessStream fs = await file.OpenAsync((FileAccessMode) 1))
      {
        using (IOutputStream outStream = fs.GetOutputStreamAt(0UL))
        {
          using (DataWriter dataWriter = new DataWriter(outStream))
          {
            if (text != null)
            {
              int num1 = (int) dataWriter.WriteString(text);
            }
            int num2 = (int) await (IAsyncOperation<uint>) dataWriter.StoreAsync();
            dataWriter.DetachStream();
          }
          int num = await outStream.FlushAsync() ? 1 : 0;
        }
      }
    }
  }
}
