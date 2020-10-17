namespace Une.TalentPool.Web.Models.CommonModels
{
    public class ActionViewModel
    {
        public string Text { get; set; }
        public string LinkUrl { get; set; }
        public string PermissionName { get; set; }
        public string CssClassName { get; set; }
        public string IconClassName { get; set; }
        public bool Enable { get; set; }
        /// <summary>
        /// 是否新窗口
        /// </summary>
        public bool Blank { get; set; }
        public ActionViewModel()
        {

        }
        public ActionViewModel(string text,
            string linkUrl,
            string permissionName = null,
            string cssClassName = null,
            string iconClassName = null,
            bool enable = true,
            bool blank = false)
        {
            Text = text;
            LinkUrl = linkUrl;
            PermissionName = permissionName;
            CssClassName = cssClassName;
            IconClassName = iconClassName;
            Enable = enable;
            Blank = blank;
        }
    }
}
