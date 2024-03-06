using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IJwtService
    {
        public string GenerateJwtToken(LoggedUser user);

        public bool ValidateToken(string  token, out JwtSecurityToken jwtSecurityToken);
    }
}
