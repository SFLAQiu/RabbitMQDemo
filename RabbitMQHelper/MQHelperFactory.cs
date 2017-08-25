using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQHelper {
    public class MQHelperFactory {
        public static MQHelper Default() =>
            new MQHelper("default");
    }
}
