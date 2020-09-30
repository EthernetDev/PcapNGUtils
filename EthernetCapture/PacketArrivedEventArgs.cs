using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthernetCapture
{

    /// <summary>
    /// 数据接收事件
    /// </summary>
    public class PacketArrivedEventArgs : EventArgs
    {
        /// <summary>
        /// 得到的数据流的长度
        /// 定值
        /// </summary>
        private static int len_receive_buf;

        public PacketArrivedEventArgs()
        {
            this.protocol = "";
            this.destination_port = 0;
            this.origination_port = 0;
            this.destination_address = "";
            this.origination_address = "";
            this.ip_version = "";

            this.total_packet_length = 0;
            this.message_length = 0;
            this.header_length = 0;

            this.receive_buf_bytes = new byte[len_receive_buf];
            this.ip_header_bytes = new byte[len_receive_buf];
            this.message_bytes = new byte[len_receive_buf];
        }
        /// <summary>
        /// 数据包协议类型
        /// </summary>
        public string Protocol
        {
            get { return protocol; }
            set { protocol = value; }
        }
        /// <summary>
        /// 目标端口
        /// </summary>
        public uint DestinationPort
        {
            get { return destination_port; }
            set { destination_port = value; }
        }
        /// <summary>
        /// 源端口
        /// </summary>
        public uint OriginationPort
        {
            get { return origination_port; }
            set { origination_port = value; }
        }
        /// <summary>
        /// 目标IP
        /// </summary>
        public string DestinationAddress
        {
            get { return destination_address; }
            set { destination_address = value; }
        }
        /// <summary>
        /// 源IP
        /// </summary>
        public string OriginationAddress
        {
            get { return origination_address; }
            set { origination_address = value; }
        }
        /// <summary>
        /// IP版本
        /// </summary>
        public string IPVersion
        {
            get { return ip_version; }
            set { ip_version = value; }
        }
        /// <summary>
        /// 数据包长度
        /// </summary>
        public uint PacketLength
        {
            get { return total_packet_length; }
            set { total_packet_length = value; }
        }
        /// <summary>
        /// 消息长度
        /// </summary>
        public uint MessageLength
        {
            get { return message_length; }
            set { message_length = value; }
        }
        /// <summary>
        /// 头部长度
        /// </summary>
        public uint HeaderLength
        {
            get { return header_length; }
            set { header_length = value; }
        }/// <summary>
         /// 接收的缓存
         /// </summary>
        public byte[] ReceiveBuffer
        {
            get { return receive_buf_bytes; }
            set { receive_buf_bytes = value; }
        }
        /// <summary>
        /// IP头部缓存
        /// </summary>
        public byte[] IPHeaderBuffer
        {
            get { return ip_header_bytes; }
            set { ip_header_bytes = value; }
        }
        /// <summary>
        /// 消息缓存
        /// </summary>
        public byte[] MessageBuffer
        {
            get { return message_bytes; }
            set { message_bytes = value; }
        }
        private string protocol;
        private uint destination_port;
        private uint origination_port;
        private string destination_address;
        private string origination_address;
        private string ip_version;
        public uint total_packet_length;
        private uint message_length;
        private uint header_length;
        private byte[] receive_buf_bytes = null;
        private byte[] ip_header_bytes = null;
        private byte[] message_bytes = null;
    }

}
