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
            //Helper.CreateXml(@"D:\TFSServerCode\新建文件夹\bin");
            var s= Helper.DeserializeFromXml(@"D:\temp\XMLFile1.xml",typeof(bin));
            var temp = s as bin;
            Console.Write(temp);
        }
    }
}
