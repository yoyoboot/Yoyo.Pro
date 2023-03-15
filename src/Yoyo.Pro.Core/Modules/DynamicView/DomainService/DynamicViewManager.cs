using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Extensions.Hosting.Internal;

namespace Yoyo.Pro.Modules.DynamicView.DomainService
{
    public class DynamicViewManager : IDynamicViewManager
    {
        static readonly ConcurrentDictionary<string, Type> _enumDict = new ConcurrentDictionary<string, Type>();

        private readonly IHostEnvironment _hostEnv;

        readonly ILogger<DynamicViewManager> _logger;

        public DynamicViewManager(ILogger<DynamicViewManager> logger, IHostEnvironment hostEnv = null)
        {
            if (hostEnv == null)
            {
                _hostEnv = new HostingEnvironment();
            }
            else
            {
                _hostEnv = hostEnv;
            }

            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<DynamicPage> GetDynamicPageInfo(string name)
        {
            try
            {
                var pageFilters = await GetPageFiltersFromFile(name);
                var columns = await GetColumnsFromFile(name);

                return new DynamicPage()
                {
                    PageFilters = pageFilters,
                    Columns = columns
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PageFilterItem>> GetPageFilters(string name)
        {
            return await GetPageFiltersFromFile(name);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ColumnItem>> GetColumns(string name)
        {
            return await GetColumnsFromFile(name);
        }

        protected async Task<List<PageFilterItem>> GetPageFiltersFromFile(string name)
        {
            var configFilePath = Path.Join(
                _hostEnv.ContentRootPath,
                    "wwwroot",
                    "configs",
                    "page-filter",
                    $"{name}.json"
                );

            if (!File.Exists(configFilePath))
            {
                return null;
            }

            var fileContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);

            var pageFilters = JsonConvert.DeserializeObject<List<PageFilterItem>>(fileContent);

            return pageFilters;
        }

        protected async Task<List<ColumnItem>> GetColumnsFromFile(string name)
        {
            var configFilePath = Path.Join(
                    _hostEnv.ContentRootPath,
                    "wwwroot",
                    "configs",
                    "list-view",
                    $"{name}.json"
                );
            if (!File.Exists(configFilePath))
            {
                return null;
            }

            var fileContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);

            var listView = JsonConvert.DeserializeObject<List<ColumnItem>>(fileContent);

            return listView;
        }

        public virtual IDynamicViewManager AddEnum(Assembly assembly)
        {
            if (assembly == null)
            {
                return this;
            }

            var enumTypes = assembly.GetTypes().Where(o => o.IsEnum).ToList();
            foreach (var type in enumTypes)
            {
                _enumDict.AddOrUpdate(type.Name, type, (k, v) => type);
            }

            return this;
        }

        public virtual IDynamicViewManager AddEnum<T>()
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                return this;
            }

            _enumDict.AddOrUpdate(type.Name, type, (k, v) => type);

            return this;
        }

        public virtual Type GetEnumType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            _enumDict.TryGetValue(name, out var val);

            return val;
        }
    }
}
