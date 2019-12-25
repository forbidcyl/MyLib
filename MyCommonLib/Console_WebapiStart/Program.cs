using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Console_WebapiStart
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 36172));

                //Thread thread = new Thread(receiveUdpMsg);//用线程接收，避免UI卡住
                //thread.IsBackground = true;
                //thread.Start(client);

                GetAvailablePrinters();


                //var ips = GetLocalIpAddress("InterNetwork");
                //foreach (var item in ips)
                //{
                //    Console.WriteLine(item.ToString());
                //}
                //Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        /// <summary>
        /// 接受UDP广播发送的消息
        /// </summary>
        /// <param name="obj"></param>
        static void receiveUdpMsg(object obj)
        {
            UdpClient client = obj as UdpClient;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                client.BeginReceive(delegate (IAsyncResult result)
                {
                    try
                    {
                        var encryptString = result.AsyncState.ToString();
                        Console.WriteLine(encryptString);//委托接收消息
                        Console.WriteLine($"解密后信息：{CommonLib.AESHelper.Decrypt(encryptString)}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }, Encoding.UTF8.GetString(client.Receive(ref endpoint)));

                Console.WriteLine("IP地址：" + endpoint.Address.ToString());
            }
        }

        /// <summary>
        /// 获取本地可用的打印机
        /// </summary>
        static void GetAvailablePrinters()
        {
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            foreach (var item in printers)
            {
                Console.WriteLine(item.ToString());
            }
        }

        /// <summary>
        /// webapi
        /// </summary>
        public class WebApiActivator
        {
            private static HttpSelfHostServer _webapi_server;
            private static bool _is_serverStarted = false;

            /// <summary>
            /// webapi服务是否开启
            /// </summary>
            public static bool IsServerStarted { get { return IsServerStarted; } private set { } }

            /// <summary>
            /// WebApi服务开启
            /// </summary>
            /// <returns></returns>
            public static async Task ServerOpenAsync()
            {
                if (_is_serverStarted)
                    return;

                _webapi_server =
                    new HttpSelfHostServer(Console_WebapiStart.App_Start.WebApiConfig.Register(new HttpSelfHostConfiguration("http://localhost:1234")));

                await _webapi_server.OpenAsync();

                _is_serverStarted = !_is_serverStarted;
            }

            /// <summary>
            /// WebApi服务关闭
            /// </summary>
            /// <returns></returns>
            public static async Task ServerCloseAsync()
            {
                if (!_is_serverStarted)
                    return;

                await _webapi_server.CloseAsync();

                _is_serverStarted = !_is_serverStarted;

                _webapi_server.Dispose();
            }
        }

        /// <summary>
        /// 获取本机所有ip地址
        /// </summary>
        /// <param name="netType">"InterNetwork":ipv4地址，"InterNetworkV6":ipv6地址</param>
        /// <returns>ip地址集合</returns>
        public static List<string> GetLocalIpAddress(string netType)
        {
            string hostName = Dns.GetHostName();                    //获取主机名称  
            IPAddress[] addresses = Dns.GetHostAddresses(hostName); //解析主机IP地址  

            List<string> IPList = new List<string>();
            if (netType == string.Empty)
            {
                for (int i = 0; i < addresses.Length; i++)
                {
                    IPList.Add(addresses[i].ToString());
                }
            }
            else
            {
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                for (int i = 0; i < addresses.Length; i++)
                {
                    if (addresses[i].AddressFamily.ToString() == netType)
                    {
                        IPList.Add(addresses[i].ToString());
                    }
                }
            }
            return IPList;
        }
    }
}
