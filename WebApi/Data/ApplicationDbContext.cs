using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<User>
    {

        public DbSet<Task> Tasks {get;set;}
        public DbSet<UserProfile> Profiles { get; set; }
    }
}