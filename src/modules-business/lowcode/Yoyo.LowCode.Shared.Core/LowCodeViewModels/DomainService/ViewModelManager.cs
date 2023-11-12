// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeViewModels.DomainService
{
    public class ViewModelManager : BasicDomainService<LowCodeModel, Guid>, IViewModelManager
    {
        private readonly IWebHostEnvironment _env;

        public ViewModelManager(IServiceProvider serviceProvider, string localizationSourceName = null, IWebHostEnvironment env = null)
            : base(serviceProvider, localizationSourceName)
        {
            _env = env;
        }

        public async Task BulkDeleteAsync(string tableName)
        {
            await EntityRepo.DeleteAsync((item) => item.ModelName == tableName);
        }

        public async Task<LowCodeModel> CreateAsync(LowCodeModel entity)
        {
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);

            return entity;
        }

        /// <summary>
        ///维护表名数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task MaintenanceTableInfo(string tableName)
        {
            var entity = await GetTableInfo(tableName);

            var oldEntity = await EntityRepo.FirstOrDefaultAsync(x => x.ModelName == entity.ModelName);

            if (oldEntity != null)
            {
                oldEntity.ModelDesc = entity.ModelDesc;
                oldEntity.IsSystem = entity.IsSystem;
                await EntityRepo.UpdateAsync(oldEntity);
            }
            else
            {
                await EntityRepo.InsertAndGetIdAsync(entity);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        public async Task UpdateAsync(LowCodeModel entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }

        /// <summary>
        /// 获取表信息集合
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="lowCodeModels"></param>
        /// <returns></returns>
        private async Task<LowCodeModel> GetTableInfo(string tableName)
        {
            var path = Path.Combine(_env.WebRootPath ?? string.Empty, "lowcode", "SystemTable.json");
            var list = new List<string>();
            if (File.Exists(path))
            {
                var listViewJson = await File.ReadAllTextAsync(path);
                JObject jObject = JObject.Parse(listViewJson);
                list = jObject["exclude"].ToObject<List<string>>();
            }
            var model = new LowCodeModel();
            model.ModelName = tableName;
            model.ModelDesc = tableName;
            model.IsSystem = false;

            var entity = list.Find(x => x.ToUpper().Trim() == tableName.ToUpper().Trim());
            if (entity != null)
            {
                model.IsSystem = true;
            }
            return model;
        }
    }
}
