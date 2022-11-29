
using TetrisServer2.Server;


Server server = new Server("127.0.0.1", 333);


try
{
    server.startServer();
    await server.ConnectNewClient();
}
   
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    server.Stop();
}


//server.();

//server.connectNewClient();


//while (true)
//{
//    server.HandleResponse();
//}