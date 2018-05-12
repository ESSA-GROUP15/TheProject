using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using VideoStore.Services.MessageTypes;
using VideoStore.WebClient.ClientModels;
using VideoStore.WebClient.ViewModels;

namespace VideoStore.WebClient.Controllers
{
    public class CartController : Controller
    {
        public ViewResult Index(Cart pCart, string pReturnUrl)
        {
            ViewData["returnUrl"] = pReturnUrl;
            ViewData["CurrentCategory"] = "Cart";
            return View(pCart);
        }

        public RedirectToRouteResult AddToCart(Cart pCart, int pMediaId, string pReturnUrl)
        {
            pCart.AddItem(FetchMediaById(pMediaId), 1);
            return RedirectToAction("Index", new { pReturnUrl });
        }


        public RedirectToRouteResult RemoveFromCart(Cart pCart, int pMediaId, string pReturnUrl)
        {
            pCart.RemoveLine(FetchMediaById(pMediaId));
            return RedirectToAction("Index", new { pReturnUrl });
        }

        public ActionResult CheckOut(Cart pCart, UserCache pUser)
        {
            try
            {
                pCart.SubmitOrderAndClearCart(pUser);
            }
            catch(Exception e)
            {
                pCart.Clear();
                pUser.UpdateUserCache();
                return RedirectToAction("ErrorPage");
            }
            return View(new CheckOutViewModel(pUser.Model));
        }

        public ActionResult ErrorPage()
        {
            return View();
        }

        public ViewResult Summary(Cart pCart)
        {
            return View(pCart);
        }

        public ActionResult InsufficientStock(String pItem)
        {
            return View(new InsufficientStockViewModel(pItem));
        }

        private Media FetchMediaById(int pId)
        {
            return ServiceFactory.Instance.CatalogueService.GetMediaById(pId);
        }
    }
}