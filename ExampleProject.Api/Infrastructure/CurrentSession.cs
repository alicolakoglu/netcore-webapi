using System.Linq;
using Microsoft.AspNetCore.Http;
using ExampleProject.Core.Domain.Users;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace ExampleProject.Api
{
    public static class CurrentSession
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static User User
        {
            get
            {
                try
                {
                    var headers = _httpContextAccessor.HttpContext.Request.Headers;
                    string tokenString = headers["Authorization"];
                    tokenString = tokenString.Substring(7);
                    var token = new JwtSecurityToken(tokenString);

                    return JsonConvert.DeserializeObject<User>(token.Claims.FirstOrDefault(y => y.Type == "UserData").Value);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Login failed! ({ex.Message})");
                }
            }
        }
    }
}
