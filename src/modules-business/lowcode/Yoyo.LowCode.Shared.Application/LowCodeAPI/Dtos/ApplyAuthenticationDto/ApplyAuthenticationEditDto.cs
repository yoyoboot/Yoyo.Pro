// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.LowCodeAPI.Dtos.ApplyAuthenticationDto
{
    public class ApplyAuthenticationEditDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 乐观锁Token
        /// </summary>
        public string ConcurrencyToken { get; set; }

        /// <summary>
        /// 应用ID (一个应用ID只能有一个验证规则)
        /// </summary>
        public Guid ApplyID { get; set; }

        /// <summary>
        /// 身份验证分类 名称(无认证、基本认证、API密钥、Oauth 2.0、JWT)
        /// </summary>
        [MaxLength(200)]
        public string AuthenticationTypeName { get; set; }

        /// <summary>
        /// 身份验证分类 Code(1000、1001、1002、1003、1004)
        /// </summary>
        public int AuthenticationTypeCode { get; set; }

        /// <summary>
        /// 基本认证 用户名
        /// </summary>
        [MaxLength(200)]
        public string UserName { get; set; }

        /// <summary>
        /// 基本认证 密码
        /// </summary>
        [MaxLength(200)]
        public string PassWord { get; set; }

        /// <summary>
        /// API密钥 json字符串
        /// </summary>
        [MaxLength(2000)]
        public string ApiSecretKeyJson { get; set; }

        /// <summary>
        /// Oauth 2.0
        /// </summary>
        [MaxLength(2000)]
        public string Oauth20Json { get; set; }

        /// <summary>
        ///
        /// </summary>
        [MaxLength(2000)]
        public string JwtSecurityKeyJson { get; set; }
    }
}
