using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;

namespace MySocialNetwork.DAO
{
    public class DialogManager
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();
        private PageManager pageManager = new PageManager();
        public Dialog OpenDialog(int firstUserId, int secondUserId)
        {
            Dialog dialog;
            try
            {
                if (dbContext.Dialogs.Any(d => d.FirstUserId == firstUserId && d.SecondUserId == secondUserId))
                {
                    dialog = LoadDialog(firstUserId, secondUserId);
                }
                else
                {
                    StartDialog(firstUserId, secondUserId);
                    dialog = LoadDialog(firstUserId, secondUserId);
                }
            }
            catch
            {
                throw;
            }
            return dialog;
        }

        private Dialog LoadDialog(int firstUserId, int secondUserId)
        {
             Dialog dialog = dbContext.Dialogs
                .Where(d => d.FirstUserId == firstUserId && d.SecondUserId == secondUserId)
                /*.Include(d => d.SecondUser)
                .Include(d => d.FirstUser)*/.Include(d => d.Wall).First();
            IEnumerable<Post> posts = dbContext.Posts.Where(p => p.WallId == dialog.Wall.Id);
            dialog.Wall.Posts = posts.ToList();
            return dialog;
        }
        
        private void StartDialog(int firstUserId, int secondUserId)
        {
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Dialog dialog = new Dialog()
                    {
                        FirstUserId = firstUserId,
                        SecondUserId = secondUserId,
                        Unread = false
                    };
                    int count = dbContext.Dialogs.Count();
                    dialog = dbContext.Dialogs.Add(dialog);
                    dbContext.SaveChanges();
                    Wall dialogWall = pageManager.CreateWall(WallTypes.Dialog, "Dialog");
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}