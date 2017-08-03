using Model;
using RabbitMQHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    class Program {
        static void Main(string[] args) {

            //Fanout Exchange 消息接收测试
            //Fanout_Test_String();
            //Fanout_Test_Obj

            //Direct Exchange 消息接收测试
            //Direct_Receive_Test_String();
            //Direct_Receive_Test_Obj();
            //Direct_Consume_Test_Obj();

            //Topic Exchange 消息接收测试
            //Topic_Test_String();
            //Topic_Test_Obj();
            Topic_Consume_String();
            Console.ReadLine();


        }

        #region "Fanout Exchange 消息消耗 Test"
        /// <summary>
        /// Fount Exchange Test object
        /// </summary>
        public static void Fanout_Test_String() {
            MQHelper.FanoutConsume<string>((mqStr) => {
                Console.WriteLine(mqStr);
            });
          
        }
        /// <summary>
        /// Fount Exchange Test string
        /// </summary>
        public static void Fanout_Test_Obj() {
            MQHelper.FanoutConsume<MSObject>((mqObj) => {
                Console.WriteLine($"Name={mqObj.Name},Age={mqObj.Age},AddTime={mqObj.AddTime.ToString()}");
            });
          
        }
        #endregion

        #region "Direct Exchange 消息消耗 Test"
        /// <summary>
        /// Direct Exchange Test string
        /// </summary>
        public static void Direct_Receive_Test_String() {
            var msg = string.Empty;
            var isSc = MQHelper.DirectReceive<string>("direct_mq", (str) => {
                Console.WriteLine(str);
            }, out msg);
            if (!string.IsNullOrWhiteSpace(msg)) Console.WriteLine(msg);
          
        }

        /// <summary>
        /// Direct Exchange Test object
        /// </summary>
        public static void Direct_Receive_Test_Obj() {
            var msg = string.Empty;
            var isSc = MQHelper.DirectReceive<MSObject>("direct_mq", (mqObj) => {
                Console.WriteLine($"Name={mqObj.Name},Age={mqObj.Age},AddTime={mqObj.AddTime.ToString()}");
            }, out msg);
            if (!string.IsNullOrWhiteSpace(msg)) Console.WriteLine(msg);
          
        }

        /// <summary>
        /// Direct Exchange Test object
        /// </summary>
        public static void Direct_Consume_Test_Obj() {
            var msg = string.Empty;
            MQHelper.DirectConsume<MSObject>((mqObj) => {
                Console.WriteLine($"Name={mqObj.Name},Age={mqObj.Age},AddTime={mqObj.AddTime.ToString()}");
            }, out msg);
            if (!string.IsNullOrWhiteSpace(msg)) Console.WriteLine(msg);
          
        }

        #endregion

        #region "Topic Exchange 消息消耗 Test"
        public static void Topic_Test_String() {
            MQHelper.TopicSubscribe<string>("MyTopic", (info) => {
                Console.WriteLine(info);
            }, "A.*", "B.*", "C.*");
          
        }

        public static void Topic_Test_Obj() {
            MQHelper.TopicSubscribe<MSObject>("MyTopic", (mqObj) => {
                Console.WriteLine($"Name={mqObj.Name},Age={mqObj.Age},AddTime={mqObj.AddTime.ToString()}");
            }, "A.*", "B.*", "C.*");
          
        }

        public static void Topic_Consume_String() {
            MQHelper.TopicConsume<string>((mqStr) => {
                Console.WriteLine(mqStr);
            }, topics: new string[] { "A.*", "B.*", "X.*" });
        }
        #endregion

    }
}
