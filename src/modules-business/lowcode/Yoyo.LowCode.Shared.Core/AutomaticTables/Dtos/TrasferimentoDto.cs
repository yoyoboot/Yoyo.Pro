// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Yoyo.LowCode.LowCodeViewModels;

namespace Yoyo.LowCode.AutomaticTables.Dtos
{
    public class TrasferimentoDto
    {
        /// <summary>
        /// 表单Id
        /// </summary>
        public Guid? BaseCustomPageId { get; set; }

        /// <summary>
        /// 建表Sql
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// 表字段信息
        /// </summary>
        public List<LowCodeField> LowCodeFields { get; set; }

        /// <summary>
        /// 关联关系信息
        /// </summary>
        public List<LowCodeModelRelation> LowCodeModelRelations { get; set; }
    }
}
