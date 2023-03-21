// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoyo.Pro.EnumHelper
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnumNameAttribute : System.Attribute
    {
        public static readonly EnumNameAttribute Default = new EnumNameAttribute();

        public EnumNameAttribute()
            : this(string.Empty)
        {
        }

        public EnumNameAttribute(string enumName) => this.EnumNameValue = enumName;

        public virtual string NameValue => this.EnumNameValue;

        protected string EnumNameValue { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is EnumNameAttribute descriptionAttribute)
            {
                return descriptionAttribute.EnumNameValue == NameValue;
            }
            return false;
        }

        public override int GetHashCode()
        {
            string description = this.NameValue;
            return description == null ? 0 : description.GetHashCode();
        }

        public override bool IsDefaultAttribute() => this.Equals(Default);
    }
}
