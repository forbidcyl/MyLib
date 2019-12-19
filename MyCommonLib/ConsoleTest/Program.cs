using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GenericMethod
    {
        /// <summary>
        /// 2.0推出的新语法
        /// 泛型方法解决用一个方法，满足不同参数类型，做同样的事情
        /// 
        /// 泛型：延迟声明（也有类型推断）：把参数类型的声明推迟到调用
        /// 泛型并不是一个语法糖，二十有框架升级提供的功能
        /// 需要编译器+JIT的支持
        /// 
        /// 泛型接口、泛型类、泛型方法、泛型事件和泛型委托
        /// 泛型约束：struct\class\unmanaged\new()\<基类名>\<接口名称>
        /// 泛型：协变、逆变
        /// 泛型缓存
        /// 作用：最大限度的重用代码，保护类型的安全和提高性能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tParameters"></param>
        public static T Show<T>(T tParameters)
        {
            Console.WriteLine($"This is {typeof(T)}");
            return default(T);
        }


        /// <summary>
        /// 泛型类：
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class TClass<T>
        {

        }

        /// <summary>
        /// 泛型接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface TInterface<T>
        {

        }

        public delegate void CodeTypeDelegate<T>();

    }
}
