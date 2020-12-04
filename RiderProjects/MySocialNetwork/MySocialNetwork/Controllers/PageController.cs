using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Models.SocialNetwork;
using MySocialNetwork.Services;

namespace MySocialNetwork.Controllers
{
    public class PageController : Controller
    {
        private PostService postService = new PostService();
        private UserService userService = new UserService();
        private PageService pageService = new PageService();
        private FriendshipService friendshipService = new FriendshipService();

        public ActionResult PrivatePage(string login)
        {
            UserDto userDto = userService.LogIn(login);
            return View(userDto);
        }

        public ActionResult Wall(int wallId)
        {
            WallDto wallDto = pageService.GetWallDto(wallId);
            return PartialView(wallDto);
        }

        public ActionResult Test()
        {
            return View("PostingForm");
        }

        public ActionResult PostingForm(int wallId)
        {
            InputPostDto newPost = new InputPostDto();
            newPost.WallId = wallId;
            return PartialView("PostingForm", newPost);
        }

        public ActionResult Post(InputPostDto postDto, int wallId)
        {
            postDto.AuthorId = ((UserDto) Session["session"]).Id;
            postDto.WallId = wallId;
            postService.Post(postDto);
            return RedirectToAction("Wall", new {wallId = wallId});
        }

        public ActionResult ScorePost(int postId, int wallId, ScoreTypes type, int userId)
        {
            postService.ScorePost(postId, type, userId);
            UserDto currentUser = (UserDto) Session["session"];
            userService.UpdateScoredPostList(currentUser);
            Session["session"] = currentUser;
            return RedirectToAction("Wall", new {wallId = wallId});
        }

        public ActionResult FindUsers()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult SendRequest(string firstName, string secondName, string middleName, int minAge, int maxAge, int senderId, int receiverId, DateTime sendingDate)
        {
            friendshipService.SendRequest(senderId, receiverId, sendingDate);
            UserDto currentUser = (UserDto) Session["session"];
            Session["session"] = userService.LogIn(currentUser.Login);
            return RedirectToAction("StartFindUsersFromPartial", new {firstName = firstName, secondName = secondName, middleName = middleName, minAge = minAge, maxAge = maxAge});
        }

        public ActionResult StartFindUsersFromPartial(string firstName, string secondName, string middleName, int minAge,
            int maxAge)
        {
            FindUsersDto userInfo = new FindUsersDto()
            {
                FirstName = firstName,
                SecondName = secondName,
                MiddleName = middleName,
                MinAge = minAge,
                MaxAge = maxAge
            };
            List<UserInfoDto> users = userService.FindUsers(userInfo);
            FoundedUsersDto foundedUsers = new FoundedUsersDto(userInfo, users);
            return PartialView("FoundedUsersWall", foundedUsers);
        }
        
        public ActionResult StartFindUsers(FindUsersDto userInfo)
        {
            if (ModelState.IsValid)
            {
                List<UserInfoDto> users = userService.FindUsers(userInfo);
                FoundedUsersDto foundedUsers = new FoundedUsersDto(userInfo, users);
                return PartialView("FoundedUsersWall", foundedUsers);   
            }

            return RedirectToAction("FindUsers");
        }
        
        public ActionResult ShowReceivedRequests()
        {
            UserDto currentUser = (UserDto) Session["session"];
            List<UserInfoDto> senders = userService.GetReceivedRequests(currentUser);
            return PartialView("ReceivedRequests", senders);
        }
        
    }
}