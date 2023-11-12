// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Yoyo.LowCode.TestInterface.Dtos
{
    public class TestRdoDto
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }

        public string Name { get; set; }
        public List<RdoList> rdoList { get; set; }

        public Guid revOfRcdId { get; set; }
    }

    public class RdoList
    {
        public Guid Id { get; set; }

        public Guid baseId { get; set; }

        public DateTime CreateTime { get; set; }

        public string revision { get; set; }
    }
}
