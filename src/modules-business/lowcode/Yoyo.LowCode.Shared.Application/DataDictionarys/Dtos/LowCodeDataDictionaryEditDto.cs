// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Yoyo.LowCode.DataDictionarys.Dtos
{
    public class LowCodeDataDictionaryEditDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        /// 系统字典
        /// </summary>
        public bool IsSystem { get; set; }

        public List<DictionaryValueEditDto> BaseDictionaryValues { get; set; }
    }
}
