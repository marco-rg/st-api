using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Web;
using System.Security.Claims;

namespace ST.Controllers
{
    public class TokenGenerator
    {
        public static string GenerateTokenJwt(string username, string password, string permissions, string companyList)
        {
            // appsetting for Token JWT
            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];//Obviamente es el parámetro de configuración más importante, ya que será la Clave que utilizaremos tanto para firmar digitalmente el Token al enviarlo, como para comprobar la validez de la firma al recibirlo.
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];// Debe indicar la audiencia o destinatario a los que se dirige el Token. En nuestro caso indicaremos la URL de nuestro Web API.
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"]; //Debe indicar quien es un emisor válido para el Token. Normalmente indicaremos el Dominio desde el cual se emite el Token.
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            //save permissions and companyList on userSystem
            using (var dbContextToken = new Models.ModelHealthAdvisor())
            {
                var user = dbContextToken.UserSystem.Where(t => t.usrActive == true && t.IsDeleted != true).FirstOrDefault(p => p.usrLogon.Equals(username));
                if (user != null)
                {
                    user.usrPermissions = permissions;
                    user.usrCompanyList = companyList;
                    dbContextToken.SaveChanges();
                }
                else
                {
                    return null;
                    //var refreshTokenId = Guid.NewGuid().ToString("n");
                    //var newUser = new Models.UserSystem
                    //{
                    //    usrLogon = username,
                    //    usrPermissions = permissions,
                    //    usrCompanyList = companyList,
                    //    usrFirstName = "" ,
                    //    usrLastName = "",
                    //    usrEmail = "",
                    //    usrRefreshToken = GetHash(refreshTokenId)
                    //};

                    //dbContextToken.UserSystem.Add(newUser);
                    //dbContextToken.SaveChanges();
                }

            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
          {
                new Claim("UserName", username),
                  new Claim("Password", password)
                 //new Claim("Permissions", permissions)
            });

            // create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);

            //desencriptar token
            //var tokenS = tokenHandler.ReadJwtToken(jwtTokenString);
            //var jti = tokenS.Claims.FirstOrDefault(x => x.Type == "Permissions")?.Value ?? "";

            return jwtTokenString;
        }

        public static string GetHash(string input)
        {
            System.Security.Cryptography.HashAlgorithm hashAlgorithm = new System.Security.Cryptography.SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string GenerateRefreshTokenJwt(string _userName, string _appVersion)
        {
            using (var dbContextToken = new Models.ModelHealthAdvisor())
            {

                var userSystemTMP = dbContextToken.UserSystem.Where(r => r.usrLogon == _userName).SingleOrDefault();
                if (userSystemTMP != null)
                {
                    if (userSystemTMP.usrRefreshToken == null)
                    {
                        var refreshTokenId = Guid.NewGuid().ToString("n");
                        userSystemTMP.usrRefreshToken = GetHash(refreshTokenId);
                        if (!String.IsNullOrEmpty(_appVersion))
                        {
                            userSystemTMP.usrAppVersion = _appVersion;
                        }
                        dbContextToken.SaveChanges();

                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(_appVersion))
                        {
                            userSystemTMP.usrAppVersion = _appVersion;
                            dbContextToken.SaveChanges();
                        }
                    }
                }
                return userSystemTMP.usrRefreshToken;

            }

        }


        /*public static bool RemoveRefreshToken(string refreshTokenId)
        {
            using (var dbContextToken = new Models.ModelHealthAdvisor())
            {

                var refreshToken = dbContextToken.RefreshTokens.Find(refreshTokenId);

                if (refreshToken != null)
                {
                    dbContextToken.RefreshTokens.Remove(refreshToken);
                    return dbContextToken.SaveChanges() > 0;
                }
            }

            return false;
        }*/


        public static bool HasPermissions(System.Net.Http.HttpRequestMessage request, string _option, string _action)
        {
            var re = request;
            var headers = re.Headers;
            string token = "";
            if (headers.Contains("Authorization"))
            {
                token = headers.GetValues("Authorization").First();
                token = token.Replace("Bearer ", "");
                if (token.Length > 0)
                {
                    //desencriptar token
                    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var tokenS = tokenHandler.ReadJwtToken(token);
                    var userName = tokenS.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? "";//UserName
                    if (userName != "")
                    {
                        using (var dbContextToken = new Models.ModelHealthAdvisor())
                        {
                            var user = dbContextToken.UserExternal.First(p => p.Username == userName);
                            if (user != null)
                            {
                                Models.Result permissionsList = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Result>(user.RefreshToken);//usrPermissions

                                var query1 = permissionsList.Modules
                                                        .Where(p => p.Name == _option)
                                                        .Select(q => q.Actions)
                                                        .FirstOrDefault();
                                if (query1 == null)
                                    return false;
                                else
                                {
                                    var query2 = query1
                                                        .Where(p => p.Name == _action)
                                                        .FirstOrDefault()
                                                        ;
                                    if (query2 == null)
                                        return false;
                                    else
                                        return true;
                                }

                            }
                            else
                                return false;

                        }



                    }
                    else
                        return false;

                }
                else
                {
                    return false;
                }



            }
            else
            {
                return false;
            }

        }


        public static string GetUserSystem(System.Net.Http.HttpRequestMessage request)
        {
            var re = request;
            var headers = re.Headers;
            string token = "";
            if (headers.Contains("Authorization"))
            {
                token = headers.GetValues("Authorization").First();
                token = token.Replace("Bearer ", "");
                if (token.Length > 0)
                {
                    //desencriptar token
                    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var tokenS = tokenHandler.ReadJwtToken(token);
                    var usuario = tokenS.Claims.FirstOrDefault(x => x.Type == "email")?.Value ?? "";//UserName
                    return usuario;
                }
                else
                {

                    return null;
                }



            }
            else
            {
                return null;
            }

        }


        public static string GetFullNameUser(string _userName)
        {
            using (var dbContextToken = new Models.ModelHealthAdvisor())
            {

                var userSystemTMP = dbContextToken.UserSystem.Where(r => r.usrLogon == _userName).SingleOrDefault();
                if (userSystemTMP != null)
                {
                    return userSystemTMP.usrFirstName + " " + userSystemTMP.usrLastName;
                }
                else
                {
                    return null;
                }


            }

        }



        public static DateTime convertToTimeZoneEcuador(DateTime _dateTime)
        {
            TimeZone curTimeZone = TimeZone.CurrentTimeZone;
            DateTime curUTC = curTimeZone.ToUniversalTime(_dateTime);
            TimeZoneInfo est = null;
            try
            {
                //Ecuador TimeZone
                est = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {


            }
            catch (InvalidTimeZoneException)
            {


            }
            DateTime targetTime = TimeZoneInfo.ConvertTime(curUTC, est);
            return targetTime;
        }

        public static Models.LoginRequest GetTokenDeserealized(System.Net.Http.HttpRequestMessage request)
        {
            Models.LoginRequest objResult = null;
            var re = request;
            var headers = re.Headers;
            string token = "";
            if (headers.Contains("Authorization"))
            {
                token = headers.GetValues("Authorization").First();
                token = token.Replace("Bearer ", "");
                if (token.Length > 0)
                {
                    objResult = new Models.LoginRequest();
                    //desencriptar token
                    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var tokenS = tokenHandler.ReadJwtToken(token);
                    objResult.Username = tokenS.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? "";
                    objResult.Password = tokenS.Claims.FirstOrDefault(x => x.Type == "Password")?.Value ?? "";
                    Models.HALogin permissionsList = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.HALogin>(tokenS.Claims.FirstOrDefault(x => x.Type == "Permissions")?.Value ?? "");
                    objResult.Permissions = permissionsList;

                    return objResult;
                }
                else
                {

                    return null;
                }



            }
            else
            {
                return null;
            }

        }
    }
}