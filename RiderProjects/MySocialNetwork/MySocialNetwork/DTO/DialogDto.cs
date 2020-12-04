namespace MySocialNetwork.DTO
{
    public class DialogDto
    {
        public UserInfoDto FirstUser { get; set; }
        public UserInfoDto SecondUser { get; set; }
        public WallDto DialogWall { get; set; }

        public UserInfoDto GetAnotherUser(UserInfoDto user)
        {
            if (FirstUser.Equals(user))
            {
                return SecondUser;
            }
            else
            {
                return FirstUser;
            }
        }
    }
}