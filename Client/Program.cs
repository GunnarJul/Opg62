using NetMQ;
using NetMQ.Sockets;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NetMQ.Monitoring;


namespace Client
{
    class Program
    {
        private const string _Endpoint = @"tcp://127.0.0.1:2020";
        static void Main(string[] args)
        {
            Console.Title = "Publisher Tast q for afslut";

            //var responser = new Responser();
            //Task.Run(() => responser.Run());
                             
            int indx = 0;
            using (var pubSocket = new PublisherSocket(_Endpoint))
            {
                Console.WriteLine("Publisher socket binding");
                Console.WriteLine();
                Console.WriteLine ("Skriv ny forespørgelse for en Bilforsikring");
                Console.Write(" >");

                while (true)
                {

                    var keys = Console.ReadLine();
                    if (keys.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                        break;

                    var topic = (indx % 2) == 0 ? "Bil" : "Hus";
                    pubSocket.SendMoreFrame(topic).SendFrame(keys);
                    
                    indx++;
                    topic = (indx % 2) == 0 ? "Bil" : "Hus";
                    Console.WriteLine($"Skriv ny forespørgelse for en {topic}forsikring");
                    Console.Write(" >");
                }
            }
        
        Console.WriteLine("I'm done slutter herfra. Press any key");
            Console.ReadKey();
        }

   

}
}
