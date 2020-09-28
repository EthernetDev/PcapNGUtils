//////////////////////////////////////////////////
/// 文件：Setting.cs
/// 说明：参数类
/// 作者：李光强
/// 时间：2020/9/29/
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthernetCapture
{
    /// <summary>
    /// 参数类
    /// </summary>
    public class Setting
    {
        #region 类实例
        /// <summary>
        /// 类实例
        /// </summary>
        private static Setting instance;

        /// <summary>
        /// 类实例
        /// </summary>
        public static Setting Instance
        {
            get {
                if (instance == null)
                    instance = new Setting();

                return instance;
            }
        }
        #endregion

        #region 私有字段

        /// <summary>
        /// 线程数量
        /// </summary>
        private int threadCount;
        /// <summary>
        /// 监听的网络协议
        /// </summary>
        private ProtocalType protocal;
        /// <summary>
        /// 监听端口
        /// </summary>
        private int capturedPort;
        /// <summary>
        /// 监听目标IP
        /// </summary>
        private string capturedIp;
        /// <summary>
        /// 数据记录格式
        /// </summary>
        private LogType logType;

        /// <summary>
        /// 监听端口
        /// </summary>ary>
        /// 数据记录文件夹
        /// </summary>
        private string logFolder;
        /// <summary>
        /// 是否按每小时命名记录
        /// </summary>
        private bool nameByHour;

        /// <summary>
        /// 数据文件的命名格式，格式如：AAA-{yyyy-MM-dd_HH}.xxx，AAA-为前缀（可选），{...}为日期格式，xxx为后缀名
        /// </summary>
        private string fileFormat;

        #endregion

        #region 公共属性

        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount { get => threadCount; set => threadCount = value; }

        /// <summary>
        /// 监听的网络协议
        /// </summary>
        public ProtocalType Protocal { 
        get=>protocal;set => this.protocal = value;
        }

        /// <summary>
        /// 监听端口
        /// </summary>
        public int CapturedPort { get => capturedPort; set => capturedPort = value; }

        /// <summary>
        /// 监听目标IP
        /// </summary>
        public string CapturedIp { get => capturedIp; set => capturedIp = value; }

        /// <summary>
        /// 数据记录格式
        /// </summary>
        public LogType LogType { get => logType; set => logType = value; }
                
        /// </summary>
        /// 数据记录文件夹
        /// </summary>
        public string LogFolder { get => logFolder; set => logFolder = value; }

        /// <summary>
        /// 是否按每小时命名记录
        /// </summary>
        public bool NameByHour { get => nameByHour; set => nameByHour = value; }

        /// <summary>
        /// 数据文件的命名格式，格式如：AAA-{yyyy-MM-dd_HH}.xxx，AAA-为前缀（可选），{...}为日期格式，xxx为后缀名
        /// </summary>
        public string FileFormat { get => fileFormat; set => fileFormat = value; }

        #endregion

        #region 公共方法
        /// <summary>
        /// 读取配置参数
        /// </summary>
        public void ReadConfig()
        {
            try
            {
                var nvc =
                    ConfigurationManager.GetSection("EthernetCaptureSettings") as NameValueCollection;
                if (nvc == null)
                {
                    throw new Exception("配置文件有错误，没有网络监听配置信息【EthernetCaptureSettings】");
                }
                string val = nvc["ThreadCount"];
                int n = 1;
                if (!Int32.TryParse(val, out n))
                    n = 1;
                this.threadCount = n;

                val = nvc["Protocal"];
                this.protocal= (ProtocalType)Enum.Parse(typeof(ProtocalType), val);

                val = nvc["CapturedPort"];
                if (!Int32.TryParse(val, out n))
                    n = 0;
                this.CapturedPort = n;

                this.CapturedIp = nvc["CapturedIp"];

                val = nvc["LogType"];
                if (!Int32.TryParse(val, out n))
                    n = 0;
                this.LogType =(LogType)n;

                this.logFolder = nvc["LogFolder"];
                this.fileFormat = nvc["FileFormat"];

                val = nvc["LogType"];
                if (!Int32.TryParse(val, out n))
                    n = 0;
                this.LogType = (LogType)n;

                this.nameByHour = (nvc["NameByHour"] == "1");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
#endregion


    }
}
