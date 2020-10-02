////////////////////////////////////////////////
/// 文件：MyArp.cs
/// 说明：Arp数据
/// 作者：李光强
/// 时间：2020/10/1/
/// 备注：建立IP与MAC对应
////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthernetCapture
{
    /// <summary>
    /// MAC与IP对应类
    /// </summary>
    public class MacIp
    {
        public string IP;
        public string MAC;

        public MacIp()
        { }

        public MacIp(string mac, string ip)
        {
            this.MAC = mac;
            this.IP = ip;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MyArp
    {
        private static List<MacIp> list = new List<MacIp>();

        /// <summary>
        /// 根据IP寻找对应Mac
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static MacIp FindArpByIP(string ip)
        {
            try
            {
                MacIp arp = null;
                lock (list)
                {
                    foreach (MacIp mi in list)
                    {
                        arp = mi;
                        break;
                    }
                }

                if (arp == null)                   
                {
                    string mac=NetUtils.GetMacAddress(ip);
                    arp = new MacIp(mac, ip);
                    list.Add(arp);
                    return arp;
                }

                return arp;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
