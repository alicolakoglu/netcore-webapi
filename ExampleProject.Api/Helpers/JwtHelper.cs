using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ExampleProject.Core.Domain.Users;

namespace ExampleProject.Api.Helpers
{
    public class JwtHelper
    {
        public static string Issuer => "Test Issuer";
        public static string Audience => "Test Audience";
        public static string SecurityKey => "Custom Test Security Key";

        public static string GenerateToken(User user)
        {
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sid, user.Key.ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.NameId, Guid.NewGuid().ToString()));

            if (user.IsAdministrator)
                userClaims.Add(new Claim("Administrator", "1"));

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: userClaims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            token.Payload["UserData"] = JsonConvert.SerializeObject(user);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
