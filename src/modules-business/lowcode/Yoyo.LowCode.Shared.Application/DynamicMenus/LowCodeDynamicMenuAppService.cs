using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Yoyo.LowCode.DynamicMenus.DomainService;
using Yoyo.LowCode.DynamicMenus.Dtos;

namespace Yoyo.LowCode.DynamicMenus
{
    /// <summary>
    ///     动态菜单
    /// </summary>
    [AbpAuthorize]
    public class LowCodeDynamicMenuAppService : LowCodeSharedAppServiceBase, ILowCodeDynamicMenuAppService
    {
        private readonly IDynamicMenuManager _dynamicMenuManager;

        public LowCodeDynamicMenuAppService(IDynamicMenuManager dynamicMenuManager)
        {
            _dynamicMenuManager = dynamicMenuManager;
        }

        /// <summary>
        ///     获取动态菜单的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<LowCodeDynamicMenuListDto>> GetPaged(GetDynamicMenusInput input)
        {
            var query = _dynamicMenuManager.Query
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                    a.Code.Contains(input.FilterText) ||
                    a.Text.Contains(input.FilterText) ||
                    a.Link.Contains(input.FilterText) ||
                    a.Remark.Contains(input.FilterText)
                );

            var count = await query.CountAsync();

            var dynamicMenuList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var dynamicMenuListDtos = ObjectMapper.Map<List<LowCodeDynamicMenuListDto>>(dynamicMenuList);

            return new PagedResultDto<LowCodeDynamicMenuListDto>(count, dynamicMenuListDtos);
        }

        /// <summary>
        ///     获取动态菜单的分页列表集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<LowCodeDynamicMenuListDto>> GetList()
        {
            var query = await _dynamicMenuManager
                .QueryAsNoTracking.ToListAsync();

            return ObjectMapper.Map<List<LowCodeDynamicMenuListDto>>(query);
        }

        /// <summary>
        ///     通过指定id获取DynamicMenuListDto信息
        /// </summary>
        public async Task<LowCodeDynamicMenuListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _dynamicMenuManager.EntityRepo.GetAsync(input.Id);

            var dto = ObjectMapper.Map<LowCodeDynamicMenuListDto>(entity);
            return dto;
        }

        /// <summary>
        ///     获取编辑 动态菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetDynamicMenuForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetDynamicMenuForEditOutput();
            LowCodeDynamicMenuEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _dynamicMenuManager.EntityRepo.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<LowCodeDynamicMenuEditDto>(entity);
            }
            else
            {
                editDto = new LowCodeDynamicMenuEditDto();
            }

            output.DynamicMenu = editDto;
            return output;
        }

        /// <summary>
        ///     添加或者修改动态菜单的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateDynamicMenuInput input)
        {
            if (input.DynamicMenu.Id.HasValue)
            {
                await Update(input.DynamicMenu);
            }
            else
            {
                await Create(input.DynamicMenu);
            }
        }

        /// <summary>
        ///     删除动态菜单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _dynamicMenuManager.Delete(input.Id);
        }

        /// <summary>
        ///     批量删除DynamicMenu的方法
        /// </summary>
        public async Task BatchDelete(List<Guid> input)
        {
            await _dynamicMenuManager.Delete(input);
        }

        /// <summary>
        ///     新增动态菜单
        /// </summary>
        protected virtual async Task<LowCodeDynamicMenuEditDto> Create(LowCodeDynamicMenuEditDto input)
        {
            var entity = ObjectMapper.Map<BaseDynamicMenu>(input);

            entity.Code = await GetNextChildCodeAsync(entity.ParentId);

            //调用领域服务
            entity = await _dynamicMenuManager.CreateOrUpdate(entity);

            var dto = ObjectMapper.Map<LowCodeDynamicMenuEditDto>(entity);
            return dto;
        }

        /// <summary>
        ///     编辑动态菜单
        /// </summary>
        protected virtual async Task Update(LowCodeDynamicMenuEditDto input)
        {
            if (input.Id != null)
            {
                var entity = await _dynamicMenuManager.EntityRepo.GetAsync(input.Id.Value);
                ObjectMapper.Map(input, entity);

                entity.Code = await GetNextChildCodeAsync(entity.ParentId);
                await _dynamicMenuManager.CreateOrUpdate(entity);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Reset()
        {
            // 删除所有动态菜单
            var entitys = await _dynamicMenuManager.Query.ToListAsync();

            foreach (var entity in entitys)
            {
                await _dynamicMenuManager.HardDelete(entity);
            }

            var jsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"configs{Path.DirectorySeparatorChar}DynamicMenu.json");
            var pageFilterJson = File.ReadAllText(jsonFileName);

            var dynamicMenuList = JsonConvert.DeserializeObject<List<DynamicMenuCreateDto>>(pageFilterJson);
            var code = 0;
            foreach (var item in dynamicMenuList)
            {
                var codeStr = code.ToString().PadLeft(5, '0');
                CreateDynamicMenu(item, codeStr);
                code++;
            }
        }

        protected virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                return BaseDynamicMenu.AppendCode(parentCode, BaseDynamicMenu.CreateCode(1));
            }

            return BaseDynamicMenu.CalculateNextCode(lastChild.Code);
        }

        /// <summary>
        ///     获取子集信息，可能为null
        /// </summary>
        /// <param name="parentId"> </param>
        /// <returns> </returns>
        protected virtual async Task<BaseDynamicMenu> GetLastChildOrNullAsync(Guid? parentId)
        {
            return await _dynamicMenuManager.QueryAsNoTracking.Where(x => x.ParentId == parentId)
                .OrderBy(s => s.Code).LastOrDefaultAsync();
        }

        /// <summary>
        ///     获取Code码
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        protected virtual async Task<string> GetCodeAsync(Guid id)
        {
            return (await _dynamicMenuManager.FindByIdAsync(id)).Code;
        }

        private void CreateDynamicMenu(DynamicMenuCreateDto entity, string codeStr, Guid? parentId = null)
        {
            var itemEntity = new BaseDynamicMenu
            {
                Id = Guid.NewGuid(),
                ParentId = parentId,
                Name = entity.name,
                Text = entity.title,
                PermissionCode = entity.permission,
                Icon = entity.icon,
                Sort = Convert.ToInt32(entity.orderNo == null ? 0 : entity.orderNo),
                Hide = entity.hideMenu,
                OpenEnum = entity.dynamicMenu,
                IsSystem = true,
                TemplatePath = string.IsNullOrWhiteSpace(entity.path) ? entity.name : entity.path,
                Link = entity.name,
                Code = codeStr
            };
            var dicnamic = CreateBaseDynamicMenuData(itemEntity);
            if (entity.Children != null && entity.Children.Count >= 0)
            {
                CreateChildrenMenu(entity.Children, codeStr, dicnamic.Id);
            }
        }

        private void CreateChildrenMenu(List<DynamicMenuCreateDto> entityList, string codeStr, Guid? parentId = null)
        {
            var codeChild = 0;
            foreach (var item in entityList)
            {
                var codeChildStr = codeChild.ToString().PadLeft(5, '0');
                var codeChilds = $"{codeStr}.{codeChildStr}";
                CreateDynamicMenu(item, codeChilds, parentId);
                codeChild++;
            }
        }

        private BaseDynamicMenu CreateBaseDynamicMenuData(BaseDynamicMenu dynamicMenu)
        {
            _dynamicMenuManager.Create(dynamicMenu);
            return dynamicMenu;
        }

        //// custom codes

        //// custom codes end
    }
}
