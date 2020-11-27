using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Models.SocialNetwork;
using MySocialNetwork.Utils;

namespace MySocialNetwork.Services
{
    public class PostService
    {
        private PostManager postManager = new PostManager();
        private Mapper mapper = new Mapper();
        public void Post(InputPostDto InputPostDto)
        {
            Post post = mapper.FromInputPostDtoToPost(InputPostDto);
            postManager.AddNewPost(post);
        }

        public void ScorePost(int postId, ScoreTypes type, int userId)
        {
            postManager.ScorePost(postId, type);
            AddScoredPost(postId, userId, type);
        }

        private void AddScoredPost(int postId, int userId, ScoreTypes type)
        {
            ScoredPost scoredPost = new ScoredPost()
            {
                PostId = postId,
                UserId = userId
            };
            switch (type)
            {
                case ScoreTypes.Positive:
                    scoredPost.Score = true;
                    break;
                case ScoreTypes.Negative:
                    scoredPost.Score = false;
                    break;
            }
            postManager.AddScoredPost(scoredPost);
        }
    }
}