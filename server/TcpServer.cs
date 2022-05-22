using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpServer
{
  private const string helpMsg = @"Suported commands:
	lb2kg [number] (convert [number] from pounds to kilograms)
	lb2kg [number] (convert [number] from kilograms to pounds)
	cm2in [number] (convert [number] from centimeters to inches)
	in2cm [number] (convert [number] from inches to centimeters)
	help (return this message)
	quit (close connection)"; 

  public static void StartServer()
  {
    TcpListener? server = null;
    try
    {
      // Set the TcpListener on port 13000.
      Int32 port = 13000;
      IPAddress localAddr = IPAddress.Parse("127.0.0.1");

      // TcpListener server = new TcpListener(port);
      server = new TcpListener(localAddr, port);

      // Start listening for client requests.
      server.Start();

      // Buffer for reading data
      Byte[] bytes = new Byte[256];
      String? data;

      // Enter the listening loop.
      while(true)
      {
        Console.Write("Waiting for a connection... ");

        // Perform a blocking call to accept requests.
        // You could also use server.AcceptSocket() here.
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!");

        data = null;

        // Get a stream object for reading and writing
        NetworkStream stream = client.GetStream();

        int i;
        // Loop to receive all the data sent by the client.
        while((i = stream.Read(bytes, 0, bytes.Length))!=0)
        {
          // Translate data bytes to a ASCII string.
          data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
          Console.WriteLine("Received: {0}", data);

          // Send back a response.
          if(data != null) {
            if(data.ToLower() == "quit") {
              // Shutdown and end connection
              byte[] res = System.Text.Encoding.ASCII.GetBytes("Closing connection.");
              stream.Write(res, 0, res.Length);
              client.Close();
              break;
            }
            else if(data.ToLower() == "help") {
              // Shutdown and end connection
              byte[] res = System.Text.Encoding.ASCII.GetBytes(helpMsg);
              stream.Write(res, 0, res.Length);
            }
            else {
              string[] args = data.Split(' ');
              if(args.Length != 2) {
                byte[] res = System.Text.Encoding.ASCII.GetBytes(
                  "Invalid arguments.\n" + helpMsg);
                stream.Write(res, 0, res.Length);
              }
              else {
                string command = args[0];
                bool isDouble = Double.TryParse(args[1], out double value);
                if(isDouble) {
                  string result = Converter(value, command);
                  byte[] res = System.Text.Encoding.ASCII.GetBytes(result);
                  stream.Write(res, 0, res.Length);
                  stream.Flush();
                }
                else {
                  byte[] res = System.Text.Encoding.ASCII.GetBytes(
                    "Invalid arguments.\n" + helpMsg
                  );
                  stream.Write(res, 0, res.Length);  
                  stream.Flush(); 
                }
              }
            }
          }
          Thread.Sleep(1000);
        }
      }
    }
    catch(SocketException e)
    {
      Console.WriteLine("SocketException: {0}", e);
    }
    finally
    {
      if (server != null)
      {
        // Stop listening for new clients.
        server.Stop();
      }
    }

    Console.WriteLine("\nHit enter to continue...");
    Console.Read();
  }

  private static string Converter(double value, string command) {
    double lb2kg = 0.45359237; // pounds to kilograms
    double in2cm = 2.54; // inches to centimeters

    switch (command.ToLower()) {
      case "lb2kg":

        return $"{value} lbs is {Math.Round(value*lb2kg, 4, MidpointRounding.ToEven)} kg";
      case "kg2lb":
        return $"{value} kg is {Math.Round(value/lb2kg, 4, MidpointRounding.ToEven)} lbs";
      case "cm2in":
        return $"{value} cm is {Math.Round(value/in2cm, 4, MidpointRounding.ToEven)} in";
      case "in2cm":
        return $"{value} in is {Math.Round(value*in2cm, 4, MidpointRounding.ToEven)} cm";
      default:
        return "Invalid arguments.\n" + helpMsg;
    }
  }
}