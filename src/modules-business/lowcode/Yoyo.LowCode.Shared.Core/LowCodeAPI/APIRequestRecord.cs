// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.Entities.Auditing;

namespace Yoyo.LowCode.LowCodeAPI
{
    public class APIRequestRecord : LowCodeFullAuditedEntity<Guid>
    {
        /// <summary>
        /// 接口中心APIID
        /// </summary>
        public Guid AgentAPIID { get; set; }

        /// <summary>
        /// 请求批次信息 相同就是同一次请求的相关记录
        /// </summary>
        public Guid StepBatch { get; set; }

        /// <summary>
        /// 步骤名称
        /// </summary>
        [MaxLength(200)]
        public string StepName { get; set; }

        /// <summary>
        /// 步骤排序
        /// </summary>
        public int StepSort { get; set; }

        /// <summary>
        /// 步骤类型
        /// </summary>
        [MaxLength(200)]
        public string StepType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 步骤状态状态 成功 失败
        /// </summary>
        public bool StepStatus { get; set; }

        /// <summary>
        /// 花费时间
        /// </summary>
        [MaxLength(100)]
        public string StepTime { get; set; }

        /// <summary>
        /// 请求来源IP
        /// </summary>
        [MaxLength(50)]
        public string RequestSourceIP { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        [MaxLength(200)]
        public string RequestURL { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        [MaxLength(50)]
        public string RequestMethod { get; set; }

        /// <summary>
        /// 请求query
        /// </summary>
        [MaxLength(2000)]
        public string RequestQuerys { get; set; }

        /// <summary>
        /// 请求header
        /// </summary>
        public string RequestHeaders { get; set; }

        /// <summary>
        /// 请求body
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// 返回状态Code
        /// </summary>
        [MaxLength(200)]
        public string ReponseCode { get; set; }

        /// <summary>
        /// 返回状态Message
        /// </summary>
        public string ReponseMessage { get; set; }

        /// <summary>
        /// 返回Headers
        /// </summary>
        public string ReponseHeaders { get; set; }

        /// <summary>
        /// 返回Body
        /// </summary>
        public string ResponseBody { get; set; }

        /// <summary>
        /// 其他报错信息
        /// </summary>
        public string Exception { get; set; }
    }
}
