
///////////////////////////////////////
/// 文件：NetUtils.cs
/// 说明：网络工具
/// 作者：李光强
/// 时间：2020/10/1
///////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EthernetCapture
{
    /// <summary>
    /// 网络工具
    /// </summary>
    public class NetUtils
    {
        [DllImport("IPHLPAPI.dll")]
        private static extern int SendARP(Int32 DestIP, Int32 ScrIP, ref Int64 pMacAddr, ref Int32 PhyAddrLen);


        [DllImport("ws2_32.dll")]
        private static extern int inet_addr(string cp);

        //ref和out在C#中既可以通过值引用传递参数。通过引用参数传递参数允许函数成员更改参数的值
        //并保持该更改。若要通过引用传递参数，可以使用ref或者out关键字。ref和out这两个关键字都
        //能够提供相似的功效，其作用很像C中的指针变量
        //使用ref型参数时，传入的参数必须先被初始化。对out而言，必须在方法中对其完成初始化。
        //使用ref和out时，在方法的参数和执行方法时，都要加Ref或Out关键字。以满足匹配。
        //out适合用在需要retrun多个返回值的地方，而ref则用在需要被调用的方法修改调用者的引用的时候。

        /// <summary>
        /// 获取远程IP（不能跨网段）的MAC地址
        /// </summary>
        /// <param name="hostip"></param>
        /// <returns></returns>
        public static string GetMacAddress(string hostip)
        {
            string Mac = "";
            try
            {
                Int32 ldest = inet_addr(hostip); //将IP地址从 点数格式转换成无符号长整型
                Int64 macinfo = new Int64();
                Int32 len = 6;
                //SendARP函数发送一个地址解析协议(ARP)请求获得指定的目的地IPv4地址相对应的物理地址
                SendARP(ldest, 0, ref macinfo, ref len);
                string TmpMac = Convert.ToString(macinfo, 16).PadLeft(12, '0');//转换成16进制　　注意有些没有十二位
                Mac = TmpMac.Substring(0, 2).ToUpper();//
                for (int i = 2; i < TmpMac.Length; i = i + 2)
                {
                    Mac = TmpMac.Substring(i, 2).ToUpper() + "-" + Mac;
                }
            }
            catch (Exception Mye)
            {
                throw Mye;
            }
            return Mac;
        }

        /// <summary>
        /// 生成链路层数据包
        /// </summary>
        /// <param name="srcIp"></param>
        /// <param name="dstIp"></param>
        /// <param name="type"></param>
        public static byte[] GeneratLinkData(string srcIp,string dstIp,PacketType type,byte[] data)
        {
            try
            {
                MacIp dstMacIp = MyArp.FindArpByIP(dstIp);
                MacIp srcMacIp = MyArp.FindArpByIP(srcIp);

                byte[] packet = new byte[data.Length + 14];
                Array.Copy(Encoding.ASCII.GetBytes(srcMacIp.MAC), 0, packet, 0, 6);
                Array.Copy(Encoding.ASCII.GetBytes(dstMacIp.MAC), 0, packet, 6, 6);
                Array.Copy(BitConverter.GetBytes(Convert.ToInt32(type)), 0, packet, 12, 2);
                Array.Copy(data, 0, packet, 14, data.Length);
                return packet;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
