using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using WebApi.Data;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Net;

namespace WebApi
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            using (var uContext = new UnitOfWork(new ApplicationDbContext()))
            {
                var user = await uContext.Users.AuthenticateUserAsync(context.UserName, context.Password);
                if (user != null)
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("UserName", user.Item1.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Item1.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Item1.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Item1.UserProfile.FirstName));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, user.Item1.UserProfile.LastName));
                    identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.Item2));
                    IDictionary<string, string> data = new Dictionary<string, string>
                    {
                        {"Username", user.Item1.UserName },
                        {"Email", user.Item1.Email },
                        {"FirstName", user.Item1.UserProfile.FirstName },
                        {"LastName", user.Item1.UserProfile.LastName },
                        { "UserRole",user.Item2}
                    };
                    var ticketData = new AuthenticationProperties(data);
                    AuthenticationTicket ticket = new AuthenticationTicket(identity, ticketData);
                    context.Validated(ticket);
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    context.SetError("User name or password is not correct");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
        }
    }
}