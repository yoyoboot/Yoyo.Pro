// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyAuthenticationDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyHealthTestingDto;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.ApplyDto
{
    public class UpdateApplySettingInput
    {
        [Required]
        public ApplyEditDto ApplyEditDto { get; set; }

        [Required]
        public ApplyAuthenticationEditDto ApplyAuthenticationEditDto { get; set; }

        [Required]
        public ApplyHealthTestingEditDto ApplyHealthTestingEditDto { get; set; }
    }
}
