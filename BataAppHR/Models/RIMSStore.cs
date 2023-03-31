using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataAppHR.Models
{
    public class RIMSStore
    {
        public string code { get; set; }
        public string batch { get; set; }
        public string store { get; set; }
        public int seq { get; set; }
        public string storetype { get; set; }
        public string language { get; set; }
        public string storename { get; set; }
        public string manager { get; set; }
        public string miscell { get; set; }
        public int week { get; set; }
        public string region { get; set; }
        public string dist { get; set; }
        public string opendate { get; set; }
        public string closedate { get; set; }
        public string addr1 { get; set; }
        public string addr2 { get; set; }

    }
}
