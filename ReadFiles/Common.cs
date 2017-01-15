using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReadFiles
{
    [Serializable]
    public class bin
    {
        public List<string> Fil { get; set; }

        public List<Dirs> Dir { get; set; }
    }
    [Serializable]
    public class Dirs
    {
        public List<string> Fil { get; set; }

        public List<Dirs> Dir { get; set; }
    }

  
}
