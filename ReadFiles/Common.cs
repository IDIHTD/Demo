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
        public List<Fils> Fil { get; set; }

        [XmlElement(ElementName = "Dir")]
        public List<Dirs> Dir { get; set; }
    }

    public class Dirs
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Fil")]
        public List<Fils> Fil { get; set; }

       
        [XmlElement(ElementName = "Dir")]
        public List<Dirs> Dir { get; set; }
    }


    public class Fils
    {
        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }

        [XmlAttribute(AttributeName = "Size")]
        public string Size { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

    }

    [Serializable]
    public class DiffentFile
    {
        public string DirName { get; set; }

        public string FilName { get; set; }

        public string DiffentValue { get; set; }
    }
  
}
