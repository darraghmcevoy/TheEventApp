using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheEventApp.Models
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db;
        public BaseController()
        {
            if (db == null)
                db = new ApplicationDbContext();
        }
        public string GetApplicationUserId
        {
            get
            {
                return System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
        }

        public ApplicationUser GetApplicationUser
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            }
        }

        public List<string> GetApplicationUserRoles
        {
            get
            {
                var appRoles = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var roles = appRoles.GetRoles(System.Web.HttpContext.Current.User.Identity.GetUserId());
                return roles.ToList();
            }
        }

        public bool IsAdmin
        {
            get
            {
                var roles = GetApplicationUserRoles;
                return roles.Where(p => p == "Admin").FirstOrDefault() != null ? true : false;
            }
        }
    }
}