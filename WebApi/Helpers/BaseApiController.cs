using System;
using System.Security.Claims;
using System.Web.Http;
using WebApi.Data;
using WebApi.Extensions;
using WebApi.Models;

namespace WebApi.Helpers
{
    public class BaseApiController : ApiController,IDisposable
    {
        protected UnitOfWork Context { get; set; }
        protected UserProfile User {  get; private set; }
        protected string UserRole { get; private set; }
        public BaseApiController()
        {
            Context = new UnitOfWork(new ApplicationDbContext());
            User = new UserProfile();
            User.User = new Models.User();
            User.Id = this.GetClaimsData(ClaimTypes.NameIdentifier);
            User.FirstName = this.GetClaimsData(ClaimTypes.Name);
            User.LastName = this.GetClaimsData(ClaimTypes.Surname);
            UserRole= this.GetClaimsData(ClaimTypes.Role);
            User.User.Id = User.Id;
            User.User.Email = this.GetClaimsData(ClaimTypes.Email);
            User.User.UserName = this.GetClaimsData("UserName");

        }
    }
}