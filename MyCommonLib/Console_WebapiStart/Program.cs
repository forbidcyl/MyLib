using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        private static ConcurrentBag<string> ipAddress = new ConcurrentBag<string>();

        static void Main(string[] args)
        {
            try
            {
                UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 36172));

                Thread thread = new Thread(receiveUdpMsg);//用线程接收，避免UI卡住
                thread.IsBackground = true;
                thread.Start(client);

                //GetAvailablePrinters();

                //ipAddress.Add("localhost");
                //Start_SharedHost();
                //var ips = GetLocalIpAddress("InterNetwork");
                //foreach (var item in ips)
                //{
                //    Console.WriteLine(item.ToString());
                //}
                //Console.ReadKey();

                Console.WriteLine(Environment.CurrentDirectory+ @"\PrintTemplate");

                //var ss = CommonLib.AESHelper.Decrypt("geOZH9PFkBamDSYwC0fpaD2Sah2WFTP1Lx/j1v1y7d4MBtRea4eMcM8l6BH6djYRso1FnWdDpAMIzBl3leIxgSgpYxfCeBcuVEI5iNWZwPZS2R4EwlOvyNN44pzpYjtNx4euwHck4YpO34nBXcO0orBliTOc4ebIV7T5g7cOkKChvzeI9TQo7Wiehmh/ApSeFJi3/rFhV0HrKQy5UGizm66SrKOlATpV0OgX7tvT7ew=Tjubxx1GGQO5Jrxbjiz3ysRC2P2fgq81RlmmZV4Ytb+HxG/UypztNf6/G+a1zwQkKBHcJCjnvJFtUz59yGrsvWgg+SWNr3STFvxaVDltKw==");
                //Console.WriteLine(ss);
                //Console.WriteLine(System.IO.Directory.GetCurrentDirectory());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }


        /// <summary>
        /// 访问远程共享主机
        /// </summary>
        private static void Start_SharedHost()
        {
            Thread remoteHostAccessThread = new Thread(async () =>
            {
                while (true)
                {
                    foreach (var ip in ipAddress)
                    {
                        if (!string.IsNullOrEmpty(ip))
                        {
                            try
                            {
                                using (HttpClient client = new HttpClient())
                                {
                                    try
                                    {
                                        client.BaseAddress = new Uri($"http://{ip}:36172");
                                        client.Timeout = TimeSpan.FromSeconds(5);
                                        var response = await client.GetAsync("api/Print/GetPrinterShareds");

                                        if (!response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.OK)
                                            throw new Exception($"请求失败，错误：StatusCode:{response.StatusCode};Content:{await response.Content.ReadAsStringAsync()}");

                                        var responseContent = JsonConvert.DeserializeObject<PrintSharedResponseModel>(await response.Content.ReadAsStringAsync());

                                        if (responseContent != null)
                                            responseContent.SharedPrinters.ForEach(r => Console.WriteLine(r));
                                    }
                                    catch (HttpRequestException e)
                                    {
                                        // Console.WriteLine(e.)
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.GetType().Name);
                                        throw ex;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            await Task.Delay(1000);
                        }
                    }
                }
            });

            remoteHostAccessThread.IsBackground = true;
            remoteHostAccessThread.Start();
        }


        /// <summary>
        /// 接受UDP广播发送的消息
        /// </summary>
        /// <param name="obj"></param>
        static void receiveUdpMsg(object obj)
        {
            UdpClient client = obj as UdpClient;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            object states = new object();
            while (true)
            {
                client.BeginReceive(delegate (IAsyncResult result)
                {
                    try
                    {
                        var encryptString = result.AsyncState.ToString();
                        Console.WriteLine(encryptString);//委托接收消息
                        var decryptString = CommonLib.AESHelper.Decrypt(encryptString);
                        Console.WriteLine($"解密后信息：{decryptString}");

                        var test = JsonConvert.DeserializeObject<Test>(decryptString);
                        if (test != null)
                        {
                            Console.WriteLine(test.Action);
                        }
                        Console.WriteLine("IP地址：" + endpoint.Address.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }, Encoding.UTF8.GetString(client.Receive(ref endpoint)));
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

    public class Test
    {
        public string Action { get; set; }

        public DateTime Time { get; set; }
    }



    public class BasePrintSharedResponseModel
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }
    }

    /// <summary>
    /// 打印机响应模板类
    /// </summary>
    public class PrintSharedResponseModel : BasePrintSharedResponseModel
    {
        /// <summary>
        /// 本地可用的打印机
        /// </summary>
        public List<string> SharedPrinters { get; set; }
    }
}
