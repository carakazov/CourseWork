using System.Web.Mvc;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Services;

namespace MySocialNetwork.Controllers
{
    public class DialogController : Controller
    {
        private DialogService dialogService = new DialogService();
        public ActionResult OpenDialog(string login)
        {
            UserInfoDto userInfo = dialogService.GetTalker(login);
            return View("Dialog", userInfo);
        }

        public ActionResult ShowMessages(int firstUserId, int secondUserId)
        {
            int wallId = dialogService.GetDialogWallId(firstUserId, secondUserId);
            return RedirectToAction("Wall", "Page", new {wallId = wallId});
        }
    }
}