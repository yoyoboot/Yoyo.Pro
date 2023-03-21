// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.BackgroundJobs;
using System.Threading.Tasks;
using System;

namespace Yoyo.Pro.BackgroundJobs
{
    public abstract class BackgroundJobAsync<TArgs> : BackgroundJob<TArgs>
    {
        protected readonly IServiceProvider ServiceProvider;

        protected BackgroundJobAsync(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public override void Execute(TArgs args)
        {
            try
            {
                ExecuteAsync(args).ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                Logger.Error($"任务执行出错! {GetType().FullName}", ex);
                throw;
            }
        }

        /// <summary>
        /// Executes the job with the args.
        /// </summary>
        /// <param name="args">job args</param>
        /// <returns></returns>
        public abstract Task ExecuteAsync(TArgs args);
    }

    public abstract class BackgroundJobAsync : BackgroundJobAsync<int>
    {
        protected BackgroundJobAsync(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }
        public virtual void Execute()
        {
            Execute(1);
        }

        public override Task ExecuteAsync(int args)
        {
            return ExecuteAsync();
        }

        public abstract Task ExecuteAsync();
    }
}
