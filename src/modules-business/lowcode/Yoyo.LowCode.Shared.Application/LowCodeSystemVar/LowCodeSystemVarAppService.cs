// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Yoyo.LowCode.LowCodeSystemVar.Dtos;

namespace Yoyo.LowCode.LowCodeSystemVar
{
    public class LowCodeSystemVarAppService : LowCodeSharedAppServiceBase
    {
        private readonly List<LowCodeSystemVariablesListDto> _lowCodeSystemVariables;

        public LowCodeSystemVarAppService()
        {
            _lowCodeSystemVariables = new List<LowCodeSystemVariablesListDto>()
            {
                new LowCodeSystemVariablesListDto()
                {
                    Name="创建时间",
                    Key="CreationTime"
                },
                new LowCodeSystemVariablesListDto()
                {
                    Name="创建人",
                    Key="CreateUserName"
                },
                new LowCodeSystemVariablesListDto()
                {
                    Name="修改时间",
                    Key="ModificationTime"
                },
                new LowCodeSystemVariablesListDto()
                {
                    Name="修改时间",
                    Key="ModificationUserName"
                }
            };
        }

        public List<LowCodeSystemVariablesListDto> GetList()
        {
            return _lowCodeSystemVariables;
        }

        public Dictionary<string, string> GetData()
        {
            var dict = new Dictionary<string, string>()
            {
                { "CreateUserName","Admin" },
                 { "ModificationTime",DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") }
            };
            return dict;
        }
    }
}
