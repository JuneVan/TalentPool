namespace Van.TalentPool.Infrastructure.Message.Email
{
    public class EmailEntry
    {
        // 收信人名字
        public string ToName { get; set; }
        // 收信人邮箱地址
        public string ToEmailAddress { get; set; }
        // 邮件主题
        public string Subject { get; set; }
        // 邮件内容
        public string Body { get; set; }
    }
}
