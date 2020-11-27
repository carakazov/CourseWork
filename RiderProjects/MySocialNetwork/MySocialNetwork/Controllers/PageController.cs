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

     
        public ActionResult StartFindUsers(FindUsersDto userInfo)
        {
            List<AuthorDto> users = userService.FindUsers(userInfo);
            return RedirectToAction("ShowFoundedUsers", "Page", new { foundedUsers = users });
        }
        
        public ActionResult AllDone(FindUsersDto test)
        {
            return View(test);
        }
        
        public ActionResult ShowFoundedUsers(IEnumerable<AuthorDto> foundedUsers)
        {
            return PartialView("FoundedUsersWall", foundedUsers);
        }
    }
}