///////////////////////////////////////////////////
/// 文件：LogType.cs
/// 说明：数据记录类型
/// 作者：李光强
/// 时间：2020/9/29/
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthernetCapture
{
    /// <summary>
    /// 数据记录类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 不记录
        /// </summary>
        NONE=0,
        /// <summary>
        /// PCAP文件格式
        /// </summary>
        PCAP_FILE=1,
        /// <summary>
        /// 十六进制文本格式
        /// </summary>
        ASCII_FILE=2,
        /// <summary>
        /// 二进制文件格式
        /// </summary>
        BIN_FILE=3
    }
}
