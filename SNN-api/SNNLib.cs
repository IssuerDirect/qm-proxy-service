﻿using System;
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

namespace snn
{
    public class SNNLib
    {
        lib clib = new lib();
        public platformDB platformDB;
        public companyHubDB companyhubDB;
        public peopleHubDB peopleHubDB;
        public IConfiguration config {
            set {
                clib.myConfiguration = value;
            }
            get {
                return clib.myConfiguration;
            }
        }
        apiResponse myResponse = new apiResponse();
        public loginUsers user;
        public apiResponse logMeIn(Dictionary<string, string> inputUser, HttpContext myContext = null)
        {
            var possibleUsers = peopleHubDB.loginUsers.Where(u => u.email == inputUser["email"]);
            if (inputUser.ContainsKey("logingroupid"))
            {
                possibleUsers = possibleUsers.Where(u => u.logingroupid == inputUser["logingroupid"]);
            }
            
            var allmatching = possibleUsers.ToList();
            user = allmatching.Where(u => u.password == inputUser["password"] || u.password == clib.encrypt(inputUser["password"])).FirstOrDefault();
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
        public apiResponse loginResponse(loginUsers login, HttpContext myContext = null)
        {
            if (myContext != null )
            {
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,login.firstName.ToString()),
                    new Claim(ClaimTypes.Sid,login.id.ToString()),
                    new Claim(ClaimTypes.Role, login.logingroupid.ToString()),
                    new Claim(ClaimTypes.Email,login.email),
                    new Claim(ClaimTypes.Expiration,DateTime.Now.AddHours(12).ToString()),
                };
                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                myContext.SignInAsync(userPrincipal);
            }

            var myResponse = standardMessages.loggedin;
            myResponse.data = loginRedirect(myContext);

            return myResponse;
        }
        public string loginRedirect(HttpContext myContext = null)
        {
            if (myContext != null && myContext.Request.Query.ContainsKey("return"))
            {
                return myContext.Request.Query["return"];
            }
            else
            {
                return "/admin/insights";
            }
        }
        public string GenerateJSONWebToken(loginUsers user)
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
                    new Claim(ClaimTypes.Email,user.email)
                });
            tokenDescriptor.SigningCredentials = credentials;
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public loginUsers myUser(ClaimsPrincipal myUser)
        {
            if (user != null) { return user; }
            if (myUser.FindFirst(ClaimTypes.Sid) != null)
            {
                int userID = int.Parse(myUser.FindFirst(ClaimTypes.Sid)?.Value);
                user = new loginUsers();
                user.id = userID;
                user.firstName = myUser.FindFirst(ClaimTypes.Name)?.Value;
                user.logingroupid = myUser.FindFirst(ClaimTypes.Role)?.Value;
                user.email = myUser.FindFirst(ClaimTypes.Email)?.Value;
            }
            return user;
        }
    }

    public class ckeditorResponse
    {
        public int uploaded { get; set; }
        public string fileName { get; set; }
        public string url { get; set; }
    }
}
