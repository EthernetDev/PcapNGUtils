///////////////////////////////////////////////
/// 文件
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthernetCapture
{
    public enum PacketType
    {
        IPv4 = 0x0800,
        ARP = 0x0806,
        PPPoE = 0x8864,
        X802_1Q = 0x8100,
        IPV6 = 0x86DD,
        MPLS = 0x8847
    }
}
