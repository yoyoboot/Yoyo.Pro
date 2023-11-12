// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yoyo.LowCode.StagingHistory.Dto;
using Yoyo.Pro.Domain;
using static Yoyo.LowCode.StagingHistory.Dto.DataFormatDto;

namespace Yoyo.LowCode.StagingHistory.DomainService
{
    internal class BaseStagingHistoryManager : BasicDomainService<BaseStagingHistory, Guid>, IBaseStagingHistoryManager
    {
        public BaseStagingHistoryManager(IServiceProvider serviceProvider, string localizationSourceName = null) : base(serviceProvider, localizationSourceName)
        {
        }

        public async Task<BaseStagingHistory> CreateAsync(BaseStagingHistoryCreateDto input)
        {
            BaseStagingHistory entity = new BaseStagingHistory();
            entity.Key = input.Id;
            entity.StagingJson = HandleFormatJson(input.Data);
            entity.Id = await EntityRepo.InsertAndGetIdAsync(entity);
            entity.UserId = AbpSession.UserId;
            return entity;
        }

        public string HandleFormatJson(BaseStagingHistoryDataFormatDto input)
        {
            var fields = input.Fields;
            var fieldsGroup = fields.Where(x => !x.Entity.IsNullOrWhiteSpace() && !x.Name.IsNullOrWhiteSpace()).GroupBy(x => x.Entity);
            var grids = input.Grids;

            dynamic jsonStr = new JObject();
            foreach (var toOneItem in fieldsGroup)
            {
                var obj1 = new JObject();
                foreach (var toOneChilditem in toOneItem)
                {
                    TypeConversion(toOneChilditem, obj1);
                }
                jsonStr.Add(toOneItem.Key, obj1);
            }
            if (grids.Count > 0)
            {
                foreach (var toMangItem in grids)
                {
                    var song = new JArray() as dynamic;
                    foreach (var toManyItem in toMangItem.Rows)
                    {
                        var toMangChildobj = new JObject();
                        foreach (var toManyChildItem in toManyItem.Cloumns)
                        {
                            TypeConversion(toManyChildItem, toMangChildobj);
                        }
                        song.Add(toMangChildobj);
                    }
                    jsonStr.Add(toMangItem.Entity, song);
                }
            }
            return JsonConvert.SerializeObject(jsonStr);
        }

        private static void TypeConversion(BaseStagingFieldsItem toManyChildItem, JObject toMangChildobj)
        {
            var isStr = toManyChildItem.Value is string;
            if (isStr)
            {
                toMangChildobj.Add(toManyChildItem.Name, (JToken)toManyChildItem.Value.ToString());
                return;
            }

            var isInt = toManyChildItem.Value is long;
            if (isInt)
            {
                toMangChildobj.Add(toManyChildItem.Name, (JToken)Convert.ToInt64(toManyChildItem.Value));
                return;
            }

            var isDou = toManyChildItem.Value is double;
            if (isDou)
            {
                toMangChildobj.Add(toManyChildItem.Name, (JToken)Convert.ToDouble(toManyChildItem.Value));
                return;
            }

            var isdes = toManyChildItem.Value is decimal;
            if (isdes)
            {
                toMangChildobj.Add(toManyChildItem.Name, (JToken)Convert.ToDecimal(toManyChildItem.Value));
                return;
            }

            var isBool = toManyChildItem.Value is bool;
            if (isBool)
            {
                toMangChildobj.Add(toManyChildItem.Name, (JToken)Convert.ToBoolean(toManyChildItem.Value));
                return;
            }

            var isTime = toManyChildItem.Value is DateTime;
            if (isTime)
            {
                toMangChildobj.Add(toManyChildItem.Name, (JToken)Convert.ToDateTime(toManyChildItem.Value));
                return;
            }

            if (toManyChildItem.Value == null)
            {
                toMangChildobj.Add(toManyChildItem.Name, (JToken)toManyChildItem.Value);
                return;
            }

            toMangChildobj.Add(toManyChildItem.Name, (JToken)toManyChildItem.Value?.ToString());
        }
    }
}
