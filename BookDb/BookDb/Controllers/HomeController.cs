﻿using System.Web.Mvc;
using BookDb.Constants;

namespace BookDb.Controllers
{
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [Route("{id:int}")]
        [Route("new")]
        [Route("authors/{*url}")]
        [Route(Name = RouteNames.HomeMvc)]
        public ActionResult Index()
        {
            return View();
        }
    }
}