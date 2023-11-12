// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Yoyo.LowCode.CustomPages.DomainService;
using Yoyo.LowCode.Dtos;
using Yoyo.LowCode.LowCodeViewModels;
using Yoyo.LowCode.LowCodeViewModels.DomainService;
using Yoyo.LowCode.Renders.Dtos;
using Yoyo.Pro.Modules.FileManager.DomainService;

namespace Yoyo.LowCode.Renders
{
    /// <summary>
    ///     渲染器的接口
    /// </summary>
    public class RenderAppService : LowCodeSharedAppServiceBase, IRenderAppService
    {
        private readonly ICustomPageManager _customPageManager;
        private readonly IRenderManager _renderManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IBasicFileManager _sysFileManager;

        private readonly ILowCodeModelRelationManager _lowCodeModelRelationManager;

        public RenderAppService(IRenderManager renderManager,
            IHttpContextAccessor httpContextAccessor,
            IBasicFileManager sysFileManager,
            ICustomPageManager customPageManager,
            ILowCodeModelRelationManager lowCodeModelRelationManager)
        {
            _renderManager = renderManager;
            _httpContextAccessor = httpContextAccessor;
            _sysFileManager = sysFileManager;
            _customPageManager = customPageManager;
            _lowCodeModelRelationManager = lowCodeModelRelationManager;
        }

        private async Task<List<LowCodeModelRelation>> GetrelationList(Guid id)
        {
            return await _lowCodeModelRelationManager.QueryAsNoTracking.Where(x => x.BaseCustomPageId == id).ToListAsync();
        }

        public async Task<PagedResultDto<object>> GetPaged(GetRenderPageInput pageInput)
        {
            var customPage = await _customPageManager.QueryAsNoTracking.FirstOrDefaultAsync(x => x.Id == pageInput.CustomPageId);

            return await _renderManager
                .GetPaged(await GetrelationList(pageInput.CustomPageId), pageInput.MaintableName, pageInput.MaxResultCount, pageInput.SkipCount,
                    pageInput.IsPaged);
        }

        public async Task<string> CreateOrUpdate(RenderCreateOrUpdateInput input)
        {
            //表单配置
            var customPage = await _customPageManager.QueryAsNoTracking.FirstOrDefaultAsync(x => x.Id == input.CustomPageId);

            if (customPage == null)
            {
                throw new UserFriendlyException("错误", $"表BaseCustomPage中{input.CustomPageId}不存在");
            }

            var result = await _renderManager.CreateOrUpdate(await GetrelationList(input.CustomPageId), input.Data);
            return JsonConvert.SerializeObject(result);
        }

        public async Task Delete(GetRenderInput input)
        {
            await _renderManager.Delete(await GetrelationList(input.CustomPageId), input.MaintableName, input.Id);
        }

        public async Task BatchDelete(BatchDeleteDto input)
        {
            foreach (var id in input.Ids)
            {
                await _renderManager.Delete(await GetrelationList(input.CustomPageId), input.MaintableName, id);
            }
        }

        public async Task<string> GetById(GetRenderInput input)
        {
            return await _renderManager.GetById(await GetrelationList(input.CustomPageId), input.MaintableName, input.Id);
        }

        public async Task<LowCodeSysFileListDto> UploadFile()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var files = _httpContextAccessor.HttpContext.Request.Form.Files;

                //根据Code节点来保存

                if (files.Count == 1)
                {
                    var entity = await _sysFileManager.ProcessUploadedBasicFileAsync(files[0], false, false);
                    //调用领域服务
                    var sysFile = await _sysFileManager.CreateBasicFileAsync(entity);

                    return ObjectMapper.Map<LowCodeSysFileListDto>(sysFile);
                }
            }

            return null;
        }

        public async Task<LowCodeSysFileListDto> GetFileById(Guid id)
        {
            var entity = await _sysFileManager.FindBasicFileByIdAsync(id);

            return ObjectMapper.Map<LowCodeSysFileListDto>(entity);
        }
    }
}
