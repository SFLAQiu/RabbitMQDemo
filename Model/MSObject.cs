using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MSObject
    {
        public string Name { get; set; }
        public int Age { get; set; } = 0;
        public DateTime AddTime { get; set; } = DateTime.Now;
    }
}
