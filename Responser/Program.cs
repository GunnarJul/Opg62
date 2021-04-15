using System;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using System.Threading.Tasks;


namespace Responser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Responser";
            Console.WriteLine ("Venter på svar fra tilbudsgivere");
            var responser = new Responser();
            Task.Run(() => responser.Run());
            Console.ReadKey();
        }
    }
    public class Responser
    {
        public Responser()
        {

        }
        public void Run()
        {

            using (var responseSocket = new ResponseSocket("@tcp://*:5555"))
            {
                while (true)
                {
                    var message = responseSocket.ReceiveFrameString();
                    Console.WriteLine($"\nResponser : Server Received ");
                    Console.WriteLine($"Tilbud er modtaget {DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToShortTimeString()}{DateTime.UtcNow.Second} : {message}");
                    responseSocket.SendFrame("Tak for det. Dit tilbud er modtaget");
                }
            }
        }
    }
}
