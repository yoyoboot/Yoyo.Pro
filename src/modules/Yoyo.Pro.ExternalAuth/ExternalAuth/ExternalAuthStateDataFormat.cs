// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthStateDataFormat : ISecureDataFormat<AuthenticationProperties>
    {
        public readonly IDataSerializer<AuthenticationProperties> _serializer;
        public readonly IDataProtector _protector;


        public ExternalAuthStateDataFormat(IDataProtector dataProtector)
        {
            _serializer = new ExternalAuthDataSerializer();
            _protector = dataProtector;
        }

        public string Protect(AuthenticationProperties data)
        {
            return Protect(data, purpose: null);
        }

        public string Protect(AuthenticationProperties data, string purpose)
        {
            var userData = _serializer.Serialize(data);

            var protector = _protector;
            if (!string.IsNullOrEmpty(purpose))
            {
                protector = protector.CreateProtector(purpose);
            }

            var protectedData = protector.Protect(userData);
            return Base64UrlTextEncoder.Encode(protectedData);
        }

        public AuthenticationProperties Unprotect(string protectedText)
        {
            return Unprotect(protectedText, null);
        }

        public AuthenticationProperties Unprotect(string protectedText, string purpose)
        {
            if (protectedText == null)
            {
                return default;
            }

            var protectedData = Base64UrlTextEncoder.Decode(protectedText);
            if (protectedData == null)
            {
                return default;
            }

            var protector = _protector;
            if (!string.IsNullOrEmpty(purpose))
            {
                protector = protector.CreateProtector(purpose);
            }

            var userData = protector.Unprotect(protectedData);
            if (userData == null)
            {
                return default;
            }

            return _serializer.Deserialize(userData);
        }
    }

}
