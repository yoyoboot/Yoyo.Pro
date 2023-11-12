// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Yoyo.LowCode.AutomaticTables.Dtos;
using static Yoyo.LowCode.AutomaticTables.Dtos.CreateOrUpdateTableData;

namespace Yoyo.LowCode.AutomaticTables.DomainService
{
    public interface IAutomaticTablesManager : ITransientDependency
    {
        Task<TrasferimentoDto> CreateSqlStatement(CreateOrUpdateTableDto input, Guid pageId, bool isAuto);

        Task FinalExecution(TrasferimentoDto trasferimentoDto);
    }
}
