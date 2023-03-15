using System;

namespace Yoyo.Pro.DbStore.Repositories
{
    public class GuidDbEntityIdGen : IDbEntityIdGen
    {
        public string Next
        {
            get => Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
