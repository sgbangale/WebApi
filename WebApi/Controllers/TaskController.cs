﻿using System.Linq;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Http;
using WebApi.Helpers;
using WebApi.Models;
namespace WebApi.Controllers
{
    [Authorize(Roles ="Admin,Candidate")]
    public class TaskController : BaseApiController
    {

        [HttpGet]
        public IHttpActionResult Get([FromUri] PagingParameterModel pagingModel)
        {
            try
            {
                Func<Task, bool> whereClause = c => 1 == 1;
                Func<Task, IComparable> orderClause = c => c.TaskName;
                var result = Context.Tasks.Page(whereClause, orderClause, true, pagingModel.pageNumber, pagingModel.pageSize);
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(result.Item1));
                return Ok(result.Item2);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }


        [HttpGet]
        public IHttpActionResult Get(long id)
        {
            try
            {
                var result = Context.Tasks.Get(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]Task obj)
        {
            try
            {

                var result = Context.Tasks.Add(obj);
                Context.Complete();
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Put(long id, [FromBody]Task obj)
        {
            try
            {
                var data = Context.Tasks.Get(id);
                if (data != null)
                {
                    data = obj;
                    Context.Complete();
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            try
            {
                var data = Context.Tasks.Get(id);
                if (data != null)
                {
                    Context.Tasks.Remove(data);
                    Context.Complete();
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

    }
}

