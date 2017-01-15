using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace ReadFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            //var xmlDoc = Helper.CreateXml(@"D:\TFSServerCode\新建文件夹\bin");
            //if (xmlDoc != null && !string.IsNullOrEmpty(xmlDoc.InnerXml))
            //    xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "\\XmlFiles\\" + DateTime.Now.ToString("yyyyMMddMMss") + ".xml");
            var s = Helper.DeserializeFromXml(AppDomain.CurrentDomain.BaseDirectory + "\\XmlFiles\\201701150155.xml", typeof(bin));
            var temp = s as bin;
            Console.Write(temp);
        }
    }
}
