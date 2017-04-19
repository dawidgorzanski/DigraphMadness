using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigraphMadness.Model
{
    public class Connection
    {
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }
        public int Weight { get; set; }
    }
}
