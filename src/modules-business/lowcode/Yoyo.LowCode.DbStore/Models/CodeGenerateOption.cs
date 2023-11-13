namespace Yoyo.LowCode.Models
{
    /// <summary>
    /// 代码生成输出
    /// 版 本：V3.0.0
    /// 版 权：引迈信息技术有限公司（https://www.jnpfsoft.com）
    /// 作 者：JNPF开发平台组
    /// </summary>
    public class CodeGenerateOption
    {
        /// <summary>
        /// 输出路径
        /// </summary>
        public virtual string OutputPath { get; set; }

        /// <summary>
        /// 实体命名空间
        /// </summary>
        public virtual string EntityNamespace { get; set; }

        /// <summary>
        /// 模型命名空间
        /// </summary>
        public virtual string ModelsNamespace { get; set; }

        /// <summary>
        /// 视图模型命名空间
        /// </summary>
        public virtual string ViewModelsNamespace { get; set; }

        /// <summary>
        /// 控制器命名空间
        /// </summary>
        public virtual string ControllersNamespace { get; set; }

        /// <summary>
        /// Repositories命名空间
        /// </summary>
        public virtual string RepositoriesNamespace { get; set; }

        /// <summary>
        /// IServices命名空间
        /// </summary>
        public virtual string IServicesNamespace { get; set; }

        /// <summary>
        /// Services命名空间
        /// </summary>
        public virtual string ServicesNamespace { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsPascalCase { get; set; } = true;

        /// <summary>
        ///
        /// </summary>
        public bool GenerateApiController { get; set; } = false;
    }
}
