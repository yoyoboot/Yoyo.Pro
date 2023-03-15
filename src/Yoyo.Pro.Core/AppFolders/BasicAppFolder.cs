// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Abp.Dependency;
using System.Collections.Immutable;
using Abp.IO;

namespace Yoyo.Pro.AppFolders
{
    public class BasicAppFolder : IBasicAppFolder, ISingletonDependency
    {
        readonly ConcurrentDictionary<string, string> _data;

        public BasicAppFolder()
        {
            _data = new ConcurrentDictionary<string, string>();
        }

        public string Get(string key)
        {
            if (this.TryGetValue(key, out var path))
            {
                return path;
            }

            throw new KeyNotFoundException($"Not found key: {key}");
        }

        public bool TryGetValue(string key, out string path)
        {
            return _data.TryGetValue(key, out path);
        }

        public IReadOnlyDictionary<string, string> GetAll()
        {
            return this._data.ToImmutableDictionary();
        }

        public void Set(string key, string path, bool createIfNotExists = false)
        {
            DirectoryHelper.CreateIfNotExists(path);

            this._data.AddOrUpdate(key, path, (ok, ov) =>
            {
                return path;
            });
        }
    }
}
