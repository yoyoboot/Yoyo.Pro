// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.ApplyHealthTestingDto
{
    public class ApplyHealthTestingEditDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 乐观锁Token
        /// </summary>
        public string ConcurrencyToken { get; set; }

        /// <summary>
        /// 网关应用ID
        /// </summary>
        public Guid ApplyID { get; set; }

        #region 超时策略

        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int ConnectionTimeout { get; set; }

        /// <summary>
        /// 连接超时时间对应分类 名称  （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string CtiTypeName { get; set; }

        /// <summary>
        /// 连接超时时间对应分类 Code  （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string CtiTypeCode { get; set; }

        #endregion 超时策略

        #region 熔断策略

        /// <summary>
        /// 熔断模式 名称（基础模式 F001  高级模式 F002）
        /// </summary>
        [MaxLength(200)]
        public string FusingModeName { get; set; }

        /// <summary>
        /// 熔断模式 Code（基础模式 F001  高级模式 F002）
        /// </summary>
        [MaxLength(200)]
        public string FusingModeCode { get; set; }

        /// <summary>
        /// 异常请求次数(基础模式)
        /// </summary>
        public int ExceptionsNumber { get; set; }

        /// <summary>
        /// 异常请求比例（百分比）
        /// </summary>
        public int ExceptionsScale { get; set; }

        /// <summary>
        /// 采样时间区间（时分秒）
        /// </summary>
        public int SamplingInterval { get; set; }

        /// <summary>
        /// 采样时间区间 类型名称 （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string SITypeName { get; set; }

        /// <summary>
        /// 采样时间区间 类型Code （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string SITypeCode { get; set; }

        /// <summary>
        /// 采样最小吞吐量
        /// </summary>
        public int MinimumThroughput { get; set; }

        /// <summary>
        /// 持续熔断时间
        /// </summary>
        public int ContinuousFusing { get; set; }

        /// <summary>
        /// 持续熔断时间 类型名称 （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string CFTypeName { get; set; }

        /// <summary>
        /// 持续熔断时间 类型Code （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string CFTypeCode { get; set; }

        #endregion 熔断策略

        #region 连接健康检测

        /// <summary>
        /// 健康检查模式 名称
        /// </summary>
        [MaxLength(200)]
        public string HealthTestingModeName { get; set; }

        /// <summary>
        /// 健康检查模式 Code
        /// </summary>
        [MaxLength(200)]
        public string HealthTestingModeCode { get; set; }

        /// <summary>
        /// 检查URL
        /// </summary>
        [MaxLength(200)]
        public string CheckUrl { get; set; }

        /// <summary>
        /// 检测时间间隔
        /// </summary>
        public int DetectionInterval { get; set; }

        /// <summary>
        /// 连接超时时间对应分类 名称  （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string DITypeName { get; set; }

        /// <summary>
        /// 连接超时时间对应分类 Code  （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string DITypeCode { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// 超时时间 名称  （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string TOTypeName { get; set; }

        /// <summary>
        /// 超时时间 Code  （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string TOTypeCode { get; set; }

        #endregion 连接健康检测

        #region 重试策略

        /// <summary>
        /// 重试策略名称
        /// </summary>
        [MaxLength(200)]
        public string RetryPolicyName { get; set; }

        /// <summary>
        /// 重试策略Code
        /// </summary>
        [MaxLength(200)]
        public string RetryPolicyCode { get; set; }

        /// <summary>
        /// 重试时间间隔
        /// </summary>
        public int RetryInterval { get; set; }

        /// <summary>
        /// 重试时间间隔 名称 （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string RITypeName { get; set; }

        /// <summary>
        /// 重试时间间隔 Code （时H  分M  秒S）
        /// </summary>
        [MaxLength(50)]
        public string RITypeCode { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryNumber { get; set; }

        #endregion 重试策略
    }
}
