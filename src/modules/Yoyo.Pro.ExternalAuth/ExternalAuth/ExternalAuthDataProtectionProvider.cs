// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using Abp.Runtime.Security;
using Microsoft.AspNetCore.DataProtection;

namespace Yoyo.Pro.ExternalAuth
{
    public class ExternalAuthDataProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector CreateProtector(string purpose)
        {
            return new ExternalAuthDataProtector(purpose);
        }


        public class ExternalAuthDataProtector : IDataProtector
        {
            protected readonly string _purpose;

            public ExternalAuthDataProtector(string purpose)
            {
                _purpose = purpose;
            }

            public IDataProtector CreateProtector(string purpose)
            {
                return new ExternalAuthDataProtector(purpose);
            }

            public byte[] Protect(byte[] plaintext)
            {
                // 未加密的数据
                var decryptString = Encoding.UTF8.GetString(plaintext);
                // 加密数据
                var encryptString = SimpleStringCipher.Instance.Encrypt(decryptString, _purpose);
                // 打包加密数据
                return Encoding.UTF8.GetBytes(encryptString);
            }

            public byte[] Unprotect(byte[] protectedData)
            {
                // 加密数据
                var encryptString = Encoding.UTF8.GetString(protectedData);
                // 已解密数据
                var decryptString = SimpleStringCipher.Instance.Decrypt(encryptString, _purpose);
                // 打包解密数据
                return Encoding.UTF8.GetBytes(decryptString);
            }
        }
    }

}
