// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiParameterDto;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto
{
    public class CreateOrUpdateAgentAPI
    {
        [Required]
        public AgentAPIEditDto AgentAPIEditDto { get; set; }

        public List<ApplyQHDataEditDto> QueryData { get; set; }
        public List<ApplyQHDataEditDto> HeadersData { get; set; }
        public List<ApplyBodyDataEditDto> RequestData { get; set; }
        public List<ApplyBodyDataEditDto> ReponseData { get; set; }
        public List<ParameterMapping> ParameterMapping { get; set; }
    }
}
