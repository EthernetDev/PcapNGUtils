using DotNetty.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace UdpClient
{
    class Program
    { 
        static UdpSocket udp = new UdpSocket(7777);
        static IPEndPoint endPiont;
        static byte[] bytes;

        static void Main(string[] args)
        {
           

            udp.OnStart(() =>
            {
                Console.WriteLine("UDP服务启动7777");
                endPiont = new IPEndPoint(IPAddress.Broadcast, 8888);
                bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")+":您好");
                Timer timer = new Timer();
                timer.Interval = 5000;
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
                timer.Start();

                udp.SendAsync(endPiont, bytes);
            });

            udp.OnRecieve(async (endPoint, bytes) =>
            {
                Console.WriteLine(endPoint);
                Console.WriteLine(Encoding.UTF8.GetString(bytes));
            });

            udp.OnException(ex =>
            {
                Console.WriteLine(ex);
            });

            udp.OnStop(ex =>
            {
                Console.WriteLine("Close:" + ex);
                //restart
                //udp.StartAsync();
            });

            udp.StartAsync();


            Console.ReadKey();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bytes = Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + ":您好");
            udp.SendAsync(endPiont, bytes);          
        }
    }
}
