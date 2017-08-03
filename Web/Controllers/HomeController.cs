using Model;
using RabbitMQHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index() {
            return Content("弄啥呢!");
        }

        #region "Fanout Exchange Test"

        public ActionResult Fanout()
        {
            return Fanout_Test_String();
        }
        /// <summary>
        /// Fanout 消息上报=>对象
        /// </summary>
        /// <returns></returns>
        private ActionResult Fanout_Test_Obj()
        {
            for (int i = 0; i < 10; i++)
            {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                var isSc = MQHelper.FanoutPush(new MSObject
                {
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
                var isSc = MQHelper.FanoutPush<string>($"我只是个Test_{time}_{i}", out msg);
                if (!isSc) return Content($"报错了:{msg}");
            }
            return Content("Bingo!");
        }
        #endregion

        #region "Direct Exchange Test"

        public ActionResult Direct()
        {
            return Direct_Test_String();
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
                MQHelper.DirectSend("direct_mq", $"我只是个Test_{time}_{i}");
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
                MQHelper.DirectSend<MSObject>("direct_mq", new MSObject
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
                MQHelper.DirectPush<MSObject>(new MSObject
                {
                    Name = $"我只是个Test_{time}_{i}",
                    Age = i,
                },out msg);
            }
            return Content("Bingo!");
        }

        #endregion

        #region "Topic Exchange Test"

        public ActionResult Topic() {

            return Topic_Sub_Test_String();
        }
        /// <summary>
        ///  Topic 消息上报=>string
        /// </summary>
        /// <returns></returns>
        public ActionResult Topic_Test_String() {
            for (int i = 0; i < 10; i++) {
                var time = DateTime.Now.ToString("HHmmss");
                var msg = string.Empty;
                var isSc = MQHelper.TopicPublish<string>("A.666", $"我只是个Test_{time}_{i}", out msg);
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
                var isSc = MQHelper.TopicPublish<MSObject>("A.666", new MSObject {
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
                var isSc = MQHelper.TopicSub<string>( $"我只是个Test_{time}_{i}", "A.666", out msg);
                if (!isSc) return Content($"报错了:{msg}");
            }
            return Content("Bingo!");
        }
        #endregion




    }
}