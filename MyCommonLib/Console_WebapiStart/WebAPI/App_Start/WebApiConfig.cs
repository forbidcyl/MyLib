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
*│　描    述：WebApiConfig                  |                                  
*│　作    者：cyl                                 |            
*│　版    本：1.0                                 |             
*│　创建时间：2019/12/24 16:55:34                        	  |
*└────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Console_WebapiStart.App_Start
{
    public static class WebApiConfig
    {
        public static System.Web.Http.SelfHost.HttpSelfHostConfiguration Register(System.Web.Http.SelfHost.HttpSelfHostConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "DefaultApi",
                                      routeTemplate: "api/{controller}/{action}/{id}",
                                      defaults: new { id = RouteParameter.Optional });

            return config;
        }
    }
}
