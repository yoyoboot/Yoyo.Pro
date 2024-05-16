// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.Renders.Dtos
{
    public class GetRenderInput
    {
        [Required]
        public Guid CustomPageId { get; set; }

        public string MaintableName { get; set; }

        [Required]
        public string Id { get; set; }
    }
}
