// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.AppAuthDto
{
    public class CreateOrUpdateAppAuth
    {
        [Required]
        public AppAuthEditDto AppAuthEditDto { get; set; }
    }
}
