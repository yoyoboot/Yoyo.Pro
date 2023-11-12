// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.LowCodeAPI.DomainService
{
    public class APIRequestRecordManager : BasicDomainService<APIRequestRecord, Guid>, IAPIRequestRecordManager
    {
        public APIRequestRecordManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task BatchDelete(List<Guid> input)
        {
            await EntityRepo.DeleteAsync(a => input.Contains(a.Id));
        }

        public async Task<APIRequestRecord> CreateAsync(APIRequestRecord entity)
        {
            APIRequestRecord new_entity = new APIRequestRecord();

            // 使用 ObjectMapper 进行深拷贝
            var config = new MapperConfiguration(cfg => cfg.CreateMap<APIRequestRecord, APIRequestRecord>());
            var mapper = config.CreateMapper();
            mapper.Map(entity, new_entity);

            entity.Id = await EntityRepo.InsertAndGetIdAsync(new_entity);
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        public async Task UpdateAsync(APIRequestRecord entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }
    }
}
