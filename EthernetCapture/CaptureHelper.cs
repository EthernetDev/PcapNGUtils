///////////////////////////////////////
/// 文件：CaptureHelper.cs
/// 说明：网络操作助手
/// 作者：李光强
/// 时间：2020/9/28/
///////////////////////////////////////

using PcapngUtils.Common;
using PcapngUtils.Pcap;
using PcapngUtils.PcapNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EthernetCapture
{
    /// <summary>
    /// 网络助手
    /// </summary>
    public class CaptureHelper
    {

        #region 类实例
        /// <summary>
        /// 类实例
        /// </summary>
        private static CaptureHelper instance;

        /// <summary>
        /// 类实例
        /// </summary>
        public static CaptureHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new CaptureHelper();

                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 监听的IP地址
        /// </summary>
        IPAddress capturedIp;

        /// <summary>
        /// 根据设置的参数，获取要监听的网络地址
        /// </summary>
        /// <returns></returns>
        private IPAddress GetIPAddress()
        {
            if (string.IsNullOrEmpty(Setting.Instance.CapturedIp))
                throw new Exception("没有设置要监测的IP地址");

            IPAddress[] iPAddress = Dns.GetHostAddresses(Dns.GetHostName());
            for (int i = 0; i < iPAddress.Length; i++)
            {
                if (iPAddress[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    if (Setting.Instance.CapturedIp == iPAddress[i].ToString())
                        return iPAddress[i];
            }

            return null;
        }

        Capture rawSocket;
        /// <summary>
        /// 开启监听
        /// </summary>
        public void StartCapturing()
        {
            try
            {
                //读取配置文件
                Setting.Instance.ReadConfig();
                //根据配置参数，获取要监听的IP
                this.capturedIp = this.GetIPAddress();

                if (this.capturedIp==null)
                {
                    throw new Exception("没有找到指定的IP地址");
                }

                rawSocket = new Capture(RunnableType.Listen);
                rawSocket.PacketArrival += OnCapture;
                rawSocket.CreateAndBindSocket(this.capturedIp, 0);
                rawSocket.Run();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void StopCapturing()
        {
            rawSocket.Stop();
        }

        PcapWriter writer = new PcapWriter(@"d:\temp\new1.pcap");

        /// <summary>
        /// 捕捉事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCapture(Object sender, PacketArrivedEventArgs args)
        {
            //Console.Clear();
            //Length = Length + args.PacketLength;
            //args.Protocol可以是TCP:/UDP:/ICMP:/IGMP:/UNKNOWN
            if (args.Protocol != Setting.Instance.Protocal.ToString())
                return;

            if (this.capturedIp.ToString()!=args.DestinationAddress
                || args.DestinationPort != Setting.Instance.CapturedPort)
            {
                return;
            }
            
            Console.WriteLine("  目标IP：" + args.DestinationAddress
                + "\t目标端口：" + args.DestinationPort
                + "\t协议版本：" + args.IPVersion
                + "\t源地址：" + args.OriginationAddress
                + "\t源端口：" + args.OriginationPort
                + "\tIP包长度:" + args.PacketLength
                + "\t协议类型：" + args.Protocol);

            Console.WriteLine(" Data: " + BitConverter.ToString(args.ReceiveBuffer, 16));
            

            Task.Run(() =>
            {
                DateTime timeCreated = DateTime.Now;
                UInt32 ts_sec = (UInt32)((timeCreated.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
                UInt32 ts_usec = (UInt32)(((timeCreated.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds) - ((UInt32)((timeCreated.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000))) * 1000;
                UInt32 incl_len =(UInt32)args.PacketLength;

                PcapPacket packet = new PcapPacket(ts_sec, ts_usec, args.ReceiveBuffer, args.PacketLength);
                               
                writer.WritePacket(packet);                   
                
            });
        }

        

        private string GetSpand(double Length, bool IsBit)
        {
            if (IsBit)
            {
                if (Length < 1024)
                    return Length.ToString("0.000") + GetLegth(Length.ToString("0.000").Length) + "Bps";
                else if (Length > 1024 && Length < (1024 * 1024))
                    return (Length / (1024)).ToString("0.000") + GetLegth((Length / (1024)).ToString("0.000").Length) + "Kbps";
                else if (Length > (1024 * 1024) && Length < (1024 * 1024 * 1024))
                    return (Length / (1024 * 1024)).ToString("0.000") + GetLegth((Length / (1024 * 1024)).ToString("0.000").Length) + "Mbps";
                else
                    return (Length / (1024 * 1024 * 1024)).ToString("0.000") + GetLegth((Length / (1024 * 1024 * 1024)).ToString("0.000").Length) + "Gbps";
            }
            else
            {
                if (Length < 1024)
                    return Length.ToString("0.000") + GetLegth(Length.ToString("0.000").Length) + "Byte";
                else if (Length > 1024 && Length < (1024 * 1024))
                    return (Length / (1024)).ToString("0.000") + GetLegth((Length / (1024)).ToString("0.000").Length) + "KB";
                else if (Length > (1024 * 1024) && Length < (1024 * 1024 * 1024))
                    return (Length / (1024 * 1024)).ToString("0.000") + GetLegth((Length / (1024 * 1024)).ToString("0.000").Length) + "MB";
                else
                    return (Length / (1024 * 1024 * 1024)).ToString("0.000") + GetLegth((Length / (1024 * 1024 * 1024)).ToString("0.000").Length) + "GB";
            }
        }
        private string GetLegth(int Length)
        {
            string Return = "";
            for (int i = Length; i < 22; i++) { Return = Return + " "; }
            return Return;
        }
    }
}
