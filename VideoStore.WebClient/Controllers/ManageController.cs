using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using VideoStore.WebClient.ViewModels;

namespace VideoStore.WebClient.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ActionResult Index()
        {
            return RedirectToAction("UpdateUser", "Manage");
        }

        public ActionResult UpdateUser(UserCache pUserCache)
        {
            return View(new EditUserDetailsViewModel(pUserCache.Model));
        }

        [HttpPost]
        public ActionResult UpdateUser(EditUserDetailsViewModel pUserViewModel, UserCache pUserCache)
        {
            bool successfulUpdate = true;
            try
            {
                ServiceFactory.Instance.UserService.UpdateUser(pUserViewModel.UpdateMessageType(pUserCache.Model));
            }
            catch(Exception e)
            {
                successfulUpdate = false;
            }
            finally
            {
                pUserCache.UpdateUserCache();
            }
            if (!successfulUpdate)
            {
                return RedirectToAction("ErrorUpdatingUserDetails");
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
        }


        public ActionResult ErrorUpdatingUserDetails()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

    }
}