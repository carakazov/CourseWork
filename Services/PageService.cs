using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Utils;

namespace MySocialNetwork.Services
{
    public class PageService
    {
        private PageManager pageManager = new PageManager();
        private Mapper mapper = new Mapper();
        public WallDto GetWallDto(int wallId)
        {
            Wall wall = pageManager.GetWall(wallId);
            WallDto wallDto = mapper.FromWallToWallDto(wall);
            return wallDto;
        }
    }
}