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

        #region 读取指定目录下的文件生成XML文件
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
        #endregion


        #region 反序列化XML
        /// <summary>
        /// 将XML反序列化
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 反序列化XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="encodingStyle"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xml, string encodingStyle = "gb2312")
        {
            Encoding encoding = Encoding.GetEncoding(encodingStyle);
            XmlSerializer xmls = new XmlSerializer(typeof(T));
            MemoryStream memstream = new MemoryStream(encoding.GetBytes(xml));
            return (T)xmls.Deserialize(memstream);
        }
        #endregion


        #region 文件对比方法
        public static List<DiffentFile> ComparedFile(bin serverFiles,bin clientFiles)
        {
            var diffentFile = new List<DiffentFile>();
            //如果服务器端或者客户端没有文件则返回空
            if ((!serverFiles.Dir.Any() && !serverFiles.Fil.Any()) || (!clientFiles.Dir.Any() && !clientFiles.Fil.Any()))
                return diffentFile;
            //先判断大版本是否相同
            if(serverFiles.Fil.Exists(o=>o.Name== "Memex.exe")&& clientFiles.Fil.Exists(o => o.Name == "Memex.exe"))
            {
                var serverExe = serverFiles.Fil.FirstOrDefault(o=>o.Name== "Memex.exe");
                var clientExe= clientFiles.Fil.FirstOrDefault(o => o.Name == "Memex.exe");
                if (serverExe.Version == clientExe.Version)
                    return diffentFile;

                #region bin下文件比较
                if (serverFiles.Fil.Any())
                {                    
                    //客户端bin下没有文件
                    if(!clientFiles.Fil.Any())
                        serverFiles.Fil.ForEach(o => {
                            diffentFile.Add(new DiffentFile
                            {
                                DiffentValue = "客户端没有的文件",
                                DirName = "bin",
                                FilName = o.Name
                            });
                        });
                    else
                    {
                        serverFiles.Fil.ForEach(o=> {
                            var firstOrDefatult = clientFiles.Fil.FirstOrDefault(c=>c.Name==o.Name);
                            if (firstOrDefatult == null)
                                diffentFile.Add(new DiffentFile {
                                     DiffentValue="客户端没有的文件",
                                      DirName="bin",
                                      FilName=o.Name
                                });
                            else
                            {
                                if(o.Size!=firstOrDefatult.Size||o.Version!=firstOrDefatult.Version)
                                    diffentFile.Add(new DiffentFile {
                                        DiffentValue=o.Size!=firstOrDefatult.Size?"文件大小不一样":"文件版本不一样",
                                        DirName="bin",
                                        FilName=o.Name
                                    });
                            }
                        });
                    }
                }
                #endregion

                if(serverFiles.Dir.Any())
                    serverFiles.Dir.ForEach(o=> {
                        var firstOrDefault = clientFiles.Dir.FirstOrDefault(c => c.Name == o.Name);
                        ComparedFileCore(o,firstOrDefault,diffentFile);
                    });                
            }
            return diffentFile;
        }


        public static void ComparedFileCore(Dirs serverFiles, Dirs clientFiles, List<DiffentFile> diffentFile)
        {
            //客户端没有文件夹
            if (clientFiles == null)
                diffentFile.Add(new DiffentFile
                {
                    DiffentValue = "客户端没有的文件夹",
                    DirName = serverFiles.Name,
                    FilName = serverFiles.Name
                });
            else
            {
                #region 文件夹下的文件对比
                //客户端文件夹没有文件
                if (serverFiles.Fil.Any() && !clientFiles.Fil.Any())
                    serverFiles.Fil.ForEach(o =>
                    {
                        diffentFile.Add(new DiffentFile
                        {
                            DiffentValue = "客户端没有的文件",
                            DirName = serverFiles.Name,
                            FilName = o.Name
                        });
                    });
                else if (serverFiles.Fil.Any() && clientFiles.Fil.Any())
                    serverFiles.Fil.ForEach(o=> {
                        var clientFile = clientFiles.Fil.FirstOrDefault(c=>c.Name==o.Name);
                        //客户端不存在的文件
                        if (clientFiles == null)
                            diffentFile.Add(new DiffentFile
                            {
                                DiffentValue = "客户端文件不存在",
                                DirName = serverFiles.Name,
                                FilName = o.Name
                            });
                        else if (o.Size != clientFile.Size|| o.Version != clientFile.Version)
                            diffentFile.Add(new DiffentFile
                            {
                                DiffentValue = o.Size != clientFile.Size?"文件大小不一致": "文件版本不一致",
                                DirName = serverFiles.Name,
                                FilName = o.Name
                            });
                    });
                #endregion

                #region 文件夹下还有文件夹
                if (serverFiles.Dir.Any())
                    serverFiles.Dir.ForEach(o=> {
                        var firstOrDefault = clientFiles.Dir.FirstOrDefault(c => c.Name == o.Name);
                        ComparedFileCore(o, firstOrDefault, diffentFile);
                    });
                #endregion
            }
        }
        #endregion
    }
}
