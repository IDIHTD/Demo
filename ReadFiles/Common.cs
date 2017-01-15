using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReadFiles
{
    [XmlRoot("bin")]
    public class bin
    {
        [XmlElement(ElementName = "Fil")]
        public List<string> Fil { get; set; }
        [XmlElement(ElementName = "Dir")]
        public List<Dirs> Dir { get; set; }
    }

    public class Dirs
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Fil")]
        public List<string> Fil { get; set; }
        [XmlElement(ElementName = "Dir")]
        public List<Dirs> Dir { get; set; }
    }

  
}
