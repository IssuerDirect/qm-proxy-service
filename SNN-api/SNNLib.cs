using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net3000.common;
using net3000.common.models;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using System.Text;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using net3000;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace snn
{
    public class SNNLib
    {
        lib clib = new lib();
        public platformDB platformDB;
        public IConfiguration config {
            set {
                clib.myConfiguration = value;
            }
            get {
                return clib.myConfiguration;
            }
        }
        apiResponse myResponse = new apiResponse();
        public snn_users user;
        public apiResponse logMeIn(Dictionary<string, string> inputUser, HttpContext myContext = null)
        {
            var possibleUsers = platformDB.snn_users.Where(u => u.email == inputUser["email"]);
            if (inputUser.ContainsKey("logingroupid"))
            {
                possibleUsers = possibleUsers.Where(u => u.logingroupid.ToString() == inputUser["logingroupid"]);
            }
            
            var allmatching = possibleUsers.ToList();
            user = allmatching.Where(u => u.password == inputUser["password"] || u.password == encrypt(inputUser["password"]) || u.password == hashMe(inputUser["password"], u.token.ToString())).FirstOrDefault();
            if (user != null)
            {
                return loginResponse(user, myContext);
            }
            else
            {
                myResponse = standardMessages.notFound;
                myResponse.data = "Can't find this user";
                return myResponse;
            }
        }
        public apiResponse loginResponse(snn_users login, HttpContext myContext = null)
        {
            if (myContext != null )
            {
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,login.firstName.ToString()),
                    new Claim(ClaimTypes.Sid,login.id.ToString()),
                    new Claim(ClaimTypes.Role, login.logingroupid.ToString()),
                    new Claim(ClaimTypes.Email,login.email),
                    new Claim(ClaimTypes.Hash,login.password),
                    new Claim(ClaimTypes.Expiration,DateTime.Now.AddMonths(3).ToString()),
                };
                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                myContext.SignInAsync(userPrincipal);
            }

            var myResponse = standardMessages.loggedin;
            myResponse.data = new { token = GenerateJSONWebToken(login), client = new { login.firstName, login.lastname, login.email, login.logingroupid } };

            return myResponse;
        }
        public string GenerateJSONWebToken(snn_users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clib.appSetting("Token")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor(); // { Subject = new ClaimsIdentity(_claims(userInfo)), SigningCredentials = credentials };           
            tokenDescriptor.Expires = DateTime.Now.AddDays(1);
            tokenDescriptor.Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,user.firstName.ToString()),
                    new Claim(ClaimTypes.Sid,user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.logingroupid.ToString()),
                    new Claim(ClaimTypes.Email,user.email),
                    new Claim(ClaimTypes.Hash,user.password)
                });
            tokenDescriptor.SigningCredentials = credentials;
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private byte[] encryptOnRead(byte[] dataBytes)
        {
            MemoryStream dataStream = new MemoryStream(dataBytes, false);
            MemoryStream encryptedDataStream = new MemoryStream();
            IBufferedCipher inCipher = createCipher(true);
            CipherStream inCipherStream = new CipherStream(dataStream, inCipher, null/* TODO Change to default(_) if this is not a reference type */);
            int ch = inCipherStream.ReadByte();
            while ((ch >= 0))
            {
                encryptedDataStream.WriteByte(System.Convert.ToByte(ch));
                ch = inCipherStream.ReadByte();
            }
            encryptedDataStream.Close();
            inCipherStream.Close();
            byte[] encryptedDataBytes = encryptedDataStream.ToArray();
            return encryptedDataBytes;
        }
        public string hashMe(string password, string mySalt)
        {
            byte[] plainText = Encoding.UTF8.GetBytes(password);
            byte[] salt = Encoding.UTF8.GetBytes(mySalt);
            HashAlgorithm algorithm = new SHA512Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }
            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
        }
        private byte[] decryptOnRead(byte[] encryptedDataBytes)
        {
            MemoryStream encryptedDataStream = new MemoryStream(encryptedDataBytes, false);
            MemoryStream dataStream = new MemoryStream();
            IBufferedCipher inCipher = createCipher(false);
            CipherStream inCipherStream = new CipherStream(encryptedDataStream, inCipher, null/* TODO Change to default(_) if this is not a reference type */);
            int ch = inCipherStream.ReadByte();
            while ((ch >= 0))
            {
                dataStream.WriteByte(System.Convert.ToByte(ch));
                ch = inCipherStream.ReadByte();
            }
            inCipherStream.Close();
            dataStream.Close();
            byte[] dataBytes = dataStream.ToArray();
            return dataBytes;
        }
        /// <summary>
        /// bouncy castle codes
        /// </summary>
        byte[] keyBytes = new byte[33];
        public string staticKey
        {
            get
            {
                if (clib.appSetting("token") != null)
                    return clib.appSetting("token");
                else
                    return "OCXI4utuqsIKWiFavPYM6yQquzPyjprREXzLD0ax2uk=";
            }
        }
        private IBufferedCipher createCipher(bool forEncryption)
        {
            keyBytes = Convert.FromBase64String(staticKey);
            // IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CFB/NoPadding");
            IBlockCipher blockCipher = new AesEngine();
            int bits = 8 * blockCipher.GetBlockSize();
            blockCipher = new CfbBlockCipher(blockCipher, bits);
            IBufferedCipher cipher = new BufferedBlockCipher(blockCipher);
            SecureRandom random = new SecureRandom();

            // //random.NextBytes(keyBytes);
            KeyParameter key = new KeyParameter(keyBytes);

            byte[] iv = new byte[cipher.GetBlockSize() - 1 + 1];
            // random.NextBytes(iv)
            cipher.Init(forEncryption, new ParametersWithIV(key, iv));
            return cipher;
        }
        public string encrypt(string data)
        {
            return Convert.ToBase64String(encryptOnRead(Encoding.ASCII.GetBytes(data)));
        }
    }
}
