using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushyFinder.Retrieve
{
    public class PushoverSoundsResponse
    {
        public Dictionary<string, string>? Sounds { get; set; }
        public int Status { get; set; }
    }
}
