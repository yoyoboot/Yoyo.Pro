// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto
{
    /// <summary>
    /// API网关统计
    /// </summary>
    public class Dashboard_TJDto
    {
        /// <summary>
        /// API接口数量
        /// </summary>
        public int ApiCounts { get; set; }

        /// <summary>
        /// API总请求次数
        /// </summary>
        public int RequestApiCounts { get; set; }

        /// <summary>
        /// 峰值吞吐量
        /// </summary>
        public decimal PeakThroughput { get; set; }

        /// <summary>
        /// 平均响应时间
        /// </summary>
        public decimal AvgResponseTime { get; set; }

        /// <summary>
        /// 异常总数
        /// </summary>
        public int ExceptionCounts { get; set; }

        /// <summary>
        /// 告警数量
        /// </summary>
        public int ReportEmergencyCounts { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        //流量趋势统计信息
        /// <summary>
        /// x轴 数据列
        /// </summary>
        public string volume_xAxis_data { get; set; }

        /// <summary>
        /// 数据列 总流量
        /// </summary>
        public string volume_series_data0 { get; set; }

        /// <summary>
        /// 数据量 成功
        /// </summary>
        public string volume_series_data1 { get; set; }

        /// <summary>
        /// 数据列失败
        /// </summary>
        public string volume_series_data2 { get; set; }

        /// <summary>
        /// 数据列 总平均响应时间
        /// </summary>
        public string reponse_series_data0 { get; set; }

        /// <summary>
        /// 数据列 网关平均响应时间
        /// </summary>
        public string reponse_series_data1 { get; set; }

        /// <summary>
        /// 数据列 后台服务平均响应时间
        /// </summary>
        public string reponse_series_data2 { get; set; }

        /// <summary>
        /// api列表
        /// </summary>
        public List<Dashboard_apiListDto> apiList { get; set; }
    }

    public class Dashboard_apiListDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// api名称
        /// </summary>
        public string apiName { get; set; }

        /// <summary>
        /// api请求路径
        /// </summary>
        public string apiPaht { get; set; }

        /// <summary>
        /// 请求总次数
        /// </summary>
        public int requestCounts { get; set; }

        /// <summary>
        /// 峰值吞吐量
        /// </summary>
        public decimal peakThroughput { get; set; }

        /// <summary>
        /// 错误数
        /// </summary>
        public decimal errorCounts { get; set; }

        /// <summary>
        /// 错误率
        /// </summary>
        public decimal errorRate { get; set; }

        /// <summary>
        /// 网关平均响应值
        /// </summary>
        public decimal wgResponse { get; set; }

        /// <summary>
        /// 后台服务平均响应值
        /// </summary>
        public decimal fwResponse { get; set; }
    }
}
