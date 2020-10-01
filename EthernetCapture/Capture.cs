///////////////////////////////////////////////////
/// 文件：Capture.cs
/// 说明：捕捉数据包
/// 作者：李光强
/// 时间：2020/9/29/
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace EthernetCapture
{
    /// <summary>
    /// 数据包捕捉类
    /// </summary>
    public class Capture
    {

        Thread thread;

        /// <summary>
        /// 是否产生错误
        /// </summary>
        private bool error_occurred;
        /// <summary>
        /// 是否继续进行
        /// </summary>
        public bool KeepRunning = true;
        /// <summary>
        /// 得到的数据流的长度
        /// 定值
        /// </summary>
        private static int len_receive_buf;
        /// <summary>
        /// 收到的字节
        /// </summary>
        byte[] receive_buf_bytes;
        /// <summary>
        /// 声明套接字
        /// </summary>
        private Socket socket = null;
        /// <summary>
        /// 常量
        /// </summary>
        const int SIO_R = unchecked((int)0x98000001);
        /// <summary>
        /// 常量
        /// </summary>
        const int SIO_1 = unchecked((int)0x98000002);
        /// <summary>
        /// 常量
        /// </summary>
        const int SIO_2 = unchecked((int)0x98000003);
        public int Threads = 2;
        /// <summary>
        /// 构造函数
        /// </summary>
        public Capture(RunnableType type)
        {
            if (type == RunnableType.Listen) { }
            else if (type == RunnableType.Connet) { }
            len_receive_buf = 4096;
            receive_buf_bytes = new byte[4096];
        }
        /// <summary>
        /// 建立并绑定套接字
        /// </summary>
        /// <param name="IP">IP地址</param>
        public void CreateAndBindSocket(IPAddress IP, int port)
        {
            socket = new Socket(IP.AddressFamily, SocketType.Raw, ProtocolType.IP);
            socket.Blocking = true;//置socket非阻塞状态
            socket.Bind(new IPEndPoint(IP, port));
            if (SetSocketOption() == false) error_occurred = true;
        }
        /// <summary>
        /// 关闭原始套接字
        /// </summary>
        public void Shutdown()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        private bool SetSocketOption()
        {
            bool ret_value = true;
            try
            {
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, 1);
                byte[] IN = new byte[4] { 1, 0, 0, 0 };
                byte[] OUT = new byte[4];
                int ret_code = socket.IOControl(SIO_R, IN, OUT);//低级别操作模式
                ret_code = OUT[0] + OUT[1] + OUT[2] + OUT[3];//把4个8位字节合成一个32位整数
                if (ret_code != 0) ret_value = false;
            }
            catch (SocketException)
            {
                ret_value = false;
            }
            return ret_value;
        }
        /// <summary>
        /// 返回是否出错
        /// </summary>
        public bool ErrorOccurred
        {
            get
            {
                return error_occurred;
            }
        }
        /// <summary>
        /// 解析接收的数据包，形成PacketArrivedEventArgs时间数据类对象，并引发PacketArrival事件
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="len"></param>
        unsafe private void Receive(byte[] buf, int len)
        {
            byte temp_protocol = 0;
            uint temp_version = 0;
            uint temp_ip_srcaddr = 0;
            uint temp_ip_destaddr = 0;
            short temp_srcport = 0;
            short temp_dstport = 0;
            IPAddress temp_ip;
            PacketArrivedEventArgs e = new PacketArrivedEventArgs();
            udp_hdr* pUdpheader;//UDP头结构体指针  
            IPHeader* head;//IP头结构体指针  
            tcp_hdr* pTcpheader;//TCP头结构体指针  
            icmp_hdr* pIcmpheader;//ICMP头结构体指针 
            IPHeader2* Portheader;//端口指针

            fixed (byte* fixed_buf = buf)
            {
                int lentcp, lenudp, lenicmp, lenip;
                head = (IPHeader*)fixed_buf;//IP头结构体指针  
                Portheader = (IPHeader2*)fixed_buf;
                pTcpheader = (tcp_hdr*)(fixed_buf + sizeof(IPHeader)); //TCP头结构体指针 
                pUdpheader = (udp_hdr*)(fixed_buf + sizeof(IPHeader));
                pIcmpheader = (icmp_hdr*)(fixed_buf + sizeof(IPHeader));

                //计算各种包的长度（只有判断是否是该包后才有意义，先计算出来）
                lenip = ntohs(head->ip_totallength);
                try
                {
                    lentcp = ntohs(head->ip_totallength) - (sizeof(IPHeader) + sizeof(tcp_hdr));
                    lenudp = ntohs(head->ip_totallength) - (sizeof(IPHeader) + sizeof(udp_hdr));
                    lenicmp = ntohs(head->ip_totallength) - (sizeof(IPHeader) + sizeof(icmp_hdr));
                

                    e.HeaderLength = (uint)(head->ip_verlen & 0x0F) << 2;
                    temp_protocol = head->ip_protocol;
                    temp_dstport = *(short*)&fixed_buf[e.HeaderLength + 2];


                    switch (temp_protocol)
                    {
                        case 1:
                            e.Protocol = "ICMP"; break;
                        case 2:
                            e.Protocol = "IGMP"; break;
                        case 6:
                            e.Protocol = "TCP";
                            e.DestinationPort = ntoUint(pTcpheader->dport);
                            e.PacketLength = (uint)lenip+ (uint)(sizeof(IPHeader) + sizeof(tcp_hdr));
                            break;
                        case 17:
                            e.Protocol = "UDP";
                            e.DestinationPort = ntoUint(pUdpheader->dport);
                            e.PacketLength = (uint)lenudp+ (uint)(sizeof(IPHeader) + sizeof(udp_hdr));                           
                            break;
                        default:
                            e.Protocol = "UNKNOWN"; break;
                    }

                    temp_version = (uint)(head->ip_verlen & 0xF0) >> 4;
                    e.IPVersion = temp_version.ToString();
                    temp_ip_srcaddr = head->ip_srcaddr;
                    temp_ip_destaddr = head->ip_destaddr;
                    temp_ip = new IPAddress(temp_ip_srcaddr);
                    e.OriginationAddress = temp_ip.ToString();
                    temp_ip = new IPAddress(temp_ip_destaddr);
                    e.DestinationAddress = temp_ip.ToString();
                    temp_srcport = *(short*)&fixed_buf[e.HeaderLength];
                    e.OriginationPort = (Portheader->sport);

                    //int acb = IPAddress.NetworkToHostOrder(Portheader->sport);
                    //e.DestinationPort = ntoUint(Portheader->dport);
                    //int abc = IPAddress.NetworkToHostOrder(Portheader->dport);

                    //e.PacketLength = (uint)lenip;
                    e.MessageLength = e.PacketLength - e.HeaderLength;
                    e.ReceiveBuffer = new byte[e.PacketLength];
                    e.IPHeaderBuffer = new byte[e.HeaderLength];
                    e.MessageBuffer = new byte[e.MessageLength];

                    //把buf中的IP头赋给PacketArrivedEventArgs中的IPHeaderBuffer
                    Array.Copy(buf, 0, e.IPHeaderBuffer, 0, (int)e.HeaderLength);
                    //把buf中的包中内容赋给PacketArrivedEventArgs中的MessageBuffer
                    Array.Copy(buf, (int)e.HeaderLength, e.MessageBuffer, 0, (int)e.MessageLength);


                    //如果是TCP或UDP协议，则生成IP数据包
                    if (temp_protocol == 6 || temp_protocol == 17)
                    {
                        byte[] data = new byte[e.PacketLength];
                        Array.Copy(buf, 0, data, 0, e.PacketLength);
                        e.ReceiveBuffer = NetUtils.GeneratLinkData(e.OriginationAddress
                            , e.DestinationAddress
                            , PacketType.IPv4
                            , data);
                    }
                    else
                    {
                        e.ReceiveBuffer = new byte[e.PacketLength];
                        Array.Copy(buf, 0, e.ReceiveBuffer, 0, e.PacketLength);
                    }
                }
                catch { }
            }

            //引发PacketArrival事件
            OnPacketArrival(e);
        }
        public void Run()
        {
            KeepRunning = true;
            socket.ReceiveBufferSize = 1024 * 1024 * 10;

            thread = new Thread(new ThreadStart(() =>
            {
                while (KeepRunning)
                {
                    int received_bytes = -1;
                    byte[] Buffer = new byte[65535];
                    try
                    {
                        //此处没有接收数据包MAC和数据包类型数据，所以少了14字节
                        received_bytes = socket.Receive(Buffer, 0, 65535, SocketFlags.None);
                        //System.Diagnostics.Debug.WriteLine(BitConverter.ToString(Buffer, 16));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        KeepRunning = false;
                    }
                    if (received_bytes > 0)
                        Receive(Buffer, received_bytes);
                }
            }));
            thread.IsBackground = true;
            thread.Start();
            Thread.Sleep(100);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("IP：" + socket.LocalEndPoint.ToString() + " 数据包接收线程已启动...");
                        
            //Console.WriteLine("抓包线程分配完毕");
            Console.ResetColor();
            //IAsyncResult ar = socket.BeginReceive(receive_buf_bytes, 0, len_receive_buf, SocketFlags.None, new AsyncCallback(CallReceive), this);
        }

        public void Stop()
        {
            this.thread.Abort();
        }

        private void CallReceive(IAsyncResult ar)
        {
            int received_bytes;
            received_bytes = receive_buf_bytes.Length;
            Receive(receive_buf_bytes, received_bytes);
            if (KeepRunning) Run();
        }

        public int ntohs(ushort n)
        {
            byte[] b = BitConverter.GetBytes(n);
            Array.Reverse(b);
            return (int)BitConverter.ToInt16(b, 0);
        }
        public uint ntoUint(ushort n)
        {
            byte[] b = BitConverter.GetBytes(n);
            Array.Reverse(b);
            return (uint)BitConverter.ToInt16(b, 0);
        }
        public delegate void PacketArrivedEventHandler(
         Object sender, PacketArrivedEventArgs args);

        public event PacketArrivedEventHandler PacketArrival;

        protected virtual void OnPacketArrival(PacketArrivedEventArgs e)
        {
            if (PacketArrival != null)
            {
                PacketArrival(this, e);
            }
        }

    }
}
