// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yoyo.Pro.Extensions;

namespace Yoyo.Pro.EnumHelper
{
    public static class EnumNameExtensions
    {
        public static string ToNameValue(this Enum value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            return !(member != null) ? value.ToString() : ToNameValue(member);
        }

        public static string ToNameValue(this MemberInfo member, bool inherit = false)
        {
            var attribute = member.GetAttribute<EnumNameAttribute>(inherit);
            return attribute != null ? attribute.NameValue : member.Name;
        }
    }
}
