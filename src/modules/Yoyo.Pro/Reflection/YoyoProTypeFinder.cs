// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Collections.Extensions;
using Abp.Reflection;
using Castle.Core.Logging;
using Microsoft.Extensions.Options;

namespace Yoyo.Pro.Reflection
{
    public class YoyoProTypeFinder : ITypeFinder
    {
        public ILogger Logger { get; set; }

        private readonly IAssemblyFinder _assemblyFinder;
        private readonly object _syncObj = new object();
        private Type[] _types;

        protected readonly TypeFinderOptions _options;

        public YoyoProTypeFinder(IAssemblyFinder assemblyFinder, IOptions<TypeFinderOptions> options)
        {
            _assemblyFinder = assemblyFinder;
            Logger = NullLogger.Instance;

            _options = options.Value;
        }

        public Type[] Find(Func<Type, bool> predicate)
        {
            return GetAllTypes().Where(predicate).ToArray();
        }

        public Type[] FindAll()
        {
            return GetAllTypes().ToArray();
        }

        private Type[] GetAllTypes()
        {
            if (_types == null)
            {
                lock (_syncObj)
                {
                    if (_types == null)
                    {
                        _types = CreateTypeList().ToArray();
                    }
                }
            }

            return _types;
        }

        private List<Type> CreateTypeList()
        {
            var allTypes = new List<Type>();

            var assemblies = _assemblyFinder.GetAllAssemblies().Distinct();

            foreach (var assembly in assemblies)
            {
                try
                {
                    Type[] typesInThisAssembly;

                    try
                    {
                        typesInThisAssembly = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        typesInThisAssembly = ex.Types;
                    }

                    if (typesInThisAssembly.IsNullOrEmpty())
                    {
                        continue;
                    }

                    allTypes.AddRange(
                        typesInThisAssembly.Where(type => type != null
                            && !_options.IsSkip(type)
                            )
                        );
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString(), ex);
                }
            }

            return allTypes;
        }
    }
}
