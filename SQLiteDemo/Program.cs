using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            //using (SQLiteConnection connection = new SQLiteConnection(SQLiteHelper.strConnectionString))
            //{
            //    //验证数据库文件是否存在
            //    if (System.IO.File.Exists(SQLiteHelper.dbPath) == false)
            //    {
            //        //创建数据库文件
            //        SQLiteConnection.CreateFile(SQLiteHelper.dbPath);
            //    }
            //    //打开数据连接
            //    connection.Open();
            //    //Command
            //    SQLiteCommand command = new SQLiteCommand(connection);
            //    command.CommandText = "CREATE TABLE tb_User(ID int,UserName varchar(60));INSERT INTO [tb_User](ID,UserName) VALUES(1,'A')";// "CREATE TABLE tb_User(ID int,UserName varchar(60));";
            //    command.CommandType = System.Data.CommandType.Text;
            //    //执行SQL
            //    int iResult = command.ExecuteNonQuery();
            //    //可省略步骤=======关闭连接
            //    connection.Close();
            //}
         var effectCount= SQLiteHelper.ExecuteNonQuery("INSERT INTO [tb_User](ID,UserName) VALUES(3,'C')");
            Console.Write(effectCount);
        }
    }
}
