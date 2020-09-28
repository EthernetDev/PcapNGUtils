///////////////////////////////////////////////////////////
/// 文件：PacketStuct.cs
/// 说明：包数据结构定义
/// 作者：李光强
/// 时间：2020/9/29/
/// ///////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EthernetCapture
{
    /// <summary>
    /// IP头部
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct IPHeader
    {
        /// <summary>
        /// I4位首部长度+4位IP版本号 
        /// </summary>
        [FieldOffset(0)] 
        public byte ip_verlen; 

        /// <summary>
        /// 8位服务类型TOS
        /// </summary>
        [FieldOffset(1)] 
        public byte ip_tos; 

        /// <summary>
        /// 16位数据包总长度（字节）
        /// </summary>
        [FieldOffset(2)] 
        public ushort ip_totallength; 

        /// <summary>
        /// 16位标识 
        /// </summary>
        [FieldOffset(4)] 
        public ushort ip_id; 

        /// <summary>
        /// 3位标志位
        /// </summary>
        [FieldOffset(6)] 
        public ushort ip_offset; 

        /// <summary>
        /// 8位生存时间 TTL 
        /// </summary>
        [FieldOffset(8)] 
        public byte ip_ttl; 

        /// <summary>
        /// 8位协议(TCP, UDP, ICMP, Etc.)
        /// </summary>
        [FieldOffset(9)] 
        public byte ip_protocol; 

        /// <summary>
        /// 16位IP首部校验和 
        /// </summary>
        [FieldOffset(10)] 
        public ushort ip_checksum; 

        /// <summary>
        /// 32位源IP地址 
        /// </summary>
        [FieldOffset(12)] 
        public uint ip_srcaddr; 

        /// <summary>
        /// 32位目的IP地址
        /// </summary>
        [FieldOffset(16)] 
        public uint ip_destaddr; 
    }

    /// <summary>
    /// IP头部
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct IPHeader2
    {
        /// <summary>
        /// I4位首部长度+4位IP版本号
        /// </summary>
        [FieldOffset(0)] 
        public byte ip_verlen; 

        /// <summary>
        /// 8位服务类型TOS 
        /// </summary>
        [FieldOffset(1)] 
        public byte ip_tos; 

        /// <summary>
        /// 16位数据包总长度（字节）
        /// </summary>
        [FieldOffset(2)] 
        public ushort ip_totallength; 

        /// <summary>
        /// 16位标识 
        /// </summary>
        [FieldOffset(4)]
        public ushort ip_id; 

        /// <summary>
        /// 3位标志位
        /// </summary>
        [FieldOffset(6)]
        public ushort ip_offset; 

        /// <summary>
        /// 8位生存时间 TTL
        /// </summary>
        [FieldOffset(8)] 
        public byte ip_ttl; 

        /// <summary>
        /// 8位协议(TCP, UDP, ICMP, Etc.) 
        /// </summary>
        [FieldOffset(9)]
        public byte ip_protocol; 

        /// <summary>
        /// 16位IP首部校验和 
        /// </summary>
        [FieldOffset(10)]
        public ushort ip_checksum; 

        /// <summary>
        /// 32位源IP地址 
        /// </summary>
        [FieldOffset(12)] 
        public uint ip_srcaddr; 

        /// <summary>
        /// 32位目的IP地址
        /// </summary>
        [FieldOffset(16)]
        public uint ip_destaddr; 

        /// <summary>
        /// 16位源端口
        /// </summary>
        [FieldOffset(20)]
        public ushort sport;

        /// <summary>
        /// 16位目的端口  
        /// </summary>
        [FieldOffset(22)] 
        public ushort dport;
    }

    /// <summary>
    /// TCP伪首部
    /// </summary>
    public struct tsd_hdr
    {
        /// <summary>
        /// 源地址 
        /// </summary>
        public ulong saddr;

        /// <summary>
        /// 目的地址 
        /// </summary>
        public ulong daddr;

        /// <summary>
        /// 
        /// </summary>
        public char mbz;

        /// <summary>
        /// 协议类型 
        /// </summary>
        public char ptcl;

        /// <summary>
        /// TCP长度  
        /// </summary>
        public ushort tcpl;
    }

    /// <summary>
    /// TCP首部 
    /// </summary>
    unsafe public struct tcp_hdr
    {
        /// <summary>
        /// 16位源端口
        /// </summary>
        public ushort sport;

        /// <summary>
        /// 16位目的端口
        /// </summary>
        public ushort dport;

        /// <summary>
        /// 32位序列号
        /// </summary>
        public uint seq;

        /// <summary>
        /// 32位确认号 
        /// </summary>
        public uint ack;

        /// <summary>
        /// 4位首部长度/6位保留字
        /// </summary>
        public char* lenres;

        /// <summary>
        /// 6位标志位
        /// </summary>
        public char* flag;

        /// <summary>
        /// 16位窗口大小
        /// </summary>
        public ushort win;

        /// <summary>
        /// 16位检验和 
        /// </summary>
        public ushort sum;

        /// <summary>
        /// 16位紧急数据偏移量
        /// </summary>
        public ushort urp;
    }

    /// <summary>
    /// UDP首部
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct udp_hdr
    {
        /// <summary>
        /// 16位源端口 
        /// </summary>
        [FieldOffset(0)] 
        public ushort sport;

        /// <summary>
        /// 16位目的端口
        /// </summary>
        [FieldOffset(2)] 
        public ushort dport;

        /// <summary>
        /// UDP 长度 
        /// </summary>
        [FieldOffset(4)] 
        public ushort len;

        /// <summary>
        /// 检查和  
        /// </summary>
        [FieldOffset(6)] 
        public ushort cksum;
    }

    /// <summary>
    /// ICMP首部
    /// </summary>
    public struct icmp_hdr
    {
        /// <summary>
        /// 发送端口
        /// </summary>
        public ushort sport;
        /// <summary>
        /// 目标端口
        /// </summary>
        public ushort dport;
        /// <summary>
        /// 协议类型
        /// </summary>
        public char type;
        /// <summary>
        /// 代码
        /// </summary>
        public char code;
        /// <summary>
        /// 校验和
        /// </summary>
        public ushort cksum;
        /// <summary>
        /// 目标IP
        /// </summary>
        public ushort id;
        /// <summary>
        /// 序号
        /// </summary>
        public ushort seq;
        /// <summary>
        /// 时间戳 
        /// </summary>
        public ulong timestamp;
    }

    /// <summary>
    /// 工作模式
    /// </summary>
    public enum RunnableType
    {
        /// <summary>
        /// 设置为窃听模式
        /// </summary>
        Listen,
        /// <summary>
        /// 设置为通信模式
        /// 需要指定网络终结点
        /// </summary>
        Connet,
        /// <summary>
        /// 指定端口的窃听模式
        /// </summary>
        PortListen
    }

    /// <summary>
    /// 协议类型
    /// </summary>
    public enum ProtocalType
    { 
        TCP=0,
        UDP=1,
        ICMP=2,
        IGMP=3
    }
}
