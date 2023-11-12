// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Abp.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Yoyo.LowCode.SettingProviders
{
    public class SystemTableSettingProvider : SettingProvider
    {
        #region Private Fields

        private readonly IWebHostEnvironment _env;

        public SystemTableSettingProvider(IWebHostEnvironment env = null)
        {
            _env = env;
        }

        #endregion Private Fields

        #region Public Methods

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
               {
                    new SettingDefinition(
                        "System",
                         Tablist(),
                        scopes: SettingScopes.Application,
                        isVisibleToClients: true
                        ),
                };
        }

        public string Tablist()
        {
            try
            {
                var path = Path.Combine(_env.WebRootPath ?? string.Empty, "lowcode", "SystemTable.json");
                var isPath = File.Exists(path);

                if (isPath)
                {
                    var json = GetFileJson(path);
                    return json;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public string GetFileJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("utf-8")))
                {
                    json = sr.ReadToEnd();
                }
            }
            return json;
        }

        #endregion Public Methods
    }
}
