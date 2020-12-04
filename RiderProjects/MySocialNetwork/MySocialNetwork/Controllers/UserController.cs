using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Web;
using System.Web.Mvc;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Services;
using MySocialNetwork.Filters;

namespace MySocialNetwork.Controllers
{
    public class UserController : Controller
    {
        private UserService userService = new UserService();
        // GET
        
        [Route("~/User/NewUser")]
        public ActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        [SqlExceptionFilter]
        public ActionResult NewUser(RegistrationDto registrationDto)
        {
            if (ModelState.IsValid)
            {
                userService.RegistrateNewUser(registrationDto);
                //return RedirectToAction("AllDone");
            }
            return View();
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginAndPasswordDto loginAndPasswordDto)
        {
            if (ModelState.IsValid)
            {
                UserDto userDto = userService.LogIn(loginAndPasswordDto.Login);
                Session["session"] = userDto;
                return RedirectToAction("PrivatePage", "Page",  new { login = loginAndPasswordDto.Login });
            }
            else
            {
                return View();
            }
        }
        
    }
}