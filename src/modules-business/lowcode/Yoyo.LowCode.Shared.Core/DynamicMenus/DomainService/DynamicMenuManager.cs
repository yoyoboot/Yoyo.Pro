using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.DynamicMenus.DomainService
{
    public class DynamicMenuManager : BasicDomainService<BaseDynamicMenu, Guid>, IDynamicMenuManager
    {
        public async Task HardDelete(BaseDynamicMenu entity)
        {
            await this.EntityRepo.HardDeleteAsync(entity);
        }

        public DynamicMenuManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
