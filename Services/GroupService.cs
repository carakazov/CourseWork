namespace MySocialNetwork.Services
{
    public class GroupService
    {
        private Mapper mapper = new Mapper();
        private GroupManager groupManager = new GroupManager();
        private PageManager pageManager = new PageManager();

        public void GroupCreation(GroupCreationDto groupcreationDto)
        {
            Group group = mapper.FromGroupCreationDtoToGroup(groupcreationDto);
            groupManager.AddGroup(group);
        }

        public void UpdateScoredPostList(GroupDto groupDto)
        {
            Group group = groupManager.GetGroupByTitle(groupDto.Title);
            groupDto.ScoredPosts = mapper.FindScoredPosts(group);
        }

        public List<GroupDto> FindGroups(FindGroupsDto groupInfo)
        {
            List<Group> groups = groupManager.FindGroups(groupInfo);
            return groups;
        }
    }
}