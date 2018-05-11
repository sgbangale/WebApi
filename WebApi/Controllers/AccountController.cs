using System.Linq;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Http;
using WebApi.Helpers;
using WebApi.Models;
namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : BaseApiController
    {  
        [HttpGet]
        public IHttpActionResult Get([FromUri] PagingParameterModel pagingModel)
        {
            try
            {
                Func<UserProfile, bool> whereClause = c => true;
                Func<UserProfile, IComparable> orderClause = c => c.FirstName;
                var result = Context.Users.Page(whereClause, orderClause, true, pagingModel.pageNumber, pagingModel.pageSize);
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(result.Item1));
                return Ok(result.Item2);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

      
        [HttpGet]
        public IHttpActionResult Get(string email)
        {
            try
            {
                var result = Context.Users.FindUser(email);
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult Post(UserRegisterModel user)
        {
            try
            {

                var result = Context.Users.RegisterUser(user.email, user.firstName, user.lastName, user.password, user.roleName);
                Context.Complete();
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Put(string email, [FromBody]User obj)
        {
            //try
            //{
            //    var data = Context.Users.FindUser(email);
            //    if (data != null)
            //    {
            //        data. = obj;
            //        Context.Complete();
            //        return Ok(data);
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }

            //}
            //catch (Exception e)
            //{
            //    return InternalServerError(e);
            //}
            return NotFound();
        }

        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            //try
            //{
            //    var data = Context.Users.Get(id);
            //    if (data != null)
            //    {
            //        Context.Users.Remove(data);
            //        Context.Complete();
            //        return Ok(data);
            //    }
            //    else
            //    {
            //        return NotFound();
            //    }

            //}
            //catch (Exception e)
            //{
            //    return InternalServerError(e);
            //}

            return NotFound();
        }

    }
}

