// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Yoyo.LowCode.LowCodeAPI.Dtos.WebServiceDto;

namespace Yoyo.LowCode.LowCodeAPI
{
    public interface IWebServiceAppService : IApplicationService
    {
        /// <summary>
        /// 解析wsdl
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<WebSrviceInterface>> GetWsdl(string wsdlUrl);

        /// <summary>
        /// 根据当前系统环境自动解析wsdl
        /// </summary>
        /// <param name="wsdlUrl"></param>
        /// <returns></returns>
        Task<List<WebSrviceInterface>> GetWbeServiceWsdl(Guid ApplyID);
    }
}
