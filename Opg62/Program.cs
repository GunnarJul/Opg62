using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Opg62
{
    //public  static   string _Endpoint = @"tcp://127.0.0.1:2020";//
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.Title = "Tilbudsgivere";
            var subScribers = new List<SubScriber>
                {
                  new SubScriber("Bil", "Bilforsikring. Dyt og båt", 1000),
                  new SubScriber("Bil", "Bilforsikring. Det blir dyr", 2000),
                  new SubScriber("Bil", "Bilforsikring. Det går nok over", 3000),
                  new SubScriber("Hus", "Husly", 8000),
                  new SubScriber("Hus", "Det har du ikke råd til", 20000),
                  new SubScriber("Hus", "Vi finder ud af det", 4000),
                };
            foreach ( var item in subScribers)
            {

                Task.Run(() => item.Run());
            }
            Console.ReadKey(); 
        }

        

    }

    public class SubScriber
    {
        private readonly string _subscribePattern;
        private readonly string _subscriberName;
        private readonly int _pris;
        public SubScriber(string subscribePattern, string subscriberName, int pris)
        {
            _subscribePattern = subscribePattern;
            _subscriberName = subscriberName;
            _pris = pris;
        }

        public void Run()
        {
            using (var subSocket = new SubscriberSocket())
            {
                subSocket.Options.ReceiveHighWatermark = 1000;
                subSocket.Connect(@"tcp://127.0.0.1:2020");
                subSocket.Subscribe(_subscribePattern);
                Console.WriteLine($"Subscriber socket <{_subscriberName}> connecting to pattern <{_subscribePattern}>");
                while (true)
                {
                    string messageTopicReceived = subSocket.ReceiveFrameString();
                    string messageReceived = subSocket.ReceiveFrameString();
                    Console.WriteLine($"<{_subscriberName}> Modtaget Tilbud : {messageReceived}");
                    System.Threading.Thread.Sleep(5000);
                    SendAnswer();
                }
            }
        }

        private void SendAnswer()
        {
            using (var requestSocket = new RequestSocket(">tcp://localhost:5555"))
            {
                var mess = $"Fra <{_subscriberName }>:  det koster :{_pris} kr.-";
                requestSocket.SendFrame(mess);
                var invoice = requestSocket.ReceiveFrameString();
                Console.WriteLine($"\nKvittering <{DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToShortTimeString()}> modtaget : Received '{invoice}'" );

            }
        }
    }
}


   
