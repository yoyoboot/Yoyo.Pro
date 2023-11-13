// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyHealthTestingDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ReponseDataDto;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.CommonDto
{
    public class HttpParamDto
    {
        public string apiUrl { get; set; }
        public Dictionary<string, string> querys { get; set; }

        public Dictionary<string, string> headers { get; set; }

        public string body { get; set; }

        public APIRequestRecord step1 { get; set; }

        public Guid ApplyID { get; set; }

        public ApplyHealthTestingListDto applyHealth { get; set; }

        public AgentAPIListDto agentAPI { get; set; }

        public ReponseData Reponsedata { get; set; }

        public Uri? baseAddress { get; set; }

        public string SOAPAction { get; set; }

        public string xmlRequest { get; set; }
    }
}
