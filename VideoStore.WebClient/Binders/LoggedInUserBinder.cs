using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoStore.Services.MessageTypes;

namespace VideoStore.WebClient
{
    public class LoggedInUserBinder : IModelBinder
    {
        private const String UserSessionKey = "_loggedInUser";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                throw new InvalidOperationException("Cannot update instances");

            UserCache lUserCache = controllerContext.HttpContext.Session[UserSessionKey] as UserCache;

            return lUserCache;
        }


        public static void BindUser(HttpSessionStateBase pSession, String pUserName, String pPassword)
        {
            pSession[LoggedInUserBinder.UserSessionKey] = new UserCache(
                ServiceFactory.Instance.UserService.GetUserByUserNamePassword(pUserName, pPassword));
        }

        public static void ClearUser(HttpSessionStateBase pSession)
        {
            pSession[LoggedInUserBinder.UserSessionKey] = null;
            CartModelBinder.ClearCartBinder(pSession);
        }
    }
    public class UserCache
    {
        public User Model
        {
            get;
            set;
        }

        public UserCache(User pModel)
        {
            Model = pModel;
        }

        public void UpdateUserCache()
        {
            Model = ServiceFactory.Instance.UserService.ReadUserById(Model.Id);
        }
    }
}