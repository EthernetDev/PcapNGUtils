using DotNetty.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var udp = new UdpSocket(2368);

            udp.OnStart(() =>
            {
                Console.WriteLine("UDP服务启动2368");
            });

            udp.OnRecieve((endPoint, bytes) =>
            {
                Console.WriteLine(endPoint);
                Console.WriteLine(Encoding.UTF8.GetString(bytes));
                udp.SendAsync(endPoint, bytes);
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
    }
}
