// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeViewModels.DomainService
{
    public class LowCodeFieldManager : BasicDomainService<LowCodeField, Guid>, ILowCodeFieldManager
    {
        public LowCodeFieldManager(IServiceProvider serviceProvider, string localizationSourceName = null)
            : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task<List<LowCodeField>> BlukOperateFieldAsync(List<LowCodeField> entityList)
        {
            var fieldItem = await EntityRepo.GetAllListAsync();
            foreach (var item in entityList)
            {
                var isEXit = fieldItem.Find(x => x.FieldName == item.FieldName && x.TableName == item.TableName);
                if (isEXit == null)
                {
                    item.Id = await EntityRepo.InsertAndGetIdAsync(item);
                }
            }
            return entityList;
        }

        public async Task BulkDeleteAsync(string tableName)
        {
            await EntityRepo.DeleteAsync((item) => item.TableName == tableName);
        }

        public async Task<LowCodeField> CreateAsync(LowCodeField lowCodeField)
        {
            lowCodeField.Id = await EntityRepo.InsertAndGetIdAsync(lowCodeField);
            return lowCodeField;
        }

        public async Task MaintainFields(LowCodeField lowCodeField)
        {
            var fieldItem = await EntityRepo.FirstOrDefaultAsync
                (x => x.FieldName == lowCodeField.FieldName && x.TableName == lowCodeField.TableName);
            if (fieldItem == null)
            {
                await EntityRepo.InsertAndGetIdAsync(lowCodeField);
            }
            else
            {
                fieldItem.DataType = lowCodeField.DataType;
                fieldItem.DataLength = lowCodeField.DataLength;
                fieldItem.FieldDesc = lowCodeField.FieldDesc;
                fieldItem.IsAllowNull = lowCodeField.IsAllowNull;
                fieldItem.IsForeignkey = lowCodeField.IsForeignkey;
                fieldItem.IsPrimaryKey = lowCodeField.IsPrimaryKey;
                await EntityRepo.UpdateAsync(fieldItem);
            }
        }

        public async Task UpdateAsync(LowCodeField lowCodeField)
        {
            await EntityRepo.UpdateAsync(lowCodeField);
        }
    }
}
