using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace Yoyo.Pro.Editions
{
    public abstract class EditionManagerBase : AbpEditionManager
    {
        public const string DefaultEditionName = "Standard";

        public EditionManagerBase(
            IRepository<Edition> editionRepository,
            IAbpZeroFeatureValueStore featureValueStore,
            IUnitOfWorkManager unitOfWorkManager
            )
            : base(
                editionRepository,
                featureValueStore,
                unitOfWorkManager)
        {
        }
    }
}
