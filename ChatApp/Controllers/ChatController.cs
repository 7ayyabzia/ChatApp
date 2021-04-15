using ChatApp.Models;
using ChatApp.Services;
using ChatApp.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class ChatController : Controller
    {
        ChatService _chatService = new ChatService();
        GroupService _groupService = new GroupService();

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var contacts = new ChatContactViewModel
            {
                Users = _chatService.GetUsers(userId),
                Groups = _groupService.GetGroups(userId),
                UserID = userId
            };
            return View(contacts);
        }

        public JsonResult GetUserMessages(string recipientId)
        {
            var messages = _chatService.GetUserMessage(recipientId, User.Identity.GetUserId()); 
            var _messages = JsonConvert.SerializeObject(messages, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Json(_messages);
        }

        public JsonResult GetGroupMessages(int id)
        {
            var messages = _chatService.GetGroupMessage(id);
            var _messages = JsonConvert.SerializeObject(messages, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Json(_messages);
        }

        public JsonResult SendMessage()
        {
            try
            {
                var message = JsonConvert.DeserializeObject<Message>(Request.Form["message"]);
                _chatService.SendMessage(message);

                var _message = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return Json(new { StatusCode = 200, Response = _message });
            }
            catch (Exception Ex)
            {
                return Json(new { StatusCode = 400, Response = Ex.Message });
            }
        }
    }
}