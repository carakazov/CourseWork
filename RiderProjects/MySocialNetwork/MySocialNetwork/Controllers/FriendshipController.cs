using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MySocialNetwork.DTO;
using MySocialNetwork.Services;

namespace MySocialNetwork.Controllers
{
    public class FriendshipController : Controller
    {
        private FriendshipService friendshipService = new FriendshipService();
        private UserService userService = new UserService();
        public ActionResult StartFriendship(int userId, int friendId)
        {
            friendshipService.StartNewFriendship(userId, friendId);
            UserDto user = (UserDto) Session["session"];
            Session["session"] = userService.LogIn(user.Login);
            return RedirectToAction("ShowReceivedRequests", "Page");
        }

        public ActionResult DeclineRequest(int userId, int senderId)
        {
            friendshipService.DeleteRequest(senderId, userId);
            UserDto user = (UserDto) Session["session"];
            Session["session"] = userService.LogIn(user.Login);
            return RedirectToAction("ShowReceivedRequests", "Page");
        }

        public ActionResult FriendList()
        {
            UserDto currentUser = (UserDto) Session["session"];
            return View("FriendList", currentUser.Friends);
        }
    }
}