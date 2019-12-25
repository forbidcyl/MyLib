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
*│　描    述：AESHelper                  |                                  
*│　作    者：cyl                                 |            
*│　版    本：1.0                                 |             
*│　创建时间：2019/12/25 16:14:18                        	  |
*└────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    /// <summary>
    /// AES加密 此算法支持的密钥长度为128、192或256位;默认为256位
    /// </summary>
    public class AESHelper
    {
        /// <summary>
        /// 凭据加密钥匙 8 * 16位
        /// </summary>
        private static readonly byte[] aes_Key = Encoding.UTF8.GetBytes("zz%MJ8$jEn10*uMu");

        /// <summary>
        /// 凭据加密向量 8 * 16位
        /// </summary>
        private static readonly byte[] aes_IV = Encoding.UTF8.GetBytes("@xF0YvsLGKABxlIB");

        /// <summary>
        /// AES 加密 输出加密后的base64字符串
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);


            RijndaelManaged rm = new RijndaelManaged
            {
                Key = aes_Key,
                IV = aes_IV,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="decryptedStr"></param>
        /// <returns></returns>
        public static string Decrypt(string decryptedStr)
        {
            if (string.IsNullOrWhiteSpace(decryptedStr))
                return null;

            try
            {
                byte[] toDecryptArray = Convert.FromBase64String(decryptedStr);
                RijndaelManaged rDel = new RijndaelManaged
                {
                    Key = aes_Key,
                    IV = aes_IV,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return null;
            }
        }



    }
}
