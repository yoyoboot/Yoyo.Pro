// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.UI;

namespace Yoyo.Pro.Auditing
{
    [Table("AbpAuditLogs2")]
    public class AuditLogs2 : Entity<string>, IMayHaveTenant
    {
        [MaxLength(32)]
        public override string Id { get; set; }

        /// <summary>
        /// 当前租户Id
        /// </summary>
        public virtual string TenantId { get; set; }


        /// <summary>
        /// 当前登录用户Id
        /// </summary>
        public virtual string UserId { get; set; }


        /// <summary>
        /// 服务名称
        /// </summary>
        [MaxLength(256)]
        public virtual string ServiceName { get; set; }


        /// <summary>
        /// 方法名称
        /// </summary>
        [MaxLength(256)]
        public virtual string MethodName { get; set; }


        /// <summary>
        /// 输入参数
        /// </summary>
        [MaxLength(1024)]
        public virtual string Parameters { get; set; }


        /// <summary>
        /// 返回值
        /// </summary>
        public virtual string ReturnValue { get; set; }


        /// <summary>
        /// 执行时间
        /// </summary>
        public virtual DateTime ExecutionTime { get; set; }


        /// <summary>
        /// 执行时长
        /// </summary>
        public virtual int ExecutionDuration { get; set; }


        /// <summary>
        /// 客户端ip地址
        /// </summary>
        [MaxLength(64)]
        public virtual string ClientIpAddress { get; set; }


        /// <summary>
        /// 客户端名称
        /// </summary>
        [MaxLength(128)]
        public virtual string ClientName { get; set; }


        /// <summary>
        /// 浏览器信息
        /// </summary>
        [MaxLength(512)]
        public virtual string BrowserInfo { get; set; }


        /// <summary>
        /// 异常信息
        /// </summary>
        [MaxLength(1024)]
        public virtual string ExceptionMessage { get; set; }


        /// <summary>
        /// 完整的异常堆栈信息
        /// </summary>
        [MaxLength(2000)]
        public virtual string Exception { get; set; }


        /// <summary>
        /// 模拟登录的用户Id
        /// </summary>
        public virtual string ImpersonatorUserId { get; set; }


        /// <summary>
        /// 模拟登录的租户Id
        /// </summary>
        public virtual string ImpersonatorTenantId { get; set; }


        /// <summary>
        /// 自定义数据
        /// </summary>
        [MaxLength(2000)]
        public virtual string CustomData { get; set; }

        public override string ToString()
        {
            return $"AUDIT LOG: {ServiceName}.{MethodName} is executed by user {UserId} in {ExecutionDuration} ms from {ClientIpAddress} IP address.";
        }
    }
}
