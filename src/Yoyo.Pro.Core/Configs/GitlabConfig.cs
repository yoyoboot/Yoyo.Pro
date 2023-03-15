namespace Yoyo.Pro.Configs
{
 
/// <summary>
/// 获取Gitlab的配置信息。
/// </summary>
    public class GitlabConfig
    {
        public bool Enabled { get; set; }

        public string ApiUrl { get; set; }
        public string RawUrl { get; set; }
        public string AccessToken { get; set; }
        public string Branch { get; set; }
        public int FileLimitSize { get; set; }
    }
}
