// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoyo.LowCode.LowCodeViewModels.Dtos;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeViewModels.DomainService
{
    public class LowCodeModelRelationManager : BasicDomainService<LowCodeModelRelation, Guid>, ILowCodeModelRelationManager
    {
        private readonly IViewModelManager _viewModelManager;

        public LowCodeModelRelationManager(IServiceProvider serviceProvider, string localizationSourceName = null, IViewModelManager viewModelManager = null) : base(serviceProvider, localizationSourceName)
        {
            _viewModelManager = viewModelManager;
        }

        public async Task<List<LowCodeModelRelation>> BlukOperateRelationAsync(List<LowCodeModelRelation> entityList)
        {
            foreach (var item in entityList)
            {
                var entity = await EntityRepo.FirstOrDefaultAsync(item.Id);
                if (entity != null)
                {
                    entity.BaseCustomPageId = item.BaseCustomPageId;
                    entity.MainModelName = item.MainModelName;
                    entity.MainModelField = item.MainModelField;
                    entity.ChildModelName = item.ChildModelName;
                    entity.ChildModelField = item.ChildModelField;
                    entity.ObjectRelation = item.ObjectRelation;
                    await EntityRepo.UpdateAsync(entity);
                }
                else
                {
                    item.Id = await EntityRepo.InsertAndGetIdAsync(item);
                }

                await _viewModelManager.MaintenanceTableInfo(item.ChildModelName);
            }
            return entityList;
        }

        public async Task BulkDeleteAsync(string tableName)
        {
            await EntityRepo.DeleteAsync((item) => item.MainModelName == tableName);
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        public RefTableFieldModel IsReferenceTable(string tabelName, string fieldName)
        {
            var query = EntityRepo.FirstOrDefault(x => (x.MainModelName == tabelName && x.MainModelField == fieldName)
                        || (x.ChildModelName == tabelName && x.ChildModelField == fieldName));
            var dto = new RefTableFieldModel();

            return dto;
        }
    }
}
