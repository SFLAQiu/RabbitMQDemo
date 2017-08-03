using EasyNetQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQHelper {
    /// <summary>
    /// 消息服务器连接器
    /// </summary>
    public class BusBuilder {
        public static IBus CreateMessageBus() {
            // 消息服务器连接字符串
            // var connectionString = ConfigurationManager.ConnectionStrings["RabbitMQ"];
            string connString = "host=127.0.0.1:5672;virtualHost=TestQueue;username=sa;password=123456";
            if (connString == null || connString == string.Empty) throw new Exception("messageserver connection string is missing or empty");
            return RabbitHutch.CreateBus(connString);
        }
    }
}
