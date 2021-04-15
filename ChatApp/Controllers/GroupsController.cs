using ChatApp.Models;
using ChatApp.Services;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class GroupsController : Controller
    {
        GroupService _groupService = new GroupService();
        ChatService _chatService = new ChatService();

        [Authorize]
        public JsonResult GetUsers()
        {
            var user = User.Identity.GetUserId();
            var users = _chatService.GetUsers(user);
            return Json(users);
        }

        [Authorize]
        public ActionResult Add()
        {
            ViewBag.Title = "Add Group";
            return View("AddOrUpdate");
        }

        [Authorize]
        public JsonResult AddOrUpdateGroup(FormCollection form)
        {
            try
            {
                var group = JsonConvert.DeserializeObject<Group>(form["group"]);
                var userId = User.Identity.GetUserId();
                _groupService.AddGroup(group, userId);
                return Json(new { StatusCode = 200 });
            }
            catch (Exception Ex)
            {
                return Json(new { StatusCode = 400, Response = Ex.Message });
            }
        }
    }
}