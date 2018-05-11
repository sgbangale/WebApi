using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
    #region Task
    public interface ITaskRepository
    {
        Models.Task Get(long id);
        IEnumerable<Task> TaskPage(Func<Task, bool> predicate);
    }
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context)
        {

        }

        public Task Get(long id)
        {

            return this.Context.Tasks.Include("AssignedTo").FirstOrDefault(x => x.TaskId == id);
        }

        public IEnumerable<Task> TaskPage(Func<Task, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
    #endregion





    #region General Repository
    public interface IRepository<TEntity> where TEntity : class
    {

        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> Search(Func<TEntity, bool> predicate);

        TEntity Remove(TEntity entity);
        Tuple<PagingReturnModel, IEnumerable<TEntity>> Page(Func<TEntity, bool> whereClause, Func<TEntity, IComparable> orderClause, bool isAsc, int pageNumber, int pageSize);

    }
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext Context { get; set; }
        public Repository(ApplicationDbContext context)
        {
            this.Context = context;

        }
        public TEntity Add(TEntity entity)
        {
            return Context.Set<TEntity>().Add(entity);
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            return Context.Set<TEntity>().AddRange(entities);
        }

        public TEntity Remove(TEntity entity)
        {
            return Context.Set<TEntity>().Remove(entity);
        }

        public IEnumerable<TEntity> Search(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public Tuple<PagingReturnModel, IEnumerable<TEntity>> Page(Func<TEntity, bool> whereClause, Func<TEntity, IComparable> orderClause, bool isAsc, int pageNumber, int pageSize)
        {


            int CurrentPage = pageNumber;
            int PageSize = pageSize;
            int TotalCount = 0;
            int TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            IEnumerable<TEntity> Data;

            if (orderClause == null)
            {
                var data = Context.Set<TEntity>().Where(whereClause);
                TotalCount = data.Count();
                Data = data.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            }
            else
            {
                if (isAsc)
                {
                    var data = Context.Set<TEntity>().Where(whereClause);
                    TotalCount = data.Count();
                    Data = data.OrderBy(orderClause).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                }
                else
                {
                    var data = Context.Set<TEntity>().Where(whereClause);
                    TotalCount = data.Count();
                    Data = data.OrderByDescending(orderClause).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                }
            }
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);



            // Object which we are going to send in header   
            var paginationMetadata = new PagingReturnModel
            {
                TotalCount = TotalCount,
                PageSize = PageSize,
                CurrentPage = CurrentPage,
                TotalPages = TotalPages,
                PreviousPage = CurrentPage > 1, // if CurrentPage is greater than 1 means it has previousPage  
                NextPage = CurrentPage < TotalPages // if TotalPages is greater than CurrentPage means it has nextPage  
            };


            return new Tuple<PagingReturnModel, IEnumerable<TEntity>>(paginationMetadata, Data);
        }
    }
    #endregion
    #region User
    public interface IUserRepository
    {
        Tuple<User, UserProfile> RegisterUser(string email, string firstName, string lastName, string password, string role);
        Tuple<User, UserProfile, IdentityResult> ChangePassword(string email, string oldPassword, string newPassword);
        System.Threading.Tasks.Task<Tuple<User, string>> AuthenticateUserAsync(string email, string Password);
        Tuple<User, UserProfile> FindUser(string email);
    }

    public class UserRepository : IUserRepository
    {
        protected ApplicationDbContext Context { get; set; }
        private UserStore<Models.User> UserStore { get; set; }
        private UserManager<Models.User> UserManager { get; set; }

        public UserRepository(ApplicationDbContext context)
        {
            Context = context;
            UserStore = new UserStore<Models.User>(Context);
            UserStore.AutoSaveChanges = false;
            UserManager = new UserManager<Models.User>(UserStore);

        }

        public Tuple<PagingReturnModel, IEnumerable<UserModel>> Page(Func<UserProfile, bool> whereClause, Func<UserProfile, IComparable> orderClause, bool isAsc, int pageNumber, int pageSize)
        {

            int CurrentPage = pageNumber;
            int PageSize = pageSize;
            int TotalCount = 0;
            int TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            IEnumerable<UserModel> Data;
            var roleData = Context.Roles.ToList();
            if (orderClause == null)
            {
                var data = Context.Set<UserProfile>().Where(whereClause);
                TotalCount = data.Count();
                Data = data.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList().Select(x => new UserModel { FirstName = x.FirstName, Id = x.Id, LastName = x.LastName, UserName = x.User.UserName, Role = roleData.First(y=>y.Id == x.User.Roles.FirstOrDefault().RoleId).Name, RoleId = x.User.Roles.FirstOrDefault().RoleId }); 

            }
            else
            {
                if (isAsc)
                {
                    var data = Context.Set<UserProfile>().Where(whereClause);
                    TotalCount = data.Count();
                    Data = data.OrderBy(orderClause).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList().Select(x => new UserModel { FirstName = x.FirstName, Id = x.Id, LastName = x.LastName, UserName = x.User.UserName, Role = roleData.First(y => y.Id == x.User.Roles.FirstOrDefault().RoleId).Name, RoleId = x.User.Roles.FirstOrDefault().RoleId });
                }
                else
                {
                    var data = Context.Set<UserProfile>().Where(whereClause);
                    TotalCount = data.Count();
                    Data = data.OrderByDescending(orderClause).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList().Select(x => new UserModel { FirstName = x.FirstName, Id = x.Id, LastName = x.LastName, UserName = x.User.UserName, Role = roleData.First(y => y.Id == x.User.Roles.FirstOrDefault().RoleId).Name, RoleId = x.User.Roles.FirstOrDefault().RoleId });
                }
            }
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);



            // Object which we are going to send in header   
            var paginationMetadata = new PagingReturnModel
            {
                TotalCount = TotalCount,
                PageSize = PageSize,
                CurrentPage = CurrentPage,
                TotalPages = TotalPages,
                PreviousPage = CurrentPage > 1, // if CurrentPage is greater than 1 means it has previousPage  
                NextPage = CurrentPage < TotalPages // if TotalPages is greater than CurrentPage means it has nextPage  
            };


            return new Tuple<PagingReturnModel, IEnumerable<UserModel>>(paginationMetadata, Data);
        }

        public Tuple<User, UserProfile> RegisterUser(string email, string firstName, string lastName, string password, string roleName)
        {
            if (UserManager.FindByEmail(email) == null)
            {
                var role = Context.Roles.First(x => x.Name == roleName);
                var user = new User { Email = email, UserName = email };
                user.UserProfile = new UserProfile { FirstName = firstName, LastName = lastName };
                UserManager.Create(user, password);
                UserManager.AddToRole(user.Id, role.Name);               
                return new Tuple<User, UserProfile>(user, user.UserProfile);
            }
            else
            {
                return null;
            }
        }

        public Tuple<User, UserProfile, IdentityResult> ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = UserManager.FindByEmail(email);

            if (user != null)
            {
                var result = UserManager.ChangePassword(user.Id, oldPassword, newPassword);
                UserProfile uProfile = Context.Profiles.First(x => x.Id == user.Id);
                return new Tuple<User, UserProfile, IdentityResult>(user, uProfile, result);
            }
            else
            {
                return null;
            }
        }

        public Tuple<User, UserProfile> FindUser(string email)
        {
            var user = UserManager.FindByEmail(email);

            if (user != null)
            {
                UserProfile uProfile = Context.Profiles.First(x => x.Id == user.Id);
                return new Tuple<User, UserProfile>(user, uProfile);
            }
            else
            {
                return null;
            }
        }

        public async System.Threading.Tasks.Task<Tuple<User, string>> AuthenticateUserAsync(string email, string Password)
        {

            var user = await UserManager.FindAsync(email, Password);

            if (user == null)
            {
                return null;
            }
            else
            {
                var roles = UserManager.GetRoles(user.Id);
                return new Tuple<User, string>(user, roles.First());
            }
        }

    }
    #endregion




}