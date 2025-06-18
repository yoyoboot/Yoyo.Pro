// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthDataSerializer : IDataSerializer<AuthenticationProperties>
    {
        public AuthenticationProperties Deserialize(byte[] data)
        {
            var dataString = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<AuthenticationProperties>(dataString);
        }

        public byte[] Serialize(AuthenticationProperties model)
        {
            var dataString = JsonSerializer.Serialize(model);
            return Encoding.UTF8.GetBytes(dataString);
        }
    }

}
