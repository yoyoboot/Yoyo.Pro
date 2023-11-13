// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.DataDictionarys.Dtos
{
    public class CreateOrUpdateDictonary
    {
        [Required]
        public LowCodeDataDictionaryEditDto LowCodeDataDictionaryPage { get; set; }
    }
}
