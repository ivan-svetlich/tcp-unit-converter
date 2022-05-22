namespace Client {
  class Program{
      public static int Main(String[] args)
    {  
      MyTcpClient.StartClient("127.0.0.1");
      return 0;
    }
  }
}