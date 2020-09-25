using PcapngUtils.Common;
using PcapngUtils.Pcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PcapNGUtils.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            OpenPcapFile(@"d:\temp\2020-05-16-20-08.pcap", tokenSource.Token);
            Console.ReadKey();
        }

        public static void OpenPcapFile(string filename, CancellationToken token)
        {
            using (var reader = new PcapReader(filename))
            {
                reader.OnReadPacketEvent += reader_OnReadPacketEvent;
                reader.ReadPackets(token);
                reader.OnReadPacketEvent -= reader_OnReadPacketEvent;
            }
        }
        public static void reader_OnReadPacketEvent(object context, IPacket packet)
        {
            Console.WriteLine(string.Format("Packet received {0}.{1}"
                , packet.Seconds+"."+ packet.Microseconds+"\t"
                , BitConverter.ToString(packet.Data,16).Replace("-"," ")));
        }
    }
}
