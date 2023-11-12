// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.LowCodeViewModels.Dtos
{
    public class LowCodeCreateOrUpdateTableInput
    {
        /// <summary>
        ///     连接ID
        /// </summary>
        public Guid? DbLinkId { get; set; }

        /// <summary>
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public NewTableInfo NewTableInfo { get; set; }

        /// <summary>
        /// </summary>
        [Required(ErrorMessage = "必填")]
        public List<LowCodeFieldEditDto> TableFieldList { get; set; }
    }

    /// <summary>
    ///     表信息
    /// </summary>
    public class NewTableInfo
    {
        /// <summary>
        ///     旧表名称
        /// </summary>
        public string OldTableName { get; set; }

        /// <summary>
        ///     新表名称
        /// </summary>
        [Required(ErrorMessage = "表名不能为空")]
        public string NewTableName { get; set; }

        /// <summary>
        ///     表说明
        /// </summary>

        [Required(ErrorMessage = "表说明不能为空")]
        public string TableDesc { get; set; }
    }
}
