using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Une.TalentPool.KeywordmapGenerator
{
    class Program
    {
        /// <summary>
        /// 用于生成简历的关键词
        /// 因为某些BUG导致Keyword生成失败或丢失问题
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            using (MySqlConnection connection = new MySqlConnection("Server=192.168.1.11;Uid=root;Password=123456;Database=TalentPoolDb;"))
            {
                connection.Open();
                Console.WriteLine("开始查询简历关键词。");
                int totalCount = 0;
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select count(1) from resumes;";
                    totalCount = Convert.ToInt32(command.ExecuteScalar());
                }
                Console.WriteLine($"检索到{totalCount}条简历数据。");
                if (totalCount > 0)
                {
                    Console.WriteLine($"开始清除关键词数据。");
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "delete from resumekeymaps;";
                        command.ExecuteNonQuery();
                    }
                }

                int perCount = 50, currentIndex = 0;
                while (currentIndex < totalCount)
                {
                    Console.WriteLine($"开始处理第{currentIndex}到{currentIndex + perCount}的简历记录。");
                    DataSet dataset = new DataSet();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"select Id,Name,Description from resumes limit {currentIndex},{perCount};";
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        adapter.Fill(dataset);
                    }
                    StringBuilder executeSql = new StringBuilder();
                    if (dataset.Tables.Count > 0 & dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dataset.Tables[0].Rows)
                        {
                            var id = Convert.ToString(row["Id"]);
                            var name = Convert.ToString(row["Name"]);
                            var body = Convert.ToString(row["Description"]);
                            Console.WriteLine($"开始处理简历id:{id}-name:{name}");
                            body = new Regex(@"<[^>]*?>").Replace(body, " ");
                            var regex = new Regex(@"(?<=(&nbsp;|公司名称：|\s))([a-zA-Z\u4e00-\u9fa5（）\(\)]{4,}公司{1}|[a-zA-Z0-9\u4e00-\u9fa5（）\(\)]{2,}集团{1})([a-zA-Z0-9\u4e00-\u9fa5（）\(\)]*)(?=\s|&nbsp;)",RegexOptions.Compiled);
                            var matches = regex.Matches(body);
                            Console.WriteLine("开始处理关键词。");
                            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                            if (matches != null)
                            {  
                                foreach (Match item in matches)
                                { 
                                    if (keyValuePairs.ContainsKey(item.Value))
                                        continue;
                                    Console.WriteLine($"匹配到关键词:{item.Value}。");
                                    keyValuePairs.Add(item.Value,null);
                                    executeSql.Append($"insert into resumekeymaps (Id,Name,Keyword,ResumeId) values('{Guid.NewGuid()}','{name}','{item.Value}','{id}');");
                                }
                                
                            }
                        }
                    }
                    if (executeSql.Length > 0)
                    {
                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = executeSql.ToString();
                            command.ExecuteNonQuery();
                        }
                    } 
                    currentIndex += perCount;
                }



                //var id = Convert.ToString(reader["Id"]);
                //var name = Convert.ToString(reader["Name"]);
                //var body = Convert.ToString(reader["Description"]);
                //Console.WriteLine($"开始处理简历id:{id}-name:{name}");
                //body = new Regex(@"<[^>]*?>").Replace(body, " ");
                //var regex = new Regex(@"(?<=(&nbsp;|公司名称：|\s))([a-zA-Z\u4e00-\u9fa5（）\(\)]{4,}公司{1}|[a-zA-Z0-9\u4e00-\u9fa5（）\(\)]{2,}集团{1})([a-zA-Z0-9\u4e00-\u9fa5（）\(\)]*)(?=\s|&nbsp;)");
                //var matches = regex.Matches(body);
                //if (matches != null)
                //{
                //    Console.WriteLine("开始处理关键词。");
                //    StringBuilder sql = new StringBuilder();
                //    foreach (Match item in matches)
                //    {
                //        Console.WriteLine($"匹配到关键词:{item.Value}。");
                //        sql.Append($"insert into resumekeymaps (Id,Name,Keyword,ResumeId) values('{Guid.NewGuid()}','{name}','{item.Value}','{id}');");
                //    }
                //    using (MySqlCommand insertKeyMapCommand = connection.CreateCommand())
                //    {
                //        insertKeyMapCommand.CommandText = sql.ToString();
                //        insertKeyMapCommand.ExecuteNonQuery();
                //    }
                //}

            }
        }
    }
}
