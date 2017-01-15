using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ReadFiles
{
   public class Helper
    {
       

        /// <summary>
        /// 将指定目录下的子目录和文件生成xml文档
        /// </summary>
        /// <param name="targetDir">根目录</param>
        /// <returns>返回XmlDocument对象</returns>
        public static XmlDocument CreateXml(string targetDir)
        {
            XmlDocument myDocument = new XmlDocument();
            XmlDeclaration declaration = myDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            myDocument.AppendChild(declaration);
            XmlElement rootElement = myDocument.CreateElement(targetDir.Substring(targetDir.LastIndexOf("\\") + 1));
            myDocument.AppendChild(rootElement);
            foreach (string fileName in Directory.GetFiles(targetDir))
            {
                XmlElement childElement = myDocument.CreateElement("Fil");
                    var fileInfo = new FileInfo(fileName);
                    FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(fileName);
                    if(myFileVersion!=null&&!string.IsNullOrEmpty(myFileVersion.FileVersion))
                    childElement.SetAttribute("Version",myFileVersion.FileVersion);
                    if (fileInfo != null)
                        childElement.SetAttribute("Size",fileInfo.Length.ToString());
                childElement.SetAttribute("Name", fileName.Substring(fileName.LastIndexOf("\\") + 1));
                rootElement.AppendChild(childElement);
            }
            foreach (string directory in Directory.GetDirectories(targetDir))
            {
                XmlElement childElement = myDocument.CreateElement("Dir");
                childElement.SetAttribute("Name", directory.Substring(directory.LastIndexOf("\\") + 1));
                rootElement.AppendChild(childElement);
                CreateBranch(directory, childElement, myDocument);
            }
            return myDocument;
        }

        /// <summary>
        /// 生成Xml分支
        /// </summary>
        /// <param name="targetDir">子目录</param>
        /// <param name="xmlNode">父目录XmlDocument</param>
        /// <param name="myDocument">XmlDocument对象</param>
        private static void CreateBranch(string targetDir, XmlElement xmlNode, XmlDocument myDocument)
        {
            foreach (string fileName in Directory.GetFiles(targetDir))
            {
                XmlElement childElement = myDocument.CreateElement("Fil");
                var fileInfo = new FileInfo(fileName);
                FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(fileName);
                if (myFileVersion != null && !string.IsNullOrEmpty(myFileVersion.FileVersion))
                    childElement.SetAttribute("Version", myFileVersion.FileVersion);
                if (fileInfo != null)
                    childElement.SetAttribute("Size", fileInfo.Length.ToString());
                childElement.SetAttribute("Name", fileName.Substring(fileName.LastIndexOf("\\") + 1));
                xmlNode.AppendChild(childElement);
            }
            foreach (string directory in Directory.GetDirectories(targetDir))
            {
                XmlElement childElement = myDocument.CreateElement("Dir");
                childElement.SetAttribute("Name", directory.Substring(directory.LastIndexOf("\\") + 1));
                xmlNode.AppendChild(childElement);
                CreateBranch(directory, childElement, myDocument);
            }
        }

        

        public static object DeserializeFromXml(string xmlFilePath, Type type)
        {
            object result = null;
            if (File.Exists(xmlFilePath))
            {
                using (StreamReader reader = new StreamReader(xmlFilePath))
                {
                    XmlSerializer xs = new XmlSerializer(type);
                    result = xs.Deserialize(reader);
                }
            }
            return result;
        }
    }
}
