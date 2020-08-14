using Forum.Controllers.Auntification;
using Forum.Filters;
using Forum.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using WebMatrix.WebData;

namespace Forum.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users model)
        {
            if (ModelState.IsValid)
            {
                new Authentification(model.LogIn, model.Password);
                bool IsValidUser = Authentification.Status;
                if (IsValidUser)
                {
                    return RedirectToAction("ViewerNameForum", "Home");
                }
            }
            ModelState.AddModelError("", "Не правильно введен логин или пароль, попробуйте снова!");
            return View();
        }

        public ActionResult LogOut()
        {
            Authentification.ExitSession();
            return RedirectToAction("ViewerNameForum", "Home");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            RegistrUserModel registrUserModel = new RegistrUserModel();
            ViewBag.ListRole = registrUserModel.GetListRole();
            return View(registrUserModel);
        }

      
        [HttpPost]
        public ActionResult Registration(RegistrUserModel model, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                bool IsCompareLogin = Authentification.NoMatches(model.LogIn);
                if (IsCompareLogin)
                {
                    Authentification.Registration(model);
                    return RedirectToAction("ViewerNameForum", "Home");
                }
            }
            return View();
        }
    }
}