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
            //读取文件写入XML
            //var xmlDoc = Helper.CreateXml(@"D:\新建文件夹\新建文件夹\bin");
            //var fileName = DateTime.Now.ToString("yyyyMMddHHss") + ".xml";
            //if (xmlDoc != null && !string.IsNullOrEmpty(xmlDoc.InnerXml))
            //    xmlDoc.Save(AppDomain.CurrentDomain.BaseDirectory + "\\XmlFiles\\" + fileName);

            //反序列化XML为实体类
            var s = Helper.DeserializeFromXml(AppDomain.CurrentDomain.BaseDirectory + "\\XmlFiles\\server.xml", typeof(bin));
            var temp = s as bin;

            var c = Helper.DeserializeFromXml(AppDomain.CurrentDomain.BaseDirectory+ "\\XmlFiles\\client.xml",typeof(bin));

            var tempc = c as bin;

            //对比方法

            Helper.ComparedFile(temp,tempc);
        }
    }
}
