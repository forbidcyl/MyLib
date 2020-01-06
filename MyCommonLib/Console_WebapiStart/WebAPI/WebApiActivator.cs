////////////////////////////////////////////////////////////////////
//                          _ooOoo_                               //
//                         o8888888o                              //
//                         88" . "88                              //
//                         (| ^_^ |)                              //
//                         O\  =  /O                              //
//                      ____/`---'\____                           //
//                    .'  \\|     |//  `.                         //
//                   /  \\|||  :  |||//  \                        //
//                  /  _||||| -:- |||||-  \                       //
//                  |   | \\\  -  /// |   |                       //
//                  | \_|  ''\---/''  |   |                       //
//                  \  .-\__  `-`  ___/-. /                       //
//                ___`. .'  /--.--\  `. . ___                     //
//              ."" '<  `.___\_<|>_/___.'  >'"".                  //
//            | | :  `- \`.;`\ _ /`;.`/ - ` : | |                 //
//            \  \ `-.   \_ __\ /__ _/   .-` /  /                 //
//      ========`-.____`-.___\_____/___.-`____.-'========         //
//                           `=---='                              //
//      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //
//             佛祖保佑       永不宕机     永无BUG          	  //
////////////////////////////////////////////////////////////////////


/*
*┌────────────────────────────────────────────────┐
*│　描    述：WebApiActivator                  |                                  
*│　作    者：cyl                                 |            
*│　版    本：1.0                                 |             
*│　创建时间：2020/1/6 17:35:54                        	  |
*└────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

namespace Console_WebapiStart.WebAPI
{

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
}
