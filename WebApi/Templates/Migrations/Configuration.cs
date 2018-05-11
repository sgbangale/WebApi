namespace WebApi.Migrations
{
    using Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApi.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApi.Data.ApplicationDbContext context)
        {

            

            var AdminRole = new IdentityRole { Name = "Admin" };
            context.Roles.AddOrUpdate(p => p.Name, AdminRole);

            var CandidateRole = new IdentityRole { Name = "Candidate" };
            context.Roles.AddOrUpdate(p => p.Name, CandidateRole);
            context.SaveChanges();

            UnitOfWork uWork = new UnitOfWork(context);
            var sgbangale = uWork.Users.RegisterUser("sgbangale@gmail.com", "Suraj G", "Bangale", "password", "Admin");
            var sgbangalea = uWork.Users.RegisterUser("sgbangale@amail.com", "Suraj A", "Bangale", "password", "Candidate");
            var sgbangaleb = uWork.Users.RegisterUser("sgbangale@bmail.com", "Suraj B", "Bangale", "password", "Candidate");

            context.Tasks.AddOrUpdate(p => p.TaskName, new Models.Task { TaskName = "Task 1", Status = Models.TaskStatus.Created, AssignedTo = sgbangale.Item2 });
            context.Tasks.AddOrUpdate(p => p.TaskName, new Models.Task { TaskName = "XTask 1", Status = Models.TaskStatus.Created, AssignedTo = sgbangalea.Item2 });
            context.Tasks.AddOrUpdate(p => p.TaskName, new Models.Task { TaskName = "YTask 1", Status = Models.TaskStatus.Created, AssignedTo = sgbangalea.Item2 });
            context.Tasks.AddOrUpdate(p => p.TaskName, new Models.Task { TaskName = "ZTask 1", Status = Models.TaskStatus.Created, AssignedTo = sgbangaleb.Item2 });
            uWork.Complete();
        }
    }
}
