using HalloDoc.BussinessAccess.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MimeKit.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HalloDoc.Services
{
    [AttributeUsage(AttributeTargets.All)]
    public class CustomAuth : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public CustomAuth(string role = "")
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>(); 

            if (jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "login" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            //Redirect To Login if not login
            if(token == null || !jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "login" }));
                return;
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

            if(roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "login" }));
                return;
            }

            //Redirect to AccessDenied only if roles mismatch
            if(string.IsNullOrWhiteSpace(_role) || roleClaim.Value != _role)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "AccessDenied" }));
            }

        }
    }
}
