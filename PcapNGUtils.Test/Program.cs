using EthernetCapture;
using PcapngUtils.Common;
using PcapngUtils.Pcap;
using PcapngUtils.PcapNG;
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
            try
            {
                //CancellationTokenSource tokenSource = new CancellationTokenSource();
                //OpenPcapFile(@"d:\temp\2020-05-16-20-08.pcap", tokenSource.Token);
                //WritePcapFile();

                CaptureHelper.Instance.StartCapturing();

                DisplayMenu();


            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" #异常：" + e.Message+System.Environment.NewLine);
                Console.ResetColor();
            }
          
        }

        #region 显示菜单及操作
        static private void DisplayMenu()
        {
            Console.WriteLine("\n");
            Console.WriteLine("=================================================");
            Console.WriteLine("        LiDar Data Collection  Application ");
            Console.WriteLine(" Copyright by Li Jingtong Beidou Inst., Since 2020.");
            Console.WriteLine("=================================================\n");
            Console.WriteLine("0.  Detect NetInterface");
            Console.WriteLine("x.  Exit");
            Console.Write("> ");
            DoMenu();
        }
        static void DoMenu()
        {
            string s = Console.ReadLine();

            switch (s)
            {
                case "0":
                    //DetectDevice();
                    break;               
                case "x":
                case "X":
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    break;
                default:
                    break;
            }
            Console.WriteLine("#### 按任意键返回主菜单！");
            Console.ReadKey();
            DisplayMenu();
        }

        #endregion

        #region 测试程序 

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

        public static void WritePcapFile()
        {
            //IPacket packet = new PcapPacket();
            //PcapNGWriter writer = new PcapNGWriter(@"d:\temp\new.pcap");            
            // writer.WritePacket();

            using (var reader = new PcapReader(@"d:\temp\2020-05-16-20-08.pcap"))
            {
                using (var writer = new PcapNGWriter(@"d:\temp\new.pcap"))
                {
                    CommonDelegates.ReadPacketEventDelegate handler = (obj, packet) =>
                    {
                        writer.WritePacket(packet);
                    };
                    CancellationTokenSource tokenSource = new CancellationTokenSource();
                    reader.OnReadPacketEvent += handler;
                    reader.ReadPackets(tokenSource.Token);
                    reader.OnReadPacketEvent -= handler;
                }
            }

        }
        #endregion
    }
}
