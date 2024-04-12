using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MimeKit.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HalloDoc.Services
{
    [AttributeUsage(AttributeTargets.All)]
    public class RoleAuth : Attribute, IAuthorizationFilter
    {
        private readonly int _menuId;
        private readonly ApplicationDbContext _db;

        public RoleAuth(int menuId = 0)
        {
            _menuId = menuId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();

            if (jwtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "Index" }));
                return;
            }

            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            //Redirect To Login if not login
            if (token == null || !jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "Index" }));
                return;
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

            if (roleClaim == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "Index" }));
                return;
            }

            string RoleIdString = jwtToken.Claims.First(c => c.Type == "RoleId").Value;
            int RoleId = int.Parse(RoleIdString);

            var menuList = context.HttpContext.Request.Cookies["menuList"];
            var menus = menuList.Split(",").ToList();
            bool flag = false;
            foreach (var menu in menus)
            {
                if (menu == _menuId.ToString())
                {
                    flag = true;
                }
            }

            if (!flag)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "AccessDenied" }));
            }
        }
    }
}
