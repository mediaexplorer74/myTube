// Decompiled with JetBrains decompiler
// Type: FrameworkElementRendering.MemoryRandomAccessStream
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace FrameworkElementRendering
{
  public class MemoryRandomAccessStream : 
    IRandomAccessStream,
    IDisposable,
    IInputStream,
    IOutputStream
  {
    private readonly Stream _internalStream;

    public MemoryRandomAccessStream(Stream stream) => this._internalStream = stream;

    public MemoryRandomAccessStream(byte[] bytes) => this._internalStream = (Stream) new MemoryStream(bytes);

    public IInputStream GetInputStreamAt(ulong position)
    {
      this._internalStream.Position = (long) position;
      return this._internalStream.AsInputStream();
    }

    public IOutputStream GetOutputStreamAt(ulong position)
    {
      this._internalStream.Position = (long) position;
      return this._internalStream.AsOutputStream();
    }

    public ulong Size
    {
      get => (ulong) this._internalStream.Length;
      set => this._internalStream.SetLength((long) value);
    }

    public bool CanRead => true;

    public bool CanWrite => true;

    public IRandomAccessStream CloneStream() => throw new NotSupportedException();

    public ulong Position => (ulong) this._internalStream.Position;

    public void Seek(ulong position) => this._internalStream.Seek((long) position, SeekOrigin.Begin);

    public void Dispose() => this._internalStream.Dispose();

    public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(
      IBuffer buffer,
      uint count,
      InputStreamOptions options)
    {
      return this.GetInputStreamAt(0UL).ReadAsync(buffer, count, options);
    }

    public IAsyncOperation<bool> FlushAsync() => this.GetOutputStreamAt(0UL).FlushAsync();

    public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer) => this.GetOutputStreamAt(0UL).WriteAsync(buffer);

    //[SpecialName]
    //void IRandomAccessStream.put_Size(ulong value) => this.Size = value;
  }
}
