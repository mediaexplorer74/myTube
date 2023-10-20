// Decompiled with JetBrains decompiler
// Type: RykenTube.Server.SocketServer
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace RykenTube.Server
{
  public abstract class SocketServer
  {
    private const int BufferSize = 8192;
    private const int ResponseBufferSize = 8192;
    private StreamSocketListener listener;
    private bool initialized;

    private async Task initialize()
    {
      SocketServer socketServer = this;
      if (socketServer.initialized)
        return;
      socketServer.initialized = true;
      socketServer.listener = new StreamSocketListener();
      StreamSocketListener listener = socketServer.listener;

       //TODO
      // ISSUE: method pointer
      /*
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<StreamSocketListener, 
          StreamSocketListenerConnectionReceivedEventArgs>>(
          new Func<TypedEventHandler<StreamSocketListener, 
          StreamSocketListenerConnectionReceivedEventArgs>, EventRegistrationToken>
          (listener.add_ConnectionReceived), 
          new Action<EventRegistrationToken>(listener.remove_ConnectionReceived), 
          new TypedEventHandler<StreamSocketListener, 
          StreamSocketListenerConnectionReceivedEventArgs>((object) socketServer,
          __methodptr(listener_ConnectionReceived)));
      */
    }

    private async void listener_ConnectionReceived(
      StreamSocketListener sender,
      StreamSocketListenerConnectionReceivedEventArgs args)
    {
      StreamSocket socket = args.Socket;
      new byte[8192].AsBuffer();
      MemoryStream memoryStream = new MemoryStream();
      using (IInputStream input = socket.InputStream)
      {
        Stream stream = await this.Respond(input.AsStreamForRead());
      }
    }

    public abstract Task<Stream> Respond(Stream stream);

    public async Task BeginListening(string hostName, int port)
    {
      await this.initialize();
      await this.listener.BindEndpointAsync(new HostName(hostName), port.ToString());
    }

    public void Dispose()
    {
      if (!this.initialized)
        return;
      this.listener.Dispose();

            //TODO
            /*
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<StreamSocketListener, 
          StreamSocketListenerConnectionReceivedEventArgs>>(
          new Action<EventRegistrationToken>(this.listener.remove_ConnectionReceived), 
          new TypedEventHandler<StreamSocketListener, 
          StreamSocketListenerConnectionReceivedEventArgs>((object) this, 
          __methodptr(listener_ConnectionReceived)));
            */
    }
  }
}
