using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace Une.TalentPool.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始数据迁移。");
            DataSet dataSet = new DataSet();
            using (var connection = new MySqlConnection("Server=127.0.0.1;Uid=root;Password=qweQWE123!@#;Database=ZeusDb;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select Id,Name,keyword from resumes;"; 
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataSet);
                }
            }
            if (dataSet.Tables.Count > 0)
            {
                Console.WriteLine($"读取到{dataSet.Tables[0].Rows.Count}条数据。"); 
                using (var connection = new MySqlConnection("Server=127.0.0.1;Uid=root;Password=qweQWE123!@#;Database=TalentPoolDb;"))
                {
                    connection.Open();
                    Console.WriteLine("开始清除新表数据。");
                    using (var deleteCommand = connection.CreateCommand())
                    {
                        deleteCommand.CommandText = "delete from resumekeymaps;";
                        deleteCommand.ExecuteNonQuery();
                    }
                    Console.WriteLine("开始插入新表数据。");
                    using (var command = connection.CreateCommand())
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            DataRow row = dataSet.Tables[0].Rows[i];
                            Console.WriteLine($"开始处理第{i}条数据：Name-{row["Name"]},Keyword-{row["Keyword"]}");
                            if (row["Keyword"] != null)
                            {
                                string[] keywords = Convert.ToString(row["Keyword"]).Split(" ", StringSplitOptions.RemoveEmptyEntries); 
                                foreach (var keyword in keywords)
                                {
                                    Console.WriteLine($"提取到关键词：{keyword}");
                                    stringBuilder.Append($"insert into  resumekeymaps  (Id,Name,Keyword,OriginData,ResumeId) values('{Guid.NewGuid()}','{row["Name"]}','{keyword}','{row["Keyword"]}','{row["Id"]}');");
                                }
                            }
                           

                            if (i % 100 == 0)
                            {
                                command.CommandText = stringBuilder.ToString();
                                command.ExecuteNonQuery();
                                Console.WriteLine("写入数据成功。");
                                stringBuilder.Clear();
                            }   
                        }
                        
                    } 
                   
                }
            }
            else
            {
                Console.WriteLine("未找到任何数据。");
            }
        }

    }
}
