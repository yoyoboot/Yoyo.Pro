// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Yoyo.Pro.WorkflowCore.workflow;
using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Interface;

namespace Yoyo.Pro.WorkflowCore
{
    public static class WorkflowInitializeExtension
    {
        /// <summary>
        /// 初始化流程
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitializeWorkflow(this IServiceProvider serviceProvider)
        {
            var host = serviceProvider.GetService<IWorkflowHost>();
            if (host != null)
            {
                host.Start();
            }
            serviceProvider.GetService<AbpWorkflowManager>()?.Initialize();
        }
    }
}
