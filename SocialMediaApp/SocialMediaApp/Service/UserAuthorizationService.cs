using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.DataDB;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMediaApp.Service
{
    public interface IUserAuthorizationService
    {
        string GetToken(User user);
        bool ValidateToken(string tokenString);
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string password);
    }

    public class UserAuthorizationService : IUserAuthorizationService
    {
        //private string _securityKey { get; set; }
        private readonly IConfigurationSection _jwtSettings;

        private int PBKDF2IterCount = 1000;
        private int PBKDF2SubkeyLength = 256 / 8;
        private int SaltSize = 128 / 8;

        public UserAuthorizationService(IConfiguration config)
        {
            _jwtSettings = config.GetSection("JwtSettings");
            //_securityKey = config.GetValue<string>("JWT:Key");
            //_securityKey = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
        }

        public string HashPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException("PASSWORD_CANNOT_BE_NULL");

            byte[] salt;
            byte[] subkey;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, PBKDF2IterCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            var outputBytes = new byte[1 + SaltSize + PBKDF2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, PBKDF2SubkeyLength);

            string parola = Convert.ToBase64String(outputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
                return false;

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            if (hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
                return false;

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);

            var storedSubkey = new byte[PBKDF2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, PBKDF2IterCount))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            return ByteArraysEqual(storedSubkey, generatedSubkey);
        }

        private bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a == null || b == null || a.Length != b.Length)
                return false;

            var areSame = true;

            for (var i = 0; i < a.Length; i++)
                areSame &= (a[i] == b[i]);

            return areSame;
        }

        public string GetToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            /* Generate keys online
             * 128-bit  
             * https://www.allkeysgenerator.com/Random/Security-Encryption-Key-Generator.aspx
            */

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var idClaim = new Claim("userId", user.UserId.ToString());
            var infoClaim = new Claim("username", user.Username.ToString());
            var roleClaim = new Claim("role", user.Role.ToString());

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings["validIssuer"],
                Audience = _jwtSettings["validAudience"],
                Subject = new ClaimsIdentity(new[] { idClaim, infoClaim, roleClaim }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptior);
            var tokenString = jwtTokenHandler.WriteToken(token);

            return tokenString;
        }

        public bool ValidateToken(string tokenString)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
            };

            if (!jwtTokenHandler.CanReadToken(tokenString.Replace("Bearer ", string.Empty)))
            {
                Console.WriteLine("Invalid Token");
                return false;
            }

            jwtTokenHandler.ValidateToken(tokenString, tokenValidationParameters, out var validatedToken);
            return validatedToken != null;
        }
    }
}
