using System;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.DynamicMenus.DomainService
{
    public interface IDynamicMenuManager : IBasicDomainService<BaseDynamicMenu, Guid>
    {
        Task HardDelete(BaseDynamicMenu entity);
    }
}
