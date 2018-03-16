using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using DFA.Common.Extensions;

using DFA.Web.Configuration;
using DFA.Web.Models.Entities;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Services
{
    public class LoginCredentialService : ILoginCredentialService
    {
        /**********************************************************************/
        #region Constructors

        public LoginCredentialService(AppConfiguration configuration)
        {
            Contract.Requires(configuration != null);

            Configuration = configuration;

            Algorithm = Configuration.PasswordHashing.Algorithm.ToEnum<KeyDerivationPrf>();
            Iterations = Configuration.PasswordHashing.Iterations.ToInt32();
            SaltByteCount = Configuration.PasswordHashing.SaltByteCount.ToInt32();
            HashByteCount = Configuration.PasswordHashing.HashByteCount.ToInt32();
        }

        #endregion Constructors

        /**********************************************************************/
        #region IPasswordHashingService

        public LoginCredential CreateCredential(string password)
        {
            var saltBytes = new byte[SaltByteCount];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            var hashBytes = KeyDerivation.Pbkdf2(password, saltBytes, Algorithm, Iterations, HashByteCount);

            return new LoginCredential()
            {
                Algorithm = Algorithm.ToString(),
                Iterations = Iterations,
                Salt = saltBytes.ToBase64String(),
                Hash = hashBytes.ToBase64String()
            };
        }

        public bool ValidateCredential(string password, LoginCredential credential)
        {
            var saltBytes = credential.Salt.ToBase64ByteArray();
            var hashBytes = credential.Hash.ToBase64ByteArray();
            var algorithm = credential.Algorithm.ToEnum<KeyDerivationPrf>();

            var passwordHashBytes = KeyDerivation.Pbkdf2(password, saltBytes, algorithm, credential.Iterations, hashBytes.Length);

            return passwordHashBytes.SequenceEqual(hashBytes);
        }

        #endregion IPasswordHashingService

        /**********************************************************************/
        #region Protected Properties

        internal protected AppConfiguration Configuration { get; }

        internal protected KeyDerivationPrf Algorithm { get; }

        internal protected int Iterations { get; }

        internal protected int SaltByteCount { get; }

        internal protected int HashByteCount { get; }

        #endregion Protected Properties
    }
}
