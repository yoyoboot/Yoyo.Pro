using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.Databases.DomainService
{
    public interface IDbLinkManager : IBasicDomainService<BaseDbLink, Guid>
    {
        /// <summary>
        ///     检查实体是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(Guid id);

        /// <summary>
        ///     添加数据连接
        /// </summary>
        /// <param name="entity">数据连接实体</param>
        /// <returns></returns>
        Task<BaseDbLink> CreateAsync(BaseDbLink entity);

        /// <summary>
        ///     修改数据连接
        /// </summary>
        /// <param name="entity">数据连接实体</param>
        /// <returns></returns>
        Task UpdateAsync(BaseDbLink entity);

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="input">Id的集合</param>
        /// <returns></returns>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 验证数据库连接
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        Task<bool> IsConnection(BaseDbLink link);

        /// <summary>
        /// 获取默认数据库连接
        /// </summary>
        /// <returns></returns>
        BaseDbLink GetDefaultDbLink();

        //// custom codes

        //// custom codes end
    }
}
