using Forum.Controllers.Auntification;
using Forum.Filters;
using Forum.Models;
using Forum.Models.ViewModel;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.WebPages;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        Entities db = new Entities();
        [HttpGet]
        public ActionResult ViewerNameForum()
        {
            List<ForumName> forumNames = db.ForumName.Select(n => n).ToList();
            return View(forumNames);
        }

        [HttpGet]
        public ActionResult ViewerTitleForum(string id)
        {
            int id_row = int.Parse(id);
            List<ForumTitles> forumTitles = db.ForumTitles.Where(n => n.Id_ForumName == id_row).ToList();
            return View(forumTitles);
        }

        [HttpGet]
        public ActionResult ViewerContentForum(string id)
        {
            int id_row = int.Parse(id);
            ForumTitles forumTitle = db.ForumTitles.Where(n => n.Id == id_row).FirstOrDefault();
            List<Users> users = db.Users.Select(n => n).ToList();
            List<ForumContent> listForumContent = db.ForumContent.Where(n => n.Forum_Id == forumTitle.Id).ToList();
            var joinList = listForumContent.Join(users,
                                                t => t.User_Id,
                                                h => h.Id,
                                                 (t, h) => new { Id = t.Id, User_Id = t.User_Id, Message = t.Message, FIO = h.FIO, Forum_Id = t.Forum_Id }).ToList();
            ViewBag.forumTitle = forumTitle.ForumTitle;
            List<ContentListView> contentListViews = new List<ContentListView>();
            foreach (var item in joinList)
            {
                contentListViews.Add(new ContentListView()
                {
                    Id = item.Id,
                    User_Id = item.User_Id,
                    FIO = item.FIO,
                    Message = item.Message,
                    Forum_id = item.Forum_Id
                });
            }
            ViewBag.joinList = contentListViews.Count > 0 ? contentListViews : null;
            return View();
        }

        [AuthorizeRole(Roles = "Администратор, Пользователь, Модератор")]
        public ActionResult JsonEditeContent(int id, string message)
        {
            Entities db = new Entities();
            ForumContent forumContent = db.ForumContent.Where(n => n.Id == id).FirstOrDefault();
            forumContent.Message = message;
            db.Entry(forumContent).State = EntityState.Modified;
            db.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(Roles = "Администратор, Пользователь, Модератор")]
        public ActionResult JsonDeliteContent(int id)
        {
            Entities db = new Entities();
            ForumContent forumContent = db.ForumContent.Where(n => n.Id == id).FirstOrDefault();
            db.Entry(forumContent).State = EntityState.Deleted;
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(Roles = "Администратор, Пользователь, Модератор")]
        public ActionResult JsonAddContent(int id, string message)
        {
            Entities db = new Entities();
            ForumContent forumContent = new ForumContent()
            {
                User_Id = Authentification._Id,
                Message = message,
                Forum_Id = id
            };
            db.ForumContent.Add(forumContent);
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult JsonSearchForumName(string textSearch)
        {
            Entities db = new Entities();
            List<ForumName> forumName = db.ForumName.Where(n => n.Name == textSearch).ToList();
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}