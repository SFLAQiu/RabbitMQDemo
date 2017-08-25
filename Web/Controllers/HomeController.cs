using Model;
using RabbitMQHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller {

        public ActionResult Index() {
            return Content("弄啥呢!");
        }

        #region "Fanout Exchange Test"
        public ActionResult Fanout(string handle = "string") {
            switch (handle) {
                case "string": return Fanout_Test_String();
                case "obj": return Fanout_Test_Obj();
                default: return Content($"what?");
            }
        }
        /// <summary>
        /// Fanout 消息上报=>对象
        /// </summary>
        /// <returns></returns>
        private ActionResult Fanout_Test_Obj() {
            for (int i = 0; i < 10; i++) {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                var isSc = MQHelperFactory.Default().FanoutPush(new MSObject {
                    Name = $"我只是个Test_{time}_{i}",
                    Age = i,
                }, out msg);
                if (!isSc) return Content($"报错了:{msg}");
            }
            return Content("Bingo!");
        }

        /// <summary>
        /// Fanout 消息上报=>字符串
        /// </summary>
        /// <returns></returns>
        private ActionResult Fanout_Test_String()
        {
            for (int i = 0; i < 10; i++)
            {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                var isSc = MQHelperFactory.Default().FanoutPush<string>($"我只是个Test_{time}_{i}", out msg);
                if (!isSc) return Content($"报错了:{msg}");
            }
            return Content("Bingo!");
        }
        #endregion

        #region "Direct Exchange Test"

        public ActionResult Direct(string handle="string") {
            switch (handle) {
                case "string": return Direct_Test_String();
                case "obj": return Direct_Test_Obj();
                case "push-obj": return Direct_Push_Test_Obj();
                default: return Content($"what?");
            }
        }

        /// <summary>
        /// Direct 消息上报=>字符串
        /// </summary>
        /// <returns></returns>
        public ActionResult Direct_Test_String()
        {
            for (int i = 0; i < 10; i++)
            {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                MQHelperFactory.Default().DirectSend("direct_mq", $"我只是个Test_{time}_{i}");
            }
            return Content("Bingo!");
        }

        /// <summary>
        /// Direct 消息上报=>Object
        /// </summary>
        /// <returns></returns>
        public ActionResult Direct_Test_Obj()
        {
            for (int i = 0; i < 10; i++)
            {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                MQHelperFactory.Default().DirectSend<MSObject>("direct_mq", new MSObject
                {
                    Name = $"我只是个Test_{time}_{i}",
                    Age = i,
                });
            }
            return Content("Bingo!");
        }
        /// <summary>
        /// Direct 消息上报=>Object
        /// </summary>
        /// <returns></returns>
        public ActionResult Direct_Push_Test_Obj()
        {
            for (int i = 0; i < 10; i++)
            {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                MQHelperFactory.Default().DirectPush<MSObject>(new MSObject
                {
                    Name = $"我只是个Test_{time}_{i}",
                    Age = i,
                },out msg);
            }
            return Content("Bingo!");
        }

        #endregion

        #region "Topic Exchange Test"

        public ActionResult Topic(string handle="string") {
            switch (handle) {
                case "string": return Topic_Test_String();
                case "obj": return Topic_Test_Obj();
                case "sub-string": return Topic_Sub_Test_String();
                default: return Content($"what?");
            }
        }
        /// <summary>
        ///  Topic 消息上报=>string
        /// </summary>
        /// <returns></returns>
        public ActionResult Topic_Test_String() {
            for (int i = 0; i < 10; i++) {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                var isSc = MQHelperFactory.Default().TopicPublish<string>("A.666", $"我只是个Test_{time}_{i}", out msg);
                if (!isSc) return Content($"报错了:{msg}");
            }
            return Content("Bingo!");
        }
        /// <summary>
        /// Topic 消息上报=>Object
        /// </summary>
        /// <returns></returns>
        public ActionResult Topic_Test_Obj() {
            for (int i = 0; i < 10; i++) {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                var isSc = MQHelperFactory.Default().TopicPublish<MSObject>("A.666", new MSObject {
                    Name = $"我只是个Test_{time}_{i}",
                    Age = i,
                }, out msg);
                if (!isSc) return Content($"报错了:{msg}");
            }
            return Content("Bingo!");
        }


        /// <summary>
        ///  Topic 消息上报=>string
        /// </summary>
        /// <returns></returns>
        public ActionResult Topic_Sub_Test_String() {
            for (int i = 0; i < 10; i++) {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                var isSc = MQHelperFactory.Default().TopicSub<string>( $"我只是个Test_{time}_{i}", "A.666", out msg);
                if (!isSc) return Content($"报错了:{msg}");
            }
            return Content("Bingo!");
        }
        #endregion

    }
}