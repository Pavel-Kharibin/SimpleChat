using System;
using System.ServiceModel;

namespace SimapleChat.Server
{
    class Program
    {
        private static ServerService _server;
        static void Main(string[] args)
        {
            _server = new ServerService();

            using (var host = new ServiceHost(_server))
            {
                host.Open();

                Console.WriteLine("Сервер запущен.");
                Console.WriteLine("<enter> для остановки.");
                Console.ReadLine();

                _server.StopAsync();
            }
        }
    }
}
