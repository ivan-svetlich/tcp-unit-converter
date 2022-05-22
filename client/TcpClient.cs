using System.Net.NetworkInformation;
using System.Net.Sockets;

public static class MyTcpClient {
  public static void StartClient(String server)
  {
    try
    {
      // Create a TcpClient.
      // Note, for this client to work you need to have a TcpServer
      // connected to the same address as specified by the server, port
      // combination.
      Int32 port = 13000;
      using(TcpClient client = new TcpClient(server, port)) {
        using(NetworkStream stream = client.GetStream()) {
          while(true) {
            if(!GetState(client)) {
              break;
            }
            Console.Write("\nEnter your message: ");
            string? message = "";
            while(message == "" || message == null) {
              message = Console.ReadLine();
            }
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            // Buffer to store the response bytes.
            Byte[] resBuffer = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Console.Write("Received: ");
            bool streamEnd = false;
            Int32 bytes;
            while(!streamEnd) {
              bytes = stream.Read(resBuffer, 0, resBuffer.Length);
              if(bytes < resBuffer.Length) {
                streamEnd = true;
              }
              responseData = System.Text.Encoding.ASCII.GetString(resBuffer, 0, bytes);
              Console.Write(responseData);
            }
            Console.Write("\n");
            if(message.ToLower() == "quit") {
              break;
            }
          }
        }
      }
    }
    catch (ArgumentNullException e)
    {
      Console.WriteLine("ArgumentNullException: {0}", e);
    }
    catch (SocketException e)
    {
      Console.WriteLine("SocketException: {0}", e);
    }
    Console.WriteLine("\n Press Enter to continue...");
    Console.Read();
  }
  public static bool GetState(this TcpClient tcpClient)
  {
    var foo = IPGlobalProperties.GetIPGlobalProperties()
      .GetActiveTcpConnections()
      .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
    return foo != null ? true : false;
  }
}