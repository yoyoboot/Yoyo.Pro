using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.Extension;
using Yoyo.Pro.Domain;
using DbType = SqlSugar.DbType;

namespace Yoyo.LowCode.Databases.DomainService
{
    /// <summary>
    ///     数据连接的领域服务方法
    /// </summary>
    public class DbLinkManager : BasicDomainService<BaseDbLink, Guid>, IDbLinkManager
    {
        private readonly IConfiguration _appConfiguration;

        /// <summary>
        ///     BaseDbLink的构造方法
        ///     通过构造函数注册服务到依赖注入容器中
        /// </summary>
        public DbLinkManager(IServiceProvider serviceProvider, IConfiguration appConfiguration) : base(
            serviceProvider)
        {
            _appConfiguration = appConfiguration;
        }

        public async Task<BaseDbLink> CreateAsync(BaseDbLink entity)
        {
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        public async Task UpdateAsync(BaseDbLink entity)
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
            await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            var result = await EntityRepo.GetAll().AnyAsync(a => a.Id == id);
            return result;
        }

        /// <summary>
        ///     测试数据库连接
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<bool> IsConnection(BaseDbLink link)
        {
            var dbType = DbType.SqlServer;
            var connectionType = _appConfiguration["ConnectionStrings:DatabaseType"];

            if (link?.DbType != null)
            {
                dbType = link.DbType;
            }

            if (!string.IsNullOrEmpty(connectionType) && link?.DbType == null)
            {
                dbType = connectionType.ToDatabaseType();
            }
            var connectionString = link == null
                ? _appConfiguration["ConnectionStrings:Default"]
                : DataHelper.ToConnectionString(dbType, link.Host, link.Port, link.UserName, link.Password,
                    link.ServiceName);
            var result = false;
            var sqlCon = DataHelper.GetConnection(dbType, connectionString);
            try
            {
                await sqlCon.OpenAsync();
                if (sqlCon.State == ConnectionState.Open)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                await sqlCon.CloseAsync();
                throw new UserFriendlyException(L("连接失败", ex.ToString()));
            }
            finally
            {
                await sqlCon.CloseAsync();
            }

            return result;
        }

        /// <summary>
        ///     获取默认数据库连接
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BaseDbLink GetDefaultDbLink()
        {
            var dbType = DbType.SqlServer;
            var connectionType = _appConfiguration["ConnectionStrings:DatabaseType"];

            if (!string.IsNullOrEmpty(connectionType))
            {
                dbType = connectionType.ToDatabaseType();
            }

            var result = new BaseDbLink { DbType = dbType, FullName = "Default" };

            return result;
        }

        //// custom codes

        //// custom codes end
    }
}
