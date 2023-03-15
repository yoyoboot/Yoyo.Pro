// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Yoyo.Pro.Modules.FileManager
{
    public class BasicFileBase : AuditedEntity<Guid>, IBasicFile
    {
        public virtual Guid? ParentId { get; set; }
        public virtual bool Dir { get; set; }
        public virtual bool IsImg { get; set; }
        public virtual string Name { get; set; }
        public virtual string FileName { get; set; }
        public virtual string Path { get; set; }
        public virtual string DateDirctoryName { get; set; }
        public virtual string ObjectType { get; set; }
        public virtual string StorageClass { get; set; }
        public virtual string TimeModified { get; set; }
        public virtual string FileExt { get; set; }
        public virtual string ContentType { get; set; }
        public virtual string FormattedSize { get; set; }
        public virtual long Size { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual bool IsHidden { get; set; }
        public virtual string Code { get; set; }
    }
}
