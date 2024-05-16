using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.CustomPages.DomainService
{
    /// <summary>
    ///     功能设计的领域服务方法
    /// </summary>
    public class CustomPageManager : BasicDomainService<BaseCustomPage, Guid>, ICustomPageManager
    {
        /// <summary>
        ///     BaseCustomPage的构造方法
        ///     通过构造函数注册服务到依赖注入容器中
        /// </summary>
        public CustomPageManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            LocalizationSourceName = LowCodeConsts.LocalizationSourceName;
        }

        public async Task<BaseCustomPage> CreateAsync(BaseCustomPage entity)
        {
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task UpdateAsync(BaseCustomPage entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            foreach (var id in input)
            {
                await DeleteAsync(id);
            }
            //await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await EntityRepo.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        //// custom codes

        //// custom codes end
    }
}
