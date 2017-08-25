using EasyNetQ;
using EasyNetQ.FluentConfiguration;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQHelper {
    public class MQHelper: BusBuilder {
        public MQHelper(string host) : base(host) {
        }

        #region "通用"
        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="adbus"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        private  IQueue CreateQueue(IAdvancedBus adbus, string queueName = "") {
            if (adbus == null) return null;
            if (string.IsNullOrEmpty(queueName)) return adbus.QueueDeclare();
            return adbus.QueueDeclare(queueName);
        }
        #endregion

        #region "fanout"

        /// <summary>
        ///  消息消耗（fanout）
        /// </summary>`
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="handler">回调</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routingKey">路由名</param>
        public  void FanoutConsume<T>(Action<T> handler, string exChangeName = "fanout_mq", string queueName = "fanout_queue_default", string routingKey = "") where T : class {
            var bus = CreateMessageBus();
            var adbus = bus.Advanced;
            var exchange = adbus.ExchangeDeclare(exChangeName, ExchangeType.Fanout);
            var queue = CreateQueue(adbus, queueName);
            adbus.Bind(exchange, queue, routingKey);
            adbus.Consume(queue, registration => {
                registration.Add<T>((message, info) => {
                    handler(message.Body);
                });
            });
        }
        /// <summary>
        /// 消息上报（fanout）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="topic">主题名</param>
        /// <param name="t">消息命名</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public  bool FanoutPush<T>(T t, out string msg, string exChangeName = "fanout_mq", string routingKey = "") where T : class {
            msg = string.Empty;
            try {
                using (var bus = CreateMessageBus()) {
                    var adbus = bus.Advanced;
                    var exchange = adbus.ExchangeDeclare(exChangeName, ExchangeType.Fanout);
                    adbus.Publish(exchange, routingKey, false, new Message<T>(t));
                    return true;
                }
            } catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
        }
        #endregion

        #region "direct"
        /// <summary>
        /// 消息发送（direct）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="queue">发送到的队列</param>
        /// <param name="message">发送内容</param>
        public  void DirectSend<T>(string queue, T message) where T : class {
            using (var bus = CreateMessageBus()) {
                bus.Send(queue, message);
            }
        }
        /// <summary>
        /// 消息接收（direct）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="queue">接收的队列</param>
        /// <param name="callback">回调操作</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public  bool DirectReceive<T>(string queue, Action<T> callback, out string msg) where T : class {
            msg = string.Empty;
            try {
                var bus = CreateMessageBus();
                bus.Receive<T>(queue, callback);
            } catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 消息发送
        /// <![CDATA[（direct EasyNetQ高级API）]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        /// <param name="exChangeName"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public  bool DirectPush<T>(T t, out string msg, string exChangeName = "direct_mq", string routingKey = "direct_rout_default") where T : class {
            msg = string.Empty;
            try {
                using (var bus = CreateMessageBus()) {
                    var adbus = bus.Advanced;
                    var exchange = adbus.ExchangeDeclare(exChangeName, ExchangeType.Direct);
                    adbus.Publish(exchange, routingKey, false, new Message<T>(t));
                    return true;
                }
            } catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 消息接收
        ///  <![CDATA[（direct EasyNetQ高级API）]]>
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="handler">回调</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routingKey">路由名</param>
        public  bool DirectConsume<T>(Action<T> handler, out string msg, string exChangeName = "direct_mq", string queueName = "direct_queue_default", string routingKey = "direct_rout_default") where T : class {
            msg = string.Empty;
            try {
                var bus = CreateMessageBus();
                var adbus = bus.Advanced;
                var exchange = adbus.ExchangeDeclare(exChangeName, ExchangeType.Direct);
                var queue = CreateQueue(adbus, queueName);
                adbus.Bind(exchange, queue, routingKey);
                adbus.Consume(queue, registration => {
                    registration.Add<T>((message, info) => {
                        handler(message.Body);
                    });
                });
            } catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
            return true;
        }
        #endregion

        #region "topic"

        /// <summary>
        /// 获取主题 
        /// </summary>
        /// <typeparam name="T">主题内容类型</typeparam>
        /// <param name="subscriptionId">订阅者ID</param>
        /// <param name="callback">消息接收响应回调</param>
        ///  <param name="topics">订阅主题集合</param>
        public  void TopicSubscribe<T>(string subscriptionId, Action<T> callback, params string[] topics) where T : class {
            var bus = CreateMessageBus();
            bus.Subscribe(subscriptionId, callback, (config) => {
                foreach (var item in topics) config.WithTopic(item);
            });
        }
        /// <summary>
        /// 发布主题
        /// </summary>
        /// <typeparam name="T">主题内容类型</typeparam>
        /// <param name="topic">主题名称</param>
        /// <param name="message">主题内容</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        public  bool TopicPublish<T>(string topic, T message, out string msg) where T : class {
            msg = string.Empty;
            try {
                using (var bus = CreateMessageBus()) {
                    bus.Publish(message, topic);
                    return true;
                }
            } catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 发布主题
        /// </summary>
        /// <![CDATA[（topic EasyNetQ高级API）]]>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="t">消息内容</param>
        /// <param name="topic">主题名</param>
        /// <param name="msg">错误信息</param>
        /// <param name="exChangeName">交换器名</param>
        /// <returns></returns>
        public  bool TopicSub<T>(T t, string topic, out string msg, string exChangeName = "topic_mq") where T : class {
            msg = string.Empty;
            try {
                if (string.IsNullOrWhiteSpace(topic)) throw new Exception("推送主题不能为空");
                using (var bus = CreateMessageBus()) {
                    var adbus = bus.Advanced;
                    //var queue = adbus.QueueDeclare("user.notice.zhangsan");
                    var exchange = adbus.ExchangeDeclare(exChangeName, ExchangeType.Topic);
                    adbus.Publish(exchange, topic, false, new Message<T>(t));
                    return true;
                }
            } catch (Exception ex) {
                msg = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 获取主题 
        /// </summary>
        /// <![CDATA[（topic EasyNetQ高级API）]]>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="subscriptionId">订阅者ID</param>
        /// <param name="callback">回调</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="topics">主题名</param>
        public  void TopicConsume<T>(Action<T> callback, string exChangeName = "topic_mq",string subscriptionId = "topic_subid", params string[] topics) where T : class {
            var bus = CreateMessageBus();
            var adbus = bus.Advanced;
            var exchange = adbus.ExchangeDeclare(exChangeName, ExchangeType.Topic);
            var queue = adbus.QueueDeclare(subscriptionId);
            foreach (var item in topics) adbus.Bind(exchange, queue, item);
            adbus.Consume(queue, registration => {
                registration.Add<T>((message, info) => {
                    callback(message.Body);
                });
            });
        }

        

        #endregion


    }
}
