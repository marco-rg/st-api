using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ST.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace ST.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }
        [HttpGet]
        [Route("connectionTesting")]
        public IHttpActionResult connectionTesting()
        {
            return Ok(true);
        }
        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        /// <summary>
        /// Devuelve el access token y refresh token de AQUASYM así como también el listado de 
        /// compañias a las que tiene perm
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Enter your username and password");

            try
            {
                using (var context = new ModelHealthAdvisor())
                {
                    var exists = context.UserExternal.FirstOrDefault(n => n.Username == login.Username);// && n.PassWord == login.Password
                    if (exists!=null) {
                        login.Password = exists.PassWord;
                        login.FullName = exists.FullName;
                        return Ok(CreateToken(login, exists.ProfileID));
                    }

                    return Content(HttpStatusCode.BadRequest, "user not migrated to DbCell.");
                }
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        private LoginRequest CreateToken(LoginRequest user, long? profile)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, user.Username)//ClaimTypes.Email
            });

            string secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = (JwtSecurityToken)tokenHandler.CreateJwtSecurityToken(
                    subject: claims,
                    signingCredentials: signinCredentials
                );

            var tokenString = tokenHandler.WriteToken(token);
            var permissions=new Profiles();
            using (var context = new ModelHealthAdvisor())
            {
                permissions = context.Profiles.FirstOrDefault(x=>x.ProfilesID==profile);
            }
            return new LoginRequest()
            {
                FullName = user.FullName,
                Username = user.Username,
                Password = user.Password,
                TokenHealthAdvisor = tokenString,
                RefreshTokenHealthAdvisor = tokenString,
                Permissions = JsonConvert.DeserializeObject<HALogin>(permissions.Permissions)
            };
        }
    }
}
