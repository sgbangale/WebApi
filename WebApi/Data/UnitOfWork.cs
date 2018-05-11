using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Data.Repositories;
using WebApi.Models;

namespace WebApi.Data
{

    public interface IUnitOfWork 
    {
        void Complete();
    }

    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        ApplicationDbContext Context { get; set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
            this.Tasks = new TaskRepository(context);
            Users = new UserRepository(context);
        }

        public TaskRepository Tasks { get; set; }
        public UserRepository Users { get; set; }

        public void Complete()
        {
            this.Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}