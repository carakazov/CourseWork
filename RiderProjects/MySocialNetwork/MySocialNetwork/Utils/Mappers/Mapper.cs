﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Models.SocialNetwork;

namespace MySocialNetwork.Utils
{
    public class Mapper
    {
        public User FromRegistrationDtoToUser(RegistrationDto registrationDto)
        {
            User user = new User()
            {
                Login = registrationDto.Login,
                Password = registrationDto.Password,
                FirstName = registrationDto.FirstName,
                SecondName = registrationDto.SecondName,
                MiddleName = registrationDto.MiddleName,
                BirthDate = registrationDto.BirthDate,
                RegistrationDate = DateTime.Now,
                SystemRoleId = 1
            };
            return user; 
        }

        public WallDto FromWallToWallDto(Wall wall)
        {
            WallDto wallDto = new WallDto();

            wallDto.Id = wall.Id;
            wallDto.WallType = wall.WallType.Title;
            
            foreach (Post post in wall.Posts)
            {
                PostDto postDto = FromPostToPostDto(post);
                wallDto.Posts.Add(postDto);
            }
            return wallDto;
        }

        public UserDto FromUserToUserDto(User user)
        {
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                MiddleName = user.MiddleName,
                Rating = user.Rating,
                Birthday = user.BirthDate,
                RegistrationDate = user.RegistrationDate
            };
            DateTime now = DateTime.Now;
            int age = now.Year - userDto.Birthday.Year;
            userDto.Age = age;
            foreach (Wall wall in user.Walls)
            {
                WallDto wallDto = FromWallToWallDto(wall);
                userDto.Walls.Add(wallDto);
            }

            userDto.Friends = GetFriendsInDto(user.Friendships);
            userDto.SentFriendshipRequests = GetFriendshipsRequestsDto(user.SentRequests.ToList());
            userDto.ReceivedFriendshipRequests = GetFriendshipsRequestsDto(user.ReceivedRequests.ToList());
            userDto.ScoredPosts = FindScoredPosts(user);
            return userDto;
        }

        public List<FriendDto> GetFriendsInDto(ICollection<Friendship> friendships)
        {
            List<FriendDto> friends = new List<FriendDto>();
            foreach (Friendship friendship in friendships)
            {
                FriendDto friend = new FriendDto();
                UserInfoDto userInfo = FromUserAuthorDto(friendship.Friend);
                friend.UserInfo = userInfo;
                if (friendship.TypeId == null)
                {
                    friend.FriendshipType = "common";
                }
                else
                {
                    friend.FriendshipType = friendship.Type.Title;
                }
                friends.Add(friend);
            }

            return friends;
        }
        
        private List<FriendshipRequestDto> GetFriendshipsRequestsDto(List<FriendshipRequest> requests)
        {
            List<FriendshipRequestDto> friendshipRequests = new List<FriendshipRequestDto>();
            foreach (FriendshipRequest request in requests)
            {
                FriendshipRequestDto requestDto = GetFriendshipRequestDto(request);
                friendshipRequests.Add(requestDto);
            }

            return friendshipRequests;
        }

        private FriendshipRequestDto GetFriendshipRequestDto(FriendshipRequest request)
        {
            FriendshipRequestDto requestDto = new FriendshipRequestDto(request.SenderId, request.ReceiverId, request.SendingDate);
            return requestDto;
        }
        
        public List<ScoredPostDto> FindScoredPosts(User user)
        {
            List<ScoredPostDto> scoredPosts = new List<ScoredPostDto>();
            foreach (ScoredPost scoredPost in user.ScoredPosts)
            {
                ScoredPostDto scoredPostDto = FromScoredPostToScoredPostDto(scoredPost);
                scoredPosts.Add(scoredPostDto);
            }

            return scoredPosts;
        }
        
        public UserInfoDto FromUserAuthorDto(User user)
        {
            UserInfoDto userInfoDto = new UserInfoDto()
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                MiddleName = user.MiddleName
            };
            TimeSpan ageTimeSpan = DateTime.Now - user.BirthDate;
            int age = ageTimeSpan.Days / 365;
            userInfoDto.Age = age;
            return userInfoDto;
        }
        
        public PostDto FromPostToPostDto(Post post)
        {
            PostDto postDto = new PostDto()
            {
                Id = post.Id,
                Rating = post.Rating,
                PostingDate = post.PostingDate,
                UserInfo = FromUserAuthorDto(post.Author),
                WallId = post.WallId,
                Text =  post.Text
            };
            foreach (Content content in post.Content)
            {
                if (content.ContentTypeId == 1)
                {
                    postDto.Photos.Add(content.Material);
                }
                else if (content.ContentTypeId == 2)
                {
                    postDto.Sounds.Add(content.Material);
                }
                else
                {
                    postDto.Videos.Add(content.Material);
                }
            }
            return postDto;
        }

        public Post FromPostDtoToPost(PostDto postDto)
        {
            Post post = new Post()
            {
                AuthorId = postDto.UserInfo.Id,
                WallId = postDto.WallId,
                PostingDate = postDto.PostingDate,
                Text = postDto.Text
            };
            return post;
        }

        public Post FromInputPostDtoToPost(InputPostDto inputPostDto)
        {
            Post post = new Post()
            {
                AuthorId = inputPostDto.AuthorId,
                Rating = 0,
                Text = inputPostDto.Text,
                PostingDate = DateTime.Now,
                WallId = inputPostDto.WallId
            };
            return post;
        }

        public ScoredPost FromScoredPostDtoToScoredPost(ScoredPostDto scoredPostDto)
        {
            ScoredPost scoredPost = new ScoredPost()
            {
                PostId = scoredPostDto.PostId,
                UserId = scoredPostDto.UserId
            };
            switch (scoredPostDto.Type)
            {
                case ScoreTypes.Positive:
                    scoredPost.Score = true;
                    break;
                case ScoreTypes.Negative:
                    scoredPost.Score = false;
                    break;
            }
            return scoredPost;
        }

        public ScoredPostDto FromScoredPostToScoredPostDto(ScoredPost scoredPost)
        {
            ScoredPostDto scoredPostDto = new ScoredPostDto()
            {
                UserId = scoredPost.UserId,
                PostId = scoredPost.PostId
            };
            if (scoredPost.Score)
            {
                scoredPostDto.Type = ScoreTypes.Positive;
            }
            else
            {
                scoredPostDto.Type = ScoreTypes.Negative;
            }

            return scoredPostDto;
        }

        public List<UserInfoDto> FromUsersToAuthorsDtos(IEnumerable<User> users)
        {
            List<UserInfoDto> authorDtos = new List<UserInfoDto>();
            foreach (User user in users)
            {
                UserInfoDto userInfoDto = FromUserAuthorDto(user);
                authorDtos.Add(userInfoDto);
            }

            return authorDtos;
        }
    }
}