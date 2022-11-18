using TetrisServer2.Server;

Server server = new Server("127.0.0.1", 333);

server.startServer();

server.connectNewClient();

while (true)
{
    server.HandleResponse();
}