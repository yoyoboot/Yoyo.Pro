// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using SqlSugar;
using Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyHealthTestingDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ReponseDataDto;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.CommonDto
{
    public class SqlserverParamDto
    {
        public ApplyListDto apply { get; set; }
        public AgentAPIListDto agentAPI { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }

        public APIRequestRecord steplog { get; set; }
        public ReponseData reponseData { get; set; }
        public List<SugarParameter> sugarParameter { get; set; }
        public ApplyHealthTestingListDto applyHealthTesting { get; set; }
    }
}
