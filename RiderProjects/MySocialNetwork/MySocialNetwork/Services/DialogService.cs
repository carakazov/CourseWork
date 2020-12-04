using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Utils;

namespace MySocialNetwork.Services
{
    public class DialogService
    {
        private DialogMapper dialogMapper = new DialogMapper();
        private Mapper mapper = new Mapper();
        private DialogManager dialogManager = new DialogManager();
        private UserManager userManager = new UserManager();
        
        public int GetDialogWallId(int firstUserId, int secondUserId)
        {
            int wallId;
            try
            {
                if (firstUserId < secondUserId)
                {
                    wallId = dialogManager.OpenDialog(firstUserId, secondUserId).Id;
                }
                else
                {
                    wallId = dialogManager.OpenDialog(secondUserId, firstUserId).Id;
                }
            }
            catch
            {
                throw;
            }
            return wallId;
        }

        public UserInfoDto GetTalker(string talkerLogin)
        {
            User user = userManager.GetUserByLogin(talkerLogin);
            UserInfoDto userInfo = mapper.FromUserAuthorDto(user);
            return userInfo;
        }
        
    }
}