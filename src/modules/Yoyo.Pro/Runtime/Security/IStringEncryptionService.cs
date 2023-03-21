// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Abp.Dependency;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Yoyo.Pro.Runtime.Security
{
    public interface IStringEncryptionService
    {
        /// <summary>
        /// Encrypts a text.
        /// </summary>
        /// <param name="plainText">The text in plain format</param>
        /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
        /// <param name="salt">Salt value (optional, uses default if not provided)</param>
        /// <returns>Enrypted text</returns>
        [CanBeNull]
        string Encrypt([CanBeNull] string plainText, string passPhrase = null, byte[] salt = null);

        /// <summary>
        /// Decrypts a text that is encrypted by the <see cref="Encrypt"/> method.
        /// </summary>
        /// <param name="cipherText">The text in encrypted format</param>
        /// <param name="passPhrase">A phrase to use as the encryption key (optional, uses default if not provided)</param>
        /// <param name="salt">Salt value (optional, uses default if not provided)</param>
        /// <returns>Decrypted text</returns>
        [CanBeNull]
        string Decrypt([CanBeNull] string cipherText, string passPhrase = null, byte[] salt = null);
    }

    /// <summary>
    /// Can be used to simply encrypt/decrypt texts.
    /// </summary>
    public class StringEncryptionService : IStringEncryptionService, ITransientDependency
    {
        protected AbpStringEncryptionOptions Options { get; }

        public StringEncryptionService(IOptions<AbpStringEncryptionOptions> options)
        {
            Options = options.Value;
        }

        public virtual string Encrypt(string plainText, string passPhrase = null, byte[] salt = null)
        {
            if (plainText == null)
            {
                return null;
            }

            if (passPhrase == null)
            {
                passPhrase = Options.DefaultPassPhrase;
            }

            if (salt == null)
            {
                salt = Options.DefaultSalt;
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, salt))
            {
                var keyBytes = password.GetBytes(Options.Keysize / 8);
                using (var symmetricKey = Aes.Create())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, Options.InitVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public virtual string Decrypt(string cipherText, string passPhrase = null, byte[] salt = null)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return null;
            }

            if (passPhrase == null)
            {
                passPhrase = Options.DefaultPassPhrase;
            }

            if (salt == null)
            {
                salt = Options.DefaultSalt;
            }

            var cipherTextBytes = Convert.FromBase64String(cipherText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, salt))
            {
                var keyBytes = password.GetBytes(Options.Keysize / 8);
                using (var symmetricKey = Aes.Create())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, Options.InitVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var totalReadCount = 0;
                                while (totalReadCount < cipherTextBytes.Length)
                                {
                                    var buffer = new byte[cipherTextBytes.Length];
                                    var readCount = cryptoStream.Read(buffer, 0, buffer.Length);
                                    if (readCount == 0)
                                    {
                                        break;
                                    }

                                    for (var i = 0; i < readCount; i++)
                                    {
                                        plainTextBytes[i + totalReadCount] = buffer[i];
                                    }

                                    totalReadCount += readCount;
                                }

                                return Encoding.UTF8.GetString(plainTextBytes, 0, totalReadCount);
                            }
                        }
                    }
                }
            }
        }
    }
}
