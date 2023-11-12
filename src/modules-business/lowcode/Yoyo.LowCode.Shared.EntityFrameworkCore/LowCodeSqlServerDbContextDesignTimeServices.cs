// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.LowCode
{
    public class LowCodeSqlServerDbContextDesignTimeServices : LowCodeSqlServerDesignTimeServices
    {
        public override void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            base.ConfigureDesignTimeServices(serviceCollection);
            //new RivenOracleDesignTimeServices().ConfigureDesignTimeServices(serviceCollection);
        }
    }
}
