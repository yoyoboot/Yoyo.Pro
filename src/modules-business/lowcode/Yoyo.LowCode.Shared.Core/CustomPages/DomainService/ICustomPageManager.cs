using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.CustomPages.DomainService
{
    public interface ICustomPageManager : IBasicDomainService<BaseCustomPage, Guid>
    {
        /// <summary>
        ///     检查实体是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> IsExistAsync(Guid id);

        /// <summary>
        ///     添加功能设计
        /// </summary>
        /// <param name="entity">功能设计实体</param>
        /// <returns></returns>
        Task<BaseCustomPage> CreateAsync(BaseCustomPage entity);

        /// <summary>
        ///     修改功能设计
        /// </summary>
        /// <param name="entity">功能设计实体</param>
        /// <returns></returns>
        Task UpdateAsync(BaseCustomPage entity);

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

        //// custom codes

        //// custom codes end
    }
}
