using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace WebApi.Extensions
{
    public static class ApiExtension
    {
        
        public static string GetClaimsData(this ApiController controllerObjs, string claimsType)
        {
            var claims = (HttpContext.Current.GetOwinContext()).Authentication.User.Claims.ToList();
            var returnValue = claims.Where(x => x.Type.Equals(claimsType.ToString(), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (returnValue != null)
            {
                return returnValue.Value;
            }
            return string.Empty;

        }

    }
}